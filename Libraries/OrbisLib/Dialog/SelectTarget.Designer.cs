namespace OrbisSuite
{
    partial class SelectTarget
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectTarget));
            this.darkScrollBar1 = new DarkUI.Controls.DarkScrollBar();
            this.TargetList = new System.Windows.Forms.DataGridView();
            this.Default = new System.Windows.Forms.DataGridViewImageColumn();
            this.TargetName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Firmware = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.TargetList)).BeginInit();
            this.SuspendLayout();
            // 
            // darkScrollBar1
            // 
            this.darkScrollBar1.Location = new System.Drawing.Point(543, 0);
            this.darkScrollBar1.Name = "darkScrollBar1";
            this.darkScrollBar1.Size = new System.Drawing.Size(17, 398);
            this.darkScrollBar1.TabIndex = 13;
            this.darkScrollBar1.Text = "darkScrollBar1";
            this.darkScrollBar1.ViewSize = 20;
            this.darkScrollBar1.ValueChanged += new System.EventHandler<DarkUI.Controls.ScrollValueEventArgs>(this.darkScrollBar1_ValueChanged);
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
            this.Default,
            this.TargetName,
            this.Firmware,
            this.Address,
            this.Status});
            this.TargetList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TargetList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(62)))));
            this.TargetList.Location = new System.Drawing.Point(0, 0);
            this.TargetList.MultiSelect = false;
            this.TargetList.Name = "TargetList";
            this.TargetList.ReadOnly = true;
            this.TargetList.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TargetList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.TargetList.RowHeadersVisible = false;
            this.TargetList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.TargetList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.TargetList.Size = new System.Drawing.Size(560, 398);
            this.TargetList.TabIndex = 12;
            this.TargetList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.TargetList_CellDoubleClick);
            this.TargetList.Scroll += new System.Windows.Forms.ScrollEventHandler(this.TargetList_Scroll);
            this.TargetList.Enter += new System.EventHandler(this.TargetList_Enter);
            this.TargetList.Leave += new System.EventHandler(this.TargetList_Leave);
            // 
            // Default
            // 
            this.Default.FillWeight = 50F;
            this.Default.HeaderText = "Default";
            this.Default.MinimumWidth = 50;
            this.Default.Name = "Default";
            this.Default.ReadOnly = true;
            this.Default.Width = 50;
            // 
            // TargetName
            // 
            this.TargetName.FillWeight = 211F;
            this.TargetName.HeaderText = "Target Name";
            this.TargetName.MinimumWidth = 211;
            this.TargetName.Name = "TargetName";
            this.TargetName.ReadOnly = true;
            this.TargetName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.TargetName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.TargetName.Width = 211;
            // 
            // Firmware
            // 
            this.Firmware.FillWeight = 70F;
            this.Firmware.HeaderText = "Firmware";
            this.Firmware.MinimumWidth = 70;
            this.Firmware.Name = "Firmware";
            this.Firmware.ReadOnly = true;
            this.Firmware.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Firmware.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Firmware.Width = 70;
            // 
            // Address
            // 
            this.Address.FillWeight = 110F;
            this.Address.HeaderText = "Address";
            this.Address.MinimumWidth = 110;
            this.Address.Name = "Address";
            this.Address.ReadOnly = true;
            this.Address.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Address.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Address.Width = 110;
            // 
            // Status
            // 
            this.Status.HeaderText = "Status";
            this.Status.MinimumWidth = 100;
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            // 
            // SelectTarget
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 443);
            this.Controls.Add(this.darkScrollBar1);
            this.Controls.Add(this.TargetList);
            this.DialogButtons = DarkUI.Forms.DarkDialogButton.OkCancel;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SelectTarget";
            this.Text = "Select Target";
            this.Load += new System.EventHandler(this.SelectTarget_Load);
            this.Controls.SetChildIndex(this.TargetList, 0);
            this.Controls.SetChildIndex(this.darkScrollBar1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.TargetList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DarkUI.Controls.DarkScrollBar darkScrollBar1;
        private System.Windows.Forms.DataGridView TargetList;
        private System.Windows.Forms.DataGridViewImageColumn Default;
        private System.Windows.Forms.DataGridViewTextBoxColumn TargetName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Firmware;
        private System.Windows.Forms.DataGridViewTextBoxColumn Address;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
    }
}