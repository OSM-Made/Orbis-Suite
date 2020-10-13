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

        public bool GetAutoLoadPayload()
        {
            return Imports.Settings.GetAutoLoadPayload();
        }

        public void SetAutoLoadPayload(bool Value)
        {
            Imports.Settings.SetAutoLoadPayload(Value);
        }

        public bool GetStartOnBoot()
        {
            return Imports.Settings.GetStartOnBoot();
        }

        public void SetStartOnBoot(bool Value)
        {
            Imports.Settings.SetStartOnBoot(Value);
        }

        public bool GetDetectGame()
        {
            return Imports.Settings.GetDetectGame();
        }

        public void SetDetectGame(bool Value)
        {
            Imports.Settings.SetDetectGame(Value);
        }

        public string GetCOMPort()
        {
            return Imports.Settings.GetCOMPort();
        }

        public void SetCOMPort(string Value)
        {
            Imports.Settings.SetCOMPort(Value);
        }

        public int GetServicePort()
        {
            return Imports.Settings.GetServicePort();
        }

        public void SetServicePort(int Value)
        {
            Imports.Settings.SetServicePort(Value);
        }

        public int GetAPIPort()
        {
            return Imports.Settings.GetAPIPort();
        }

        public void SetAPIPort(int Value)
        {
            Imports.Settings.SetAPIPort(Value);
        }

        public bool GetCensorIDPS()
        {
            return Imports.Settings.GetCensorIDPS();
        }

        public void SetCensorIDPS(bool Value)
        {
            Imports.Settings.SetCensorIDPS(Value);
        }

        public bool GetCensorPSID()
        {
            return Imports.Settings.GetCensorPSID();
        }

        public void SetCensorPSID(bool Value)
        {
            Imports.Settings.SetCensorPSID(Value);
        }

        public bool GetDebug()
        {
            return Imports.Settings.GetDebug();
        }

        public void SetDebug(bool Value)
        {
            Imports.Settings.SetDebug(Value);
        }

        public bool GetCreateLogs()
        {
            return Imports.Settings.GetCreateLogs();
        }

        public void SetCreateLogs(bool Value)
        {
            Imports.Settings.SetCreateLogs(Value);
        }

        public bool GetShowTimestamps()
        {
            return Imports.Settings.GetShowTimestamps();
        }

        public void SetShowTimestamps(bool Value)
        {
            Imports.Settings.SetShowTimestamps(Value);
        }

        public bool GetWordWrap()
        {
            return Imports.Settings.GetWordWrap();
        }

        public void SetWordWrap(bool Value)
        {
            Imports.Settings.SetWordWrap(Value);
        }
    }
}
