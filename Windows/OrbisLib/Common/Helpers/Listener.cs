using System.Net.Sockets;
using System.Net;

namespace OrbisLib2.Common.Helpers
{
    public class Listener
    {
        private Socket s_Listener;

        public bool Listening
        {
            get;
            private set;
        }

        public int Port
        {
            get;
            private set;
        }

        public Listener(int Port)
        {
            this.Port = Port;
        }

        public void Start()
        {
            //Make sure we have not started before.
            if (Listening)
                return;

            //Create Socket to listen on.
            s_Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //Bind Socket to Port and Listen with a backlog of 100.
            s_Listener.Bind(new IPEndPoint(0, Port));
            s_Listener.Listen(100);

            //Call BeginAccept with our call back so on every accept of a new socket connection the call back is called.
            s_Listener.BeginAccept(CallBack, null);
            Listening = true;
        }

        public void Stop()
        {
            if (!Listening)
                return;

            //Clean up for next connection.
            s_Listener.Close();
            s_Listener.Dispose();

            Listening = false;
        }

        /*
            Call back to handle all of our incoming connections and call our event.
        */
        void CallBack(IAsyncResult ar)
        {
            try
            {
                //Complete the Accept of the incoming Connection and call the event if registered.
                Socket s_Client = s_Listener.EndAccept(ar);
                if (SocketAccepted != null)
                {
                    Task.Run(() => SocketAccepted(s_Client));
                }

                //Begin Accepting other Connections again with our call back.
                s_Listener.BeginAccept(CallBack, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public delegate void SocketAcceptedHandler(Socket e);
        public event SocketAcceptedHandler SocketAccepted;
    }
}
