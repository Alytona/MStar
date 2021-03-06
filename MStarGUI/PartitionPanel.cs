﻿using System;
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

        public enum Columns
        {
            Name,
            Size,
            Type, 
            Chunks
        }

        CheckBox SelectCheckBox;
        Label NameLabel;
        Label SizeLabel;
//        Label FileSizeLabel;
        Label TypeLabel;
        Label ChunksLabel;

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

        public PartitionPanel (Partition partition, float[] columnsWidths)
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

            //FileSizeLabel = new Label();
            //FileSizeLabel.Location = new Point( SizeLabel.Left + SizeLabel.Width + 4, 7 );
            //FileSizeLabel.AutoSize = false;
            //FileSizeLabel.Width = (int)(sizeWidth + .5) + 4;
            //FileSizeLabel.Height = FileSizeLabel.Font.Height;
            //FileSizeLabel.Text = Partition.getChunksSizeString();
            //FileSizeLabel.TextAlign = ContentAlignment.TopRight;
            //Controls.Add( FileSizeLabel );

            TypeLabel = new Label();
            TypeLabel.Location = new Point( SizeLabel.Left + SizeLabel.Width + 5, 7 );
            TypeLabel.AutoSize = false;
            TypeLabel.Width = (int)(columnsWidths[(int)Columns.Type] + .5) + 5;
            TypeLabel.Height = TypeLabel.Font.Height;
            TypeLabel.Text = Partition.getTypeString();
            //TypeLabel.TextAlign = ContentAlignment.TopLeft;
            Controls.Add( TypeLabel );

            ChunksLabel = new Label();
            ChunksLabel.Location = new Point( TypeLabel.Left + TypeLabel.Width + 5, 7 );
            ChunksLabel.AutoSize = false;
            ChunksLabel.Width = (int)(columnsWidths[(int)Columns.Chunks] + .5) + 5;
            ChunksLabel.Height = ChunksLabel.Font.Height;
            ChunksLabel.Text = Partition.getChunksString();
            //ChunksLabel.TextAlign = ContentAlignment.TopLeft;
            Controls.Add( ChunksLabel );

            Click += selectPartition;
        }

        void selectPartition (object sender, EventArgs e)
        {
            if (PartitionPropertyGrid != null)
                PartitionPropertyGrid.SelectedObject = Partition;
        }
    }
}
