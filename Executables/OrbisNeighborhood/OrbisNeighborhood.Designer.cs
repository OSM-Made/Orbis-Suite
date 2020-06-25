namespace nsOrbisNeighborhood
{
    partial class OrbisNeighborhood
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrbisNeighborhood));
            this.CurrentTargetLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.CurrentProcLabel = new System.Windows.Forms.ToolStripLabel();
            this.Target_ContextMenu = new DarkUI.Controls.DarkContextMenu();
            this.connectToTargetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteTargetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editTargetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addTargetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.mStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mIPAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mFirmware = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mTargetName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mDefault = new System.Windows.Forms.DataGridViewImageColumn();
            this.TargetList = new System.Windows.Forms.DataGridView();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.darkStatusStrip1 = new DarkUI.Controls.DarkStatusStrip();
            this.toolStripStatusLabel6 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel7 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel8 = new System.Windows.Forms.ToolStripStatusLabel();
            this.darkToolStrip1 = new DarkUI.Controls.DarkToolStrip();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.Target_ContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TargetList)).BeginInit();
            this.darkStatusStrip1.SuspendLayout();
            this.darkToolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CurrentTargetLabel
            // 
            this.CurrentTargetLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.CurrentTargetLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.CurrentTargetLabel.Name = "CurrentTargetLabel";
            this.CurrentTargetLabel.Size = new System.Drawing.Size(67, 25);
            this.CurrentTargetLabel.Text = "Target: N/A";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripSeparator1.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 28);
            // 
            // CurrentProcLabel
            // 
            this.CurrentProcLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.CurrentProcLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.CurrentProcLabel.Name = "CurrentProcLabel";
            this.CurrentProcLabel.Size = new System.Drawing.Size(75, 25);
            this.CurrentProcLabel.Text = "Process: N/A";
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
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.StatusLabel.Size = new System.Drawing.Size(41, 17);
            this.StatusLabel.Text = "Ready";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Margin = new System.Windows.Forms.Padding(0, 0, 50, 2);
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(512, 20);
            this.toolStripStatusLabel4.Spring = true;
            this.toolStripStatusLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Margin = new System.Windows.Forms.Padding(0, 3, 1, 2);
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(64, 17);
            this.toolStripStatusLabel2.Text = "Orbis Suite";
            // 
            // mStatus
            // 
            this.mStatus.HeaderText = "Status";
            this.mStatus.Name = "mStatus";
            // 
            // mIPAddress
            // 
            this.mIPAddress.FillWeight = 110F;
            this.mIPAddress.HeaderText = "Address";
            this.mIPAddress.MinimumWidth = 110;
            this.mIPAddress.Name = "mIPAddress";
            this.mIPAddress.ReadOnly = true;
            this.mIPAddress.Width = 110;
            // 
            // mFirmware
            // 
            this.mFirmware.FillWeight = 70F;
            this.mFirmware.HeaderText = "Firmware";
            this.mFirmware.MinimumWidth = 70;
            this.mFirmware.Name = "mFirmware";
            this.mFirmware.ReadOnly = true;
            this.mFirmware.Width = 70;
            // 
            // mTargetName
            // 
            this.mTargetName.FillWeight = 211F;
            this.mTargetName.HeaderText = "Target Name";
            this.mTargetName.MinimumWidth = 211;
            this.mTargetName.Name = "mTargetName";
            this.mTargetName.ReadOnly = true;
            this.mTargetName.Width = 211;
            // 
            // mDefault
            // 
            this.mDefault.HeaderText = "Default";
            this.mDefault.MinimumWidth = 50;
            this.mDefault.Name = "mDefault";
            this.mDefault.Width = 50;
            // 
            // TargetList
            // 
            this.TargetList.AllowUserToAddRows = false;
            this.TargetList.AllowUserToDeleteRows = false;
            this.TargetList.AllowUserToResizeColumns = false;
            this.TargetList.AllowUserToResizeRows = false;
            this.TargetList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.TargetList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TargetList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.TargetList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.TargetList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TargetList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.mDefault,
            this.mTargetName,
            this.mFirmware,
            this.mIPAddress,
            this.mStatus});
            this.TargetList.ContextMenuStrip = this.Target_ContextMenu;
            this.TargetList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(62)))));
            this.TargetList.Location = new System.Drawing.Point(0, 33);
            this.TargetList.MultiSelect = false;
            this.TargetList.Name = "TargetList";
            this.TargetList.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TargetList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.TargetList.RowHeadersVisible = false;
            this.TargetList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.TargetList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.TargetList.Size = new System.Drawing.Size(544, 306);
            this.TargetList.TabIndex = 8;
            this.TargetList.Click += new System.EventHandler(this.TargetList_Click);
            this.TargetList.Enter += new System.EventHandler(this.TargetList_Enter);
            this.TargetList.Leave += new System.EventHandler(this.TargetList_Leave);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(41, 17);
            this.toolStripStatusLabel1.Text = "Ready";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Margin = new System.Windows.Forms.Padding(0, 0, 50, 2);
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(512, 20);
            this.toolStripStatusLabel3.Spring = true;
            this.toolStripStatusLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.Margin = new System.Windows.Forms.Padding(0, 3, 1, 2);
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(64, 17);
            this.toolStripStatusLabel5.Text = "Orbis Suite";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(67, 25);
            this.toolStripLabel1.Text = "Target: N/A";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripSeparator2.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 28);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(75, 25);
            this.toolStripLabel2.Text = "Process: N/A";
            // 
            // darkStatusStrip1
            // 
            this.darkStatusStrip1.AutoSize = false;
            this.darkStatusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.darkStatusStrip1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkStatusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel6,
            this.toolStripStatusLabel7,
            this.toolStripStatusLabel8});
            this.darkStatusStrip1.Location = new System.Drawing.Point(0, 242);
            this.darkStatusStrip1.Name = "darkStatusStrip1";
            this.darkStatusStrip1.Padding = new System.Windows.Forms.Padding(0, 5, 0, 3);
            this.darkStatusStrip1.Size = new System.Drawing.Size(540, 30);
            this.darkStatusStrip1.SizingGrip = false;
            this.darkStatusStrip1.TabIndex = 9;
            this.darkStatusStrip1.Text = "darkStatusStrip1";
            // 
            // toolStripStatusLabel6
            // 
            this.toolStripStatusLabel6.Name = "toolStripStatusLabel6";
            this.toolStripStatusLabel6.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.toolStripStatusLabel6.Size = new System.Drawing.Size(41, 17);
            this.toolStripStatusLabel6.Text = "Ready";
            // 
            // toolStripStatusLabel7
            // 
            this.toolStripStatusLabel7.Margin = new System.Windows.Forms.Padding(0, 0, 50, 2);
            this.toolStripStatusLabel7.Name = "toolStripStatusLabel7";
            this.toolStripStatusLabel7.Size = new System.Drawing.Size(384, 20);
            this.toolStripStatusLabel7.Spring = true;
            this.toolStripStatusLabel7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel8
            // 
            this.toolStripStatusLabel8.Margin = new System.Windows.Forms.Padding(0, 3, 1, 2);
            this.toolStripStatusLabel8.Name = "toolStripStatusLabel8";
            this.toolStripStatusLabel8.Size = new System.Drawing.Size(64, 17);
            this.toolStripStatusLabel8.Text = "Orbis Suite";
            // 
            // darkToolStrip1
            // 
            this.darkToolStrip1.AutoSize = false;
            this.darkToolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.darkToolStrip1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkToolStrip1.GripMargin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.darkToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.darkToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel3,
            this.toolStripSeparator3,
            this.toolStripLabel4});
            this.darkToolStrip1.Location = new System.Drawing.Point(0, 0);
            this.darkToolStrip1.Name = "darkToolStrip1";
            this.darkToolStrip1.Padding = new System.Windows.Forms.Padding(5, 0, 1, 2);
            this.darkToolStrip1.Size = new System.Drawing.Size(540, 30);
            this.darkToolStrip1.TabIndex = 10;
            this.darkToolStrip1.Text = "darkToolStrip1";
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(67, 25);
            this.toolStripLabel3.Text = "Target: N/A";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripSeparator3.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 28);
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(75, 25);
            this.toolStripLabel4.Text = "Process: N/A";
            // 
            // OrbisNeighborhood
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 272);
            this.Controls.Add(this.darkToolStrip1);
            this.Controls.Add(this.darkStatusStrip1);
            this.Controls.Add(this.TargetList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "OrbisNeighborhood";
            this.Text = "Orbis Neighborhood";
            this.Target_ContextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TargetList)).EndInit();
            this.darkStatusStrip1.ResumeLayout(false);
            this.darkStatusStrip1.PerformLayout();
            this.darkToolStrip1.ResumeLayout(false);
            this.darkToolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolStripLabel CurrentTargetLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel CurrentProcLabel;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private DarkUI.Controls.DarkContextMenu Target_ContextMenu;
        private System.Windows.Forms.ToolStripMenuItem connectToTargetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteTargetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editTargetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addTargetToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn mStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn mIPAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn mFirmware;
        private System.Windows.Forms.DataGridViewTextBoxColumn mTargetName;
        private System.Windows.Forms.DataGridViewImageColumn mDefault;
        private System.Windows.Forms.DataGridView TargetList;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private DarkUI.Controls.DarkStatusStrip darkStatusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel6;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel7;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel8;
        private DarkUI.Controls.DarkToolStrip darkToolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
    }
}

