namespace OrbisTaskbarApp
{
    partial class OrbisTaskbarApp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrbisTaskbarApp));
            this.Target_ContextMenu = new DarkUI.Controls.DarkContextMenu();
            this.connectToTargetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteTargetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editTargetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addTargetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Target_ContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // Target_ContextMenu
            // 
            this.Target_ContextMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.Target_ContextMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.Target_ContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToTargetToolStripMenuItem,
            this.deleteTargetToolStripMenuItem,
            this.editTargetToolStripMenuItem,
            this.addTargetToolStripMenuItem});
            this.Target_ContextMenu.Name = "Target_ContextMenu";
            this.Target_ContextMenu.Size = new System.Drawing.Size(169, 92);
            // 
            // connectToTargetToolStripMenuItem
            // 
            this.connectToTargetToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.connectToTargetToolStripMenuItem.Name = "connectToTargetToolStripMenuItem";
            this.connectToTargetToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.connectToTargetToolStripMenuItem.Text = "Connect to Target";
            // 
            // deleteTargetToolStripMenuItem
            // 
            this.deleteTargetToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.deleteTargetToolStripMenuItem.Name = "deleteTargetToolStripMenuItem";
            this.deleteTargetToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.deleteTargetToolStripMenuItem.Text = "Delete Target";
            // 
            // editTargetToolStripMenuItem
            // 
            this.editTargetToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.editTargetToolStripMenuItem.Name = "editTargetToolStripMenuItem";
            this.editTargetToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.editTargetToolStripMenuItem.Text = "Edit Target";
            // 
            // addTargetToolStripMenuItem
            // 
            this.addTargetToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.addTargetToolStripMenuItem.Name = "addTargetToolStripMenuItem";
            this.addTargetToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.addTargetToolStripMenuItem.Text = "Add Target";
            // 
            // OrbisTaskbarApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(252, 100);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "OrbisTaskbarApp";
            this.Opacity = 0D;
            this.ShowInTaskbar = false;
            this.Text = "Orbis Taskbar App";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Target_ContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DarkUI.Controls.DarkContextMenu Target_ContextMenu;
        private System.Windows.Forms.ToolStripMenuItem connectToTargetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteTargetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editTargetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addTargetToolStripMenuItem;
    }
}

