﻿namespace ImageProcessing
{
    partial class ImageProcessingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.currentImagePictureBox = new System.Windows.Forms.PictureBox();
            this.statusLabel = new System.Windows.Forms.Label();
            this.loadButton = new System.Windows.Forms.Button();
            this.runButton = new System.Windows.Forms.Button();
            this.baseNeedleHeightTextBox = new System.Windows.Forms.TextBox();
            this.unitsLabel = new System.Windows.Forms.Label();
            this.frameRateNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.formControlsGroupBox = new System.Windows.Forms.GroupBox();
            this.saveDestinationTextBox = new System.Windows.Forms.TextBox();
            this.saveDestinationLabel = new System.Windows.Forms.Label();
            this.browseButton = new System.Windows.Forms.Button();
            this.fpsLabel = new System.Windows.Forms.Label();
            this.blackWhiteCalibrationLabel = new System.Windows.Forms.Label();
            this.blackWhiteNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.frameRateHeaderLabel = new System.Windows.Forms.Label();
            this.calibrateButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.baseNeedleHeightLabel = new System.Windows.Forms.Label();
            this.runProgressBar = new System.Windows.Forms.ProgressBar();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutUsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadImagesDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.currentImagePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.frameRateNumericUpDown)).BeginInit();
            this.formControlsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.blackWhiteNumericUpDown)).BeginInit();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // currentImagePictureBox
            // 
            this.currentImagePictureBox.BackColor = System.Drawing.SystemColors.ControlDark;
            this.currentImagePictureBox.Location = new System.Drawing.Point(67, 32);
            this.currentImagePictureBox.Name = "currentImagePictureBox";
            this.currentImagePictureBox.Size = new System.Drawing.Size(512, 384);
            this.currentImagePictureBox.TabIndex = 0;
            this.currentImagePictureBox.TabStop = false;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(61, 441);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(38, 13);
            this.statusLabel.TabIndex = 1;
            this.statusLabel.Text = "Ready";
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(18, 40);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(75, 36);
            this.loadButton.TabIndex = 2;
            this.loadButton.Text = "Load";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // runButton
            // 
            this.runButton.Enabled = false;
            this.runButton.Location = new System.Drawing.Point(482, 40);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(77, 36);
            this.runButton.TabIndex = 3;
            this.runButton.Text = "Run";
            this.runButton.UseVisualStyleBackColor = true;
            this.runButton.Click += new System.EventHandler(this.runButton_Click);
            // 
            // baseNeedleHeightTextBox
            // 
            this.baseNeedleHeightTextBox.Location = new System.Drawing.Point(386, 53);
            this.baseNeedleHeightTextBox.Name = "baseNeedleHeightTextBox";
            this.baseNeedleHeightTextBox.Size = new System.Drawing.Size(55, 20);
            this.baseNeedleHeightTextBox.TabIndex = 5;
            // 
            // unitsLabel
            // 
            this.unitsLabel.AutoSize = true;
            this.unitsLabel.Location = new System.Drawing.Point(445, 60);
            this.unitsLabel.Name = "unitsLabel";
            this.unitsLabel.Size = new System.Drawing.Size(21, 13);
            this.unitsLabel.TabIndex = 6;
            this.unitsLabel.Text = "cm";
            // 
            // frameRateNumericUpDown
            // 
            this.frameRateNumericUpDown.Location = new System.Drawing.Point(251, 53);
            this.frameRateNumericUpDown.Maximum = new decimal(new int[] {
            1410065407,
            2,
            0,
            0});
            this.frameRateNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.frameRateNumericUpDown.Name = "frameRateNumericUpDown";
            this.frameRateNumericUpDown.Size = new System.Drawing.Size(82, 20);
            this.frameRateNumericUpDown.TabIndex = 7;
            this.frameRateNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.frameRateNumericUpDown.ValueChanged += new System.EventHandler(this.frameRateNumericUpDown_ValueChanged);
            // 
            // formControlsGroupBox
            // 
            this.formControlsGroupBox.Controls.Add(this.saveDestinationTextBox);
            this.formControlsGroupBox.Controls.Add(this.saveDestinationLabel);
            this.formControlsGroupBox.Controls.Add(this.browseButton);
            this.formControlsGroupBox.Controls.Add(this.fpsLabel);
            this.formControlsGroupBox.Controls.Add(this.blackWhiteCalibrationLabel);
            this.formControlsGroupBox.Controls.Add(this.blackWhiteNumericUpDown);
            this.formControlsGroupBox.Controls.Add(this.frameRateHeaderLabel);
            this.formControlsGroupBox.Controls.Add(this.calibrateButton);
            this.formControlsGroupBox.Controls.Add(this.label2);
            this.formControlsGroupBox.Controls.Add(this.baseNeedleHeightLabel);
            this.formControlsGroupBox.Controls.Add(this.loadButton);
            this.formControlsGroupBox.Controls.Add(this.runButton);
            this.formControlsGroupBox.Controls.Add(this.frameRateNumericUpDown);
            this.formControlsGroupBox.Controls.Add(this.unitsLabel);
            this.formControlsGroupBox.Controls.Add(this.baseNeedleHeightTextBox);
            this.formControlsGroupBox.Location = new System.Drawing.Point(38, 474);
            this.formControlsGroupBox.Name = "formControlsGroupBox";
            this.formControlsGroupBox.Size = new System.Drawing.Size(580, 131);
            this.formControlsGroupBox.TabIndex = 8;
            this.formControlsGroupBox.TabStop = false;
            this.formControlsGroupBox.Text = "Controls";
            // 
            // saveDestinationTextBox
            // 
            this.saveDestinationTextBox.Enabled = false;
            this.saveDestinationTextBox.Location = new System.Drawing.Point(116, 92);
            this.saveDestinationTextBox.Name = "saveDestinationTextBox";
            this.saveDestinationTextBox.Size = new System.Drawing.Size(369, 20);
            this.saveDestinationTextBox.TabIndex = 20;
            // 
            // saveDestinationLabel
            // 
            this.saveDestinationLabel.AutoSize = true;
            this.saveDestinationLabel.Location = new System.Drawing.Point(19, 95);
            this.saveDestinationLabel.Name = "saveDestinationLabel";
            this.saveDestinationLabel.Size = new System.Drawing.Size(91, 13);
            this.saveDestinationLabel.TabIndex = 19;
            this.saveDestinationLabel.Text = "Save Destination:";
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(491, 91);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(58, 23);
            this.browseButton.TabIndex = 18;
            this.browseButton.Text = "Browse";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // fpsLabel
            // 
            this.fpsLabel.AutoSize = true;
            this.fpsLabel.Location = new System.Drawing.Point(335, 57);
            this.fpsLabel.Name = "fpsLabel";
            this.fpsLabel.Size = new System.Drawing.Size(27, 13);
            this.fpsLabel.TabIndex = 17;
            this.fpsLabel.Text = "FPS";
            // 
            // blackWhiteCalibrationLabel
            // 
            this.blackWhiteCalibrationLabel.AutoSize = true;
            this.blackWhiteCalibrationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blackWhiteCalibrationLabel.Location = new System.Drawing.Point(113, 31);
            this.blackWhiteCalibrationLabel.Name = "blackWhiteCalibrationLabel";
            this.blackWhiteCalibrationLabel.Size = new System.Drawing.Size(119, 13);
            this.blackWhiteCalibrationLabel.TabIndex = 16;
            this.blackWhiteCalibrationLabel.Text = "Black/White Calibration";
            // 
            // blackWhiteNumericUpDown
            // 
            this.blackWhiteNumericUpDown.Location = new System.Drawing.Point(114, 53);
            this.blackWhiteNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.blackWhiteNumericUpDown.Name = "blackWhiteNumericUpDown";
            this.blackWhiteNumericUpDown.Size = new System.Drawing.Size(46, 20);
            this.blackWhiteNumericUpDown.TabIndex = 15;
            this.blackWhiteNumericUpDown.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
            // 
            // frameRateHeaderLabel
            // 
            this.frameRateHeaderLabel.AutoSize = true;
            this.frameRateHeaderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.frameRateHeaderLabel.Location = new System.Drawing.Point(268, 31);
            this.frameRateHeaderLabel.Name = "frameRateHeaderLabel";
            this.frameRateHeaderLabel.Size = new System.Drawing.Size(62, 13);
            this.frameRateHeaderLabel.TabIndex = 15;
            this.frameRateHeaderLabel.Text = "Frame Rate";
            // 
            // calibrateButton
            // 
            this.calibrateButton.Enabled = false;
            this.calibrateButton.Location = new System.Drawing.Point(167, 52);
            this.calibrateButton.Name = "calibrateButton";
            this.calibrateButton.Size = new System.Drawing.Size(65, 23);
            this.calibrateButton.TabIndex = 14;
            this.calibrateButton.Text = "Calibrate";
            this.calibrateButton.UseVisualStyleBackColor = true;
            this.calibrateButton.Click += new System.EventHandler(this.calibrateButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(146, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 13;
            // 
            // baseNeedleHeightLabel
            // 
            this.baseNeedleHeightLabel.AutoSize = true;
            this.baseNeedleHeightLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseNeedleHeightLabel.Location = new System.Drawing.Point(369, 31);
            this.baseNeedleHeightLabel.Name = "baseNeedleHeightLabel";
            this.baseNeedleHeightLabel.Size = new System.Drawing.Size(104, 13);
            this.baseNeedleHeightLabel.TabIndex = 12;
            this.baseNeedleHeightLabel.Text = "Base/Needle Height";
            // 
            // runProgressBar
            // 
            this.runProgressBar.Location = new System.Drawing.Point(190, 434);
            this.runProgressBar.Name = "runProgressBar";
            this.runProgressBar.Size = new System.Drawing.Size(395, 23);
            this.runProgressBar.TabIndex = 9;
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.imageToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(665, 24);
            this.menuStrip.TabIndex = 10;
            this.menuStrip.Text = "menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // imageToolStripMenuItem
            // 
            this.imageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runToolStripMenuItem});
            this.imageToolStripMenuItem.Name = "imageToolStripMenuItem";
            this.imageToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.imageToolStripMenuItem.Text = "Image";
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.Enabled = false;
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(95, 22);
            this.runToolStripMenuItem.Text = "Run";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutUsToolStripMenuItem});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // aboutUsToolStripMenuItem
            // 
            this.aboutUsToolStripMenuItem.Name = "aboutUsToolStripMenuItem";
            this.aboutUsToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.aboutUsToolStripMenuItem.Text = "About \'Image Processing\'";
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // ImageProcessingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 620);
            this.Controls.Add(this.runProgressBar);
            this.Controls.Add(this.formControlsGroupBox);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.currentImagePictureBox);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "ImageProcessingForm";
            this.Text = "Image Processing";
            ((System.ComponentModel.ISupportInitialize)(this.currentImagePictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.frameRateNumericUpDown)).EndInit();
            this.formControlsGroupBox.ResumeLayout(false);
            this.formControlsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.blackWhiteNumericUpDown)).EndInit();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox currentImagePictureBox;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.Button runButton;
        private System.Windows.Forms.TextBox baseNeedleHeightTextBox;
        private System.Windows.Forms.Label unitsLabel;
        private System.Windows.Forms.NumericUpDown frameRateNumericUpDown;
        private System.Windows.Forms.GroupBox formControlsGroupBox;
        private System.Windows.Forms.ProgressBar runProgressBar;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Label frameRateHeaderLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label baseNeedleHeightLabel;
        private System.Windows.Forms.FolderBrowserDialog loadImagesDialog;
        private System.Windows.Forms.Label fpsLabel;
        private System.Windows.Forms.Label blackWhiteCalibrationLabel;
        private System.Windows.Forms.NumericUpDown blackWhiteNumericUpDown;
        private System.Windows.Forms.Button calibrateButton;
        private System.Windows.Forms.ToolStripMenuItem aboutUsToolStripMenuItem;
        private System.Windows.Forms.TextBox saveDestinationTextBox;
        private System.Windows.Forms.Label saveDestinationLabel;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
    }
}

