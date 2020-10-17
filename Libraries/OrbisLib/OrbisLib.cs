using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using DarkUI.Forms;
using OrbisSuite.Classes;
using OrbisSuite.Dialog;

namespace OrbisSuite
{
    public class OrbisLib
    {
        internal string OrbisLib_Dir;

        #region Internal Class Defines

        private Target Internal_DefaultTarget;
        public Target DefaultTarget
        {
            get
            {
                foreach (TargetInfo TargetInfo in TargetManagement.TargetList)
                {
                    if (Internal_Targets.ContainsKey(TargetInfo.Name))
                        Internal_Targets[TargetInfo.Name].Info = TargetInfo;
                    else
                        Internal_Targets.Add(TargetInfo.Name, new Target(this, TargetInfo));

                    if (TargetInfo.Default)
                        Internal_DefaultTarget = Internal_Targets[TargetInfo.Name];
                }

                return Internal_DefaultTarget;
            }
        }

        private Dictionary<string, Target> Internal_Targets = new Dictionary<string, Target>();
        public Dictionary<string, Target> Target
        {
            get
            {
                //Updates the dictionary every time its referenced to make sure its up to date.
                //Need to test and see if the overhead on this is too much.
                foreach(TargetInfo TargetInfo in TargetManagement.TargetList)
                {
                    if (Internal_Targets.ContainsKey(TargetInfo.Name))
                        Internal_Targets[TargetInfo.Name].Info = TargetInfo;
                    else
                        Internal_Targets.Add(TargetInfo.Name, new Target(this, TargetInfo));
                }

                return Internal_Targets;
            }
        }

        private Events Internal_Events;
        public Events Events
        {
            get { return Internal_Events ?? (Internal_Events = new Events(this)); }
        }

        private TargetManagement Internal_TargetManagement;
        public TargetManagement TargetManagement
        {
            get { return Internal_TargetManagement ?? (Internal_TargetManagement = new TargetManagement()); }
        }

        private Classes.Settings Internal_Settings;
        public Classes.Settings Settings
        {
            get { return Internal_Settings ?? (Internal_Settings = new Classes.Settings(this)); }
        }

        private Dialogs Internal_Dialogs;
        public Dialogs Dialogs
        {
            get { return Internal_Dialogs ?? (Internal_Dialogs = new Dialogs(this)); }
        }

        #endregion

        #region Kernel Imports

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool SetDllDirectory(string lpPathName);

        #endregion

        [DllImport("OrbisLibCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SetupCPP(bool WinService);

        public OrbisLib()
        {
            try
            {
                OrbisLib_Dir = (Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Orbis Suite\\");

                if (!Directory.Exists(OrbisLib_Dir))
                {
                    DarkMessageBox.ShowError("In order to use the functionality of the OrbisLib.dll you need to first install Orbis Suite on this machine.", "Orbis Suite not Installed.");

                    throw new System.Exception("Orbis Suite not Installed.");
                }

                SetDllDirectory(OrbisLib_Dir);

                //Set up our instance of the OrbisLibCPP.dll.
                SetupCPP(false);

                if (DefaultTarget == null)
                {
                    DarkMessageBox.ShowError("Please add a default target First.", "Default Target Required.", DarkDialogButton.Ok, FormStartPosition.CenterScreen);
                    Dialogs.AddTarget(FormStartPosition.CenterScreen);
                    if (DefaultTarget == null)
                        Environment.Exit(0);
                }
            }
            catch
            {

            }
        }
    }
}
