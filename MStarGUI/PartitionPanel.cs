using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MStarGUI
{
    class PartitionPanel : Panel
    {
        public readonly Partition Partition;

        CheckBox SelectCheckBox;
        Label NameLabel;
        Label SizeLabel;
        Label FileSizeLabel;

        public PropertyGrid PartitionPropertyGrid
        {
            get;
            set;
        }

        public bool Checked
        {
            get => SelectCheckBox.Checked;
            set => SelectCheckBox.Checked = value;
        }

        public PartitionPanel (Partition partition, float nameWidth, float sizeWidth)
        {
            Partition = partition;
            initControls( nameWidth, sizeWidth );
        }

        void initControls (float nameWidth, float sizeWidth)
        {
            Left = 1;
            Height = 30;
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            BorderStyle = BorderStyle.FixedSingle;
            BackColor = SystemColors.Control;

            SelectCheckBox = new CheckBox();
            SelectCheckBox.Location = new Point( 10, 3 );
            SelectCheckBox.Width = 20;
            Controls.Add( SelectCheckBox );

            NameLabel = new Label();
            NameLabel.Location = new Point( 35, 7 );
            NameLabel.AutoSize = false;
            NameLabel.Width = (int)(nameWidth + .5) + 2;
            NameLabel.Height = NameLabel.Font.Height;
            NameLabel.Text = Partition.Name;
            Controls.Add( NameLabel );

            SizeLabel = new Label();
            SizeLabel.Location = new Point( NameLabel.Left + NameLabel.Width + 4, 7 );
            SizeLabel.AutoSize = false;
            SizeLabel.Width = (int)(sizeWidth + .5) + 4;
            SizeLabel.Height = SizeLabel.Font.Height;
            SizeLabel.Text = Partition.getSizeString();
            SizeLabel.TextAlign = ContentAlignment.TopRight;
            Controls.Add( SizeLabel );

            FileSizeLabel = new Label();
            FileSizeLabel.Location = new Point( SizeLabel.Left + SizeLabel.Width + 4, 7 );
            FileSizeLabel.AutoSize = false;
            FileSizeLabel.Width = (int)(sizeWidth + .5) + 4;
            FileSizeLabel.Height = FileSizeLabel.Font.Height;
            FileSizeLabel.Text = Partition.getChunksSizeString();
            FileSizeLabel.TextAlign = ContentAlignment.TopRight;
            Controls.Add( FileSizeLabel );

            Click += selectPartition;
        }

        void selectPartition (object sender, EventArgs e)
        {
            if (PartitionPropertyGrid != null)
                PartitionPropertyGrid.SelectedObject = Partition;
        }
    }
}
