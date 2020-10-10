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
            return Imports.GetAutoLoadPayload();
        }

        public void SetAutoLoadPayload(bool Value)
        {
            Imports.SetAutoLoadPayload(Value);
        }

        public bool GetStartOnBoot()
        {
            return Imports.GetStartOnBoot();
        }

        public void SetStartOnBoot(bool Value)
        {
            Imports.SetStartOnBoot(Value);
        }

        public bool GetDetectGame()
        {
            return Imports.GetDetectGame();
        }

        public void SetDetectGame(bool Value)
        {
            Imports.SetDetectGame(Value);
        }

        public string GetCOMPort()
        {
            return Imports.GetCOMPort();
        }

        public void SetCOMPort(string Value)
        {
            Imports.SetCOMPort(Value);
        }

        public int GetServicePort()
        {
            return Imports.GetServicePort();
        }

        public void SetServicePort(int Value)
        {
            Imports.SetServicePort(Value);
        }

        public int GetAPIPort()
        {
            return Imports.GetAPIPort();
        }

        public void SetAPIPort(int Value)
        {
            Imports.SetAPIPort(Value);
        }

        public bool GetCensorIDPS()
        {
            return Imports.GetCensorIDPS();
        }

        public void SetCensorIDPS(bool Value)
        {
            Imports.SetCensorIDPS(Value);
        }

        public bool GetCensorPSID()
        {
            return Imports.GetCensorPSID();
        }

        public void SetCensorPSID(bool Value)
        {
            Imports.SetCensorPSID(Value);
        }

        public bool GetDebug()
        {
            return Imports.GetDebug();
        }

        public void SetDebug(bool Value)
        {
            Imports.SetDebug(Value);
        }

        public bool GetCreateLogs()
        {
            return Imports.GetCreateLogs();
        }

        public void SetCreateLogs(bool Value)
        {
            Imports.SetCreateLogs(Value);
        }

        public bool GetShowTimestamps()
        {
            return Imports.GetShowTimestamps();
        }

        public void SetShowTimestamps(bool Value)
        {
            Imports.SetShowTimestamps(Value);
        }

        public bool GetWordWrap()
        {
            return Imports.GetWordWrap();
        }

        public void SetWordWrap(bool Value)
        {
            Imports.SetWordWrap(Value);
        }
    }
}
