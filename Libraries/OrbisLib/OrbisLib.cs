using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using DarkUI.Forms;
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
                //initialize the variable to start.
                if (Internal_DefaultTarget == null)
                    Internal_DefaultTarget = new Target(this);

                //Need to Get the default Target.
                bool FoundDefaultTarget = false;
                foreach (TargetInfo TargetInfo in TargetManagement.TargetList)
                {
                    if (Internal_Targets.ContainsKey(TargetInfo.Name))
                        Internal_Targets[TargetInfo.Name].Info = TargetInfo;
                    else
                        Internal_Targets.Add(TargetInfo.Name, new Target(this, TargetInfo));

                    if (TargetInfo.Default)
                    {
                        Internal_DefaultTarget = Internal_Targets[TargetInfo.Name];
                        FoundDefaultTarget = true;
                        break;
                    }
                }

                //If we dont find the default target we set the Active flag as false
                Internal_DefaultTarget.Active = FoundDefaultTarget;

                //Return the instanced version of the default target.
                return Internal_DefaultTarget;
            }
        }

        private Target internal_SelectedTarget;
        public Target SelectedTarget
        {
            get
            {
                //initialize the variable to start.
                if (internal_SelectedTarget == null)
                    internal_SelectedTarget = new Target(this);

                TargetInfo TargetInfo;
                if ((TargetInfo = TargetManagement.TargetList.Find(x => x.Name == internal_SelectedTarget.Info.Name)) != null)
                    internal_SelectedTarget.Active = true;
                else if(DefaultTarget.Active)
                {
                    internal_SelectedTarget.Info = DefaultTarget.Info;
                    internal_SelectedTarget.Active = true;
                }
                else
                    internal_SelectedTarget.Active = false;

                return internal_SelectedTarget;
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
            get { return Internal_TargetManagement ?? (Internal_TargetManagement = new TargetManagement(this)); }
        }

        private Settings Internal_Settings;
        public Settings Settings
        {
            get { return Internal_Settings ?? (Internal_Settings = new Settings(this)); }
        }

        private Dialogs Internal_Dialogs;
        public Dialogs Dialogs
        {
            get { return Internal_Dialogs ?? (Internal_Dialogs = new Dialogs(this)); }
        }

        #endregion

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

                Utilities.SetDllDirectory(OrbisLib_Dir);

                //Set up our instance of the OrbisLibCPP.dll.
                Imports.SetupCPP(false);

                //Set up selected target as default target initially.
                SelectedTarget.Info = DefaultTarget.Info;

            }
            catch
            {

            }
        }
    }
}
