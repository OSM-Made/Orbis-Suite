using Google.Protobuf;
using OrbisLib2.Common.Database;
using OrbisLib2.Common.Database.Types;
using OrbisLib2.Common.Helpers;
using OrbisLib2.Targets;
using System.Net.Sockets;

namespace OrbisLib2.Common.API
{
    public static class API
    {
        private static readonly uint MagicNumber = 0xDEADBEEF;
        private static readonly int PacketVersion = 5;

        /// <summary>
        /// Makes an API call to the remote target.
        /// </summary>
        /// <param name="DesiredTarget">The desired target to recieve the command.</param>
        /// <param name="TimeOut">The time the socket should wait before timing out represented as seconds.</param>
        /// <param name="Command">The command to be run.</param>
        /// <param name="AdditionalCommunications">Optional lambda to send/recv additional data.</param>
        /// <returns>Returns result of the communications with the API.</returns>
        public static async Task<ResultState> SendCommand(Target DesiredTarget, int TimeOut, APICommand Command, Func<Socket, Task<ResultState>>? AdditionalCommunications = null)
        {
            // If the API isnt up were just giving up here.
            if(DesiredTarget.MutableInfo.Status < TargetStatusType.APIAvailable)
                return new ResultState { Succeeded = false, ErrorMessage = $"The API is not available on the selected target {DesiredTarget.Name} ({DesiredTarget.IPAddress})." };

            try 
            {
                (var connectionResult, var sock) = await Sockets.ConnectAsync(DesiredTarget.IPAddress, Settings.CreateInstance().APIPort, TimeOut);
                if (connectionResult)
                {
                    // Send the Magic Number.
                    await sock.SendAsync(BitConverter.GetBytes(MagicNumber), SocketFlags.None);

                    // Make sure the target is happy and ready to move on.
                    if (await sock.RecvInt32Async() != 1)
                        return new ResultState { Succeeded = false, ErrorMessage = $"The target {DesiredTarget.Name} ({DesiredTarget.IPAddress}) has rejected our initial communications." };

                    // Send the Initial Packet.
                    var initialResult = await SendNextPacket(sock, new InitialPacket { Command = (int)Command, PacketVersion = PacketVersion });

                    // Check to see if we failed here and report back the message.
                    if (!initialResult.Succeeded)
                        return initialResult;

                    // Set up the default respose.
                    var result = new ResultState { Succeeded = true, ErrorMessage = string.Empty };

                    // See if we have extra work to do.
                    if (AdditionalCommunications != null)
                        result = await AdditionalCommunications.Invoke(sock);

                    // Were done here, Clean up.
                    sock.Close();

                    // Return either the default response or the edited response from the additional communications.
                    return result;
                }
                else
                    return new ResultState { Succeeded = false, ErrorMessage = $"Failed to connect to the target {DesiredTarget.Name} ({DesiredTarget.IPAddress})." };
            }
            catch (SocketException ex)
            {
                return new ResultState { Succeeded = false, ErrorMessage = $"Failed with the error: {ex.Message}" };
            }
        }

        /// <summary>
        /// Asynchronously gets the result state of the API.
        /// </summary>
        /// <param name="s">The socket open to the API.</param>
        /// <returns>The Task that represents the asynchronous operation and returns the result state.</returns>
        public static async Task<ResultState> GetState(Socket s)
        {
            try
            {
                // Asynchronously receive the result state.
                var rawResult = await s.ReceiveSizeAsync();

                // Parse and return the result state.
                return ResultState.Parser.ParseFrom(rawResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ResultState { Succeeded = false, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// Asynchronously sends the next protobuf packet and receives the result state.
        /// </summary>
        /// <param name="s">The Socket to send the proto packet on.</param>
        /// <param name="Packet">The packet that contains the data.</param>
        /// <returns>The Task that represents the asynchronous operation and returns the result state of the packet request.</returns>
        public static async Task<ResultState> SendNextPacket(Socket s, IMessage Packet)
        {
            try
            {
                // Asynchronously send the packet.
                await s.SendSizeAsync(Packet.ToByteArray());

                // Asynchronously receive the result state.
                ResultState result = await GetState(s);

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ResultState { Succeeded = false, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// Asynchronously sends an integer (Int32) to the socket.
        /// </summary>
        /// <param name="Sock">The socket to send the integer to.</param>
        /// <param name="Value">The integer value to send.</param>
        /// <returns>A Task that represents the asynchronous operation and returns the result state of the operation.</returns>
        public static async Task<ResultState> SendInt32Async(Socket Sock, int Value)
        {
            try
            {
                // Asynchronously send the integer.
                await Sock.SendAsync(BitConverter.GetBytes(Value), SocketFlags.None);

                // Asynchronously return the parsed state.
                return await GetState(Sock);
            }
            catch (SocketException ex)
            {
                return new ResultState { Succeeded = false, ErrorMessage = $"Failed to send int. {ex.Message}" };
            }
        }
    }
}
