using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OSLib.Classes
{
    public class TargetListUpdate : EventArgs
    {
        public List<Targets> List { get; private set; }

        public TargetListUpdate(List<Targets> List)
        {
            this.List = List;
        }
    }

    public class TargetManagement
    {
        internal OrbisLib PS4;
        public event EventHandler<TargetListUpdate> TargetListUpdate;

        string DefaultTarget = "";
        string DefualtTargetIP = "";

        internal void UpdateDefaultTarget()
        {
            //Update Default Target Name
            try
            {
                using (SQLiteConnection SQLDB = new SQLiteConnection(@"Data Source=" + Path.Combine((Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Orbis Suite\\"), "Orbis3-User-Data.db")))
                {
                    SQLDB.Open();
                    using (SQLiteCommand Command = new SQLiteCommand(@"SELECT * FROM Settings", SQLDB))
                    {
                        SQLiteDataReader Reader = Command.ExecuteReader();

                        if (Reader.Read())
                            DefaultTarget = Convert.ToString(Reader["DefaultConsole"]);

                        Reader.Dispose();
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error:\n " + ex.ToString());
            }

            //Update Default Target IP Address
            try
            {
                using (SQLiteConnection SQLDB = new SQLiteConnection(@"Data Source=" + Path.Combine((Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Orbis Suite\\"), "Orbis3-User-Data.db")))
                {
                    SQLDB.Open();
                    using (SQLiteCommand Command = new SQLiteCommand(@"SELECT * FROM Consoles where ConsoleName=@ConsoleName", SQLDB))
                    {
                        Command.Parameters.AddWithValue("@ConsoleName", DefaultTarget);

                        SQLiteDataReader Reader = Command.ExecuteReader();

                        if (Reader.Read())
                            DefualtTargetIP = Convert.ToString(Reader["IPAddress"]);

                        Reader.Dispose();
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error:\n " + ex.ToString());
            }
        }

        public TargetManagement(OrbisLib InPS4)
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = (Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Orbis Suite\\");
            watcher.Filter = "Orbis3-User-Data.db";
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Changed += Watcher_Changed;
            watcher.EnableRaisingEvents = true;

            UpdateDefaultTarget();

            PS4 = InPS4;
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            UpdateDefaultTarget();

            if (TargetListUpdate != null)
                TargetListUpdate(this, new TargetListUpdate(GetTargets()));
        }

        public int AddNewTarget(string Name, string IP, string FW)
        {
            IPAddress IPAddr;
            if (!IPAddress.TryParse(IP, out IPAddr))
                return 0;

            if (Name.Equals(""))
                return -1;

            try
            {
                using (SQLiteConnection SQLDB = new SQLiteConnection(@"Data Source=" + Path.Combine((Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Orbis Suite\\"), "Orbis3-User-Data.db")))
                {
                    SQLDB.Open();
                    using (SQLiteCommand Command = new SQLiteCommand(SQLDB))
                    {
                        Command.CommandText = "SELECT count(*) FROM Consoles WHERE ConsoleName=@ConsoleName";
                        Command.Parameters.AddWithValue("@ConsoleName", Name);

                        int count = Convert.ToInt32(Command.ExecuteScalar());
                        if (count == 0)
                        {
                            Command.CommandText = @"INSERT into Consoles (ConsoleName, IPAddress, Firmware) values (@ConsoleName, @IPAddress, @Firmware)";
                            Command.Parameters.AddWithValue("@ConsoleName", Name);
                            Command.Parameters.AddWithValue("@IPAddress", IP);
                            Command.Parameters.AddWithValue("@Firmware", FW);
                            Command.ExecuteNonQuery();

                            Command.CommandText = @"SELECT * FROM Settings";
                            SQLiteDataReader Reader = Command.ExecuteReader();

                            string DefaultConsole = "";
                            if (Reader.Read())
                            {
                                DefaultConsole = Convert.ToString(Reader["DefaultConsole"]);
                            }

                            Reader.Dispose();

                            if (DefaultConsole.Equals(""))
                            {
                                Command.CommandText = @"update Settings set DefaultConsole=@DefaultConsole";
                                Command.Parameters.AddWithValue("@DefaultConsole", Name);
                                Command.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            Command.CommandText = @"UPDATE Consoles set IPAddress=@IPAddress, Firmware=@Firmware where ConsoleName=@ConsoleName";
                            Command.Parameters.AddWithValue("@ConsoleName", Name);
                            Command.Parameters.AddWithValue("@IPAddress", IP);
                            Command.Parameters.AddWithValue("@Firmware", FW);
                            Command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error:\n " + ex.ToString());
            }

            return 1;
        }

        public bool UpdateTarget(string OriginalTargetName, string NewTargetName, string NewTargetIP, string NewTargetFW)
        {
            IPAddress IPAddr;
            if (!IPAddress.TryParse(NewTargetIP, out IPAddr))
                return false;

            if (NewTargetName.Equals(""))
                return false;

            try
            {
                using (SQLiteConnection SQLDB = new SQLiteConnection(@"Data Source=" + Path.Combine((Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Orbis Suite\\"), "Orbis3-User-Data.db")))
                {
                    SQLDB.Open();
                    using (SQLiteCommand Command = new SQLiteCommand(SQLDB))
                    {
                        Command.CommandText = @"UPDATE Consoles set IPAddress=@IPAddress, Firmware=@Firmware, ConsoleName=@ConsoleName where ConsoleName=@OldConsoleName";
                        Command.Parameters.AddWithValue("@ConsoleName", NewTargetName);
                        Command.Parameters.AddWithValue("@IPAddress", NewTargetIP);
                        Command.Parameters.AddWithValue("@Firmware", NewTargetFW);
                        Command.Parameters.AddWithValue("@OldConsoleName", OriginalTargetName);
                        Command.ExecuteNonQuery();
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error:\n " + ex.ToString());
            }

            return true;
        }

        public string GetTargetIP(string Name)
        {
            string ConsoleIP = "";
            try
            {
                using (SQLiteConnection SQLDB = new SQLiteConnection(@"Data Source=" + Path.Combine((Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Orbis Suite\\"), "Orbis3-User-Data.db")))
                {
                    SQLDB.Open();
                    using (SQLiteCommand Command = new SQLiteCommand(@"SELECT * FROM Consoles where ConsoleName=@ConsoleName", SQLDB))
                    {
                        Command.Parameters.AddWithValue("@ConsoleName", Name);

                        SQLiteDataReader Reader = Command.ExecuteReader();

                        if (Reader.Read())
                            ConsoleIP = Convert.ToString(Reader["IPAddress"]);

                        Reader.Dispose();
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error:\n " + ex.ToString());
            }

            return ConsoleIP;
        }

        public string GetTargetIP()
        {
            string DefaultConsole = GetDefaultTarget();
            if (DefaultConsole.Equals(""))
                return "Error";

            return DefualtTargetIP;
        }

        public string GetTargetFW(string Name)
        {
            string ConsoleFirmware = "";
            try
            {
                using (SQLiteConnection SQLDB = new SQLiteConnection(@"Data Source=" + Path.Combine((Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Orbis Suite\\"), "Orbis3-User-Data.db")))
                {
                    SQLDB.Open();
                    using (SQLiteCommand Command = new SQLiteCommand(@"SELECT * FROM Consoles where ConsoleName=@ConsoleName", SQLDB))
                    {
                        Command.Parameters.AddWithValue("@ConsoleName", Name);

                        SQLiteDataReader Reader = Command.ExecuteReader();

                        if (Reader.Read())
                            ConsoleFirmware = Convert.ToString(Reader["Firmware"]);

                        Reader.Dispose();
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error:\n " + ex.ToString());
            }

            return ConsoleFirmware;
        }

        public string GetTargetFW()
        {
            string DefaultConsole = GetDefaultTarget();
            if (DefaultConsole.Equals(""))
                return "";

            return GetTargetFW(DefaultConsole);
        }

        public bool DeleteTarget(string Name)
        {
            if (!GetDefaultTarget().Equals(Name))
            {
                try
                {
                    using (SQLiteConnection SQLDB = new SQLiteConnection(@"Data Source=" + Path.Combine((Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Orbis Suite\\"), "Orbis3-User-Data.db")))
                    {
                        SQLDB.Open();
                        using (SQLiteCommand Command = new SQLiteCommand(@"DELETE FROM Consoles where ConsoleName=@ConsoleName", SQLDB))
                        {
                            Command.Parameters.AddWithValue("@ConsoleName", Name);
                            Command.ExecuteNonQuery();
                        }
                    }
                }
                catch (SQLiteException ex)
                {
                    Console.WriteLine("Error:\n " + ex.ToString());
                }

                return true;
            }

            return false;
        }

        public string GetDefaultTarget()
        {
            return DefaultTarget;
        }

        public void SetDefaultTarget(string Name)
        {
            try
            {
                using (SQLiteConnection SQLDB = new SQLiteConnection(@"Data Source=" + Path.Combine((Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Orbis Suite\\"), "Orbis3-User-Data.db")))
                {
                    SQLDB.Open();
                    using (SQLiteCommand Command = new SQLiteCommand(@"update Settings set DefaultConsole=@DefaultConsole", SQLDB))
                    {
                        Command.Parameters.AddWithValue("@DefaultConsole", Name);
                        Command.ExecuteNonQuery();
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error:\n " + ex.ToString());
            }
        }

        public List<Targets> GetTargets()
        {
            List<Targets> List = new List<Targets>();

            try
            {
                using (SQLiteConnection SQLDB = new SQLiteConnection(@"Data Source=" + Path.Combine((Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Orbis Suite\\"), "Orbis3-User-Data.db")))
                {
                    SQLDB.Open();
                    using (SQLiteCommand Command = new SQLiteCommand(@"SELECT * FROM Consoles", SQLDB))
                    {
                        SQLiteDataReader Reader = Command.ExecuteReader();

                        while (Reader.Read())
                            List.Add(new Targets(Convert.ToString(Reader["ConsoleName"]), Convert.ToString(Reader["IPAddress"]), Convert.ToString(Reader["Firmware"])));

                        Reader.Dispose();
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error:\n " + ex.ToString());
            }

            return List;
        }

        public void AddTargetPrompt()
        {
            //TODO: Add dialog
            /*Add_Target AddTarget = new Add_Target(PS4);
            if (AddTarget.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                AddTarget.Close();
            }*/
        }
    }

    public class Targets
    {
        public string Name;
        public string IPAddress;
        public string FirmWare;

        public Targets(string Name, string IPAddress, string FirmWare)
        {
            this.Name = Name;
            this.IPAddress = IPAddress;
            this.FirmWare = FirmWare;
        }
    }
}
