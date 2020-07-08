using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace OrbisSuite.Classes
{
    public class Payload
    {
        /// <summary>
        /// Sends Orbis Suite Payloads to Playstation 4 Console
        /// </summary>
        /// <param name="IP">PlayStation 4 IP address</param>
        /// <param name="KernelVersion">PlayStation 4 Kernel Version Ex:5.05</param>
        /// <param name="Port">Port used to recieve payload default value is 9020</param>
        public bool InjectPayload(String IP, string KernelVersion, int Port = 9020)
        {
            try
            {
                Socket psocket;

                psocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                psocket.ReceiveTimeout = 200;
                psocket.SendTimeout = 200;
                psocket.Connect(new IPEndPoint(IPAddress.Parse(IP), Port));

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

        /// <summary>
        /// Sends Custom Payloads to Playstation 4 Console
        /// </summary>
        /// <param name="IP">PlayStation 4 IP address</param>
        /// <param name="PayloadBuffer">Byte array of payload</param>
        /// <param name="Port">Port used to recieve payload default value is 9020</param>
        public bool InjectPayload(String IP, byte[] PayloadBuffer, int Port = 9020)
        {
            try
            {
                Socket psocket;

                psocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                psocket.ReceiveTimeout = 200;
                psocket.SendTimeout = 200;
                psocket.Connect(new IPEndPoint(IPAddress.Parse(IP), Port));

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
