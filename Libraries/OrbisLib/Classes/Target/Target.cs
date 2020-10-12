﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OrbisSuite.Classes
{
    public class Target
    {
        private OrbisLib PS4;
        public TargetEvents Events;
        public Debug Debug;
        public Payload Payload;
        public Process Process;
        public FTP FTP;
        public TargetInfo Info;

        public Target(OrbisLib PS4, TargetInfo TargetInfo)
        {
            Info = TargetInfo;
            this.PS4 = PS4;
            Events = new TargetEvents(this);
            Debug = new Debug(this);
            Payload = new Payload(this);
            Process = new Process(PS4, this);
            FTP = new FTP(this);
        }

        public bool Edit(bool Default, string Name, string IPAddr, int Firmware)
        {
            return Imports.SetTarget(Info.Name, Default, Name, IPAddr, Firmware);
        }

        public void SetDefault()
        {
            Imports.SetDefault(Info.Name);
        }

        public bool Delete()
        {
            return Imports.DeleteTarget(Info.Name);
        }

        public int Shutdown()
        {
            return Imports.Shutdown(Info.IPAddr);
        }

        public int Reboot()
        {
            return Imports.Reboot(Info.IPAddr);
        }

        public int Suspend()
        {
            return Imports.Suspend(Info.IPAddr);
        }

        public int Notify(string Message)
        {
            return Imports.Notify(Info.IPAddr, -1, Message);
        }

        public int Notify(int Type, string Message)
        {
            return Imports.Notify(Info.IPAddr, Type, Message);
        }

        public int Beep(int Count)
        {
            return Imports.DoBeep(Info.IPAddr, Count);
        }

        public int SetLED(byte R, byte G, byte B, byte A)
        {
            return Imports.SetLED(Info.IPAddr, R, G, B, A);
        }

        public int GetLED(out byte R, out byte G, out byte B, out byte A)
        {
            return Imports.GetLED(Info.IPAddr, out R, out G, out B, out A);
        }

        public int DumpProcess(string ProcName, out UInt64 Size, byte[] Out)
        {
            return Imports.DumpProcess(Info.IPAddr, ProcName, out Size, Out);
        }
    }
}