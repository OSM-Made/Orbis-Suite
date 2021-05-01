namespace OrbisSuite.Dialog
{
    partial class SelectProcess
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectProcess));
            this.darkContextMenu1 = new DarkUI.Controls.DarkContextMenu();
            this.ToolStrip_DetachProcess = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.darkScrollBar1 = new DarkUI.Controls.DarkScrollBar();
            this.ProcessList = new System.Windows.Forms.DataGridView();
            this.PID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProcName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TitleID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Attached = new System.Windows.Forms.DataGridViewImageColumn();
            this.darkSectionPanel1 = new DarkUI.Controls.DarkSectionPanel();
            this.ToolStrip_KillProcess = new System.Windows.Forms.ToolStripMenuItem();
            this.darkContextMenu1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProcessList)).BeginInit();
            this.darkSectionPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // darkContextMenu1
            // 
            this.darkContextMenu1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.darkContextMenu1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkContextMenu1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStrip_DetachProcess,
            this.ToolStrip_KillProcess,
            this.refreshToolStripMenuItem});
            this.darkContextMenu1.Name = "darkContextMenu1";
            this.darkContextMenu1.Size = new System.Drawing.Size(181, 92);
            // 
            // ToolStrip_DetachProcess
            // 
            this.ToolStrip_DetachProcess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ToolStrip_DetachProcess.Name = "ToolStrip_DetachProcess";
            this.ToolStrip_DetachProcess.Size = new System.Drawing.Size(180, 22);
            this.ToolStrip_DetachProcess.Text = "Detach Process";
            this.ToolStrip_DetachProcess.Click += new System.EventHandler(this.ToolStrip_DetachProcess_Click);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // darkScrollBar1
            // 
            this.darkScrollBar1.Location = new System.Drawing.Point(394, 25);
            this.darkScrollBar1.Name = "darkScrollBar1";
            this.darkScrollBar1.Size = new System.Drawing.Size(17, 398);
            this.darkScrollBar1.TabIndex = 11;
            this.darkScrollBar1.Text = "darkScrollBar1";
            this.darkScrollBar1.ViewSize = 20;
            this.darkScrollBar1.ValueChanged += new System.EventHandler<DarkUI.Controls.ScrollValueEventArgs>(this.darkScrollBar1_ValueChanged);
            // 
            // ProcessList
            // 
            this.ProcessList.AllowUserToAddRows = false;
            this.ProcessList.AllowUserToDeleteRows = false;
            this.ProcessList.AllowUserToResizeColumns = false;
            this.ProcessList.AllowUserToResizeRows = false;
            this.ProcessList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.ProcessList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ProcessList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.ProcessList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.ProcessList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ProcessList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PID,
            this.ProcName,
            this.TitleID,
            this.Attached});
            this.ProcessList.ContextMenuStrip = this.darkContextMenu1;
            this.ProcessList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProcessList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(62)))));
            this.ProcessList.Location = new System.Drawing.Point(1, 25);
            this.ProcessList.MultiSelect = false;
            this.ProcessList.Name = "ProcessList";
            this.ProcessList.ReadOnly = true;
            this.ProcessList.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ProcessList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.ProcessList.RowHeadersVisible = false;
            this.ProcessList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.ProcessList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ProcessList.Size = new System.Drawing.Size(410, 395);
            this.ProcessList.TabIndex = 10;
            this.ProcessList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ProcessList_CellDoubleClick);
            this.ProcessList.Scroll += new System.Windows.Forms.ScrollEventHandler(this.ProcessList_Scroll);
            this.ProcessList.Enter += new System.EventHandler(this.ProcessList_Enter);
            this.ProcessList.Leave += new System.EventHandler(this.ProcessList_Leave);
            // 
            // PID
            // 
            this.PID.FillWeight = 45F;
            this.PID.HeaderText = "PID";
            this.PID.MinimumWidth = 45;
            this.PID.Name = "PID";
            this.PID.ReadOnly = true;
            this.PID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.PID.Width = 45;
            // 
            // ProcName
            // 
            this.ProcName.FillWeight = 178F;
            this.ProcName.HeaderText = "Name";
            this.ProcName.MinimumWidth = 178;
            this.ProcName.Name = "ProcName";
            this.ProcName.ReadOnly = true;
            this.ProcName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ProcName.Width = 178;
            // 
            // TitleID
            // 
            this.TitleID.FillWeight = 110F;
            this.TitleID.HeaderText = "Title ID";
            this.TitleID.MinimumWidth = 110;
            this.TitleID.Name = "TitleID";
            this.TitleID.ReadOnly = true;
            this.TitleID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.TitleID.Width = 110;
            // 
            // Attached
            // 
            this.Attached.FillWeight = 60F;
            this.Attached.HeaderText = "Attached";
            this.Attached.MinimumWidth = 60;
            this.Attached.Name = "Attached";
            this.Attached.ReadOnly = true;
            this.Attached.Width = 60;
            // 
            // darkSectionPanel1
            // 
            this.darkSectionPanel1.Controls.Add(this.darkScrollBar1);
            this.darkSectionPanel1.Controls.Add(this.ProcessList);
            this.darkSectionPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.darkSectionPanel1.Location = new System.Drawing.Point(0, 0);
            this.darkSectionPanel1.Name = "darkSectionPanel1";
            this.darkSectionPanel1.SectionHeader = null;
            this.darkSectionPanel1.Size = new System.Drawing.Size(412, 421);
            this.darkSectionPanel1.TabIndex = 12;
            // 
            // ToolStrip_KillProcess
            // 
            this.ToolStrip_KillProcess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ToolStrip_KillProcess.Name = "ToolStrip_KillProcess";
            this.ToolStrip_KillProcess.Size = new System.Drawing.Size(180, 22);
            this.ToolStrip_KillProcess.Text = "Kill Process";
            this.ToolStrip_KillProcess.Click += new System.EventHandler(this.ToolStrip_KillProcess_Click);
            // 
            // SelectProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 466);
            this.Controls.Add(this.darkSectionPanel1);
            this.DialogButtons = DarkUI.Forms.DarkDialogButton.OkCancel;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SelectProcess";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Process";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SelectProcess_FormClosing);
            this.Load += new System.EventHandler(this.SelectProcess_Load);
            this.Controls.SetChildIndex(this.darkSectionPanel1, 0);
            this.darkContextMenu1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ProcessList)).EndInit();
            this.darkSectionPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DarkUI.Controls.DarkContextMenu darkContextMenu1;
        private System.Windows.Forms.ToolStripMenuItem ToolStrip_DetachProcess;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private DarkUI.Controls.DarkScrollBar darkScrollBar1;
        private System.Windows.Forms.DataGridView ProcessList;
        private System.Windows.Forms.DataGridViewTextBoxColumn PID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProcName;
        private System.Windows.Forms.DataGridViewTextBoxColumn TitleID;
        private System.Windows.Forms.DataGridViewImageColumn Attached;
        private DarkUI.Controls.DarkSectionPanel darkSectionPanel1;
        private System.Windows.Forms.ToolStripMenuItem ToolStrip_KillProcess;
    }
}