﻿namespace MStarGUI
{
    partial class MainWindow
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
            if (disposing && (components != null))
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.MainTabControl = new System.Windows.Forms.TabControl();
            this.UnpackPage = new System.Windows.Forms.TabPage();
            this.LeftPanel = new System.Windows.Forms.Panel();
            this.PartitionsTablePanel = new System.Windows.Forms.Panel();
            this.PartitionsTableTitlePanel = new System.Windows.Forms.Panel();
            this.PartitionChunksTitleLabel = new System.Windows.Forms.Label();
            this.PartitionTypeTitleLabel = new System.Windows.Forms.Label();
            this.PartitionSizeTitleLabel = new System.Windows.Forms.Label();
            this.PartitionNameTitleLabel = new System.Windows.Forms.Label();
            this.SelectAllPartitionsCheckBox = new System.Windows.Forms.CheckBox();
            this.FirmwareChooseButton = new System.Windows.Forms.Button();
            this.FirmwareFileLabel = new System.Windows.Forms.Label();
            this.FirmwareChooseComboBox = new System.Windows.Forms.ComboBox();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.UnpackRightPanel = new System.Windows.Forms.Panel();
            this.UnpackingProtocolTextBox = new System.Windows.Forms.TextBox();
            this.UnpackButton = new System.Windows.Forms.Button();
            this.PackagePage = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ImagesTablePanel = new System.Windows.Forms.Panel();
            this.ImagesTableTitlePanel = new System.Windows.Forms.Panel();
            this.ImageTypeTitleLabel = new System.Windows.Forms.Label();
            this.ImageSizeTitleLabel = new System.Windows.Forms.Label();
            this.ImageNameTitleLabel = new System.Windows.Forms.Label();
            this.SelectAllImagesCheckBox = new System.Windows.Forms.CheckBox();
            this.PackageFolderChooseButton = new System.Windows.Forms.Button();
            this.PackageFolderLabel = new System.Windows.Forms.Label();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.PackRightPanel = new System.Windows.Forms.Panel();
            this.TunesButton = new System.Windows.Forms.Button();
            this.PackingProtocolTextBox = new System.Windows.Forms.TextBox();
            this.PackButton = new System.Windows.Forms.Button();
            this.MainTabControl.SuspendLayout();
            this.UnpackPage.SuspendLayout();
            this.LeftPanel.SuspendLayout();
            this.PartitionsTablePanel.SuspendLayout();
            this.PartitionsTableTitlePanel.SuspendLayout();
            this.UnpackRightPanel.SuspendLayout();
            this.PackagePage.SuspendLayout();
            this.panel1.SuspendLayout();
            this.ImagesTablePanel.SuspendLayout();
            this.ImagesTableTitlePanel.SuspendLayout();
            this.PackRightPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainTabControl
            // 
            this.MainTabControl.Controls.Add(this.UnpackPage);
            this.MainTabControl.Controls.Add(this.PackagePage);
            this.MainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTabControl.Location = new System.Drawing.Point(0, 0);
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(944, 579);
            this.MainTabControl.TabIndex = 0;
            // 
            // UnpackPage
            // 
            this.UnpackPage.Controls.Add(this.LeftPanel);
            this.UnpackPage.Controls.Add(this.splitter2);
            this.UnpackPage.Controls.Add(this.UnpackRightPanel);
            this.UnpackPage.Location = new System.Drawing.Point(4, 22);
            this.UnpackPage.Name = "UnpackPage";
            this.UnpackPage.Padding = new System.Windows.Forms.Padding(3);
            this.UnpackPage.Size = new System.Drawing.Size(936, 553);
            this.UnpackPage.TabIndex = 0;
            this.UnpackPage.Text = "Распаковка";
            this.UnpackPage.UseVisualStyleBackColor = true;
            // 
            // LeftPanel
            // 
            this.LeftPanel.Controls.Add(this.PartitionsTablePanel);
            this.LeftPanel.Controls.Add(this.FirmwareChooseButton);
            this.LeftPanel.Controls.Add(this.FirmwareFileLabel);
            this.LeftPanel.Controls.Add(this.FirmwareChooseComboBox);
            this.LeftPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LeftPanel.Location = new System.Drawing.Point(3, 3);
            this.LeftPanel.MinimumSize = new System.Drawing.Size(100, 100);
            this.LeftPanel.Name = "LeftPanel";
            this.LeftPanel.Size = new System.Drawing.Size(573, 547);
            this.LeftPanel.TabIndex = 6;
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
            this.PartitionsTablePanel.Location = new System.Drawing.Point(2, 35);
            this.PartitionsTablePanel.Name = "PartitionsTablePanel";
            this.PartitionsTablePanel.Size = new System.Drawing.Size(565, 508);
            this.PartitionsTablePanel.TabIndex = 6;
            // 
            // PartitionsTableTitlePanel
            // 
            this.PartitionsTableTitlePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PartitionsTableTitlePanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.PartitionsTableTitlePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PartitionsTableTitlePanel.Controls.Add(this.PartitionChunksTitleLabel);
            this.PartitionsTableTitlePanel.Controls.Add(this.PartitionTypeTitleLabel);
            this.PartitionsTableTitlePanel.Controls.Add(this.PartitionSizeTitleLabel);
            this.PartitionsTableTitlePanel.Controls.Add(this.PartitionNameTitleLabel);
            this.PartitionsTableTitlePanel.Controls.Add(this.SelectAllPartitionsCheckBox);
            this.PartitionsTableTitlePanel.Location = new System.Drawing.Point(1, 1);
            this.PartitionsTableTitlePanel.Name = "PartitionsTableTitlePanel";
            this.PartitionsTableTitlePanel.Padding = new System.Windows.Forms.Padding(5);
            this.PartitionsTableTitlePanel.Size = new System.Drawing.Size(560, 30);
            this.PartitionsTableTitlePanel.TabIndex = 0;
            // 
            // PartitionChunksTitleLabel
            // 
            this.PartitionChunksTitleLabel.AutoSize = true;
            this.PartitionChunksTitleLabel.Location = new System.Drawing.Point(267, 7);
            this.PartitionChunksTitleLabel.Name = "PartitionChunksTitleLabel";
            this.PartitionChunksTitleLabel.Size = new System.Drawing.Size(103, 13);
            this.PartitionChunksTitleLabel.TabIndex = 7;
            this.PartitionChunksTitleLabel.Text = "Количество частей";
            // 
            // PartitionTypeTitleLabel
            // 
            this.PartitionTypeTitleLabel.AutoSize = true;
            this.PartitionTypeTitleLabel.Location = new System.Drawing.Point(201, 7);
            this.PartitionTypeTitleLabel.Name = "PartitionTypeTitleLabel";
            this.PartitionTypeTitleLabel.Size = new System.Drawing.Size(26, 13);
            this.PartitionTypeTitleLabel.TabIndex = 6;
            this.PartitionTypeTitleLabel.Text = "Тип";
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
            // FirmwareChooseButton
            // 
            this.FirmwareChooseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FirmwareChooseButton.Image = ((System.Drawing.Image)(resources.GetObject("FirmwareChooseButton.Image")));
            this.FirmwareChooseButton.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.FirmwareChooseButton.Location = new System.Drawing.Point(531, 4);
            this.FirmwareChooseButton.Name = "FirmwareChooseButton";
            this.FirmwareChooseButton.Size = new System.Drawing.Size(36, 28);
            this.FirmwareChooseButton.TabIndex = 5;
            this.FirmwareChooseButton.UseVisualStyleBackColor = true;
            this.FirmwareChooseButton.Click += new System.EventHandler(this.FirmwareChooseButton_Click);
            // 
            // FirmwareFileLabel
            // 
            this.FirmwareFileLabel.AutoSize = true;
            this.FirmwareFileLabel.Location = new System.Drawing.Point(9, 10);
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
            this.FirmwareChooseComboBox.Location = new System.Drawing.Point(111, 8);
            this.FirmwareChooseComboBox.Name = "FirmwareChooseComboBox";
            this.FirmwareChooseComboBox.Size = new System.Drawing.Size(414, 21);
            this.FirmwareChooseComboBox.TabIndex = 1;
            this.FirmwareChooseComboBox.SelectedIndexChanged += new System.EventHandler(this.FirmwareChooseComboBox_SelectedIndexChanged);
            // 
            // splitter2
            // 
            this.splitter2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter2.Location = new System.Drawing.Point(576, 3);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(7, 547);
            this.splitter2.TabIndex = 5;
            this.splitter2.TabStop = false;
            // 
            // UnpackRightPanel
            // 
            this.UnpackRightPanel.Controls.Add(this.UnpackingProtocolTextBox);
            this.UnpackRightPanel.Controls.Add(this.UnpackButton);
            this.UnpackRightPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.UnpackRightPanel.Location = new System.Drawing.Point(583, 3);
            this.UnpackRightPanel.MinimumSize = new System.Drawing.Size(50, 100);
            this.UnpackRightPanel.Name = "UnpackRightPanel";
            this.UnpackRightPanel.Size = new System.Drawing.Size(350, 547);
            this.UnpackRightPanel.TabIndex = 7;
            // 
            // UnpackingProtocolTextBox
            // 
            this.UnpackingProtocolTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UnpackingProtocolTextBox.Location = new System.Drawing.Point(6, 35);
            this.UnpackingProtocolTextBox.Multiline = true;
            this.UnpackingProtocolTextBox.Name = "UnpackingProtocolTextBox";
            this.UnpackingProtocolTextBox.ReadOnly = true;
            this.UnpackingProtocolTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.UnpackingProtocolTextBox.Size = new System.Drawing.Size(340, 508);
            this.UnpackingProtocolTextBox.TabIndex = 4;
            // 
            // UnpackButton
            // 
            this.UnpackButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.UnpackButton.Enabled = false;
            this.UnpackButton.Location = new System.Drawing.Point(196, 7);
            this.UnpackButton.Name = "UnpackButton";
            this.UnpackButton.Size = new System.Drawing.Size(150, 23);
            this.UnpackButton.TabIndex = 2;
            this.UnpackButton.Text = "Распаковать";
            this.UnpackButton.UseVisualStyleBackColor = true;
            this.UnpackButton.Click += new System.EventHandler(this.UnpackButton_Click);
            // 
            // PackagePage
            // 
            this.PackagePage.Controls.Add(this.panel1);
            this.PackagePage.Controls.Add(this.splitter1);
            this.PackagePage.Controls.Add(this.PackRightPanel);
            this.PackagePage.Location = new System.Drawing.Point(4, 22);
            this.PackagePage.Name = "PackagePage";
            this.PackagePage.Padding = new System.Windows.Forms.Padding(3);
            this.PackagePage.Size = new System.Drawing.Size(936, 553);
            this.PackagePage.TabIndex = 1;
            this.PackagePage.Text = "Сборка";
            this.PackagePage.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ImagesTablePanel);
            this.panel1.Controls.Add(this.PackageFolderChooseButton);
            this.panel1.Controls.Add(this.PackageFolderLabel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.MinimumSize = new System.Drawing.Size(150, 100);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(573, 547);
            this.panel1.TabIndex = 6;
            // 
            // ImagesTablePanel
            // 
            this.ImagesTablePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ImagesTablePanel.AutoScroll = true;
            this.ImagesTablePanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ImagesTablePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ImagesTablePanel.Controls.Add(this.ImagesTableTitlePanel);
            this.ImagesTablePanel.Location = new System.Drawing.Point(2, 35);
            this.ImagesTablePanel.Name = "ImagesTablePanel";
            this.ImagesTablePanel.Size = new System.Drawing.Size(565, 508);
            this.ImagesTablePanel.TabIndex = 6;
            // 
            // ImagesTableTitlePanel
            // 
            this.ImagesTableTitlePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ImagesTableTitlePanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ImagesTableTitlePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ImagesTableTitlePanel.Controls.Add(this.ImageTypeTitleLabel);
            this.ImagesTableTitlePanel.Controls.Add(this.ImageSizeTitleLabel);
            this.ImagesTableTitlePanel.Controls.Add(this.ImageNameTitleLabel);
            this.ImagesTableTitlePanel.Controls.Add(this.SelectAllImagesCheckBox);
            this.ImagesTableTitlePanel.Location = new System.Drawing.Point(1, 1);
            this.ImagesTableTitlePanel.Name = "ImagesTableTitlePanel";
            this.ImagesTableTitlePanel.Padding = new System.Windows.Forms.Padding(5);
            this.ImagesTableTitlePanel.Size = new System.Drawing.Size(560, 30);
            this.ImagesTableTitlePanel.TabIndex = 0;
            // 
            // ImageTypeTitleLabel
            // 
            this.ImageTypeTitleLabel.AutoSize = true;
            this.ImageTypeTitleLabel.Location = new System.Drawing.Point(208, 7);
            this.ImageTypeTitleLabel.Name = "ImageTypeTitleLabel";
            this.ImageTypeTitleLabel.Size = new System.Drawing.Size(26, 13);
            this.ImageTypeTitleLabel.TabIndex = 7;
            this.ImageTypeTitleLabel.Text = "Тип";
            // 
            // ImageSizeTitleLabel
            // 
            this.ImageSizeTitleLabel.AutoSize = true;
            this.ImageSizeTitleLabel.Location = new System.Drawing.Point(111, 7);
            this.ImageSizeTitleLabel.Name = "ImageSizeTitleLabel";
            this.ImageSizeTitleLabel.Size = new System.Drawing.Size(46, 13);
            this.ImageSizeTitleLabel.TabIndex = 5;
            this.ImageSizeTitleLabel.Text = "Размер";
            // 
            // ImageNameTitleLabel
            // 
            this.ImageNameTitleLabel.AutoSize = true;
            this.ImageNameTitleLabel.Location = new System.Drawing.Point(35, 7);
            this.ImageNameTitleLabel.Name = "ImageNameTitleLabel";
            this.ImageNameTitleLabel.Size = new System.Drawing.Size(29, 13);
            this.ImageNameTitleLabel.TabIndex = 4;
            this.ImageNameTitleLabel.Text = "Имя";
            // 
            // SelectAllImagesCheckBox
            // 
            this.SelectAllImagesCheckBox.AutoSize = true;
            this.SelectAllImagesCheckBox.Location = new System.Drawing.Point(10, 7);
            this.SelectAllImagesCheckBox.Name = "SelectAllImagesCheckBox";
            this.SelectAllImagesCheckBox.Size = new System.Drawing.Size(15, 14);
            this.SelectAllImagesCheckBox.TabIndex = 3;
            this.SelectAllImagesCheckBox.UseVisualStyleBackColor = true;
            this.SelectAllImagesCheckBox.CheckedChanged += new System.EventHandler(this.SelectAllImagesCheckBox_CheckedChanged);
            // 
            // PackageFolderChooseButton
            // 
            this.PackageFolderChooseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PackageFolderChooseButton.Image = ((System.Drawing.Image)(resources.GetObject("PackageFolderChooseButton.Image")));
            this.PackageFolderChooseButton.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.PackageFolderChooseButton.Location = new System.Drawing.Point(531, 4);
            this.PackageFolderChooseButton.Margin = new System.Windows.Forms.Padding(0);
            this.PackageFolderChooseButton.Name = "PackageFolderChooseButton";
            this.PackageFolderChooseButton.Size = new System.Drawing.Size(36, 28);
            this.PackageFolderChooseButton.TabIndex = 5;
            this.PackageFolderChooseButton.UseVisualStyleBackColor = true;
            this.PackageFolderChooseButton.Click += new System.EventHandler(this.PackageFolderChooseButton_Click);
            // 
            // PackageFolderLabel
            // 
            this.PackageFolderLabel.AutoSize = true;
            this.PackageFolderLabel.Location = new System.Drawing.Point(10, 11);
            this.PackageFolderLabel.Name = "PackageFolderLabel";
            this.PackageFolderLabel.Size = new System.Drawing.Size(84, 13);
            this.PackageFolderLabel.TabIndex = 2;
            this.PackageFolderLabel.Text = "Папка сборки :";
            // 
            // splitter1
            // 
            this.splitter1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(576, 3);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(7, 547);
            this.splitter1.TabIndex = 4;
            this.splitter1.TabStop = false;
            // 
            // PackRightPanel
            // 
            this.PackRightPanel.Controls.Add(this.TunesButton);
            this.PackRightPanel.Controls.Add(this.PackingProtocolTextBox);
            this.PackRightPanel.Controls.Add(this.PackButton);
            this.PackRightPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.PackRightPanel.Location = new System.Drawing.Point(583, 3);
            this.PackRightPanel.MinimumSize = new System.Drawing.Size(50, 100);
            this.PackRightPanel.Name = "PackRightPanel";
            this.PackRightPanel.Size = new System.Drawing.Size(350, 547);
            this.PackRightPanel.TabIndex = 7;
            // 
            // TunesButton
            // 
            this.TunesButton.Image = ((System.Drawing.Image)(resources.GetObject("TunesButton.Image")));
            this.TunesButton.Location = new System.Drawing.Point(6, 4);
            this.TunesButton.Margin = new System.Windows.Forms.Padding(0);
            this.TunesButton.Name = "TunesButton";
            this.TunesButton.Size = new System.Drawing.Size(36, 28);
            this.TunesButton.TabIndex = 6;
            this.TunesButton.UseVisualStyleBackColor = true;
            this.TunesButton.Click += new System.EventHandler(this.TunesButton_Click);
            // 
            // PackingProtocolTextBox
            // 
            this.PackingProtocolTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PackingProtocolTextBox.Location = new System.Drawing.Point(6, 35);
            this.PackingProtocolTextBox.Multiline = true;
            this.PackingProtocolTextBox.Name = "PackingProtocolTextBox";
            this.PackingProtocolTextBox.ReadOnly = true;
            this.PackingProtocolTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.PackingProtocolTextBox.Size = new System.Drawing.Size(340, 508);
            this.PackingProtocolTextBox.TabIndex = 4;
            // 
            // PackButton
            // 
            this.PackButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PackButton.Enabled = false;
            this.PackButton.Location = new System.Drawing.Point(196, 7);
            this.PackButton.Name = "PackButton";
            this.PackButton.Size = new System.Drawing.Size(150, 23);
            this.PackButton.TabIndex = 3;
            this.PackButton.Text = "Упаковать";
            this.PackButton.UseVisualStyleBackColor = true;
            this.PackButton.Click += new System.EventHandler(this.PackPutton_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 579);
            this.Controls.Add(this.MainTabControl);
            this.MinimumSize = new System.Drawing.Size(200, 150);
            this.Name = "MainWindow";
            this.Text = "Утилита разборки и сборки прошивок";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Shown += new System.EventHandler(this.MainWindow_Shown);
            this.MainTabControl.ResumeLayout(false);
            this.UnpackPage.ResumeLayout(false);
            this.LeftPanel.ResumeLayout(false);
            this.LeftPanel.PerformLayout();
            this.PartitionsTablePanel.ResumeLayout(false);
            this.PartitionsTableTitlePanel.ResumeLayout(false);
            this.PartitionsTableTitlePanel.PerformLayout();
            this.UnpackRightPanel.ResumeLayout(false);
            this.UnpackRightPanel.PerformLayout();
            this.PackagePage.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ImagesTablePanel.ResumeLayout(false);
            this.ImagesTableTitlePanel.ResumeLayout(false);
            this.ImagesTableTitlePanel.PerformLayout();
            this.PackRightPanel.ResumeLayout(false);
            this.PackRightPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl MainTabControl;
        private System.Windows.Forms.TabPage UnpackPage;
        private System.Windows.Forms.TabPage PackagePage;
        private System.Windows.Forms.Panel LeftPanel;
        private System.Windows.Forms.Panel PartitionsTablePanel;
        private System.Windows.Forms.Panel PartitionsTableTitlePanel;
        private System.Windows.Forms.Label PartitionSizeTitleLabel;
        private System.Windows.Forms.Label PartitionNameTitleLabel;
        private System.Windows.Forms.CheckBox SelectAllPartitionsCheckBox;
        private System.Windows.Forms.Button FirmwareChooseButton;
        private System.Windows.Forms.Label FirmwareFileLabel;
        private System.Windows.Forms.ComboBox FirmwareChooseComboBox;
        private System.Windows.Forms.Panel UnpackRightPanel;
        private System.Windows.Forms.TextBox UnpackingProtocolTextBox;
        private System.Windows.Forms.Button UnpackButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel ImagesTablePanel;
        private System.Windows.Forms.Panel ImagesTableTitlePanel;
        private System.Windows.Forms.Label ImageSizeTitleLabel;
        private System.Windows.Forms.Label ImageNameTitleLabel;
        private System.Windows.Forms.CheckBox SelectAllImagesCheckBox;
        private System.Windows.Forms.Button PackageFolderChooseButton;
        private System.Windows.Forms.Label PackageFolderLabel;
        private System.Windows.Forms.Panel PackRightPanel;
        private System.Windows.Forms.TextBox PackingProtocolTextBox;
        private System.Windows.Forms.Button PackButton;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Label PartitionTypeTitleLabel;
        private System.Windows.Forms.Label PartitionChunksTitleLabel;
        private System.Windows.Forms.Label ImageTypeTitleLabel;
        private System.Windows.Forms.Button TunesButton;
    }
}