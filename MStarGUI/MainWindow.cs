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

        readonly Dictionary<string, ImagePanel> ImagePanels = new Dictionary<string, ImagePanel>();

        readonly IMessageLogger UnpackLogger;
        readonly IMessageLogger PackLogger;

        bool FullPackage;

        public MainWindow ()
        {
            InitializeComponent();
            WorkDirectory = Environment.CurrentDirectory;

            UnpackLogger = new TextBoxLogger( UnpackingProtocolTextBox );
            PackLogger = new TextBoxLogger( PackingProtocolTextBox );
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
                    FirmwareDir = Path.Combine( WorkDirectory, Path.GetFileNameWithoutExtension( SourceFirmwareFilename ) );
                    if (!Directory.Exists( FirmwareDir ))
                        Directory.CreateDirectory( FirmwareDir );

                    if (Directory.GetFiles( FirmwareDir, ".Header" ).Length == 0) 
                        ScriptHolder.saveTo( Path.Combine( FirmwareDir, ".Header" ) );

                    bool successfully = true;
                    try
                    {
                        using (FileStream fileStream = new FileStream( SourceFirmwareFilename, FileMode.Open, FileAccess.Read ))
                        {
                            foreach (Partition partition in partitionsToUnpack)
                            {
                                if (!partition.unpack( FirmwareDir, fileStream, UnpackLogger ))
                                    successfully = false;
                            }
                        }
                    }
                    catch (Exception error)
                    {
                        UnpackLogger.logMessage( error.ToString() );
                        successfully = false;
                    }
                    if (successfully)
                        UnpackLogger.logMessage( "Успешно распаковалось." );

                    PackageFolderLabel.Text = "Папка сборки : " + FirmwareDir;
                    fillImagesPanel( ScriptHolder.getPartitions() );
                }
            }
            catch (Exception error)
            {
                MessageBox.Show( this, error.ToString(), "Разбор прошивки завершился с ошибкой" );
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
            FullPackage = partitions.Count == imagePartitions.Count;

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

            //            PartitionTypeTitleLabel.Width = (int)(nameWidth + .5) + 2;
            //PartitionChunksTitleLabel.Left = PartitionTypeTitleLabel.Left + (int)(widths[(int)PartitionPanel.Columns.Type] + .5) + 10;

            ImagePanels.Clear();
            int index = 1;
            foreach (Partition partition in imagePartitions)
            {
                ImagePanel rowPanel = new ImagePanel( partition, widths );
                rowPanel.Top = (index++) * (rowPanel.Height - 1) + 1;
                rowPanel.Width = ImagesTablePanel.ClientSize.Width - 3;
                ImagesTablePanel.Controls.Add( rowPanel );
                ImagePanels.Add( partition.Name, rowPanel );

                rowPanel.PartitionPropertyGrid = PartitionPropertyGrid;
            }
        }

        private void PackPutton_Click (object sender, EventArgs e)
        {
            PackingProtocolTextBox.Clear();
            Application.DoEvents();
            if (packFirmware( PackLogger ))
            {
                PackLogger.logMessage( "Успешно упаковано." );
            }
        }

        public bool packFirmware (IMessageLogger messageLogger)
        {
            try
            {
                ScriptElementsHolder newScriptHolder = new ScriptElementsHolder();

                List<string> writtenPartitions = new List<string>();

                string newFirmwareFilename = FirmwareDir + "_new.bin";
                using (FileStream outputStream = new FileStream( newFirmwareFilename, FileMode.Create, FileAccess.Write ))
                {
                    messageLogger.logMessage( "Добавление заголовка." );

                    for (int i = 0; i < 16 * 1024; i++)
                        outputStream.WriteByte( 0xFF );

                    IHeaderScriptElement eraseCommandElement = null;
                    int postEnvIndex = -1;

                    foreach (IHeaderScriptElement element in ScriptHolder.Elements)
                    {
                        if (element is InformationalBlock || element is PartionsListSection)
                        {
                            newScriptHolder.Elements.Add( element );
                        }
                        else if (element is EraseCommand)
                        {
                            eraseCommandElement = element;
                        }
                        else if (element is WriteFileCommand command)
                        {
                            if (writtenPartitions.Contains( command.PartitionName ))
                                continue;
                            writtenPartitions.Add( command.PartitionName );

                            if (ImagePanels.TryGetValue( command.PartitionName, out ImagePanel panel ))
                            {
                                string filename = Path.Combine( FirmwareDir, command.PartitionName + ".img" );
                                if (File.Exists( filename ) && panel.Checked)
                                {
                                    if (eraseCommandElement != null)
                                    {
                                        newScriptHolder.Elements.Add( eraseCommandElement );
                                        eraseCommandElement = null;
                                    }
                                    var newChunks = panel.Partition.pack( FirmwareDir, outputStream, messageLogger );
                                    newScriptHolder.Elements.AddRange( newChunks );
                                }
                                else
                                {
                                    FullPackage = false;
                                    // partition.copy( inputStream, outputStream, messageLogger );
                                }
                            }
                            postEnvIndex = newScriptHolder.Elements.Count;
                        }
                    }

                    messageLogger.logMessage( "Запись заголовка." );

                    outputStream.Seek( 0, SeekOrigin.Begin );
                    using (StreamWriter writer = new StreamWriter( outputStream ))
                    {
                        if (FullPackage)
                        {
                            foreach (IHeaderScriptElement element in newScriptHolder.Elements)
                                element.writeToHeader( writer );
                        }
                        else
                        {
                            int index = 0;
                            bool partitionsStart = false;

                            foreach (IHeaderScriptElement element in newScriptHolder.Elements)
                            {
                                if (element is PartionsListSection)
                                    partitionsStart = true;
                                else
                                {
                                    if (!partitionsStart || index >= postEnvIndex || element is EraseCommand || element is WriteFileCommand)
                                        element.writeToHeader( writer );
                                }
                                index++;
                            }
                        }
                    }
                }

                messageLogger.logMessage( "Расчет и запись контрольных сумм." );
                using (FileStream outputStream = new FileStream( newFirmwareFilename, FileMode.Append, FileAccess.Write, FileShare.Read ))
                {
                    uint firmwareBodyCrc = Partition.computeCrc32( outputStream.Name, 16 * 1024, outputStream.Length - 16 * 1024 );
                    uint firmwareHeaderCrc = Partition.computeCrc32( outputStream.Name, 0, 16 * 1024 );
                    if (ScriptHolder.CrcType == ScriptElementsHolder.CrcTypes.Third)
                    {
                        outputStream.Write( BitConverter.GetBytes( firmwareBodyCrc ), 0, 4 );
                    }
                    outputStream.Write( new byte[] { 49, 50, 51, 52, 53, 54, 55, 56 }, 0, 8 );
                    outputStream.Write( BitConverter.GetBytes( firmwareHeaderCrc ), 0, 4 );
                    if (ScriptHolder.CrcType == ScriptElementsHolder.CrcTypes.First)
                    {
                        outputStream.Write( BitConverter.GetBytes( firmwareBodyCrc ), 0, 4 );
                    }
                    outputStream.Flush();

                    if (ScriptHolder.CrcType > ScriptElementsHolder.CrcTypes.First)
                    {
                        uint firmwareCrc = Partition.computeCrc32( outputStream, 0, outputStream.Length );
                        outputStream.Write( BitConverter.GetBytes( firmwareCrc ), 0, 4 );
                    }

                    string starter = (newScriptHolder.Elements[0] is InformationalBlock firstComment) ? firstComment.getStarter() : null;
                    if (starter == null)
                        starter = "# MSTAR FIRMWARE";
                    outputStream.Write( Encoding.ASCII.GetBytes( starter ), 0, 16 );
                }
            }
            catch (Exception error)
            {
                messageLogger.logMessage( error.ToString() );
                return false;
            }
            return true;
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
    }
}
