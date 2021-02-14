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
    public partial class UnpackDialog : Form, IMessageLogger
    {
        string WorkDirectory;
        string Filename;
        List<Partition> PartitionsToUnpack;

        public UnpackDialog (string workDirectory, string filename, List<Partition> partitionsToUnpack)
        {
            InitializeComponent();

            WorkDirectory = workDirectory;
            Filename = filename;
            PartitionsToUnpack = partitionsToUnpack;
        }
        private void UnpackDialog_Shown (object sender, EventArgs e)
        {
            bool successfully = true;
            try
            {
                using( FileStream fileStream = new FileStream( Filename, FileMode.Open, FileAccess.Read ) )
                {
                    foreach( Partition partition in PartitionsToUnpack )
                    {
                        if( !partition.unpack( WorkDirectory, fileStream, this ) )
                            successfully = false;
                    }
                }
            }
            catch( Exception error )
            {
                logMessage( error.ToString() );
                successfully = false;
            }
            if (successfully)
                logMessage( "Успешно распаковалось." );
            //    Close();
        }
        public void logMessage (string message)
        {
            ProtocolTextBox.AppendText( message + "\r\n" );
        }
    }
}
