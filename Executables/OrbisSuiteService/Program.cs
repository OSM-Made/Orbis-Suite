using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace OrbisSuiteService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if DEBUG
            OrbisSuiteService Service = new OrbisSuiteService();

            Service.OnDebug();
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new OrbisSuiteService()
            };
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}
