using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MStarGUI
{
    public partial class ProtocolForm : Form
    {
        public ProtocolForm ()
        {
            InitializeComponent();
        }

        public void setProtocol (List<string> protocolList)
        {
            ProtocolListBox.Items.Clear();
            foreach (string item in protocolList) 
            {
                ProtocolListBox.Items.Add( item );
            }
        }

        private void CloseToolStripMenuItem_Click (object sender, EventArgs e)
        {
            Close();
        }
    }
}
