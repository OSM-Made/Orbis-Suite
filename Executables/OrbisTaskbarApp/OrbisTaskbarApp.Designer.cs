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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrbisTaskbarApp));
            this.DarkContextMenu_Main = new DarkUI.Controls.DarkContextMenu();
            this.ToolStrip_AddTarget = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStrip_TargetList = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.ConsoleOutput_Launcher = new System.Windows.Forms.ToolStripMenuItem();
            this.Debugger_Launcher = new System.Windows.Forms.ToolStripMenuItem();
            this.ModuleManager_Launcher = new System.Windows.Forms.ToolStripMenuItem();
            this.TargetSettings_Launcher = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStrip_Reboot = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStrip_Shutdown = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStrip_SendPayload = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStrip_AutoLoadPayload = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStrip_AutoLaunch = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStrip_Help = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStrip_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.NotifyIcon_TaskBarApp = new System.Windows.Forms.NotifyIcon(this.components);
            this.DarkContextMenu_ConsoleList = new DarkUI.Controls.DarkContextMenu();
            this.ToolStrip_Suspend = new System.Windows.Forms.ToolStripMenuItem();
            this.Neigborhood_Launcher = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStrip_SendOrbisPayload = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStrip_Settings = new System.Windows.Forms.ToolStripMenuItem();
            this.DarkContextMenu_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // DarkContextMenu_Main
            // 
            this.DarkContextMenu_Main.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.DarkContextMenu_Main.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.DarkContextMenu_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStrip_AddTarget,
            this.ToolStrip_TargetList,
            this.toolStripSeparator7,
            this.Neigborhood_Launcher,
            this.ConsoleOutput_Launcher,
            this.Debugger_Launcher,
            this.ModuleManager_Launcher,
            this.TargetSettings_Launcher,
            this.toolStripSeparator8,
            this.ToolStrip_Reboot,
            this.ToolStrip_Shutdown,
            this.ToolStrip_Suspend,
            this.toolStripSeparator9,
            this.ToolStrip_SendPayload,
            this.ToolStrip_SendOrbisPayload,
            this.toolStripSeparator10,
            this.ToolStrip_AutoLoadPayload,
            this.ToolStrip_AutoLaunch,
            this.ToolStrip_Settings,
            this.ToolStrip_Help,
            this.ToolStrip_Exit});
            this.DarkContextMenu_Main.Name = "DarkContextMenu_Main";
            this.DarkContextMenu_Main.Size = new System.Drawing.Size(206, 428);
            // 
            // ToolStrip_AddTarget
            // 
            this.ToolStrip_AddTarget.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ToolStrip_AddTarget.Name = "ToolStrip_AddTarget";
            this.ToolStrip_AddTarget.Size = new System.Drawing.Size(205, 22);
            this.ToolStrip_AddTarget.Text = "Add Target...";
            this.ToolStrip_AddTarget.Click += new System.EventHandler(this.ToolStrip_AddTarget_Click);
            // 
            // ToolStrip_TargetList
            // 
            this.ToolStrip_TargetList.DropDown = this.DarkContextMenu_ConsoleList;
            this.ToolStrip_TargetList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ToolStrip_TargetList.Name = "ToolStrip_TargetList";
            this.ToolStrip_TargetList.Size = new System.Drawing.Size(205, 22);
            this.ToolStrip_TargetList.Text = "Default Target: N/A";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripSeparator7.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(202, 6);
            // 
            // ConsoleOutput_Launcher
            // 
            this.ConsoleOutput_Launcher.Enabled = false;
            this.ConsoleOutput_Launcher.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.ConsoleOutput_Launcher.Image = global::OrbisTaskbarApp.Properties.Resources.OrbisConsoleOutput_100;
            this.ConsoleOutput_Launcher.Name = "ConsoleOutput_Launcher";
            this.ConsoleOutput_Launcher.Size = new System.Drawing.Size(205, 22);
            this.ConsoleOutput_Launcher.Text = "Console Output...";
            this.ConsoleOutput_Launcher.Click += new System.EventHandler(this.ConsoleOutput_Launcher_Click);
            // 
            // Debugger_Launcher
            // 
            this.Debugger_Launcher.Enabled = false;
            this.Debugger_Launcher.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.Debugger_Launcher.Image = global::OrbisTaskbarApp.Properties.Resources.OrbisSettings_4496;
            this.Debugger_Launcher.Name = "Debugger_Launcher";
            this.Debugger_Launcher.Size = new System.Drawing.Size(205, 22);
            this.Debugger_Launcher.Text = "Debugger...";
            this.Debugger_Launcher.Click += new System.EventHandler(this.Debugger_Launcher_Click);
            // 
            // ModuleManager_Launcher
            // 
            this.ModuleManager_Launcher.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ModuleManager_Launcher.Image = global::OrbisTaskbarApp.Properties.Resources.orbis_razorCpuLiveUIx64_100;
            this.ModuleManager_Launcher.Name = "ModuleManager_Launcher";
            this.ModuleManager_Launcher.Size = new System.Drawing.Size(205, 22);
            this.ModuleManager_Launcher.Text = "Module Manager...";
            this.ModuleManager_Launcher.Click += new System.EventHandler(this.ModuleManager_Launcher_Click);
            // 
            // TargetSettings_Launcher
            // 
            this.TargetSettings_Launcher.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.TargetSettings_Launcher.Image = global::OrbisTaskbarApp.Properties.Resources.OrbisSettings_100;
            this.TargetSettings_Launcher.Name = "TargetSettings_Launcher";
            this.TargetSettings_Launcher.Size = new System.Drawing.Size(205, 22);
            this.TargetSettings_Launcher.Text = "Target Settings...";
            this.TargetSettings_Launcher.Click += new System.EventHandler(this.TargetSettings_Launcher_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripSeparator8.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(202, 6);
            // 
            // ToolStrip_Reboot
            // 
            this.ToolStrip_Reboot.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ToolStrip_Reboot.Name = "ToolStrip_Reboot";
            this.ToolStrip_Reboot.Size = new System.Drawing.Size(205, 22);
            this.ToolStrip_Reboot.Text = "Reboot";
            this.ToolStrip_Reboot.Click += new System.EventHandler(this.ToolStrip_Reboot_Click);
            // 
            // ToolStrip_Shutdown
            // 
            this.ToolStrip_Shutdown.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ToolStrip_Shutdown.Name = "ToolStrip_Shutdown";
            this.ToolStrip_Shutdown.Size = new System.Drawing.Size(205, 22);
            this.ToolStrip_Shutdown.Text = "Shutdown";
            this.ToolStrip_Shutdown.Click += new System.EventHandler(this.ToolStrip_Shutdown_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripSeparator9.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(202, 6);
            // 
            // ToolStrip_SendPayload
            // 
            this.ToolStrip_SendPayload.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ToolStrip_SendPayload.Name = "ToolStrip_SendPayload";
            this.ToolStrip_SendPayload.Size = new System.Drawing.Size(205, 22);
            this.ToolStrip_SendPayload.Text = "Send Payload";
            this.ToolStrip_SendPayload.Click += new System.EventHandler(this.ToolStrip_SendPayload_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripSeparator10.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(202, 6);
            // 
            // ToolStrip_AutoLoadPayload
            // 
            this.ToolStrip_AutoLoadPayload.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ToolStrip_AutoLoadPayload.Name = "ToolStrip_AutoLoadPayload";
            this.ToolStrip_AutoLoadPayload.Size = new System.Drawing.Size(205, 22);
            this.ToolStrip_AutoLoadPayload.Text = "Auto Load Payload";
            this.ToolStrip_AutoLoadPayload.Click += new System.EventHandler(this.ToolStrip_AutoLoadPayload_Click);
            // 
            // ToolStrip_AutoLaunch
            // 
            this.ToolStrip_AutoLaunch.Checked = true;
            this.ToolStrip_AutoLaunch.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToolStrip_AutoLaunch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ToolStrip_AutoLaunch.Name = "ToolStrip_AutoLaunch";
            this.ToolStrip_AutoLaunch.Size = new System.Drawing.Size(205, 22);
            this.ToolStrip_AutoLaunch.Text = "Load Orbis Suite on boot";
            this.ToolStrip_AutoLaunch.Click += new System.EventHandler(this.ToolStrip_AutoLaunch_Click);
            // 
            // ToolStrip_Help
            // 
            this.ToolStrip_Help.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ToolStrip_Help.Name = "ToolStrip_Help";
            this.ToolStrip_Help.Size = new System.Drawing.Size(205, 22);
            this.ToolStrip_Help.Text = "About";
            this.ToolStrip_Help.Click += new System.EventHandler(this.ToolStrip_Help_Click);
            // 
            // ToolStrip_Exit
            // 
            this.ToolStrip_Exit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ToolStrip_Exit.Name = "ToolStrip_Exit";
            this.ToolStrip_Exit.Size = new System.Drawing.Size(205, 22);
            this.ToolStrip_Exit.Text = "Exit";
            this.ToolStrip_Exit.Click += new System.EventHandler(this.ToolStrip_Exit_Click);
            // 
            // NotifyIcon_TaskBarApp
            // 
            this.NotifyIcon_TaskBarApp.BalloonTipTitle = "Orbis Suite";
            this.NotifyIcon_TaskBarApp.ContextMenuStrip = this.DarkContextMenu_Main;
            this.NotifyIcon_TaskBarApp.Icon = ((System.Drawing.Icon)(resources.GetObject("NotifyIcon_TaskBarApp.Icon")));
            this.NotifyIcon_TaskBarApp.Text = "Taskbar Application for Orbis Suite";
            this.NotifyIcon_TaskBarApp.Visible = true;
            // 
            // DarkContextMenu_ConsoleList
            // 
            this.DarkContextMenu_ConsoleList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.DarkContextMenu_ConsoleList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.DarkContextMenu_ConsoleList.Name = "DarkContextMenu_ConsoleList";
            this.DarkContextMenu_ConsoleList.OwnerItem = this.ToolStrip_TargetList;
            this.DarkContextMenu_ConsoleList.Size = new System.Drawing.Size(61, 4);
            // 
            // ToolStrip_Suspend
            // 
            this.ToolStrip_Suspend.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ToolStrip_Suspend.Name = "ToolStrip_Suspend";
            this.ToolStrip_Suspend.Size = new System.Drawing.Size(205, 22);
            this.ToolStrip_Suspend.Text = "Suspend";
            this.ToolStrip_Suspend.Click += new System.EventHandler(this.ToolStrip_Suspend_Click);
            // 
            // Neigborhood_Launcher
            // 
            this.Neigborhood_Launcher.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.Neigborhood_Launcher.Image = global::OrbisTaskbarApp.Properties.Resources.OrbisNeighborhoodx64_4055;
            this.Neigborhood_Launcher.Name = "Neigborhood_Launcher";
            this.Neigborhood_Launcher.Size = new System.Drawing.Size(205, 22);
            this.Neigborhood_Launcher.Text = "Neighborhood...";
            this.Neigborhood_Launcher.Click += new System.EventHandler(this.Neigborhood_Launcher_Click);
            // 
            // ToolStrip_SendOrbisPayload
            // 
            this.ToolStrip_SendOrbisPayload.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ToolStrip_SendOrbisPayload.Name = "ToolStrip_SendOrbisPayload";
            this.ToolStrip_SendOrbisPayload.Size = new System.Drawing.Size(205, 22);
            this.ToolStrip_SendOrbisPayload.Text = "Send Orbis Suite Payload";
            this.ToolStrip_SendOrbisPayload.Click += new System.EventHandler(this.ToolStrip_SendOrbisPayload_Click);
            // 
            // ToolStrip_Settings
            // 
            this.ToolStrip_Settings.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ToolStrip_Settings.Name = "ToolStrip_Settings";
            this.ToolStrip_Settings.Size = new System.Drawing.Size(205, 22);
            this.ToolStrip_Settings.Text = "Settings";
            this.ToolStrip_Settings.Click += new System.EventHandler(this.ToolStrip_Settings_Click);
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
            this.DarkContextMenu_Main.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DarkUI.Controls.DarkContextMenu DarkContextMenu_Main;
        private System.Windows.Forms.ToolStripMenuItem ToolStrip_AddTarget;
        private System.Windows.Forms.ToolStripMenuItem ToolStrip_TargetList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem Neigborhood_Launcher;
        private System.Windows.Forms.ToolStripMenuItem ConsoleOutput_Launcher;
        private System.Windows.Forms.ToolStripMenuItem Debugger_Launcher;
        private System.Windows.Forms.ToolStripMenuItem ModuleManager_Launcher;
        private System.Windows.Forms.ToolStripMenuItem TargetSettings_Launcher;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem ToolStrip_Reboot;
        private System.Windows.Forms.ToolStripMenuItem ToolStrip_Shutdown;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem ToolStrip_SendPayload;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem ToolStrip_AutoLoadPayload;
        private System.Windows.Forms.ToolStripMenuItem ToolStrip_AutoLaunch;
        private System.Windows.Forms.ToolStripMenuItem ToolStrip_Help;
        private System.Windows.Forms.ToolStripMenuItem ToolStrip_Exit;
        private System.Windows.Forms.NotifyIcon NotifyIcon_TaskBarApp;
        private DarkUI.Controls.DarkContextMenu DarkContextMenu_ConsoleList;
        private System.Windows.Forms.ToolStripMenuItem ToolStrip_Suspend;
        private System.Windows.Forms.ToolStripMenuItem ToolStrip_SendOrbisPayload;
        private System.Windows.Forms.ToolStripMenuItem ToolStrip_Settings;
    }
}

