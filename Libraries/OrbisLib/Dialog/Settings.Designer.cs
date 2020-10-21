namespace OrbisSuite.Dialog
{
    partial class Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.darkSectionPanel1 = new DarkUI.Controls.DarkSectionPanel();
            this.PromptAttach = new DarkUI.Controls.DarkCheckBox();
            this.APIPort = new DarkUI.Controls.DarkTextBox();
            this.darkTitle3 = new DarkUI.Controls.DarkTitle();
            this.darkTitle4 = new DarkUI.Controls.DarkTitle();
            this.ServicePort = new DarkUI.Controls.DarkTextBox();
            this.DefaultCOMPort = new DarkUI.Controls.DarkComboBox();
            this.darkTitle2 = new DarkUI.Controls.DarkTitle();
            this.DefaultTargetComboBox = new DarkUI.Controls.DarkComboBox();
            this.darkTitle1 = new DarkUI.Controls.DarkTitle();
            this.StartOnBoot = new DarkUI.Controls.DarkCheckBox();
            this.AutoLoadPayload = new DarkUI.Controls.DarkCheckBox();
            this.darkSectionPanel2 = new DarkUI.Controls.DarkSectionPanel();
            this.CensorPSID = new DarkUI.Controls.DarkCheckBox();
            this.CensorIDPS = new DarkUI.Controls.DarkCheckBox();
            this.darkSectionPanel3 = new DarkUI.Controls.DarkSectionPanel();
            this.OrbisLibLogs = new DarkUI.Controls.DarkCheckBox();
            this.OrbisLibDebug = new DarkUI.Controls.DarkCheckBox();
            this.darkSectionPanel4 = new DarkUI.Controls.DarkSectionPanel();
            this.WordWrap = new DarkUI.Controls.DarkCheckBox();
            this.ShowTimestamps = new DarkUI.Controls.DarkCheckBox();
            this.darkSectionPanel1.SuspendLayout();
            this.darkSectionPanel2.SuspendLayout();
            this.darkSectionPanel3.SuspendLayout();
            this.darkSectionPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // darkSectionPanel1
            // 
            this.darkSectionPanel1.Controls.Add(this.PromptAttach);
            this.darkSectionPanel1.Controls.Add(this.APIPort);
            this.darkSectionPanel1.Controls.Add(this.darkTitle3);
            this.darkSectionPanel1.Controls.Add(this.darkTitle4);
            this.darkSectionPanel1.Controls.Add(this.ServicePort);
            this.darkSectionPanel1.Controls.Add(this.DefaultCOMPort);
            this.darkSectionPanel1.Controls.Add(this.darkTitle2);
            this.darkSectionPanel1.Controls.Add(this.DefaultTargetComboBox);
            this.darkSectionPanel1.Controls.Add(this.darkTitle1);
            this.darkSectionPanel1.Controls.Add(this.StartOnBoot);
            this.darkSectionPanel1.Controls.Add(this.AutoLoadPayload);
            this.darkSectionPanel1.Location = new System.Drawing.Point(12, 12);
            this.darkSectionPanel1.Name = "darkSectionPanel1";
            this.darkSectionPanel1.SectionHeader = "General";
            this.darkSectionPanel1.Size = new System.Drawing.Size(236, 276);
            this.darkSectionPanel1.TabIndex = 2;
            // 
            // PromptAttach
            // 
            this.PromptAttach.AutoSize = true;
            this.PromptAttach.Location = new System.Drawing.Point(11, 79);
            this.PromptAttach.Name = "PromptAttach";
            this.PromptAttach.Size = new System.Drawing.Size(197, 17);
            this.PromptAttach.TabIndex = 11;
            this.PromptAttach.Text = "Prompt attach when game Detected";
            // 
            // APIPort
            // 
            this.APIPort.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.APIPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.APIPort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.APIPort.Location = new System.Drawing.Point(124, 244);
            this.APIPort.MaxLength = 5;
            this.APIPort.Name = "APIPort";
            this.APIPort.Size = new System.Drawing.Size(100, 20);
            this.APIPort.TabIndex = 10;
            this.APIPort.Text = "6900";
            this.APIPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.APIPort_KeyPress);
            // 
            // darkTitle3
            // 
            this.darkTitle3.Location = new System.Drawing.Point(125, 221);
            this.darkTitle3.Name = "darkTitle3";
            this.darkTitle3.Size = new System.Drawing.Size(107, 20);
            this.darkTitle3.TabIndex = 9;
            this.darkTitle3.Text = "API Port";
            // 
            // darkTitle4
            // 
            this.darkTitle4.Location = new System.Drawing.Point(8, 221);
            this.darkTitle4.Name = "darkTitle4";
            this.darkTitle4.Size = new System.Drawing.Size(107, 20);
            this.darkTitle4.TabIndex = 8;
            this.darkTitle4.Text = "Service Port";
            // 
            // ServicePort
            // 
            this.ServicePort.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.ServicePort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ServicePort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ServicePort.Location = new System.Drawing.Point(11, 244);
            this.ServicePort.MaxLength = 5;
            this.ServicePort.Name = "ServicePort";
            this.ServicePort.Size = new System.Drawing.Size(104, 20);
            this.ServicePort.TabIndex = 7;
            this.ServicePort.Text = "6901";
            this.ServicePort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ServicePort_KeyPress);
            // 
            // DefaultCOMPort
            // 
            this.DefaultCOMPort.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.DefaultCOMPort.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(92)))), ((int)(((byte)(92)))));
            this.DefaultCOMPort.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.DefaultCOMPort.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(92)))), ((int)(((byte)(92)))));
            this.DefaultCOMPort.ForeColor = System.Drawing.Color.Gainsboro;
            this.DefaultCOMPort.FormattingEnabled = true;
            this.DefaultCOMPort.Location = new System.Drawing.Point(11, 187);
            this.DefaultCOMPort.Name = "DefaultCOMPort";
            this.DefaultCOMPort.Size = new System.Drawing.Size(213, 21);
            this.DefaultCOMPort.TabIndex = 5;
            // 
            // darkTitle2
            // 
            this.darkTitle2.Location = new System.Drawing.Point(4, 164);
            this.darkTitle2.Name = "darkTitle2";
            this.darkTitle2.Size = new System.Drawing.Size(228, 20);
            this.darkTitle2.TabIndex = 4;
            this.darkTitle2.Text = "Default COM Port";
            // 
            // DefaultTargetComboBox
            // 
            this.DefaultTargetComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.DefaultTargetComboBox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(92)))), ((int)(((byte)(92)))));
            this.DefaultTargetComboBox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.DefaultTargetComboBox.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(92)))), ((int)(((byte)(92)))));
            this.DefaultTargetComboBox.ForeColor = System.Drawing.Color.Gainsboro;
            this.DefaultTargetComboBox.FormattingEnabled = true;
            this.DefaultTargetComboBox.Location = new System.Drawing.Point(11, 130);
            this.DefaultTargetComboBox.Name = "DefaultTargetComboBox";
            this.DefaultTargetComboBox.Size = new System.Drawing.Size(213, 21);
            this.DefaultTargetComboBox.TabIndex = 3;
            // 
            // darkTitle1
            // 
            this.darkTitle1.Location = new System.Drawing.Point(4, 107);
            this.darkTitle1.Name = "darkTitle1";
            this.darkTitle1.Size = new System.Drawing.Size(228, 20);
            this.darkTitle1.TabIndex = 2;
            this.darkTitle1.Text = "Default Target";
            // 
            // StartOnBoot
            // 
            this.StartOnBoot.AutoSize = true;
            this.StartOnBoot.Location = new System.Drawing.Point(11, 56);
            this.StartOnBoot.Name = "StartOnBoot";
            this.StartOnBoot.Size = new System.Drawing.Size(142, 17);
            this.StartOnBoot.TabIndex = 1;
            this.StartOnBoot.Text = "Start Orbis Suite on Boot";
            // 
            // AutoLoadPayload
            // 
            this.AutoLoadPayload.AutoSize = true;
            this.AutoLoadPayload.Location = new System.Drawing.Point(11, 33);
            this.AutoLoadPayload.Name = "AutoLoadPayload";
            this.AutoLoadPayload.Size = new System.Drawing.Size(112, 17);
            this.AutoLoadPayload.TabIndex = 0;
            this.AutoLoadPayload.Text = "Auto load Payload";
            // 
            // darkSectionPanel2
            // 
            this.darkSectionPanel2.Controls.Add(this.CensorPSID);
            this.darkSectionPanel2.Controls.Add(this.CensorIDPS);
            this.darkSectionPanel2.Location = new System.Drawing.Point(254, 12);
            this.darkSectionPanel2.Name = "darkSectionPanel2";
            this.darkSectionPanel2.SectionHeader = "Neighborhood";
            this.darkSectionPanel2.Size = new System.Drawing.Size(182, 88);
            this.darkSectionPanel2.TabIndex = 3;
            // 
            // CensorPSID
            // 
            this.CensorPSID.AutoSize = true;
            this.CensorPSID.Location = new System.Drawing.Point(12, 56);
            this.CensorPSID.Name = "CensorPSID";
            this.CensorPSID.Size = new System.Drawing.Size(87, 17);
            this.CensorPSID.TabIndex = 2;
            this.CensorPSID.Text = "Censor PSID";
            // 
            // CensorIDPS
            // 
            this.CensorIDPS.AutoSize = true;
            this.CensorIDPS.Location = new System.Drawing.Point(12, 33);
            this.CensorIDPS.Name = "CensorIDPS";
            this.CensorIDPS.Size = new System.Drawing.Size(87, 17);
            this.CensorIDPS.TabIndex = 1;
            this.CensorIDPS.Text = "Censor IDPS";
            // 
            // darkSectionPanel3
            // 
            this.darkSectionPanel3.Controls.Add(this.OrbisLibLogs);
            this.darkSectionPanel3.Controls.Add(this.OrbisLibDebug);
            this.darkSectionPanel3.Location = new System.Drawing.Point(254, 106);
            this.darkSectionPanel3.Name = "darkSectionPanel3";
            this.darkSectionPanel3.SectionHeader = "Debugging";
            this.darkSectionPanel3.Size = new System.Drawing.Size(182, 88);
            this.darkSectionPanel3.TabIndex = 4;
            // 
            // OrbisLibLogs
            // 
            this.OrbisLibLogs.AutoSize = true;
            this.OrbisLibLogs.Location = new System.Drawing.Point(12, 57);
            this.OrbisLibLogs.Name = "OrbisLibLogs";
            this.OrbisLibLogs.Size = new System.Drawing.Size(118, 17);
            this.OrbisLibLogs.TabIndex = 4;
            this.OrbisLibLogs.Text = "Create Debug Logs";
            // 
            // OrbisLibDebug
            // 
            this.OrbisLibDebug.AutoSize = true;
            this.OrbisLibDebug.Location = new System.Drawing.Point(12, 34);
            this.OrbisLibDebug.Name = "OrbisLibDebug";
            this.OrbisLibDebug.Size = new System.Drawing.Size(99, 17);
            this.OrbisLibDebug.TabIndex = 3;
            this.OrbisLibDebug.Text = "OrbisLib Debug";
            // 
            // darkSectionPanel4
            // 
            this.darkSectionPanel4.Controls.Add(this.WordWrap);
            this.darkSectionPanel4.Controls.Add(this.ShowTimestamps);
            this.darkSectionPanel4.Location = new System.Drawing.Point(254, 200);
            this.darkSectionPanel4.Name = "darkSectionPanel4";
            this.darkSectionPanel4.SectionHeader = "Console Output";
            this.darkSectionPanel4.Size = new System.Drawing.Size(182, 88);
            this.darkSectionPanel4.TabIndex = 5;
            // 
            // WordWrap
            // 
            this.WordWrap.AutoSize = true;
            this.WordWrap.Location = new System.Drawing.Point(12, 56);
            this.WordWrap.Name = "WordWrap";
            this.WordWrap.Size = new System.Drawing.Size(81, 17);
            this.WordWrap.TabIndex = 2;
            this.WordWrap.Text = "Word Wrap";
            // 
            // ShowTimestamps
            // 
            this.ShowTimestamps.AutoSize = true;
            this.ShowTimestamps.Location = new System.Drawing.Point(12, 33);
            this.ShowTimestamps.Name = "ShowTimestamps";
            this.ShowTimestamps.Size = new System.Drawing.Size(112, 17);
            this.ShowTimestamps.TabIndex = 1;
            this.ShowTimestamps.Text = "Show Timestamps";
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 336);
            this.Controls.Add(this.darkSectionPanel4);
            this.Controls.Add(this.darkSectionPanel3);
            this.Controls.Add(this.darkSectionPanel2);
            this.Controls.Add(this.darkSectionPanel1);
            this.DialogButtons = DarkUI.Forms.DarkDialogButton.OkCancel;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Settings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Orbis Suite Settings";
            this.Controls.SetChildIndex(this.darkSectionPanel1, 0);
            this.Controls.SetChildIndex(this.darkSectionPanel2, 0);
            this.Controls.SetChildIndex(this.darkSectionPanel3, 0);
            this.Controls.SetChildIndex(this.darkSectionPanel4, 0);
            this.darkSectionPanel1.ResumeLayout(false);
            this.darkSectionPanel1.PerformLayout();
            this.darkSectionPanel2.ResumeLayout(false);
            this.darkSectionPanel2.PerformLayout();
            this.darkSectionPanel3.ResumeLayout(false);
            this.darkSectionPanel3.PerformLayout();
            this.darkSectionPanel4.ResumeLayout(false);
            this.darkSectionPanel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DarkUI.Controls.DarkSectionPanel darkSectionPanel1;
        private DarkUI.Controls.DarkTitle darkTitle1;
        private DarkUI.Controls.DarkCheckBox StartOnBoot;
        private DarkUI.Controls.DarkCheckBox AutoLoadPayload;
        private DarkUI.Controls.DarkSectionPanel darkSectionPanel2;
        private DarkUI.Controls.DarkCheckBox CensorIDPS;
        private DarkUI.Controls.DarkComboBox DefaultTargetComboBox;
        private DarkUI.Controls.DarkCheckBox CensorPSID;
        private DarkUI.Controls.DarkTextBox APIPort;
        private DarkUI.Controls.DarkTitle darkTitle3;
        private DarkUI.Controls.DarkTitle darkTitle4;
        private DarkUI.Controls.DarkTextBox ServicePort;
        private DarkUI.Controls.DarkComboBox DefaultCOMPort;
        private DarkUI.Controls.DarkTitle darkTitle2;
        private DarkUI.Controls.DarkSectionPanel darkSectionPanel3;
        private DarkUI.Controls.DarkCheckBox OrbisLibLogs;
        private DarkUI.Controls.DarkCheckBox OrbisLibDebug;
        private DarkUI.Controls.DarkSectionPanel darkSectionPanel4;
        private DarkUI.Controls.DarkCheckBox WordWrap;
        private DarkUI.Controls.DarkCheckBox ShowTimestamps;
        private DarkUI.Controls.DarkCheckBox PromptAttach;
    }
}