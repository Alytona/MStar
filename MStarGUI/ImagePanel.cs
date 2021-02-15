using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MStarGUI
{
    class ImagePanel : Panel
    {
        public readonly Partition Partition;

        public enum Columns
        {
            Name,
            Size,
            Type
        }

        CheckBox SelectCheckBox;
        Label NameLabel;
        Label SizeLabel;
        Label TypeLabel;

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

        public ImagePanel (Partition partition, float[] columnsWidths)
        {
            Partition = partition;
            initControls( columnsWidths );
        }

        void initControls (float[] columnsWidths)
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
            NameLabel.Width = (int)(columnsWidths[(int)Columns.Name] + .5) + 5;
            NameLabel.Height = NameLabel.Font.Height;
            NameLabel.Text = Partition.Name;
            Controls.Add( NameLabel );

            SizeLabel = new Label();
            SizeLabel.Location = new Point( NameLabel.Left + NameLabel.Width + 5, 7 );
            SizeLabel.AutoSize = false;
            SizeLabel.Width = (int)(columnsWidths[(int)Columns.Size] + .5) + 5;
            SizeLabel.Height = SizeLabel.Font.Height;
            SizeLabel.Text = Partition.getSizeString();
            SizeLabel.TextAlign = ContentAlignment.TopRight;
            Controls.Add( SizeLabel );

            TypeLabel = new Label();
            TypeLabel.Location = new Point( SizeLabel.Left + SizeLabel.Width + 5, 7 );
            TypeLabel.AutoSize = false;
            TypeLabel.Width = (int)(columnsWidths[(int)Columns.Type] + .5) + 5;
            TypeLabel.Height = TypeLabel.Font.Height;
            TypeLabel.Text = Partition.getTypeString();
            //TypeLabel.TextAlign = ContentAlignment.TopLeft;
            Controls.Add( TypeLabel );

            Click += selectPartition;
        }

        void selectPartition (object sender, EventArgs e)
        {
            if (PartitionPropertyGrid != null)
                PartitionPropertyGrid.SelectedObject = Partition;
        }
    }
}