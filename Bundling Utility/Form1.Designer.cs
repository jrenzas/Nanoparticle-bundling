namespace Bundling_Utility
{
    partial class mainForm
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
            this.btnProcess = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.lblCoverage = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.lblImageSize = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnThreshold = new System.Windows.Forms.Button();
            this.numericUpDownThreshold = new System.Windows.Forms.NumericUpDown();
            this.btnOriginal = new System.Windows.Forms.Button();
            this.lblThreshold = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblProgress = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownThreshold)).BeginInit();
            this.SuspendLayout();
            // 
            // btnProcess
            // 
            this.btnProcess.Enabled = false;
            this.btnProcess.Location = new System.Drawing.Point(46, 132);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(113, 25);
            this.btnProcess.TabIndex = 0;
            this.btnProcess.Text = "Process Image";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 19);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(348, 383);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Location = new System.Drawing.Point(55, 19);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(92, 22);
            this.btnOpenFile.TabIndex = 3;
            this.btnOpenFile.Text = "Load Image File";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // lblCoverage
            // 
            this.lblCoverage.AutoSize = true;
            this.lblCoverage.Location = new System.Drawing.Point(43, 79);
            this.lblCoverage.Name = "lblCoverage";
            this.lblCoverage.Size = new System.Drawing.Size(70, 13);
            this.lblCoverage.TabIndex = 6;
            this.lblCoverage.Text = "Coverage: -%";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxName);
            this.groupBox1.Controls.Add(this.btnOpenFile);
            this.groupBox1.Controls.Add(this.btnProcess);
            this.groupBox1.Controls.Add(this.lblImageSize);
            this.groupBox1.Controls.Add(this.lblCoverage);
            this.groupBox1.Location = new System.Drawing.Point(413, 104);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(212, 165);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Controls";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Sample Name";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(96, 47);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(100, 20);
            this.textBoxName.TabIndex = 9;
            // 
            // lblImageSize
            // 
            this.lblImageSize.AutoSize = true;
            this.lblImageSize.Location = new System.Drawing.Point(6, 103);
            this.lblImageSize.Name = "lblImageSize";
            this.lblImageSize.Size = new System.Drawing.Size(116, 13);
            this.lblImageSize.TabIndex = 7;
            this.lblImageSize.Text = "Image Size: - nm x - nm";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.btnThreshold);
            this.groupBox2.Controls.Add(this.numericUpDownThreshold);
            this.groupBox2.Controls.Add(this.btnOriginal);
            this.groupBox2.Controls.Add(this.lblThreshold);
            this.groupBox2.Controls.Add(this.pictureBox1);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(379, 462);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Image for Processing";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(224, 415);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Lower Value = More Yellow";
            // 
            // btnThreshold
            // 
            this.btnThreshold.Location = new System.Drawing.Point(12, 434);
            this.btnThreshold.Name = "btnThreshold";
            this.btnThreshold.Size = new System.Drawing.Size(75, 23);
            this.btnThreshold.TabIndex = 13;
            this.btnThreshold.Text = "Threshold";
            this.btnThreshold.UseVisualStyleBackColor = true;
            this.btnThreshold.Click += new System.EventHandler(this.btnThreshold_Click);
            // 
            // numericUpDownThreshold
            // 
            this.numericUpDownThreshold.Location = new System.Drawing.Point(99, 413);
            this.numericUpDownThreshold.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownThreshold.Name = "numericUpDownThreshold";
            this.numericUpDownThreshold.Size = new System.Drawing.Size(51, 20);
            this.numericUpDownThreshold.TabIndex = 5;
            this.numericUpDownThreshold.Value = new decimal(new int[] {
            150,
            0,
            0,
            0});
            // 
            // btnOriginal
            // 
            this.btnOriginal.Location = new System.Drawing.Point(280, 434);
            this.btnOriginal.Name = "btnOriginal";
            this.btnOriginal.Size = new System.Drawing.Size(80, 23);
            this.btnOriginal.TabIndex = 4;
            this.btnOriginal.Text = "View Original";
            this.btnOriginal.UseVisualStyleBackColor = true;
            this.btnOriginal.Click += new System.EventHandler(this.btnOriginal_Click);
            // 
            // lblThreshold
            // 
            this.lblThreshold.AutoSize = true;
            this.lblThreshold.Location = new System.Drawing.Point(9, 415);
            this.lblThreshold.Name = "lblThreshold";
            this.lblThreshold.Size = new System.Drawing.Size(84, 13);
            this.lblThreshold.TabIndex = 3;
            this.lblThreshold.Text = "Threshold Value";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(413, 446);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(209, 23);
            this.progressBar1.TabIndex = 11;
            this.progressBar1.Visible = false;
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(413, 427);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(62, 13);
            this.lblProgress.TabIndex = 12;
            this.lblProgress.Text = "Processing:";
            this.lblProgress.Visible = false;
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 494);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "mainForm";
            this.Text = "Bundling Measurement Utility - Russ Renzas, 2014";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownThreshold)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Label lblCoverage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblThreshold;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Button btnOriginal;
        private System.Windows.Forms.Label lblImageSize;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Button btnThreshold;
        private System.Windows.Forms.NumericUpDown numericUpDownThreshold;
        private System.Windows.Forms.Label label1;
    }
}

