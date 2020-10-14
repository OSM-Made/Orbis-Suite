using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OrbisSuite.Classes
{
    public class FTP
    {
        private Target Target;

        public FTP(Target Target)
        {
            this.Target = Target;
        }

        public static Regex FtpListDirectoryDetailsRegex = new Regex(@".*(?<month>(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))\s*(?<day>[0-9]*)\s*(?<yearTime>([0-9]|:)*)\s*(?<fileName>.*)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public List<FtpFileInfo> GetDir(string Dir)
        {
            try
            {
                string Directory = Dir;
                FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create($"ftp://{ Target.Info.IPAddr }:6904/{Directory}");

                ftp.Credentials = new NetworkCredential("anonymous", "anonymous");

                ftp.UseBinary = true;
                ftp.UsePassive = true;
                ftp.KeepAlive = false;

                ftp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                FtpWebResponse ftpResponse = (FtpWebResponse)ftp.GetResponse();

                Stream ftpStream = ftpResponse.GetResponseStream();

                StreamReader ftpReader = new StreamReader(ftpStream);

                string RawData = ftpReader.ReadToEnd();

                ftpReader.Dispose();
                ftpStream.Dispose();
                ftpResponse.Dispose();
                ftp = null;

                string[] RawArray = RawData.Split("\n".ToCharArray());
                List<FtpFileInfo> List = new List<FtpFileInfo>();

                foreach(string Member in RawArray)
                {
                    Match match = FtpListDirectoryDetailsRegex.Match(Member);
                    string FileName = match.Groups["fileName"].Value;

                    if (Member.Length == 0 || (FileName.StartsWith(".") && FileName.Length == 2))
                        continue;

                    List.Add(new FtpFileInfo(
                        Member.StartsWith("d"),
                        match.Groups["month"].Value + match.Groups["day"].Value + match.Groups["yearTime"].Value,
                        match.Groups["fileName"].Value.Replace("\r", "")
                        ));
                }

                return List;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return new List<FtpFileInfo>();
        }
    }

    public class FtpFileInfo
    {
        public bool Directory;
        //Permissions?
        public string FileCreationDate;
        public string FileName;

        public FtpFileInfo(bool Directory, string FileCreationDate, string FileName)
        {
            this.Directory = Directory;
            this.FileCreationDate = FileCreationDate;
            this.FileName = FileName;
        }
    }
}
