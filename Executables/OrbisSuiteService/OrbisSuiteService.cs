using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace OrbisSuiteService
{
    public partial class OrbisSuiteService : ServiceBase
    {
        string CurrentTarget = "", CurrentProcess = "";
        bool IsConnected = false, IsAttached = false;

        private enum CustomCommands : int
        {
            //Target
            CMD_GET_CURRENT_TARGET = 0,
            CMD_SET_CURRENT_TARGET,
            CMD_CONNECT_TO_TARGET,
            CMD_DISCONNECT_FROM_TARGET,
            CMD_GET_CURRENT_PROCESS,
            CMD_SET_CURRENT_PROCESS,

            //Target Management
            CMD_SET_DEFAULT_TARGET,
            CMD_GET_DEFAULT_TARGET,
            CMD_ADD_NEW_TARGET,
            CMD_DELETE_TARGET,
            CMD_UPDATE_TARGET,
            CMD_GET_TARGET_INFO,
            CMD_GET_TARGET_LIST,

            //Callbacks
            CMD_CLIENT_CONNECT,
            CMD_CLIENT_DISCONNECT,
            CMD_CLIENT_PING
        };

        private enum ClientCallbacks //Make in c++?
        {
            CLIENT_PRINT,
            CLIENT_STOP,
            CLIENT_PAUSE,
            CLIENT_RESUME,
            CLIENT_PROCESS_KILLED,
            CLIENT_TARGET_KILLED,
        };

        public OrbisSuiteService()
        {
            InitializeComponent();
        }

        protected override void OnCustomCommand(int command)
        {
            switch ((CustomCommands)command)
            {
                default:
                    Console.WriteLine("[OrbisSuiteService] Invalid Command Recieved\n");
                    break;
                case CustomCommands.CMD_GET_CURRENT_TARGET:

                    break;
            }
        }

        protected override void OnStart(string[] args)
        {

        }

        protected override void OnStop()
        {

        }
    }
}
