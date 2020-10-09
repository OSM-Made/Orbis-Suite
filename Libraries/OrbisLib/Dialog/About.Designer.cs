namespace OrbisSuite.Dialog
{
    partial class About
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
            this.blVersion = new DarkUI.Controls.DarkLabel();
            this.darkLabel3 = new DarkUI.Controls.DarkLabel();
            this.darkLabel2 = new DarkUI.Controls.DarkLabel();
            this.darkLabel1 = new DarkUI.Controls.DarkLabel();
            this.lblHeader = new DarkUI.Controls.DarkLabel();
            this.SuspendLayout();
            // 
            // blVersion
            // 
            this.blVersion.Dock = System.Windows.Forms.DockStyle.Top;
            this.blVersion.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.blVersion.Location = new System.Drawing.Point(0, 177);
            this.blVersion.Name = "blVersion";
            this.blVersion.Size = new System.Drawing.Size(343, 36);
            this.blVersion.TabIndex = 17;
            this.blVersion.Text = "Version: [version]";
            this.blVersion.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // darkLabel3
            // 
            this.darkLabel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.darkLabel3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.darkLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel3.Location = new System.Drawing.Point(0, 137);
            this.darkLabel3.Name = "darkLabel3";
            this.darkLabel3.Size = new System.Drawing.Size(343, 40);
            this.darkLabel3.TabIndex = 16;
            this.darkLabel3.Text = "Special thanks to Matrox, John V, Fub, Sabotage, Krazaki, Nigel, Reap and my Cat." +
    "";
            this.darkLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // darkLabel2
            // 
            this.darkLabel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.darkLabel2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.darkLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel2.Location = new System.Drawing.Point(0, 86);
            this.darkLabel2.Name = "darkLabel2";
            this.darkLabel2.Size = new System.Drawing.Size(343, 51);
            this.darkLabel2.TabIndex = 15;
            this.darkLabel2.Text = "Developed by OSM with the help of a few key people and the internet.";
            this.darkLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // darkLabel1
            // 
            this.darkLabel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.darkLabel1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.darkLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel1.Location = new System.Drawing.Point(0, 32);
            this.darkLabel1.Name = "darkLabel1";
            this.darkLabel1.Size = new System.Drawing.Size(343, 54);
            this.darkLabel1.TabIndex = 14;
            this.darkLabel1.Text = "A Suite of programs to aid in Development and General Modification for Playstatio" +
    "n 4.";
            this.darkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblHeader
            // 
            this.lblHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lblHeader.Location = new System.Drawing.Point(0, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(343, 32);
            this.lblHeader.TabIndex = 13;
            this.lblHeader.Text = "Orbis Suite";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 264);
            this.Controls.Add(this.blVersion);
            this.Controls.Add(this.darkLabel3);
            this.Controls.Add(this.darkLabel2);
            this.Controls.Add(this.darkLabel1);
            this.Controls.Add(this.lblHeader);
            this.DialogButtons = DarkUI.Forms.DarkDialogButton.Close;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "About";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            this.Controls.SetChildIndex(this.lblHeader, 0);
            this.Controls.SetChildIndex(this.darkLabel1, 0);
            this.Controls.SetChildIndex(this.darkLabel2, 0);
            this.Controls.SetChildIndex(this.darkLabel3, 0);
            this.Controls.SetChildIndex(this.blVersion, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private DarkUI.Controls.DarkLabel blVersion;
        private DarkUI.Controls.DarkLabel darkLabel3;
        private DarkUI.Controls.DarkLabel darkLabel2;
        private DarkUI.Controls.DarkLabel darkLabel1;
        private DarkUI.Controls.DarkLabel lblHeader;
    }
}