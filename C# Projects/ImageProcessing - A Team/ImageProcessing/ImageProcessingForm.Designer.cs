namespace ImageProcessing
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
            this.frameRateHeaderLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.baseNeedleHeightLabel = new System.Windows.Forms.Label();
            this.pauseButton = new System.Windows.Forms.Button();
            this.runProgressBar = new System.Windows.Forms.ProgressBar();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadImagesDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.blackWhiteNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.calibrateButton = new System.Windows.Forms.Button();
            this.blackWhiteCalibrationLabel = new System.Windows.Forms.Label();
            this.fpsLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.currentImagePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.frameRateNumericUpDown)).BeginInit();
            this.formControlsGroupBox.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.blackWhiteNumericUpDown)).BeginInit();
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
            this.statusLabel.Size = new System.Drawing.Size(98, 13);
            this.statusLabel.TabIndex = 1;
            this.statusLabel.Text = "Processed: 20/500";
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(14, 40);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(75, 36);
            this.loadButton.TabIndex = 2;
            this.loadButton.Text = "Load";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // runButton
            // 
            this.runButton.Location = new System.Drawing.Point(478, 40);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(77, 36);
            this.runButton.TabIndex = 3;
            this.runButton.Text = "Run";
            this.runButton.UseVisualStyleBackColor = true;
            // 
            // baseNeedleHeightTextBox
            // 
            this.baseNeedleHeightTextBox.Location = new System.Drawing.Point(238, 52);
            this.baseNeedleHeightTextBox.Name = "baseNeedleHeightTextBox";
            this.baseNeedleHeightTextBox.Size = new System.Drawing.Size(55, 20);
            this.baseNeedleHeightTextBox.TabIndex = 5;
            // 
            // unitsLabel
            // 
            this.unitsLabel.AutoSize = true;
            this.unitsLabel.Location = new System.Drawing.Point(297, 59);
            this.unitsLabel.Name = "unitsLabel";
            this.unitsLabel.Size = new System.Drawing.Size(21, 13);
            this.unitsLabel.TabIndex = 6;
            this.unitsLabel.Text = "cm";
            // 
            // frameRateNumericUpDown
            // 
            this.frameRateNumericUpDown.Location = new System.Drawing.Point(103, 52);
            this.frameRateNumericUpDown.Name = "frameRateNumericUpDown";
            this.frameRateNumericUpDown.Size = new System.Drawing.Size(82, 20);
            this.frameRateNumericUpDown.TabIndex = 7;
            // 
            // formControlsGroupBox
            // 
            this.formControlsGroupBox.Controls.Add(this.fpsLabel);
            this.formControlsGroupBox.Controls.Add(this.blackWhiteCalibrationLabel);
            this.formControlsGroupBox.Controls.Add(this.blackWhiteNumericUpDown);
            this.formControlsGroupBox.Controls.Add(this.frameRateHeaderLabel);
            this.formControlsGroupBox.Controls.Add(this.calibrateButton);
            this.formControlsGroupBox.Controls.Add(this.label2);
            this.formControlsGroupBox.Controls.Add(this.baseNeedleHeightLabel);
            this.formControlsGroupBox.Controls.Add(this.pauseButton);
            this.formControlsGroupBox.Controls.Add(this.loadButton);
            this.formControlsGroupBox.Controls.Add(this.runButton);
            this.formControlsGroupBox.Controls.Add(this.frameRateNumericUpDown);
            this.formControlsGroupBox.Controls.Add(this.unitsLabel);
            this.formControlsGroupBox.Controls.Add(this.baseNeedleHeightTextBox);
            this.formControlsGroupBox.Location = new System.Drawing.Point(22, 474);
            this.formControlsGroupBox.Name = "formControlsGroupBox";
            this.formControlsGroupBox.Size = new System.Drawing.Size(607, 100);
            this.formControlsGroupBox.TabIndex = 8;
            this.formControlsGroupBox.TabStop = false;
            this.formControlsGroupBox.Text = "Controls";
            // 
            // frameRateHeaderLabel
            // 
            this.frameRateHeaderLabel.AutoSize = true;
            this.frameRateHeaderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.frameRateHeaderLabel.Location = new System.Drawing.Point(120, 30);
            this.frameRateHeaderLabel.Name = "frameRateHeaderLabel";
            this.frameRateHeaderLabel.Size = new System.Drawing.Size(62, 13);
            this.frameRateHeaderLabel.TabIndex = 15;
            this.frameRateHeaderLabel.Text = "Frame Rate";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(142, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 13;
            // 
            // baseNeedleHeightLabel
            // 
            this.baseNeedleHeightLabel.AutoSize = true;
            this.baseNeedleHeightLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseNeedleHeightLabel.Location = new System.Drawing.Point(221, 30);
            this.baseNeedleHeightLabel.Name = "baseNeedleHeightLabel";
            this.baseNeedleHeightLabel.Size = new System.Drawing.Size(104, 13);
            this.baseNeedleHeightLabel.TabIndex = 12;
            this.baseNeedleHeightLabel.Text = "Base/Needle Height";
            // 
            // pauseButton
            // 
            this.pauseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pauseButton.Location = new System.Drawing.Point(561, 47);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(34, 23);
            this.pauseButton.TabIndex = 11;
            this.pauseButton.Text = "| |";
            this.pauseButton.UseVisualStyleBackColor = true;
            // 
            // runProgressBar
            // 
            this.runProgressBar.Location = new System.Drawing.Point(190, 434);
            this.runProgressBar.Name = "runProgressBar";
            this.runProgressBar.Size = new System.Drawing.Size(395, 23);
            this.runProgressBar.TabIndex = 9;
            this.runProgressBar.Value = 4;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.imageToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(665, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
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
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.quitToolStripMenuItem.Text = "Quit";
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
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(95, 22);
            this.runToolStripMenuItem.Text = "Run";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // blackWhiteNumericUpDown
            // 
            this.blackWhiteNumericUpDown.Location = new System.Drawing.Point(346, 52);
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
            // calibrateButton
            // 
            this.calibrateButton.Location = new System.Drawing.Point(399, 51);
            this.calibrateButton.Name = "calibrateButton";
            this.calibrateButton.Size = new System.Drawing.Size(65, 23);
            this.calibrateButton.TabIndex = 14;
            this.calibrateButton.Text = "Calibrate";
            this.calibrateButton.UseVisualStyleBackColor = true;
            // 
            // blackWhiteCalibrationLabel
            // 
            this.blackWhiteCalibrationLabel.AutoSize = true;
            this.blackWhiteCalibrationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blackWhiteCalibrationLabel.Location = new System.Drawing.Point(345, 30);
            this.blackWhiteCalibrationLabel.Name = "blackWhiteCalibrationLabel";
            this.blackWhiteCalibrationLabel.Size = new System.Drawing.Size(119, 13);
            this.blackWhiteCalibrationLabel.TabIndex = 16;
            this.blackWhiteCalibrationLabel.Text = "Black/White Calibration";
            // 
            // fpsLabel
            // 
            this.fpsLabel.AutoSize = true;
            this.fpsLabel.Location = new System.Drawing.Point(187, 56);
            this.fpsLabel.Name = "fpsLabel";
            this.fpsLabel.Size = new System.Drawing.Size(27, 13);
            this.fpsLabel.TabIndex = 17;
            this.fpsLabel.Text = "FPS";
            // 
            // ImageProcessingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 586);
            this.Controls.Add(this.runProgressBar);
            this.Controls.Add(this.formControlsGroupBox);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.currentImagePictureBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ImageProcessingForm";
            this.Text = "Image Processing";
            ((System.ComponentModel.ISupportInitialize)(this.currentImagePictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.frameRateNumericUpDown)).EndInit();
            this.formControlsGroupBox.ResumeLayout(false);
            this.formControlsGroupBox.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.blackWhiteNumericUpDown)).EndInit();
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
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Button pauseButton;
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
    }
}

