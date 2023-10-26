using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace OrbisLib2.Common.Helpers
{
    public static class Sockets
    {
        /// <summary>
        /// Connects to the socket asynchronously.
        /// </summary>
        /// <param name="address">IP Address of the remote.</param>
        /// <param name="port">The port of the remote.</param>
        /// <param name="timeOut">The time we should wait before timing out represented as milliseconds.</param>
        /// <returns>Returns a Task that represents the result of the connection attempt (true if successful, false otherwise) along with the Socket created when connecting.</returns>
        public static async Task<(bool, Socket)> ConnectAsync(string address, int port, int timeOut)
        {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            bool connected = await sock.EasyConnectAsync(address, port, timeOut);

            return (connected, sock);
        }

        /// <summary>
        /// Recieve large amounts of data from a socket that is larger than the recieve buffer size.
        /// </summary>
        /// <param name="s">The current socket.</param>
        /// <param name="data">The data to be recieved.</param>
        public static int RecvLarge(this Socket s, byte[] data)
        {
            int Left = data.Length;
            int Received = 0;

            try
            {
                while (Left > 0)
                {
                    var chunkSize = Math.Min(8192, Left);
                    var res = s.Receive(data, Received, chunkSize, 0);

                    Received += res;
                    Left -= res;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Received;
        }

        /// <summary>
        /// Asynchronously receives large amounts of data from a socket that may be larger than the receive buffer size.
        /// </summary>
        /// <param name="s">The current socket.</param>
        /// <param name="data">The data to be received.</param>
        /// <returns>A Task that represents the asynchronous operation and returns the total number of bytes received.</returns>
        public static async Task<int> RecvLargeAsync(this Socket s, byte[] data)
        {
            int left = data.Length;
            int received = 0;

            try
            {
                while (left > 0)
                {
                    var chunkSize = Math.Min(8192, left);
                    var res = await s.ReceiveAsync(new ArraySegment<byte>(data, received, chunkSize), SocketFlags.None);

                    received += res;
                    left -= res;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return received;
        }

        public static void SendLarge(this Socket s, byte[] data)
        {
            try
            {
                int Left = data.Length;
                int CurrentPosition = 0;

                while (Left > 0)
                {
                    var chunkSize = Math.Min(8192, Left);
                    var res = s.Send(data, CurrentPosition, chunkSize, 0);

                    Left -= res;
                    CurrentPosition += res;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Asynchronously sends a large byte array over the socket.
        /// </summary>
        /// <param name="s">The Socket to send the data over.</param>
        /// <param name="data">The byte array to send.</param>
        /// <returns>A Task that represents the asynchronous operation.</returns>
        public static async Task SendLargeAsync(this Socket s, byte[] data)
        {
            try
            {
                int left = data.Length;
                int currentPosition = 0;

                while (left > 0)
                {
                    var chunkSize = Math.Min(8192, left);
                    var res = await s.SendAsync(new ArraySegment<byte>(data, currentPosition, chunkSize), SocketFlags.None);

                    left -= res;
                    currentPosition += res;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Sends an int32 over socket.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="Data"></param>
        public static void SendInt32(this Socket s, int Data)
        {
            s.Send(BitConverter.GetBytes(Data));
        }

        /// <summary>
        /// Asynchronously sends an integer (Int32) over the socket.
        /// </summary>
        /// <param name="s">The socket to send the integer over.</param>
        /// <param name="Data">The integer value to send.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public static async Task SendInt32Async(this Socket s, int Data)
        {
            await s.SendAsync(BitConverter.GetBytes(Data), SocketFlags.None);
        }

        /// <summary>
        /// Receives an int32 over sockets.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int RecvInt32(this Socket s)
        {
            var Data = new byte[sizeof(int)];
            s.Receive(Data);
            return BitConverter.ToInt32(Data, 0);
        }

        /// <summary>
        /// Asynchronously receives an integer (Int32) from the socket.
        /// </summary>
        /// <param name="s">The Socket from which to receive the integer.</param>
        /// <returns>A Task that represents the asynchronous operation and returns the received integer (Int32).</returns>
        public static async Task<int> RecvInt32Async(this Socket s)
        {
            // Allocate a buffer to store the received data.
            var data = new byte[sizeof(int)];

            // Asynchronously receive data into the buffer.
            await s.ReceiveAsync(data, SocketFlags.None);

            // Convert the received bytes to an integer and return it.
            return BitConverter.ToInt32(data, 0);
        }

        /// <summary>
        /// Attempts to ping a host.
        /// </summary>
        /// <param name="Host">Host Address</param>
        /// <returns></returns>
        public static bool PingHost(string Host)
        {
            try
            {
                var pingSender = new Ping();
                var options = new PingOptions();
                options.DontFragment = true;

                var reply = pingSender.Send(Host, 120, Encoding.ASCII.GetBytes("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"), options);
                return (reply.Status == IPStatus.Success);
            }
            catch
            {

            }

            return false;
        }

        /// <summary>
        /// Tests the availability of a tcp host.
        /// </summary>
        /// <param name="Host">Host Address.</param>
        /// <param name="Port">Host Port.</param>
        /// <returns></returns>
        public static bool TestTcpConnection(string Host, int Port)
        {
            try
            {
                var client = new TcpClient();
                var result = client.BeginConnect(Host, Port, null, null);

                var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));

                if (!success)
                {
                    client.Close(); // Close the TcpClient if the connection attempt fails.
                    return false;
                }

                client.EndConnect(result); // Complete the connection.

                // Get the network stream for sending data.
                NetworkStream stream = client.GetStream();

                // Send the byte array over the network stream.
                byte[] byteArray = BitConverter.GetBytes(0xFEED);
                stream.Write(byteArray, 0, byteArray.Length);

                // Close the network stream and client when done.
                stream.Close();
                client.Close();

                return true;
            }
            catch
            {
                
            }

            return false;
        }

        /// <summary>
        /// Easily connect to a socket asynchronously and handle the time out.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="address">The address we would like to connect to.</param>
        /// <param name="port">The port of the socket we would like to connect to.</param>
        /// <param name="timeOut">The time we would like to wait for connection in milliseconds.</param>
        /// <returns>Returns a Task that represents the result of the connection attempt (true if successful, false otherwise).</returns>
        public static async Task<bool> EasyConnectAsync(this Socket s, string address, int port, int timeOut)
        {
            try
            {
                s.ReceiveTimeout = s.SendTimeout = timeOut;
                var connectTask = Task.Factory.FromAsync(s.BeginConnect, s.EndConnect, address, port, null);

                // Wait for the connection attempt with a timeout.
                if (await Task.WhenAny(connectTask, Task.Delay(timeOut)) == connectTask)
                {
                    if (s.Connected)
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Failed to connect to socket.");
                        s.Close();
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Connection attempt timed out.");
                    s.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while connecting: " + ex.Message);
                s.Close();
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte[] ReceiveSize(this Socket s)
        {
            // First we get the size of the request packet.
            var packetSize = s.RecvInt32();

            // Allocate space and recieve the data.
            var packet = new byte[packetSize];
            s.RecvLarge(packet);

            // return the result
            return packet;
        }

        /// <summary>
        /// Asynchronously receives data with a specified size from the socket.
        /// </summary>
        /// <param name="s">The Socket from which to receive the data.</param>
        /// <returns>A Task that represents the asynchronous operation and returns the received data as a byte array.</returns>
        public static async Task<byte[]> ReceiveSizeAsync(this Socket s)
        {
            try
            {
                // First, receive the size of the data packet as an Int32.
                int packetSize = await s.RecvInt32Async();

                // Allocate space and asynchronously receive the data.
                var packet = new byte[packetSize];
                await s.RecvLargeAsync(packet);

                // Return the received data as a byte array.
                return packet;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new byte[0]; // Return an empty byte array on error or exception.
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="data"></param>
        public static void SendSize(this Socket s, byte[] data)
        {
            s.SendLarge(BitConverter.GetBytes(data.Length).Concat(data).ToArray());
        }

        /// <summary>
        /// Asynchronously sends data with its size prepended to the socket.
        /// </summary>
        /// <param name="s">The Socket to send the data to.</param>
        /// <param name="data">The byte array of data to send.</param>
        /// <returns>A Task that represents the asynchronous operation.</returns>
        public static async Task SendSizeAsync(this Socket s, byte[] data)
        {
            // Get the size of the data as a byte array and prepend it to the data.
            byte[] sizeBytes = BitConverter.GetBytes(data.Length);
            byte[] combinedData = sizeBytes.Concat(data).ToArray();

            // Asynchronously send the combined data.
            await s.SendLargeAsync(combinedData);
        }
    }
}
