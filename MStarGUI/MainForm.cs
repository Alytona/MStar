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
    public partial class MainForm : Form
    {
        string RootDirectory;
        string WorkDirectory;

        ScriptElementsHolder ScriptHolder;
        string Filename;

        Dictionary<string, Partition> Partitions;
        readonly Dictionary<string, PartitionPanel> PartitionPanels = new Dictionary<string, PartitionPanel>();

        public MainForm ()
        {
            InitializeComponent();

            WorkDirectory = Environment.CurrentDirectory;
            RootDirectory = Directory.GetParent( WorkDirectory )?.FullName;
            if( string.IsNullOrEmpty( RootDirectory ) )
                RootDirectory = WorkDirectory;
        }

        private void DirectoryChooseButton_Click (object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = WorkDirectory;
            if (ofd.ShowDialog( this ) == DialogResult.OK) {
                WorkDirectory = Path.GetDirectoryName( ofd.FileName );
                refreshFirmwareList( ofd.FileName );
            }

            //FolderBrowserDialog selectWorkDirectoryDialog = new FolderBrowserDialog();
            //selectWorkDirectoryDialog.SelectedPath = WorkDirectory;
            //if( selectWorkDirectoryDialog.ShowDialog( this ) == DialogResult.OK )
            //    WorkDirectory = selectWorkDirectoryDialog.SelectedPath;
            //refreshFirmwareList();
        }

        private void refreshFirmwareList (string selectedFilename)
        {
            string[] firmwareFilenames = Directory.GetFiles( WorkDirectory, "*.bin" );
            FirmwareChooseComboBox.Items.Clear();
            int selectedItemIndex = -1;
            foreach (string filename in firmwareFilenames) {
                FirmwareChooseComboBox.Items.Add( Path.GetFileName( filename ) );
                if (filename == selectedFilename) {
                    selectedItemIndex = FirmwareChooseComboBox.Items.Count - 1;
                }
            }
            if( FirmwareChooseComboBox.Items.Count > 0 )
                FirmwareChooseComboBox.SelectedIndex = selectedItemIndex;
        }

        private void FirmwareChooseComboBox_SelectedIndexChanged (object sender, EventArgs e)
        {
            if( FirmwareChooseComboBox.SelectedIndex == -1 )
                return;

            Filename = Path.Combine( WorkDirectory, (string)FirmwareChooseComboBox.SelectedItem );
            List<string> errorMessages = new List<string>();

            using( StreamReader reader = new StreamReader( Filename ) )
            {
                ScriptHolder = new ScriptElementsHolder();

                while( !reader.EndOfStream )
                {
                    string line = reader.ReadLine();
                    try
                    {
                        if( !ScriptHolder.parseScriptLine( line ) )
                            break;
                    }
                    catch( Exception error )
                    {
                        // Вывод ошибки в протокол
                        errorMessages.Add( "> " + line );
                        errorMessages.Add( error.Message );
                    }
                }
            }
            if( errorMessages.Count != 0 )
            {
                MessageBox.Show( this, "При загрузке заголовка прошивки возникли ошибки" );

                // Показываем протокол загрузки
                ProtocolForm protocolForm = new ProtocolForm();
                protocolForm.setProtocol( errorMessages );
                protocolForm.ShowDialog( this );

//                UnpackToolStripMenuItem.Enabled = false;
            }
            else
            {
                Partitions = new Dictionary<string, Partition> ();
                foreach( WriteFileCommand command in ScriptHolder.getWriteCommands() )
                {
                    Partition partition;
                    if( !Partitions.TryGetValue( command.PartitionName, out partition ) )
                    {
                        partition = new Partition( command );
                        Partitions.Add( command.PartitionName, partition );
                    }
                    partition.addChunk( command );
                }

                foreach( PartitionCreateCommand command in ScriptHolder.PartitionsList.PartitionCreateCommands.Values )
                {
                    Partition partition;
                    if( !Partitions.TryGetValue( command.Name, out partition ) )
                    {
                        partition = new Partition( command );
                        Partitions.Add( command.Name, partition );
                    }
                    partition.Size = Convert.ToInt64( command.Size, 16 );
                }

                PartitionsTablePanel.Controls.Clear();
                PartitionsTablePanel.Controls.Add( PartitionsTableTitlePanel );

                Graphics formGraphics = CreateGraphics();
                float nameWidth = 0;
                float sizeWidth = 0;
                foreach( Partition partition in Partitions.Values )
                {
                    float width = formGraphics.MeasureString( partition.Name, FirmwareFileLabel.Font ).Width;
                    if( width > nameWidth )
                        nameWidth = width;

                    width = formGraphics.MeasureString( partition.getSizeString(), FirmwareFileLabel.Font ).Width;
                    if( width > sizeWidth )

                        sizeWidth = width;
                }

                PartitionSizeTitleLabel.Width = (int)(nameWidth + .5) + 2;
                PartitionSizeTitleLabel.Left = PartitionNameTitleLabel.Left + (int)(nameWidth + .5) + 4;

                PartitionPanels.Clear();
                int index = 1;
                foreach( Partition partition in Partitions.Values )
                {
                    PartitionPanel rowPanel = new PartitionPanel( partition, nameWidth, sizeWidth );
                    rowPanel.Top = (index++) * (rowPanel.Height - 1) + 1;
                    rowPanel.Width = PartitionsTablePanel.ClientSize.Width - 3;
                    PartitionsTablePanel.Controls.Add( rowPanel );
                    rowPanel.PartitionPropertyGrid = PartitionPropertyGrid;

                    PartitionPanels.Add( partition.Name, rowPanel );
                }
            }
        }

        private void SelectAllPartitionsCheckBox_CheckedChanged (object sender, EventArgs e)
        {
            foreach( Control control in PartitionsTablePanel.Controls )
            {
                if( control is PartitionPanel partitionPanel )
                    partitionPanel.Checked = SelectAllPartitionsCheckBox.Checked;
            }
        }

        private void PackPutton_Click (object sender, EventArgs e)
        {
            var logger = new TextBoxLogger( ProtocolTextBox );
            if (packFirmware( logger ))
            {
                logger.logMessage( "Успешно упаковано." );
            }
        }

        public bool packFirmware(IMessageLogger messageLogger)
        {
            try
            {
                bool FullPackage = true;

                string firmwareDir = Path.Combine( WorkDirectory, Path.GetFileNameWithoutExtension( Filename ) );

                ScriptElementsHolder newScriptHolder = new ScriptElementsHolder();

                List<string> writtenPartitions = new List<string>();

                List<Partition> partitionsToPack = new List<Partition>();
                foreach (Control control in PartitionsTablePanel.Controls)
                {
                    if (control is PartitionPanel partitionPanel && partitionPanel.Checked)
                        partitionsToPack.Add( partitionPanel.Partition );
                }

                using ( FileStream inputStream = new FileStream( Filename, FileMode.Open, FileAccess.Read ) )
                using( FileStream outputStream = new FileStream( Filename + ".new", FileMode.Create, FileAccess.Write ) )
                {
                    messageLogger.logMessage( "Добавление заголовка." );

                    for( int i = 0; i < 16 * 1024; i++ )
                        outputStream.WriteByte( 0xFF );

                    IHeaderScriptElement eraseCommandElement = null;
                    int postEnvIndex = -1;

                    foreach( IHeaderScriptElement element in ScriptHolder.Elements )
                    {
                        if( element is InformationalBlock || element is PartionsListSection )
                        {
                            newScriptHolder.Elements.Add( element );
                        }
                        else if (element is EraseCommand)
                        {
                            eraseCommandElement = element;
                        }
                        else if( element is WriteFileCommand command )
                        {
                            if( writtenPartitions.Contains( command.PartitionName ) )
                                continue;
                            writtenPartitions.Add( command.PartitionName );

                            if ( Partitions.TryGetValue( command.PartitionName, out Partition partition ) )
                            {
                                string filename = Path.Combine( firmwareDir, command.PartitionName + ".img" );
                                if (PartitionPanels.TryGetValue( command.PartitionName, out PartitionPanel panel ) && panel.Checked && File.Exists( filename )) 
                                {
                                    if (eraseCommandElement != null)
                                    {
                                        newScriptHolder.Elements.Add( eraseCommandElement );
                                        eraseCommandElement = null;
                                    }
                                    var newChunks = partition.pack( firmwareDir, outputStream, messageLogger );
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
                using (FileStream outputStream = new FileStream( Filename + ".new", FileMode.Append, FileAccess.Write, FileShare.Read ))
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
            catch( Exception error )
            {
                messageLogger.logMessage( error.ToString() );
                return false;
            }
            return true;
        }
    }

    class TextBoxLogger : IMessageLogger
    {
        readonly TextBox LoggerTextBox;

        public TextBoxLogger (TextBox loggerTextBox)
        {
            LoggerTextBox = loggerTextBox;
        }

        public void logMessage (string message)
        {
            LoggerTextBox.AppendText( message + "\r\n" );
        }
    }
}
