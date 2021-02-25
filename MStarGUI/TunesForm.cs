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
    public partial class TunesForm : Form
    {
        public TunesForm ()
        {
            InitializeComponent();
        }

        public void useConfigFrom (MStarConfiguration programConfiguration)
        {
            FirmwareTitleTextBox.Text = programConfiguration.FirmwareTitle;
        }

        public void saveConfigTo (MStarConfiguration programConfiguration)
        {
            programConfiguration.FirmwareTitle = FirmwareTitleTextBox.Text;
        }
    }
}
