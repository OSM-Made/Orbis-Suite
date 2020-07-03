using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace OSLib.Classes
{
    public class Payload
    {
        public bool InjectPayload(String IP, string KernelVersion, int Socket = 9020)
        {
            try
            {
                Socket psocket;

                psocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                psocket.ReceiveTimeout = 200;
                psocket.SendTimeout = 200;
                psocket.Connect(new IPEndPoint(IPAddress.Parse(IP), Socket));

                FileStream fPayload = File.Open(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Orbis Suite\\Payload-" + KernelVersion + ".bin", FileMode.Open);

                if (fPayload.CanRead)
                {
                    byte[] PayloadBuffer = new byte[fPayload.Length];

                    if (fPayload.Read(PayloadBuffer, 0, (int)fPayload.Length) == fPayload.Length)
                    {
                        //Send Payload
                        psocket.Send(PayloadBuffer);

                        psocket.Close();

                        return true;
                    }
                    else
                    {
                        Console.WriteLine("[Payload/InjectPayload] Failed to read payload data\n");
                        psocket.Close();

                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("[Payload/InjectPayload] Kernel Version not currently supported!\n");
                    psocket.Close();

                    return false;
                }
            }
            catch
            {
                Console.WriteLine("[Payload/InjectPayload] Failed to load Payload\n");
                return false;
            }
        }

        public bool InjectPayload(String IP, byte[] PayloadBuffer, int Socket = 9020)
        {
            try
            {
                Socket psocket;

                psocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                psocket.ReceiveTimeout = 200;
                psocket.SendTimeout = 200;
                psocket.Connect(new IPEndPoint(IPAddress.Parse(IP), Socket));

                //Send Payload
                psocket.Send(PayloadBuffer);

                //close the socket connection
                psocket.Close();

                return true;
            }
            catch
            {
                Console.WriteLine("[Payload/InjectPayload] Failed to load Payload\n");
                return false;
            }
        }
    }
}
