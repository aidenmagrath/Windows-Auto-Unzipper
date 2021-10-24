
using Windows_Auto_Unzipper.Properties;

namespace Windows_Auto_Unzipper
{
    partial class FormSettings
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
            this.directoryBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.btnChangeDirectory = new System.Windows.Forms.Button();
            this.labelTargetDirectory = new System.Windows.Forms.Label();
            this.labelTargetDirectoryHeader = new System.Windows.Forms.Label();
            this.toolTipTargetDirectory = new System.Windows.Forms.ToolTip(this.components);
            this.labelStartMode = new System.Windows.Forms.Label();
            this.comboBoxStartMode = new System.Windows.Forms.ComboBox();
            this.labelAutoLaunch = new System.Windows.Forms.Label();
            this.checkBoxAutoLaunch = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDone = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnChangeFolder
            // 
            this.btnChangeDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChangeDirectory.Location = new System.Drawing.Point(317, 27);
            this.btnChangeDirectory.Name = "btnChangeDirectory";
            this.btnChangeDirectory.Size = new System.Drawing.Size(75, 23);
            this.btnChangeDirectory.TabIndex = 0;
            this.btnChangeDirectory.Text = "Change";
            this.btnChangeDirectory.UseVisualStyleBackColor = true;
            this.btnChangeDirectory.Click += new System.EventHandler(this.btnChangeDirectory_Click);
            // 
            // labelTargetFolder
            // 
            this.labelTargetDirectory.AutoEllipsis = true;
            this.labelTargetDirectory.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelTargetDirectory.Location = new System.Drawing.Point(23, 31);
            this.labelTargetDirectory.Name = "labelTargetDirectory";
            this.labelTargetDirectory.Size = new System.Drawing.Size(288, 15);
            this.labelTargetDirectory.TabIndex = 1;
            // 
            // labelTargetFolderHeader
            // 
            this.labelTargetDirectoryHeader.AutoSize = true;
            this.labelTargetDirectoryHeader.Location = new System.Drawing.Point(12, 13);
            this.labelTargetDirectoryHeader.Name = "labelTargetDirectoryHeader";
            this.labelTargetDirectoryHeader.Size = new System.Drawing.Size(75, 15);
            this.labelTargetDirectoryHeader.TabIndex = 2;
            this.labelTargetDirectoryHeader.Text = "Target Folder";
            // 
            // labelStartMode
            // 
            this.labelStartMode.AutoSize = true;
            this.labelStartMode.Location = new System.Drawing.Point(12, 59);
            this.labelStartMode.Name = "labelStartMode";
            this.labelStartMode.Size = new System.Drawing.Size(65, 15);
            this.labelStartMode.TabIndex = 3;
            this.labelStartMode.Text = "Start Mode";
            // 
            // comboBoxStartMode
            // 
            this.comboBoxStartMode.ForeColor = System.Drawing.SystemColors.ControlText;
            this.comboBoxStartMode.FormattingEnabled = true;
            this.comboBoxStartMode.Items.AddRange(new object[] {
            "Running",
            "Stopped",
            "Remember from last session"});
            this.comboBoxStartMode.Location = new System.Drawing.Point(23, 77);
            this.comboBoxStartMode.Name = "comboBoxStartMode";
            this.comboBoxStartMode.Size = new System.Drawing.Size(288, 23);
            this.comboBoxStartMode.TabIndex = 4;
            // 
            // labelAutoLaunch
            // 
            this.labelAutoLaunch.AutoSize = true;
            this.labelAutoLaunch.Location = new System.Drawing.Point(12, 112);
            this.labelAutoLaunch.Name = "labelAutoLaunch";
            this.labelAutoLaunch.Size = new System.Drawing.Size(159, 15);
            this.labelAutoLaunch.TabIndex = 5;
            this.labelAutoLaunch.Text = "Launch when windows starts";
            // 
            // checkBoxAutoLaunch
            // 
            this.checkBoxAutoLaunch.AutoSize = true;
            this.checkBoxAutoLaunch.Location = new System.Drawing.Point(296, 113);
            this.checkBoxAutoLaunch.Name = "checkBoxAutoLaunch";
            this.checkBoxAutoLaunch.Size = new System.Drawing.Size(15, 14);
            this.checkBoxAutoLaunch.TabIndex = 6;
            this.checkBoxAutoLaunch.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(317, 184);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDone
            // 
            this.btnDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDone.Location = new System.Drawing.Point(235, 184);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(75, 23);
            this.btnDone.TabIndex = 8;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 221);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.checkBoxAutoLaunch);
            this.Controls.Add(this.labelAutoLaunch);
            this.Controls.Add(this.comboBoxStartMode);
            this.Controls.Add(this.labelStartMode);
            this.Controls.Add(this.labelTargetDirectoryHeader);
            this.Controls.Add(this.labelTargetDirectory);
            this.Controls.Add(this.btnChangeDirectory);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSettings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Windows Auto Unzipper";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.VisibleChanged += new System.EventHandler(this.FormSettings_VisibleChanged);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.FolderBrowserDialog directoryBrowserDialog;
        private System.Windows.Forms.Button btnChangeDirectory;
        private System.Windows.Forms.Label labelTargetDirectory;
        private System.Windows.Forms.Label labelTargetDirectoryHeader;
        private System.Windows.Forms.ToolTip toolTipTargetDirectory;
        private System.Windows.Forms.Label labelStartMode;
        private System.Windows.Forms.ComboBox comboBoxStartMode;
        private System.Windows.Forms.Label labelAutoLaunch;
        private System.Windows.Forms.CheckBox checkBoxAutoLaunch;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDone;
    }
}

