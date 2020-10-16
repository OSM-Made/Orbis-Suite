namespace OrbisSuite.Dialog
{
    partial class AddTarget
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddTarget));
            this.panel4 = new System.Windows.Forms.Panel();
            this.darkTitle3 = new DarkUI.Controls.DarkTitle();
            this.TargetName = new DarkUI.Controls.DarkTextBox();
            this.darkTitle4 = new DarkUI.Controls.DarkTitle();
            this.panel5 = new System.Windows.Forms.Panel();
            this.TargetFW455 = new DarkUI.Controls.DarkRadioButton();
            this.TargetFW405 = new DarkUI.Controls.DarkRadioButton();
            this.TargetFW176 = new DarkUI.Controls.DarkRadioButton();
            this.TargetFW505 = new DarkUI.Controls.DarkRadioButton();
            this.TargetFW672 = new DarkUI.Controls.DarkRadioButton();
            this.TargetFW702 = new DarkUI.Controls.DarkRadioButton();
            this.Button_AddTarget = new DarkUI.Controls.DarkButton();
            this.Button_Cancel = new DarkUI.Controls.DarkButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.darkTitle2 = new DarkUI.Controls.DarkTitle();
            this.TargetIPAddress = new DarkUI.Controls.DarkTextBox();
            this.darkTitle5 = new DarkUI.Controls.DarkTitle();
            this.PayloadPort = new DarkUI.Controls.DarkTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.IsDefaultTarget = new DarkUI.Controls.DarkCheckBox();
            this.darkTitle1 = new DarkUI.Controls.DarkTitle();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.AutoSize = true;
            this.panel4.Controls.Add(this.darkTitle3);
            this.panel4.Controls.Add(this.TargetName);
            this.panel4.Location = new System.Drawing.Point(10, 12);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.panel4.Size = new System.Drawing.Size(252, 62);
            this.panel4.TabIndex = 1;
            // 
            // darkTitle3
            // 
            this.darkTitle3.Dock = System.Windows.Forms.DockStyle.Top;
            this.darkTitle3.Location = new System.Drawing.Point(0, 0);
            this.darkTitle3.Name = "darkTitle3";
            this.darkTitle3.Size = new System.Drawing.Size(252, 26);
            this.darkTitle3.TabIndex = 0;
            this.darkTitle3.Text = "Target Name";
            // 
            // TargetName
            // 
            this.TargetName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TargetName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.TargetName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TargetName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.TargetName.Location = new System.Drawing.Point(3, 29);
            this.TargetName.MaxLength = 256;
            this.TargetName.Name = "TargetName";
            this.TargetName.Size = new System.Drawing.Size(246, 20);
            this.TargetName.TabIndex = 0;
            // 
            // darkTitle4
            // 
            this.darkTitle4.Dock = System.Windows.Forms.DockStyle.Top;
            this.darkTitle4.Location = new System.Drawing.Point(0, 0);
            this.darkTitle4.Name = "darkTitle4";
            this.darkTitle4.Size = new System.Drawing.Size(252, 26);
            this.darkTitle4.TabIndex = 6;
            this.darkTitle4.Text = "Target Firmware";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.TargetFW455);
            this.panel5.Controls.Add(this.TargetFW405);
            this.panel5.Controls.Add(this.TargetFW176);
            this.panel5.Controls.Add(this.TargetFW505);
            this.panel5.Controls.Add(this.TargetFW672);
            this.panel5.Controls.Add(this.TargetFW702);
            this.panel5.Controls.Add(this.darkTitle4);
            this.panel5.Location = new System.Drawing.Point(10, 148);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(252, 66);
            this.panel5.TabIndex = 3;
            // 
            // TargetFW455
            // 
            this.TargetFW455.AutoSize = true;
            this.TargetFW455.Enabled = false;
            this.TargetFW455.Location = new System.Drawing.Point(138, 19);
            this.TargetFW455.Name = "TargetFW455";
            this.TargetFW455.Size = new System.Drawing.Size(46, 17);
            this.TargetFW455.TabIndex = 6;
            this.TargetFW455.Text = "4.55";
            // 
            // TargetFW405
            // 
            this.TargetFW405.AutoSize = true;
            this.TargetFW405.Enabled = false;
            this.TargetFW405.Location = new System.Drawing.Point(70, 19);
            this.TargetFW405.Name = "TargetFW405";
            this.TargetFW405.Size = new System.Drawing.Size(46, 17);
            this.TargetFW405.TabIndex = 5;
            this.TargetFW405.Text = "4.05";
            // 
            // TargetFW176
            // 
            this.TargetFW176.AutoSize = true;
            this.TargetFW176.Enabled = false;
            this.TargetFW176.Location = new System.Drawing.Point(3, 19);
            this.TargetFW176.Name = "TargetFW176";
            this.TargetFW176.Size = new System.Drawing.Size(46, 17);
            this.TargetFW176.TabIndex = 4;
            this.TargetFW176.Text = "1.76";
            // 
            // TargetFW505
            // 
            this.TargetFW505.AutoSize = true;
            this.TargetFW505.Checked = true;
            this.TargetFW505.Location = new System.Drawing.Point(203, 19);
            this.TargetFW505.Name = "TargetFW505";
            this.TargetFW505.Size = new System.Drawing.Size(46, 17);
            this.TargetFW505.TabIndex = 2;
            this.TargetFW505.Text = "5.05";
            // 
            // TargetFW672
            // 
            this.TargetFW672.AutoSize = true;
            this.TargetFW672.Location = new System.Drawing.Point(3, 42);
            this.TargetFW672.Name = "TargetFW672";
            this.TargetFW672.Size = new System.Drawing.Size(46, 17);
            this.TargetFW672.TabIndex = 8;
            this.TargetFW672.Text = "6.72";
            // 
            // TargetFW702
            // 
            this.TargetFW702.AutoSize = true;
            this.TargetFW702.Location = new System.Drawing.Point(70, 42);
            this.TargetFW702.Name = "TargetFW702";
            this.TargetFW702.Size = new System.Drawing.Size(46, 17);
            this.TargetFW702.TabIndex = 9;
            this.TargetFW702.Text = "7.02";
            // 
            // Button_AddTarget
            // 
            this.Button_AddTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_AddTarget.Location = new System.Drawing.Point(167, 287);
            this.Button_AddTarget.Name = "Button_AddTarget";
            this.Button_AddTarget.Padding = new System.Windows.Forms.Padding(5);
            this.Button_AddTarget.Size = new System.Drawing.Size(95, 23);
            this.Button_AddTarget.TabIndex = 5;
            this.Button_AddTarget.Text = "Add Target";
            this.Button_AddTarget.Click += new System.EventHandler(this.Button_AddTarget_Click);
            // 
            // Button_Cancel
            // 
            this.Button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Button_Cancel.Location = new System.Drawing.Point(10, 287);
            this.Button_Cancel.Name = "Button_Cancel";
            this.Button_Cancel.Padding = new System.Windows.Forms.Padding(5);
            this.Button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.Button_Cancel.TabIndex = 0;
            this.Button_Cancel.Text = "Cancel";
            this.Button_Cancel.Click += new System.EventHandler(this.Button_Cancel_Click);
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.darkTitle2);
            this.panel1.Controls.Add(this.TargetIPAddress);
            this.panel1.Location = new System.Drawing.Point(10, 80);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.panel1.Size = new System.Drawing.Size(252, 62);
            this.panel1.TabIndex = 2;
            // 
            // darkTitle2
            // 
            this.darkTitle2.Dock = System.Windows.Forms.DockStyle.Top;
            this.darkTitle2.Location = new System.Drawing.Point(0, 0);
            this.darkTitle2.Name = "darkTitle2";
            this.darkTitle2.Size = new System.Drawing.Size(252, 26);
            this.darkTitle2.TabIndex = 15;
            this.darkTitle2.Text = "Target IP address";
            // 
            // TargetIPAddress
            // 
            this.TargetIPAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TargetIPAddress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.TargetIPAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TargetIPAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.TargetIPAddress.Location = new System.Drawing.Point(3, 29);
            this.TargetIPAddress.Name = "TargetIPAddress";
            this.TargetIPAddress.Size = new System.Drawing.Size(246, 20);
            this.TargetIPAddress.TabIndex = 1;
            // 
            // darkTitle5
            // 
            this.darkTitle5.Location = new System.Drawing.Point(138, 0);
            this.darkTitle5.Name = "darkTitle5";
            this.darkTitle5.Size = new System.Drawing.Size(111, 20);
            this.darkTitle5.TabIndex = 19;
            this.darkTitle5.Text = "Payload Port";
            // 
            // PayloadPort
            // 
            this.PayloadPort.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.PayloadPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PayloadPort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.PayloadPort.Location = new System.Drawing.Point(138, 23);
            this.PayloadPort.MaxLength = 2;
            this.PayloadPort.Name = "PayloadPort";
            this.PayloadPort.Size = new System.Drawing.Size(111, 20);
            this.PayloadPort.TabIndex = 3;
            this.PayloadPort.Text = "9020";
            this.PayloadPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PayloadPort_KeyPress);
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.Controls.Add(this.PayloadPort);
            this.panel2.Controls.Add(this.darkTitle5);
            this.panel2.Controls.Add(this.IsDefaultTarget);
            this.panel2.Controls.Add(this.darkTitle1);
            this.panel2.Location = new System.Drawing.Point(10, 220);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.panel2.Size = new System.Drawing.Size(252, 56);
            this.panel2.TabIndex = 5;
            // 
            // IsDefaultTarget
            // 
            this.IsDefaultTarget.AutoSize = true;
            this.IsDefaultTarget.Location = new System.Drawing.Point(3, 24);
            this.IsDefaultTarget.Name = "IsDefaultTarget";
            this.IsDefaultTarget.Size = new System.Drawing.Size(130, 17);
            this.IsDefaultTarget.TabIndex = 2;
            this.IsDefaultTarget.Text = "Use as Default Target";
            // 
            // darkTitle1
            // 
            this.darkTitle1.Location = new System.Drawing.Point(0, 0);
            this.darkTitle1.Name = "darkTitle1";
            this.darkTitle1.Size = new System.Drawing.Size(133, 20);
            this.darkTitle1.TabIndex = 15;
            this.darkTitle1.Text = "Default Target";
            // 
            // AddTarget
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(272, 322);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.Button_AddTarget);
            this.Controls.Add(this.Button_Cancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddTarget";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Target";
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel4;
        private DarkUI.Controls.DarkTitle darkTitle3;
        private DarkUI.Controls.DarkTextBox TargetName;
        private DarkUI.Controls.DarkTitle darkTitle4;
        private System.Windows.Forms.Panel panel5;
        private DarkUI.Controls.DarkRadioButton TargetFW455;
        private DarkUI.Controls.DarkRadioButton TargetFW405;
        private DarkUI.Controls.DarkRadioButton TargetFW176;
        private DarkUI.Controls.DarkRadioButton TargetFW505;
        private DarkUI.Controls.DarkRadioButton TargetFW672;
        private DarkUI.Controls.DarkRadioButton TargetFW702;
        private DarkUI.Controls.DarkButton Button_AddTarget;
        private DarkUI.Controls.DarkButton Button_Cancel;
        private System.Windows.Forms.Panel panel1;
        private DarkUI.Controls.DarkTitle darkTitle2;
        private DarkUI.Controls.DarkTextBox TargetIPAddress;
        private DarkUI.Controls.DarkTitle darkTitle5;
        private DarkUI.Controls.DarkTextBox PayloadPort;
        private System.Windows.Forms.Panel panel2;
        private DarkUI.Controls.DarkCheckBox IsDefaultTarget;
        private DarkUI.Controls.DarkTitle darkTitle1;
    }
}