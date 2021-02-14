namespace MStarGUI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing)
        {
            if( disposing && (components != null) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.PartitionPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.LeftPanel = new System.Windows.Forms.Panel();
            this.PartitionsTablePanel = new System.Windows.Forms.Panel();
            this.PartitionsTableTitlePanel = new System.Windows.Forms.Panel();
            this.PartitionSizeTitleLabel = new System.Windows.Forms.Label();
            this.PartitionNameTitleLabel = new System.Windows.Forms.Label();
            this.SelectAllPartitionsCheckBox = new System.Windows.Forms.CheckBox();
            this.DirectoryChooseButton = new System.Windows.Forms.Button();
            this.FirmwareFileLabel = new System.Windows.Forms.Label();
            this.FirmwareChooseComboBox = new System.Windows.Forms.ComboBox();
            this.RightPanel = new System.Windows.Forms.Panel();
            this.ProtocolTextBox = new System.Windows.Forms.TextBox();
            this.PackPutton = new System.Windows.Forms.Button();
            this.UnpackButton = new System.Windows.Forms.Button();
            this.LeftPanel.SuspendLayout();
            this.PartitionsTablePanel.SuspendLayout();
            this.PartitionsTableTitlePanel.SuspendLayout();
            this.RightPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // PartitionPropertyGrid
            // 
            this.PartitionPropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PartitionPropertyGrid.Location = new System.Drawing.Point(6, 69);
            this.PartitionPropertyGrid.Name = "PartitionPropertyGrid";
            this.PartitionPropertyGrid.Size = new System.Drawing.Size(407, 196);
            this.PartitionPropertyGrid.TabIndex = 1;
            // 
            // splitter1
            // 
            this.splitter1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(833, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(10, 804);
            this.splitter1.TabIndex = 3;
            this.splitter1.TabStop = false;
            // 
            // LeftPanel
            // 
            this.LeftPanel.Controls.Add(this.PartitionsTablePanel);
            this.LeftPanel.Controls.Add(this.DirectoryChooseButton);
            this.LeftPanel.Controls.Add(this.FirmwareFileLabel);
            this.LeftPanel.Controls.Add(this.FirmwareChooseComboBox);
            this.LeftPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LeftPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftPanel.Name = "LeftPanel";
            this.LeftPanel.Size = new System.Drawing.Size(833, 804);
            this.LeftPanel.TabIndex = 4;
            // 
            // PartitionsTablePanel
            // 
            this.PartitionsTablePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PartitionsTablePanel.AutoScroll = true;
            this.PartitionsTablePanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.PartitionsTablePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PartitionsTablePanel.Controls.Add(this.PartitionsTableTitlePanel);
            this.PartitionsTablePanel.Location = new System.Drawing.Point(7, 43);
            this.PartitionsTablePanel.Name = "PartitionsTablePanel";
            this.PartitionsTablePanel.Size = new System.Drawing.Size(820, 753);
            this.PartitionsTablePanel.TabIndex = 6;
            // 
            // PartitionsTableTitlePanel
            // 
            this.PartitionsTableTitlePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PartitionsTableTitlePanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.PartitionsTableTitlePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PartitionsTableTitlePanel.Controls.Add(this.PartitionSizeTitleLabel);
            this.PartitionsTableTitlePanel.Controls.Add(this.PartitionNameTitleLabel);
            this.PartitionsTableTitlePanel.Controls.Add(this.SelectAllPartitionsCheckBox);
            this.PartitionsTableTitlePanel.Location = new System.Drawing.Point(1, 1);
            this.PartitionsTableTitlePanel.Name = "PartitionsTableTitlePanel";
            this.PartitionsTableTitlePanel.Padding = new System.Windows.Forms.Padding(5);
            this.PartitionsTableTitlePanel.Size = new System.Drawing.Size(815, 30);
            this.PartitionsTableTitlePanel.TabIndex = 0;
            // 
            // PartitionSizeTitleLabel
            // 
            this.PartitionSizeTitleLabel.AutoSize = true;
            this.PartitionSizeTitleLabel.Location = new System.Drawing.Point(111, 7);
            this.PartitionSizeTitleLabel.Name = "PartitionSizeTitleLabel";
            this.PartitionSizeTitleLabel.Size = new System.Drawing.Size(46, 13);
            this.PartitionSizeTitleLabel.TabIndex = 5;
            this.PartitionSizeTitleLabel.Text = "Размер";
            // 
            // PartitionNameTitleLabel
            // 
            this.PartitionNameTitleLabel.AutoSize = true;
            this.PartitionNameTitleLabel.Location = new System.Drawing.Point(35, 7);
            this.PartitionNameTitleLabel.Name = "PartitionNameTitleLabel";
            this.PartitionNameTitleLabel.Size = new System.Drawing.Size(29, 13);
            this.PartitionNameTitleLabel.TabIndex = 4;
            this.PartitionNameTitleLabel.Text = "Имя";
            // 
            // SelectAllPartitionsCheckBox
            // 
            this.SelectAllPartitionsCheckBox.AutoSize = true;
            this.SelectAllPartitionsCheckBox.Location = new System.Drawing.Point(10, 7);
            this.SelectAllPartitionsCheckBox.Name = "SelectAllPartitionsCheckBox";
            this.SelectAllPartitionsCheckBox.Size = new System.Drawing.Size(15, 14);
            this.SelectAllPartitionsCheckBox.TabIndex = 3;
            this.SelectAllPartitionsCheckBox.UseVisualStyleBackColor = true;
            this.SelectAllPartitionsCheckBox.CheckedChanged += new System.EventHandler(this.SelectAllPartitionsCheckBox_CheckedChanged);
            // 
            // DirectoryChooseButton
            // 
            this.DirectoryChooseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DirectoryChooseButton.Image = ((System.Drawing.Image)(resources.GetObject("DirectoryChooseButton.Image")));
            this.DirectoryChooseButton.Location = new System.Drawing.Point(791, 9);
            this.DirectoryChooseButton.Name = "DirectoryChooseButton";
            this.DirectoryChooseButton.Size = new System.Drawing.Size(36, 28);
            this.DirectoryChooseButton.TabIndex = 5;
            this.DirectoryChooseButton.UseVisualStyleBackColor = true;
            this.DirectoryChooseButton.Click += new System.EventHandler(this.DirectoryChooseButton_Click);
            // 
            // FirmwareFileLabel
            // 
            this.FirmwareFileLabel.AutoSize = true;
            this.FirmwareFileLabel.Location = new System.Drawing.Point(10, 16);
            this.FirmwareFileLabel.Name = "FirmwareFileLabel";
            this.FirmwareFileLabel.Size = new System.Drawing.Size(95, 13);
            this.FirmwareFileLabel.TabIndex = 2;
            this.FirmwareFileLabel.Text = "Файл прошивки :";
            // 
            // FirmwareChooseComboBox
            // 
            this.FirmwareChooseComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FirmwareChooseComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FirmwareChooseComboBox.FormattingEnabled = true;
            this.FirmwareChooseComboBox.Location = new System.Drawing.Point(111, 13);
            this.FirmwareChooseComboBox.Name = "FirmwareChooseComboBox";
            this.FirmwareChooseComboBox.Size = new System.Drawing.Size(674, 21);
            this.FirmwareChooseComboBox.TabIndex = 1;
            this.FirmwareChooseComboBox.SelectedIndexChanged += new System.EventHandler(this.FirmwareChooseComboBox_SelectedIndexChanged);
            // 
            // RightPanel
            // 
            this.RightPanel.Controls.Add(this.ProtocolTextBox);
            this.RightPanel.Controls.Add(this.PackPutton);
            this.RightPanel.Controls.Add(this.UnpackButton);
            this.RightPanel.Controls.Add(this.PartitionPropertyGrid);
            this.RightPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.RightPanel.Location = new System.Drawing.Point(843, 0);
            this.RightPanel.Name = "RightPanel";
            this.RightPanel.Size = new System.Drawing.Size(420, 804);
            this.RightPanel.TabIndex = 5;
            // 
            // ProtocolTextBox
            // 
            this.ProtocolTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProtocolTextBox.Location = new System.Drawing.Point(6, 271);
            this.ProtocolTextBox.Multiline = true;
            this.ProtocolTextBox.Name = "ProtocolTextBox";
            this.ProtocolTextBox.ReadOnly = true;
            this.ProtocolTextBox.Size = new System.Drawing.Size(406, 524);
            this.ProtocolTextBox.TabIndex = 4;
            // 
            // PackPutton
            // 
            this.PackPutton.Location = new System.Drawing.Point(6, 38);
            this.PackPutton.Name = "PackPutton";
            this.PackPutton.Size = new System.Drawing.Size(214, 23);
            this.PackPutton.TabIndex = 3;
            this.PackPutton.Text = "Упаковать";
            this.PackPutton.UseVisualStyleBackColor = true;
            this.PackPutton.Click += new System.EventHandler(this.PackPutton_Click);
            // 
            // UnpackButton
            // 
            this.UnpackButton.Location = new System.Drawing.Point(6, 9);
            this.UnpackButton.Name = "UnpackButton";
            this.UnpackButton.Size = new System.Drawing.Size(214, 23);
            this.UnpackButton.TabIndex = 2;
            this.UnpackButton.Text = "Распаковать выбранные разделы";
            this.UnpackButton.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1263, 804);
            this.Controls.Add(this.LeftPanel);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.RightPanel);
            this.Name = "MainForm";
            this.Text = "Утилита разборки и сборки прошивок";
            this.LeftPanel.ResumeLayout(false);
            this.LeftPanel.PerformLayout();
            this.PartitionsTablePanel.ResumeLayout(false);
            this.PartitionsTableTitlePanel.ResumeLayout(false);
            this.PartitionsTableTitlePanel.PerformLayout();
            this.RightPanel.ResumeLayout(false);
            this.RightPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PropertyGrid PartitionPropertyGrid;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel LeftPanel;
        private System.Windows.Forms.Label FirmwareFileLabel;
        private System.Windows.Forms.ComboBox FirmwareChooseComboBox;
        private System.Windows.Forms.Panel RightPanel;
        private System.Windows.Forms.CheckBox SelectAllPartitionsCheckBox;
        private System.Windows.Forms.Button PackPutton;
        private System.Windows.Forms.Button UnpackButton;
        private System.Windows.Forms.Button DirectoryChooseButton;
        private System.Windows.Forms.Panel PartitionsTablePanel;
        private System.Windows.Forms.Panel PartitionsTableTitlePanel;
        private System.Windows.Forms.Label PartitionSizeTitleLabel;
        private System.Windows.Forms.Label PartitionNameTitleLabel;
        private System.Windows.Forms.TextBox ProtocolTextBox;
    }
}

