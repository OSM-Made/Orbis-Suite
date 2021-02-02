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
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.TargetList = new System.Windows.Forms.DataGridView();
            this.mDefault = new System.Windows.Forms.DataGridViewImageColumn();
            this.mTargetName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mFirmware = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mIPAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mSDKVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mConsoleName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mConsoleType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TargetContextMenu = new DarkUI.Controls.DarkContextMenu();
            this.SendPayload = new System.Windows.Forms.ToolStripMenuItem();
            this.SendOrbisPayload = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.Target_Reboot = new System.Windows.Forms.ToolStripMenuItem();
            this.Target_Shutdown = new System.Windows.Forms.ToolStripMenuItem();
            this.Target_Suspend = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.Target_SetDefault = new System.Windows.Forms.ToolStripMenuItem();
            this.Target_Edit = new System.Windows.Forms.ToolStripMenuItem();
            this.Target_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.Target_Details = new System.Windows.Forms.ToolStripMenuItem();
            this.TitleDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.Locate_Target = new System.Windows.Forms.ToolStripMenuItem();
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
            this.Settings_DropDown = new System.Windows.Forms.ToolStripDropDownButton();
            this.AutoLoadPayload_Button = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadOnBoot_Button = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.SelectTargetButton = new System.Windows.Forms.ToolStripMenuItem();
            this.SettingsButton = new System.Windows.Forms.ToolStripMenuItem();
            this.About_Button = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.Button_Attach = new System.Windows.Forms.ToolStripButton();
            this.Button_Detach = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.AddTarget_Button = new System.Windows.Forms.ToolStripButton();
            this.CurrentTarget = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.CurrentProc = new System.Windows.Forms.ToolStripLabel();
            ((System.ComponentModel.ISupportInitialize)(this.TargetList)).BeginInit();
            this.TargetContextMenu.SuspendLayout();
            this.darkStatusStrip1.SuspendLayout();
            this.darkToolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 6);
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
            // TargetList
            // 
            this.TargetList.AllowUserToAddRows = false;
            this.TargetList.AllowUserToDeleteRows = false;
            this.TargetList.AllowUserToResizeColumns = false;
            this.TargetList.AllowUserToResizeRows = false;
            this.TargetList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.mStatus,
            this.mTitle,
            this.mSDKVersion,
            this.mConsoleName,
            this.mConsoleType});
            this.TargetList.ContextMenuStrip = this.TargetContextMenu;
            this.TargetList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(60)))), ((int)(((byte)(62)))));
            this.TargetList.Location = new System.Drawing.Point(0, 33);
            this.TargetList.MultiSelect = false;
            this.TargetList.Name = "TargetList";
            this.TargetList.ReadOnly = true;
            this.TargetList.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TargetList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.TargetList.RowHeadersVisible = false;
            this.TargetList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.TargetList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.TargetList.Size = new System.Drawing.Size(1004, 386);
            this.TargetList.TabIndex = 8;
            this.TargetList.Enter += new System.EventHandler(this.TargetList_Enter);
            this.TargetList.Leave += new System.EventHandler(this.TargetList_Leave);
            // 
            // mDefault
            // 
            this.mDefault.HeaderText = "Default";
            this.mDefault.MinimumWidth = 50;
            this.mDefault.Name = "mDefault";
            this.mDefault.ReadOnly = true;
            this.mDefault.Width = 50;
            // 
            // mTargetName
            // 
            this.mTargetName.FillWeight = 161F;
            this.mTargetName.HeaderText = "Target Name";
            this.mTargetName.MinimumWidth = 161;
            this.mTargetName.Name = "mTargetName";
            this.mTargetName.ReadOnly = true;
            this.mTargetName.Width = 161;
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
            // mIPAddress
            // 
            this.mIPAddress.FillWeight = 110F;
            this.mIPAddress.HeaderText = "Address";
            this.mIPAddress.MinimumWidth = 110;
            this.mIPAddress.Name = "mIPAddress";
            this.mIPAddress.ReadOnly = true;
            this.mIPAddress.Width = 110;
            // 
            // mStatus
            // 
            this.mStatus.HeaderText = "Status";
            this.mStatus.MinimumWidth = 100;
            this.mStatus.Name = "mStatus";
            this.mStatus.ReadOnly = true;
            // 
            // mTitle
            // 
            this.mTitle.FillWeight = 200F;
            this.mTitle.HeaderText = "Title";
            this.mTitle.MinimumWidth = 200;
            this.mTitle.Name = "mTitle";
            this.mTitle.ReadOnly = true;
            this.mTitle.Width = 200;
            // 
            // mSDKVersion
            // 
            this.mSDKVersion.HeaderText = "SDK Version";
            this.mSDKVersion.Name = "mSDKVersion";
            this.mSDKVersion.ReadOnly = true;
            // 
            // mConsoleName
            // 
            this.mConsoleName.FillWeight = 112F;
            this.mConsoleName.HeaderText = "Console Name";
            this.mConsoleName.MinimumWidth = 112;
            this.mConsoleName.Name = "mConsoleName";
            this.mConsoleName.ReadOnly = true;
            this.mConsoleName.Width = 112;
            // 
            // mConsoleType
            // 
            this.mConsoleType.HeaderText = "Console Type";
            this.mConsoleType.Name = "mConsoleType";
            this.mConsoleType.ReadOnly = true;
            // 
            // TargetContextMenu
            // 
            this.TargetContextMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.TargetContextMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.TargetContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SendPayload,
            this.SendOrbisPayload,
            this.toolStripSeparator4,
            this.Target_Reboot,
            this.Target_Shutdown,
            this.Target_Suspend,
            this.toolStripSeparator5,
            this.Target_SetDefault,
            this.Target_Edit,
            this.Target_Delete,
            this.toolStripSeparator6,
            this.Target_Details,
            this.TitleDetails,
            this.Locate_Target});
            this.TargetContextMenu.Name = "TargetContextMenu";
            this.TargetContextMenu.Size = new System.Drawing.Size(206, 289);
            this.TargetContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.TargetContextMenu_Opening);
            // 
            // SendPayload
            // 
            this.SendPayload.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.SendPayload.Name = "SendPayload";
            this.SendPayload.Size = new System.Drawing.Size(205, 22);
            this.SendPayload.Text = "Send Payload";
            this.SendPayload.Click += new System.EventHandler(this.SendPayload_Click);
            // 
            // SendOrbisPayload
            // 
            this.SendOrbisPayload.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.SendOrbisPayload.Name = "SendOrbisPayload";
            this.SendOrbisPayload.Size = new System.Drawing.Size(205, 22);
            this.SendOrbisPayload.Text = "Send Orbis Suite Payload";
            this.SendOrbisPayload.Click += new System.EventHandler(this.SendOrbisPayload_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripSeparator4.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(202, 6);
            // 
            // Target_Reboot
            // 
            this.Target_Reboot.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.Target_Reboot.Name = "Target_Reboot";
            this.Target_Reboot.Size = new System.Drawing.Size(205, 22);
            this.Target_Reboot.Text = "Reboot";
            this.Target_Reboot.Click += new System.EventHandler(this.Target_Reboot_Click);
            // 
            // Target_Shutdown
            // 
            this.Target_Shutdown.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.Target_Shutdown.Name = "Target_Shutdown";
            this.Target_Shutdown.Size = new System.Drawing.Size(205, 22);
            this.Target_Shutdown.Text = "Shutdown";
            this.Target_Shutdown.Click += new System.EventHandler(this.Target_Shutdown_Click);
            // 
            // Target_Suspend
            // 
            this.Target_Suspend.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.Target_Suspend.Name = "Target_Suspend";
            this.Target_Suspend.Size = new System.Drawing.Size(205, 22);
            this.Target_Suspend.Text = "Suspend";
            this.Target_Suspend.Click += new System.EventHandler(this.Target_Suspend_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripSeparator5.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(202, 6);
            // 
            // Target_SetDefault
            // 
            this.Target_SetDefault.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.Target_SetDefault.Name = "Target_SetDefault";
            this.Target_SetDefault.Size = new System.Drawing.Size(205, 22);
            this.Target_SetDefault.Text = "Default";
            this.Target_SetDefault.Click += new System.EventHandler(this.Target_SetDefault_Click);
            // 
            // Target_Edit
            // 
            this.Target_Edit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.Target_Edit.Name = "Target_Edit";
            this.Target_Edit.Size = new System.Drawing.Size(205, 22);
            this.Target_Edit.Text = "Edit";
            this.Target_Edit.Click += new System.EventHandler(this.Target_Edit_Click);
            // 
            // Target_Delete
            // 
            this.Target_Delete.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.Target_Delete.Name = "Target_Delete";
            this.Target_Delete.Size = new System.Drawing.Size(205, 22);
            this.Target_Delete.Text = "Delete";
            this.Target_Delete.Click += new System.EventHandler(this.Target_Delete_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripSeparator6.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(202, 6);
            // 
            // Target_Details
            // 
            this.Target_Details.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.Target_Details.Name = "Target_Details";
            this.Target_Details.Size = new System.Drawing.Size(205, 22);
            this.Target_Details.Text = "Target Details";
            this.Target_Details.Click += new System.EventHandler(this.Target_Details_Click);
            // 
            // TitleDetails
            // 
            this.TitleDetails.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.TitleDetails.Name = "TitleDetails";
            this.TitleDetails.Size = new System.Drawing.Size(205, 22);
            this.TitleDetails.Text = "Title Details";
            this.TitleDetails.Click += new System.EventHandler(this.TitleDetails_Click);
            // 
            // Locate_Target
            // 
            this.Locate_Target.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.Locate_Target.Name = "Locate_Target";
            this.Locate_Target.Size = new System.Drawing.Size(205, 22);
            this.Locate_Target.Text = "Locate";
            this.Locate_Target.Click += new System.EventHandler(this.locateToolStripMenuItem_Click);
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
            this.toolStripLabel1.Text = "Target: -";
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
            this.toolStripLabel2.Text = "Process: -";
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
            this.darkStatusStrip1.Location = new System.Drawing.Point(0, 422);
            this.darkStatusStrip1.Name = "darkStatusStrip1";
            this.darkStatusStrip1.Padding = new System.Windows.Forms.Padding(0, 5, 0, 3);
            this.darkStatusStrip1.Size = new System.Drawing.Size(1004, 30);
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
            this.toolStripStatusLabel7.Size = new System.Drawing.Size(848, 20);
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
            this.Settings_DropDown,
            this.toolStripSeparator8,
            this.Button_Attach,
            this.Button_Detach,
            this.toolStripSeparator9,
            this.AddTarget_Button,
            this.CurrentTarget,
            this.toolStripSeparator7,
            this.CurrentProc});
            this.darkToolStrip1.Location = new System.Drawing.Point(0, 0);
            this.darkToolStrip1.Name = "darkToolStrip1";
            this.darkToolStrip1.Padding = new System.Windows.Forms.Padding(5, 0, 1, 2);
            this.darkToolStrip1.Size = new System.Drawing.Size(1004, 30);
            this.darkToolStrip1.TabIndex = 10;
            this.darkToolStrip1.Text = "darkToolStrip1";
            // 
            // Settings_DropDown
            // 
            this.Settings_DropDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Settings_DropDown.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AutoLoadPayload_Button,
            this.LoadOnBoot_Button,
            this.toolStripSeparator3,
            this.SelectTargetButton,
            this.SettingsButton,
            this.About_Button});
            this.Settings_DropDown.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.Settings_DropDown.Image = ((System.Drawing.Image)(resources.GetObject("Settings_DropDown.Image")));
            this.Settings_DropDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Settings_DropDown.Name = "Settings_DropDown";
            this.Settings_DropDown.Size = new System.Drawing.Size(29, 25);
            this.Settings_DropDown.Text = "Settings";
            // 
            // AutoLoadPayload_Button
            // 
            this.AutoLoadPayload_Button.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.AutoLoadPayload_Button.Name = "AutoLoadPayload_Button";
            this.AutoLoadPayload_Button.Size = new System.Drawing.Size(205, 22);
            this.AutoLoadPayload_Button.Text = "Auto Load Payload";
            this.AutoLoadPayload_Button.Click += new System.EventHandler(this.AutoLoadPayload_Button_Click);
            // 
            // LoadOnBoot_Button
            // 
            this.LoadOnBoot_Button.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.LoadOnBoot_Button.Name = "LoadOnBoot_Button";
            this.LoadOnBoot_Button.Size = new System.Drawing.Size(205, 22);
            this.LoadOnBoot_Button.Text = "Load Orbis Suite on boot";
            this.LoadOnBoot_Button.Click += new System.EventHandler(this.LoadOnBoot_Button_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripSeparator3.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(202, 6);
            // 
            // SelectTargetButton
            // 
            this.SelectTargetButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.SelectTargetButton.Name = "SelectTargetButton";
            this.SelectTargetButton.Size = new System.Drawing.Size(205, 22);
            this.SelectTargetButton.Text = "Select Target";
            this.SelectTargetButton.Click += new System.EventHandler(this.SelectTargetButton_Click);
            // 
            // SettingsButton
            // 
            this.SettingsButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.SettingsButton.Name = "SettingsButton";
            this.SettingsButton.Size = new System.Drawing.Size(205, 22);
            this.SettingsButton.Text = "Settings";
            this.SettingsButton.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // About_Button
            // 
            this.About_Button.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.About_Button.Name = "About_Button";
            this.About_Button.Size = new System.Drawing.Size(205, 22);
            this.About_Button.Text = "About";
            this.About_Button.Click += new System.EventHandler(this.About_Button_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripSeparator8.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 28);
            // 
            // Button_Attach
            // 
            this.Button_Attach.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_Attach.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.Button_Attach.Image = ((System.Drawing.Image)(resources.GetObject("Button_Attach.Image")));
            this.Button_Attach.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Attach.Name = "Button_Attach";
            this.Button_Attach.Size = new System.Drawing.Size(23, 25);
            this.Button_Attach.Text = "Button_Attach";
            this.Button_Attach.ToolTipText = "Attach";
            this.Button_Attach.Click += new System.EventHandler(this.Button_Attach_Click);
            // 
            // Button_Detach
            // 
            this.Button_Detach.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_Detach.Enabled = false;
            this.Button_Detach.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.Button_Detach.Image = ((System.Drawing.Image)(resources.GetObject("Button_Detach.Image")));
            this.Button_Detach.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Detach.Name = "Button_Detach";
            this.Button_Detach.Size = new System.Drawing.Size(23, 25);
            this.Button_Detach.Text = "Detach";
            this.Button_Detach.Click += new System.EventHandler(this.Button_Detach_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripSeparator9.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 28);
            // 
            // AddTarget_Button
            // 
            this.AddTarget_Button.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.AddTarget_Button.Image = ((System.Drawing.Image)(resources.GetObject("AddTarget_Button.Image")));
            this.AddTarget_Button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddTarget_Button.Name = "AddTarget_Button";
            this.AddTarget_Button.Size = new System.Drawing.Size(84, 25);
            this.AddTarget_Button.Text = "Add Target";
            this.AddTarget_Button.Click += new System.EventHandler(this.AddTarget_Button_Click);
            // 
            // CurrentTarget
            // 
            this.CurrentTarget.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.CurrentTarget.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.CurrentTarget.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.CurrentTarget.Image = ((System.Drawing.Image)(resources.GetObject("CurrentTarget.Image")));
            this.CurrentTarget.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CurrentTarget.Name = "CurrentTarget";
            this.CurrentTarget.Size = new System.Drawing.Size(63, 25);
            this.CurrentTarget.Text = "Target: -";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripSeparator7.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 28);
            // 
            // CurrentProc
            // 
            this.CurrentProc.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.CurrentProc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.CurrentProc.Name = "CurrentProc";
            this.CurrentProc.Size = new System.Drawing.Size(58, 25);
            this.CurrentProc.Text = "Process: -";
            // 
            // OrbisNeighborhood
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1004, 452);
            this.Controls.Add(this.darkToolStrip1);
            this.Controls.Add(this.darkStatusStrip1);
            this.Controls.Add(this.TargetList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1020, 600);
            this.MinimumSize = new System.Drawing.Size(300, 200);
            this.Name = "OrbisNeighborhood";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Orbis Neighborhood";
            ((System.ComponentModel.ISupportInitialize)(this.TargetList)).EndInit();
            this.TargetContextMenu.ResumeLayout(false);
            this.darkStatusStrip1.ResumeLayout(false);
            this.darkStatusStrip1.PerformLayout();
            this.darkToolStrip1.ResumeLayout(false);
            this.darkToolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
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
        private System.Windows.Forms.ToolStripButton AddTarget_Button;
        private DarkUI.Controls.DarkContextMenu TargetContextMenu;
        private System.Windows.Forms.ToolStripMenuItem SendOrbisPayload;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem Target_Reboot;
        private System.Windows.Forms.ToolStripMenuItem Target_Shutdown;
        private System.Windows.Forms.ToolStripMenuItem Target_Suspend;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem Target_SetDefault;
        private System.Windows.Forms.ToolStripMenuItem Target_Edit;
        private System.Windows.Forms.ToolStripMenuItem Target_Delete;
        private System.Windows.Forms.ToolStripMenuItem Target_Details;
        private System.Windows.Forms.ToolStripDropDownButton Settings_DropDown;
        private System.Windows.Forms.ToolStripMenuItem AutoLoadPayload_Button;
        private System.Windows.Forms.ToolStripMenuItem LoadOnBoot_Button;
        private System.Windows.Forms.ToolStripMenuItem About_Button;
        private System.Windows.Forms.ToolStripMenuItem SettingsButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripLabel CurrentProc;
        private System.Windows.Forms.ToolStripMenuItem SendPayload;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripButton Button_Attach;
        private System.Windows.Forms.ToolStripButton Button_Detach;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripDropDownButton CurrentTarget;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem SelectTargetButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem TitleDetails;
        private System.Windows.Forms.DataGridViewImageColumn mDefault;
        private System.Windows.Forms.DataGridViewTextBoxColumn mTargetName;
        private System.Windows.Forms.DataGridViewTextBoxColumn mFirmware;
        private System.Windows.Forms.DataGridViewTextBoxColumn mIPAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn mStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn mTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn mSDKVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn mConsoleName;
        private System.Windows.Forms.DataGridViewTextBoxColumn mConsoleType;
        private System.Windows.Forms.ToolStripMenuItem Locate_Target;
    }
}

