using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MStarGUI
{
    public partial class MainWindow : Form
    {
        string WorkDirectory;
        string FirmwareDir;
        string SourceFirmwareFilename;

        ScriptElementsHolder ScriptHolder;
        MStarConfiguration ProgramConfiguration;
        UserPreferences ProgramPreferences;

        readonly List<ImagePanel> ImagePanels = new List<ImagePanel>();

        readonly IMessageLogger UnpackLogger;
        readonly IMessageLogger PackLogger;

        bool PackingProcessing;
        bool UnpackingProcessing;


        public MainWindow ()
        {
            InitializeComponent();
            WorkDirectory = Environment.CurrentDirectory;

            UnpackLogger = new TextBoxLogger( UnpackingProtocolTextBox );
            PackLogger = new TextBoxLogger( PackingProtocolTextBox );

            ProgramConfiguration = new MStarConfiguration( Path.Combine( WorkDirectory, "mstar.config" ) );
            ProgramConfiguration.load();

            ProgramPreferences = new UserPreferences( Path.Combine( WorkDirectory, "mstar.preferences" ) );
            if (ProgramPreferences.load()) {
                if (Directory.Exists( ProgramPreferences.WorkingDirectory ))
                    WorkDirectory = ProgramPreferences.WorkingDirectory;
            }
        }

        private void FirmwareChooseButton_Click (object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Файлы прошивок | *.bin";
            ofd.InitialDirectory = WorkDirectory;
            if (ofd.ShowDialog( this ) == DialogResult.OK)
            {
                WorkDirectory = Path.GetDirectoryName( ofd.FileName );
                refreshFirmwareList( ofd.FileName );
            }

            UnpackButton.Enabled = true;
        }

        private void refreshFirmwareList (string selectedFilename)
        {
            string[] firmwareFilenames = Directory.GetFiles( WorkDirectory, "*.bin" );
            FirmwareChooseComboBox.Items.Clear();
            int selectedItemIndex = -1;
            foreach (string filename in firmwareFilenames)
            {
                FirmwareChooseComboBox.Items.Add( Path.GetFileName( filename ) );
                if (filename == selectedFilename)
                {
                    selectedItemIndex = FirmwareChooseComboBox.Items.Count - 1;
                }
            }
            if (FirmwareChooseComboBox.Items.Count > 0)
                FirmwareChooseComboBox.SelectedIndex = selectedItemIndex;
        }

        private void FirmwareChooseComboBox_SelectedIndexChanged (object sender, EventArgs e)
        {
            if (FirmwareChooseComboBox.SelectedIndex == -1)
                return;

            SourceFirmwareFilename = Path.Combine( WorkDirectory, (string)FirmwareChooseComboBox.SelectedItem );

            ScriptHolder = new ScriptElementsHolder();
            if (ScriptHolder.loadFrom( SourceFirmwareFilename, UnpackLogger )) 
            {
                fillPartitionsPanel( ScriptHolder.getPartitions() );
            }
        }

        private void fillPartitionsPanel (Dictionary<string, Partition> partitions)
        {
            SelectAllPartitionsCheckBox.Checked = false;

            PartitionsTablePanel.Controls.Clear();
            PartitionsTablePanel.Controls.Add( PartitionsTableTitlePanel );

            Graphics formGraphics = CreateGraphics();
            float[] widths = { 0, 0, 0, 0 };
            foreach (Partition partition in partitions.Values)
            {
                float width = formGraphics.MeasureString( partition.Name, FirmwareFileLabel.Font ).Width;
                if (width > widths[(int)PartitionPanel.Columns.Name])
                    widths[(int)PartitionPanel.Columns.Name] = width;

                width = formGraphics.MeasureString( partition.getSizeString(), FirmwareFileLabel.Font ).Width;
                if (width > widths[(int)PartitionPanel.Columns.Size])
                    widths[(int)PartitionPanel.Columns.Size] = width;

                width = formGraphics.MeasureString( partition.getTypeString(), FirmwareFileLabel.Font ).Width;
                if (width > widths[(int)PartitionPanel.Columns.Type])
                    widths[(int)PartitionPanel.Columns.Type] = width;

                width = formGraphics.MeasureString( partition.getChunksString(), FirmwareFileLabel.Font ).Width;
                if (width > widths[(int)PartitionPanel.Columns.Chunks])
                    widths[(int)PartitionPanel.Columns.Chunks] = width;
            }

            PartitionSizeTitleLabel.Left = PartitionNameTitleLabel.Left + (int)(widths[(int)PartitionPanel.Columns.Name] + .5) + 10;
            PartitionTypeTitleLabel.Left = PartitionSizeTitleLabel.Left + (int)(widths[(int)PartitionPanel.Columns.Size] + .5) + 10;
            PartitionChunksTitleLabel.Left = PartitionTypeTitleLabel.Left + (int)(widths[(int)PartitionPanel.Columns.Type] + .5) + 10;

            int index = 1;
            foreach (Partition partition in partitions.Values)
            {
                PartitionPanel rowPanel = new PartitionPanel( partition, widths );
                rowPanel.Top = (index++) * (rowPanel.Height - 1) + 1;
                rowPanel.Width = PartitionsTablePanel.ClientSize.Width - 3;
                PartitionsTablePanel.Controls.Add( rowPanel );
            }
        }

        private void SelectAllPartitionsCheckBox_CheckedChanged (object sender, EventArgs e)
        {
            foreach (Control control in PartitionsTablePanel.Controls)
            {
                if (control is PartitionPanel partitionPanel)
                    partitionPanel.Checked = SelectAllPartitionsCheckBox.Checked;
            }
        }
        private void UnpackButton_Click (object sender, EventArgs e)
        {
            try
            {
                UnpackingProtocolTextBox.Clear();
                Application.DoEvents();

                List<Partition> partitionsToUnpack = new List<Partition>();
                foreach (Control control in PartitionsTablePanel.Controls)
                {
                    if (control is PartitionPanel partitionPanel && partitionPanel.Checked)
                        partitionsToUnpack.Add( partitionPanel.Partition );
                }

                if (partitionsToUnpack.Count > 0)
                {
                    UnpackButton.Enabled = false;
                    PackButton.Enabled = false;
                    PackageFolderChooseButton.Enabled = false;

                    UnpackingProcessing = true;

                    FirmwareDir = Path.Combine( WorkDirectory, Path.GetFileNameWithoutExtension( SourceFirmwareFilename ) );
                    if (!Directory.Exists( FirmwareDir ))
                        Directory.CreateDirectory( FirmwareDir );

                    if (Directory.GetFiles( FirmwareDir, ".Header" ).Length == 0)
                        ScriptHolder.saveTo( Path.Combine( FirmwareDir, ".Header" ) );

                    bool successfully = true;
                    StringListLogger taskLogger = new StringListLogger();

                    Task unpacking = new Task( delegate ()
                    {
                        try
                        {
                            using (FileStream fileStream = new FileStream( SourceFirmwareFilename, FileMode.Open, FileAccess.Read ))
                            {
                                foreach (Partition partition in partitionsToUnpack)
                                {
                                    if (!partition.unpack( FirmwareDir, fileStream, taskLogger ))
                                        successfully = false;
                                }
                            }
                        }
                        catch (Exception error)
                        {
                            UnpackLogger.logMessage( error.ToString() );
                            successfully = false;
                        }
                    } );

                    unpacking.Start();

                    TextBoxPointsProgressIndicator progressIndicator = new TextBoxPointsProgressIndicator( UnpackingProtocolTextBox );
                    while (!unpacking.Wait( 100 ))
                    {
                        Application.DoEvents();
                        if (taskLogger.hasNewMessages())
                        {
                            progressIndicator.hide();
                            taskLogger.exportMessages( UnpackLogger );
                        }
                        else
                        {
                            UnpackLogger.logMessage( progressIndicator.getNextState(), false );
                        }
                    }
                    progressIndicator.hide();
                    taskLogger.exportMessages( UnpackLogger );

                    //try
                    //{
                    //    using (FileStream fileStream = new FileStream( SourceFirmwareFilename, FileMode.Open, FileAccess.Read ))
                    //    {
                    //        foreach (Partition partition in partitionsToUnpack)
                    //        {
                    //            if (!partition.unpack( FirmwareDir, fileStream, UnpackLogger ))
                    //                successfully = false;
                    //        }
                    //    }
                    //}
                    //catch (Exception error)
                    //{
                    //    UnpackLogger.logMessage( error.ToString() );
                    //    successfully = false;
                    //}

                    if (successfully)
                        UnpackLogger.logMessage( "Успешно распаковалось." );

                    PackageFolderLabel.Text = "Папка сборки : " + FirmwareDir;
                    fillImagesPanel( ScriptHolder.getPartitions() );

                    PackButton.Enabled = true;
                }
            }
            catch (Exception error)
            {
                MessageBox.Show( this, error.ToString(), "Разбор прошивки завершился с ошибкой" );
            }
            finally 
            {
                UnpackingProcessing = false;
                UnpackButton.Enabled = true;
                PackButton.Enabled = true;
                PackageFolderChooseButton.Enabled = true;
            }
        }

        private void fillImagesPanel (Dictionary<string, Partition> partitions)
        {
            SelectAllImagesCheckBox.Checked = false;

            List<Partition> imagePartitions = new List<Partition>();
            foreach (Partition partition in partitions.Values)
            {
                if (File.Exists( Path.Combine( FirmwareDir, partition.Name + ".img" ) ))
                    imagePartitions.Add( partition );
            }
            // FullPackage = partitions.Count == imagePartitions.Count;

            ImagesTablePanel.Controls.Clear();
            ImagesTablePanel.Controls.Add( ImagesTableTitlePanel );

            Graphics formGraphics = CreateGraphics();
            float[] widths = { 0, 0, 0 };
            foreach (Partition partition in imagePartitions)
            {
                float width = formGraphics.MeasureString( partition.Name, FirmwareFileLabel.Font ).Width;
                if (width > widths[(int)ImagePanel.Columns.Name])
                    widths[(int)ImagePanel.Columns.Name] = width;

                width = formGraphics.MeasureString( partition.getSizeString(), FirmwareFileLabel.Font ).Width;
                if (width > widths[(int)ImagePanel.Columns.Size])
                    widths[(int)ImagePanel.Columns.Size] = width;

                width = formGraphics.MeasureString( partition.getTypeString(), FirmwareFileLabel.Font ).Width;
                if (width > widths[(int)ImagePanel.Columns.Type])
                    widths[(int)ImagePanel.Columns.Type] = width;
            }

            ImageSizeTitleLabel.Left = ImageNameTitleLabel.Left + (int)(widths[(int)ImagePanel.Columns.Name] + .5) + 10;
            ImageTypeTitleLabel.Left = ImageSizeTitleLabel.Left + (int)(widths[(int)ImagePanel.Columns.Size] + .5) + 10;

            ImagePanels.Clear();
            int index = 1;
            foreach (Partition partition in imagePartitions)
            {
                ImagePanel rowPanel = new ImagePanel( partition, widths );
                rowPanel.Top = (index++) * (rowPanel.Height - 1) + 1;
                rowPanel.Width = ImagesTablePanel.ClientSize.Width - 3;
                ImagesTablePanel.Controls.Add( rowPanel );
                ImagePanels.Add( rowPanel );
            }
        }

        private void PackPutton_Click (object sender, EventArgs e)
        {
            try
            {
                PackButton.Enabled = false;

                PackingProtocolTextBox.Clear();
                Application.DoEvents();
                if (packFirmware( PackLogger ))
                {
                    PackLogger.logMessage( "Успешно упаковано." );
                }
            }
            finally {
                PackButton.Enabled = true;
            }
        }

        public bool packFirmware (IMessageLogger messageLogger)
        {
            try
            {
                if (ScriptHolder == null)
                {
                    messageLogger.logMessage( "Нужно выбрать папку сборки." );
                    return false;
                }

                Dictionary<string, Partition> partitionsToPack = new Dictionary<string, Partition>();
                foreach (ImagePanel panel in ImagePanels)
                {
                    if (panel.Checked)
                        partitionsToPack.Add( panel.Partition.Name, panel.Partition );
                }

                StringListLogger taskLogger = new StringListLogger();

                var firmwareBuilder = new FirmwareBuilder( FirmwareDir, ProgramConfiguration.FirmwareTitle );
                Task packing = new Task( delegate ()
                {
                    firmwareBuilder.pack( ScriptHolder, partitionsToPack, taskLogger );
                } );

                PackageFolderChooseButton.Enabled = false;
                UnpackButton.Enabled = false;

                PackingProcessing = true;
                packing.Start();

                TextBoxPointsProgressIndicator progressIndicator = new TextBoxPointsProgressIndicator( PackingProtocolTextBox );
                while (!packing.Wait( 100 ))
                {
                    Application.DoEvents();

                    if (taskLogger.hasNewMessages()) {
                        progressIndicator.hide();
                        taskLogger.exportMessages( messageLogger );
                    } else {
                        messageLogger.logMessage( progressIndicator.getNextState(), false );
                    }
                }
                progressIndicator.hide();
                taskLogger.exportMessages( messageLogger );

                return true;
            }
            catch (Exception error)
            {
                messageLogger.logMessage( error.ToString() );
                return false;
            }
            finally {
                PackageFolderChooseButton.Enabled = true;
                PackingProcessing = false;
                UnpackButton.Enabled = FirmwareChooseComboBox.Items.Count > 0;
            }
        } 

        private void PackageFolderChooseButton_Click (object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Заголовки прошивок | .Header";
            ofd.DefaultExt = ".Header";
            ofd.InitialDirectory = WorkDirectory;
            if (ofd.ShowDialog( this ) == DialogResult.OK)
            {
                if (Path.GetFileName( ofd.FileName ) != ".Header")
                {
                    MessageBox.Show( this, "Нужно выбрать файл .Header", "Неверно выбран файл" );
                }
                else 
                {
                    var newScriptHolder = new ScriptElementsHolder();
                    if (newScriptHolder.loadFrom( ofd.FileName, PackLogger )) 
                    {
                        ScriptHolder = newScriptHolder;
                        FirmwareDir = Path.GetDirectoryName( ofd.FileName );
                        PackageFolderLabel.Text = "Папка сборки : " + FirmwareDir;
                        fillImagesPanel( ScriptHolder.getPartitions() );
                    }
                    PackButton.Enabled = true;
                    PackingProtocolTextBox.Clear();
                }
            }
        }

        private void SelectAllImagesCheckBox_CheckedChanged (object sender, EventArgs e)
        {
            foreach (Control control in ImagesTablePanel.Controls)
            {
                if (control is ImagePanel imagePanel)
                    imagePanel.Checked = SelectAllImagesCheckBox.Checked;
            }
        }

        private void TunesButton_Click (object sender, EventArgs e)
        {
            var tunesForm = new TunesForm();
            tunesForm.useConfigFrom( ProgramConfiguration );
            if (tunesForm.ShowDialog( this ) == DialogResult.OK) {
                tunesForm.saveConfigTo( ProgramConfiguration );
                ProgramConfiguration.save();
            }
        }

        private void MainWindow_FormClosing (object sender, FormClosingEventArgs e)
        {
            if (PackingProcessing || UnpackingProcessing)
                e.Cancel = true;
            else {
                ProgramPreferences.WorkingDirectory = WorkDirectory;
                ProgramPreferences.MainWindowSize = Size;
                ProgramPreferences.MainWindowPosition = Location;
                ProgramPreferences.UnpackSplitterPosition = UnpackRightPanel.Width;
                ProgramPreferences.PackSplitterPosition = PackRightPanel.Width;
                ProgramPreferences.save();
            }
        }

        private void MainWindow_Shown (object sender, EventArgs e)
        {
            if (ProgramPreferences.Loaded) 
            {
                if (ProgramPreferences.MainWindowSize.Width >= 100 && ProgramPreferences.MainWindowSize.Height >= 100 && 
                    ProgramPreferences.MainWindowPosition.X > -10000 && ProgramPreferences.MainWindowPosition.Y > -10000)
                {
                    this.SetBounds( ProgramPreferences.MainWindowPosition.X, ProgramPreferences.MainWindowPosition.Y, ProgramPreferences.MainWindowSize.Width, ProgramPreferences.MainWindowSize.Height );
                }

                if (ProgramPreferences.UnpackSplitterPosition >= 50)
                    UnpackRightPanel.Width = ProgramPreferences.UnpackSplitterPosition;

                if (ProgramPreferences.PackSplitterPosition >= 50)
                    PackRightPanel.Width = ProgramPreferences.PackSplitterPosition;
            }
        }
    }

    class StringListLogger : IMessageLogger
    {
        readonly object MessagesLock = new object();
        readonly List<string> Messages = new List<string>();

        int StartIndex;

        public void logMessage (string message, bool crlf = true)
        {
            lock (MessagesLock) {
                Messages.Add( message ); // + (crlf ? "\r\n" : "") );
            }
        }
        public bool hasNewMessages ()
        {
            lock (MessagesLock)
            {
                return StartIndex < Messages.Count;
            }
        }
        public bool exportMessages ( IMessageLogger logger )
        {
            lock (MessagesLock)
            {
                bool result = StartIndex < Messages.Count;
                while (StartIndex < Messages.Count)
                    logger.logMessage( Messages[StartIndex++] );
                return result;
            }
        }
    }

    class TextBoxProgressIndicator
    {
        const string IndicatorStates = "\\|/-";

        readonly TextBox TargetTextBox;

        int State = 0;
        bool Visible;

        public TextBoxProgressIndicator (TextBox textBox)
        {
            TargetTextBox = textBox;
        }

        public string getNextState ()
        {
            Visible = true;
            char state = IndicatorStates[State++];
            if (State >= IndicatorStates.Length)
                State = 0;
            return $" {state}";
        }
        public void hide ()
        {
            if (Visible)
            {
                TargetTextBox.Text = TargetTextBox.Text.Remove( TargetTextBox.Text.Length - 2, 2 );
                Visible = false;
            }
        }
    }

    class TextBoxPointsProgressIndicator
    {
        readonly TextBox TargetTextBox;

        int State = 0;
        bool Visible;

        public TextBoxPointsProgressIndicator (TextBox textBox)
        {
            TargetTextBox = textBox;
        }

        public string getNextState ()
        {
            if (State >= 10) {
                hide();
            }
            Visible = true;
            State++;
            return State == 1? " >" : ">";
        }
        public void hide ()
        {
            if (Visible)
            {
                if (State > 0)
                    TargetTextBox.Text = TargetTextBox.Text.Remove( TargetTextBox.Text.Length - (State + 1), (State + 1) );
                State = 0;
                Visible = false;
            }
        }
    }
}
