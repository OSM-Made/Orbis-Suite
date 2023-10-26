using OrbisLib2.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrbisLib2.Targets
{
    public class Payload
    {
        private Target Target;

        public Payload(Target Target)
        {
            this.Target = Target;
        }

        /// <summary>
        /// Sends payloads to the target console.
        /// </summary>
        /// <param name="payloadBuffer">Byte array of payload data.</param>
        public async Task<bool> InjectPayload(byte[] payloadBuffer)
        {
            (var connectionResult, var sock) = await Sockets.ConnectAsync(Target.IPAddress, Target.PayloadPort, 1000);

            if (!connectionResult)
                return false;

            var bytesSent = await sock.SendAsync(payloadBuffer);

            sock.Close();

            return (bytesSent == payloadBuffer.Length);
        }
    }
}
