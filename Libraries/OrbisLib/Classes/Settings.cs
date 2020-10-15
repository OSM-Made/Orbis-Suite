using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbisSuite.Classes
{
    public class Settings
    {
        OrbisLib PS4;

        public Settings(OrbisLib PS4)
        {
            this.PS4 = PS4;
        }

        private bool _AutoLoadPayload;
        public bool AutoLoadPayload
        {
            get 
            {
                return _AutoLoadPayload = Imports.Settings.GetAutoLoadPayload();
            }
            set
            {
                _AutoLoadPayload = value;
                Imports.Settings.SetAutoLoadPayload(value);
            }
        }

        private bool _StartOnBoot;
        public bool StartOnBoot
        {
            get
            {
                return _StartOnBoot = Imports.Settings.GetStartOnBoot();
            }
            set
            {
                _StartOnBoot = value;
                Imports.Settings.SetStartOnBoot(value);

                //Get windows registry key to set app on start up.
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

                //add or remove the value.
                if (value)
                    key.SetValue("OrbisTaskbarApp", AppDomain.CurrentDomain.BaseDirectory + "OrbisTaskbarApp.exe");
                else
                    key.DeleteValue("OrbisTaskbarApp", false);

                //Close the key after use.
                key.Close();
            }
        }

        private bool _DetectGame;
        public bool DetectGame
        {
            get
            {
                return _DetectGame = Imports.Settings.GetDetectGame();
            }
            set
            {
                _DetectGame = value;
                Imports.Settings.SetDetectGame(value);
            }
        }

        private string _COMPort;
        public string COMPort
        {
            get
            {
                return _COMPort = Imports.Settings.GetCOMPort();
            }
            set
            {
                _COMPort = value;
                Imports.Settings.SetCOMPort(value);
            }
        }

        private int _ServicePort;
        public int ServicePort
        {
            get
            {
                return _ServicePort = Imports.Settings.GetServicePort();
            }
            set
            {
                _ServicePort = value;
                Imports.Settings.SetServicePort(value);
            }
        }

        private int _APIPort;
        public int APIPort
        {
            get
            {
                return _APIPort = Imports.Settings.GetAPIPort();
            }
            set
            {
                _APIPort = value;
                Imports.Settings.SetAPIPort(value);
            }
        }

        private bool _CensorIDPS;
        public bool CensorIDPS
        {
            get
            {
                return _CensorIDPS = Imports.Settings.GetCensorIDPS();
            }
            set
            {
                _CensorIDPS = value;
                Imports.Settings.SetCensorIDPS(value);
            }
        }

        private bool _CensorPSID;
        public bool CensorPSID
        {
            get
            {
                return _CensorPSID = Imports.Settings.GetCensorPSID();
            }
            set
            {
                _CensorPSID = value;
                Imports.Settings.SetCensorPSID(value);
            }
        }

        private bool _Debug;
        public bool Debug
        {
            get
            {
                return _Debug = Imports.Settings.GetDebug();
            }
            set
            {
                _Debug = value;
                Imports.Settings.SetDebug(value);
            }
        }

        private bool _CreateLogs;
        public bool CreateLogs
        {
            get
            {
                return _CreateLogs = Imports.Settings.GetCreateLogs();
            }
            set
            {
                _CreateLogs = value;
                Imports.Settings.SetCreateLogs(value);
            }
        }

        private bool _ShowTimestamps;
        public bool ShowTimestamps
        {
            get
            {
                return _ShowTimestamps = Imports.Settings.GetShowTimestamps();
            }
            set
            {
                _ShowTimestamps = value;
                Imports.Settings.SetShowTimestamps(value);
            }
        }

        private bool _WordWrap;
        public bool WordWrap
        {
            get
            {
                return _WordWrap = Imports.Settings.GetWordWrap();
            }
            set
            {
                _WordWrap = value;
                Imports.Settings.SetWordWrap(value);
            }
        }
    }
}
