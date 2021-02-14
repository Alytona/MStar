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
    public partial class PackDialog : Form, IMessageLogger
    {
        MainForm MainForm;

        public PackDialog (MainForm mainForm)
        {
            InitializeComponent();

            MainForm = mainForm;
        }

        private void PackDialog_Shown (object sender, EventArgs e)
        {
            if (MainForm.packFirmware( this )) {
                logMessage( "Успешно упаковано." );
//                Close();
            }
        }

        public void logMessage (string message)
        {
            ProtocolTextBox.AppendText( message + "\r\n" );
        }
    }
}
