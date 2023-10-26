using Limilabs.FTP.Client;
using OrbisLib2.Common;
using System.IO;

namespace OrbisLib2.Targets
{
    public class FTP
    {
        private Target Target;

        public FTP(Target Target)
        {
            this.Target = Target;
        }

        public bool DownloadFile(string RemoteFilePath, string LocalFilePath)
        {
            using (Ftp ftp = new Ftp())
            {
                try
                {
                    ftp.Connect(Target.IPAddress, Config.FTPPort);
                    ftp.Login("anonymous", "anonymous");

                    ftp.Download(RemoteFilePath, LocalFilePath);

                    ftp.Close();
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("The requested address is not valid in its context.") || ex.Message.Contains("File not found."))
                    {
                        File.Delete(LocalFilePath);
                        Console.WriteLine(ex.Message);
                        return false;
                    }
                    else if (ex.Message.Contains("No connection could be made because the target machine actively refused it.") ||
                            ex.Message.Contains("A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond."))
                    {
                        Console.WriteLine(ex.Message);
                        return false;
                    }
                    else
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                }
            }

            return true;
        }

        public void SendFile(string RemoteFilePath, string LocalFilePath)
        {
            using (Ftp ftp = new Ftp())
            {
                ftp.Connect(Target.IPAddress, Config.FTPPort);
                ftp.Login("anonymous", "anonymous");

                ftp.Upload(RemoteFilePath, LocalFilePath);

                ftp.Close();
            }
        }

        public List<FtpItem> GetDir(string Dir)
        {
            var ftpItems = new List<FtpItem>();
            using (Ftp ftp = new Ftp())
            {
                ftp.Connect(Target.IPAddress, Config.FTPPort);
                ftp.Login("anonymous", "anonymous");

                ftp.ChangeFolder(Dir);
                ftpItems = ftp.GetList();

                ftp.Close();
            }

            return ftpItems;
        }
    }
}
