namespace rm2.UI
{
    partial class OptionsUI
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtMaxNoPeers = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkScrlLogRM = new System.Windows.Forms.CheckBox();
            this.chkScrlGLog = new System.Windows.Forms.CheckBox();
            this.chkShowBallon = new System.Windows.Forms.CheckBox();
            this.grpbxSkin = new System.Windows.Forms.GroupBox();
            this.btnChangeSkin = new System.Windows.Forms.Button();
            this.lblSkinPath = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.RM2SettingsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.chkChkVer = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.grpbxSkin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RM2SettingsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnApply);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.grpbxSkin);
            this.groupBox1.Location = new System.Drawing.Point(2, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(528, 310);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "User Interface Related";
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(420, 281);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(95, 23);
            this.btnApply.TabIndex = 7;
            this.btnApply.Text = "Apply Settings";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chkChkVer);
            this.groupBox4.Controls.Add(this.txtMaxNoPeers);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Location = new System.Drawing.Point(6, 185);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(509, 90);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Global Settings";
            // 
            // txtMaxNoPeers
            // 
            this.txtMaxNoPeers.Location = new System.Drawing.Point(454, 17);
            this.txtMaxNoPeers.Name = "txtMaxNoPeers";
            this.txtMaxNoPeers.Size = new System.Drawing.Size(49, 20);
            this.txtMaxNoPeers.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(409, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Max number of peers(received from the tracker) to show in the log window of a tor" +
                "rent";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkScrlLogRM);
            this.groupBox3.Controls.Add(this.chkScrlGLog);
            this.groupBox3.Controls.Add(this.chkShowBallon);
            this.groupBox3.Location = new System.Drawing.Point(6, 87);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(509, 91);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Genaral";
            // 
            // chkScrlLogRM
            // 
            this.chkScrlLogRM.AutoSize = true;
            this.chkScrlLogRM.Checked = true;
            this.chkScrlLogRM.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkScrlLogRM.Location = new System.Drawing.Point(9, 64);
            this.chkScrlLogRM.Name = "chkScrlLogRM";
            this.chkScrlLogRM.Size = new System.Drawing.Size(196, 17);
            this.chkScrlLogRM.TabIndex = 6;
            this.chkScrlLogRM.Text = "Scroll the Torrent Log View Window";
            this.chkScrlLogRM.UseVisualStyleBackColor = true;
            // 
            // chkScrlGLog
            // 
            this.chkScrlGLog.AutoSize = true;
            this.chkScrlGLog.Checked = true;
            this.chkScrlGLog.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkScrlGLog.Location = new System.Drawing.Point(9, 42);
            this.chkScrlGLog.Name = "chkScrlGLog";
            this.chkScrlGLog.Size = new System.Drawing.Size(199, 17);
            this.chkScrlGLog.TabIndex = 5;
            this.chkScrlGLog.Text = "Scroll the Genaral Log View Window";
            this.chkScrlGLog.UseVisualStyleBackColor = true;
            // 
            // chkShowBallon
            // 
            this.chkShowBallon.AutoSize = true;
            this.chkShowBallon.Location = new System.Drawing.Point(9, 19);
            this.chkShowBallon.Name = "chkShowBallon";
            this.chkShowBallon.Size = new System.Drawing.Size(297, 17);
            this.chkShowBallon.TabIndex = 4;
            this.chkShowBallon.Text = "Show ToolTip Ballon in the System Tray(near the clock...)";
            this.chkShowBallon.UseVisualStyleBackColor = true;
            // 
            // grpbxSkin
            // 
            this.grpbxSkin.Controls.Add(this.btnChangeSkin);
            this.grpbxSkin.Controls.Add(this.lblSkinPath);
            this.grpbxSkin.Controls.Add(this.label1);
            this.grpbxSkin.Location = new System.Drawing.Point(6, 19);
            this.grpbxSkin.Name = "grpbxSkin";
            this.grpbxSkin.Size = new System.Drawing.Size(509, 62);
            this.grpbxSkin.TabIndex = 4;
            this.grpbxSkin.TabStop = false;
            this.grpbxSkin.Text = "Skin Options";
            // 
            // btnChangeSkin
            // 
            this.btnChangeSkin.Location = new System.Drawing.Point(416, 31);
            this.btnChangeSkin.Name = "btnChangeSkin";
            this.btnChangeSkin.Size = new System.Drawing.Size(87, 23);
            this.btnChangeSkin.TabIndex = 5;
            this.btnChangeSkin.Text = "Change Skin";
            this.btnChangeSkin.UseVisualStyleBackColor = true;
            this.btnChangeSkin.Click += new System.EventHandler(this.btnChangeSkin_Click);
            // 
            // lblSkinPath
            // 
            this.lblSkinPath.AutoSize = true;
            this.lblSkinPath.Location = new System.Drawing.Point(89, 16);
            this.lblSkinPath.Name = "lblSkinPath";
            this.lblSkinPath.Size = new System.Drawing.Size(84, 13);
            this.lblSkinPath.TabIndex = 4;
            this.lblSkinPath.Text = "no skin selected";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Skin Path";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "skf";
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "SkinCrafter files (*.skf)|*.skf|All files (*.*)|*.*";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // chkChkVer
            // 
            this.chkChkVer.AutoSize = true;
            this.chkChkVer.Checked = true;
            this.chkChkVer.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkChkVer.Location = new System.Drawing.Point(12, 52);
            this.chkChkVer.Name = "chkChkVer";
            this.chkChkVer.Size = new System.Drawing.Size(192, 17);
            this.chkChkVer.TabIndex = 7;
            this.chkChkVer.Text = "Check For New Version On Startup";
            this.chkChkVer.UseVisualStyleBackColor = true;
            // 
            // clsOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 334);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "clsOptions";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.groupBox1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.grpbxSkin.ResumeLayout(false);
            this.grpbxSkin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RM2SettingsBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox grpbxSkin;
        private System.Windows.Forms.Button btnChangeSkin;
        private System.Windows.Forms.Label lblSkinPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkShowBallon;
        private System.Windows.Forms.CheckBox chkScrlGLog;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkScrlLogRM;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.TextBox txtMaxNoPeers;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.BindingSource RM2SettingsBindingSource;
        private System.Windows.Forms.CheckBox chkChkVer;
    }
}