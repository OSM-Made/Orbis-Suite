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
        private Target Target;

        public Payload(Target Target)
        {
            this.Target = Target;
        }

        /// <summary>
        /// Sends Orbis Suite Payloads to Playstation 4 Console. Payloads are read from the Orbis Suite Appdata folder with the format OrbisLib-{Firmware}.bin ex. Payload-505.bin.
        /// </summary>
        /// <param name="IP">PlayStation 4 IP address</param>
        /// <param name="KernelVersion">PlayStation 4 Kernel Version Ex:5.05</param>
        /// <param name="Port">Port used to recieve payload default value is 9020</param>
        public bool InjectPayload()
        {
            try
            {
                Socket socket;

                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.ReceiveTimeout = 1000;
                socket.SendTimeout = 1000;
                IAsyncResult result = socket.BeginConnect(Target.Info.IPAddr, Target.Info.PayloadPort, null, null);

                result.AsyncWaitHandle.WaitOne(3000, true);

                if (!socket.Connected)
                {
                    Console.WriteLine("Failed to connect to socket.");

                    socket.Close();
                    return false;
                }

                // we have connected
                socket.EndConnect(result);

                FileStream fPayload = File.Open($"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\Orbis Suite\\OrbisLib-{(Convert.ToDouble(Target.Info.Firmware) * 100).ToString()}.bin", FileMode.Open);

                if (!fPayload.CanRead)
                {
                    Console.WriteLine("Kernel Version not currently supported!");
                    socket.Close();
                    fPayload.Close();

                    return false;
                }

                byte[] PayloadBuffer = new byte[fPayload.Length];

                if (fPayload.Read(PayloadBuffer, 0, (int)fPayload.Length) != fPayload.Length)
                {
                    Console.WriteLine("Failed to read payload data");
                    socket.Close();
                    fPayload.Close();

                    return false;
                }

                //Send Payload
                socket.Send(PayloadBuffer);

                socket.Close();
                fPayload.Close();

                return true;
            }
            catch
            {
                Console.WriteLine("Failed to load Payload");
                return false;
            }
        }

        /// <summary>
        /// Sends Custom Payloads to Playstation 4 Console
        /// </summary>
        /// <param name="IP">PlayStation 4 IP address</param>
        /// <param name="PayloadBuffer">Byte array of payload</param>
        /// <param name="Port">Port used to recieve payload default value is 9020</param>
        public bool InjectPayload(byte[] PayloadBuffer)
        {
            try
            {
                Socket socket;

                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.ReceiveTimeout = 1000;
                socket.SendTimeout = 1000;
                IAsyncResult result = socket.BeginConnect(Target.Info.IPAddr, Target.Info.PayloadPort, null, null);

                result.AsyncWaitHandle.WaitOne(3000, true);

                if (!socket.Connected)
                {
                    Console.WriteLine("Failed to connect to socket.");

                    socket.Close();
                    return false;
                }

                // we have connected
                socket.EndConnect(result);

                //Send Payload
                socket.Send(PayloadBuffer);

                socket.Close();

                return true;
            }
            catch
            {
                Console.WriteLine("Failed to load Payload");
                return false;
            }
        }
    }
}
