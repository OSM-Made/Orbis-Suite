using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using OrbisSuite;

namespace OrbisSPRXHelper
{
    class Program
    {
        static OrbisLib PS4 = new OrbisLib();

        static void Main(string[] args)
        {
            int NumberOfArgs = 0;

            //Python script parameters.
            string PRX_Directory = string.Empty; //The PRX and SPRX directors so PRX in and the SPRX goes out.
            string SPRX_Directory = string.Empty;
            string Python_Directory = string.Empty; //The directory to the python exe.
            string PythonSctipt_Directory = (Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Orbis Suite\\make_fself.py"); //The Location of our python script.
            


            //FTP Pramaters
            bool CopyToHDD = false; //If we are going to copy to the HDD.
            string FTP_Path = string.Empty; //The File Path we need to copy.
            string TargetName = string.Empty; //Custom Target Name to send SPRX to.
            bool UsingDefaultTarget = false; //If were just going to send it ot the default Target.

            try
            {
                //Fetch the incoming arguments and parse them.
                foreach (string Argument in args)
                {
                    if (Argument.Contains("-PythonDirectory"))
                        Python_Directory = args[NumberOfArgs + 1];
                    else if (Argument.Contains("-PRXDirectory"))
                    {
                        PRX_Directory = args[NumberOfArgs + 1];
                        SPRX_Directory = PRX_Directory.Replace("prx", "sprx");
                    }
                    else if (Argument.Contains("-FTPFilePath"))
                    {
                        FTP_Path = args[NumberOfArgs + 1];
                        CopyToHDD = true;
                    }
                    else if (Argument.Contains("-Target"))
                        TargetName = args[NumberOfArgs + 1];
                    else if (Argument.Contains("-DefaultTarget"))
                        UsingDefaultTarget = true;
                }

                if (TargetName == string.Empty && !UsingDefaultTarget)
                    throw new Exception("Missing Target or DefaultTarget Argument.");

                if (PRX_Directory == string.Empty)
                    throw new Exception("Missing PRXDirectory Argument.");

                if (!File.Exists(Python_Directory))
                    throw new Exception("Failed to find Python directory!");

                /*
                    PRX Fake signing thanks to Flatz.
                */

                //Delete the SPRX last built before we do anything
                if (File.Exists(SPRX_Directory))
                    File.Delete(SPRX_Directory);

                //Call the python script.
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                ProcessStartInfo start = new ProcessStartInfo
                {
                    FileName = Python_Directory,
                    Arguments = string.Format("\"{0}\" \"{1}\" \"{2}\"", PythonSctipt_Directory, PRX_Directory, SPRX_Directory),
                    UseShellExecute = false,// Do not use OS shell
                    CreateNoWindow = true // We don't need new window
                };

                //Start the program python.exe and wait for it to complete.
                p.StartInfo = start;
                p.Start();
                p.WaitForExit();
                p.Dispose();

                //We need to check and see if it was sucessful in making the fake signed prx
                if (!File.Exists(SPRX_Directory))
                    throw new Exception("Fake sign failed!");

                /*
                    FTP Upload of the SPRX. 
                */

                //If we are copying to the HDD we will copy to the desired Target or custom IP Address
                if(CopyToHDD)
                {
                    if (UsingDefaultTarget)
                    {
                        if(!PS4.DefaultTarget.FTP.SendFile(SPRX_Directory, FTP_Path + Path.GetFileName(SPRX_Directory)))
                            throw new Exception($"Failed to upload SPRX to Target {PS4.DefaultTarget.Info.Name}.");
                    } 
                    else
                    {
                        if (!PS4.Target[TargetName].FTP.SendFile(SPRX_Directory, FTP_Path + Path.GetFileName(SPRX_Directory)))
                            throw new Exception($"Failed to upload SPRX to Target {PS4.Target[TargetName].Info.Name}.");
                    }
                }
            }
            catch
            {

            }
        }
    }
}
