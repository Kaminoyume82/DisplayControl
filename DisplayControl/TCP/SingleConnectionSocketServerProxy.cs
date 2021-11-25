using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DisplayControl.Log;

namespace DisplayControl.TCP
{
    public class SingleConnectionSocketServerProxy : ICommunicationProxy
    {
        private readonly bool permanentlyListenForIncomingConnections;

        private TcpListener listener;

        private ConnectionStatus connectionState;

        private TcpClient tcpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleConnectionSocketServerProxy"/> class.
        /// </summary>
        /// <param name="listeningPort"></param>
        /// <param name="permanentlyListenForIncomingConnections"></param>
        public SingleConnectionSocketServerProxy(int listeningPort, bool permanentlyListenForIncomingConnections, Logger workerLogger)
        {
            this.ListeningEndpoint = new IPEndPoint(IPAddress.Any, listeningPort);
            this.connectionState = ConnectionStatus.None;
            this.permanentlyListenForIncomingConnections = permanentlyListenForIncomingConnections;
            this.Protocol = workerLogger;
        }

        private event EventHandler<TcpConnectionStatusChangeEventArgs> ConnectionStatusChange;

        public IPEndPoint ListeningEndpoint { get; private set; }

        /// <summary>
        /// Gets or sets the tracing object.
        /// </summary>
        public Logger Protocol { get; set; }

        /// <summary>
        /// Gets a id for the connection.
        /// </summary>
        public string Id
        {
            get
            {
                return $"{this.ListeningEndpoint.Port}";
            }
        }

        public ConnectionStatus ConnectionStatus
        {
            get
            {
                return this.connectionState;
            }

            private set
            {
                if (this.connectionState != value)
                {
                    this.FireConnectionStatusChange(this.connectionState, value);
                    this.connectionState = value;
                }
            }
        }

        public void Close()
        {
            CloseConnection();
            StopListening();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IDataSendReceive Setup()
        {
            bool isConnected = this.tcpClient != null && this.tcpClient.Client.Connected;
            bool isListening = this.listener != null;

            try
            {
                if (!isConnected)
                {
                    if (!isListening)
                    {
                        this.StartListening();
                    }

                    if (this.listener.Pending())
                    {
                        this.AcceptClient();
                        if (!this.permanentlyListenForIncomingConnections)
                        {
                            this.StopListening();
                        }
                    }
                }
                else
                {
                    if (isListening && this.listener.Pending())
                    {
                        this.Protocol.LogWarning("SingleConnectionSocketServerProxy: A new connection is pending.");
                        this.CloseConnection();
                    }
                    else
                    {
                        return new SocketSendReceive(this.tcpClient.Client);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Protocol.LogWarning("SingleConnectionSocketServerProxy: Error trying to setup tcp/ip connection: " + ex.Message);
            }

            return null;
        }

        public void RegisterConnectionStatusChangeHandler(EventHandler<TcpConnectionStatusChangeEventArgs> handler)
        {
            this.ConnectionStatusChange -= handler;
            this.ConnectionStatusChange += handler;
        }

        private void FireConnectionStatusChange(ConnectionStatus oldState, ConnectionStatus newState)
        {
            this.ConnectionStatusChange?.Invoke(this, new TcpConnectionStatusChangeEventArgs() { OldStatus = oldState, NewStatus = newState });
        }

        private void StartListening()
        {
            if (this.listener == null)
            {
                this.listener = new TcpListener(this.ListeningEndpoint);
                this.listener.Start();
                ConnectionStatus = ConnectionStatus.WaitingForConnection;
            }
        }

        private void StopListening()
        {
            if (this.listener != null)
            {
                this.listener.Stop();
                this.listener = null;
            }
        }

        private void AcceptClient()
        {
            this.tcpClient = this.listener.AcceptTcpClient();
            this.ConnectionStatus = ConnectionStatus.Connected;

            IPEndPoint ep = this.tcpClient.Client.RemoteEndPoint as IPEndPoint;

            this.Protocol.LogInfo($"SingleConnectionSocketServerProxy: A new client was accepted: {ep.Address}:{ep.Port}.");
        }

        private void CloseConnection()
        {
            try
            {
                if (this.tcpClient != null)
                {
                    IPEndPoint ep = this.tcpClient.Client.RemoteEndPoint as IPEndPoint;
                    this.Protocol.LogInfo($"SingleConnectionSocketServerProxy: Closing connection with {ep.Address}:{ep.Port}.");
                    this.tcpClient.Close();
                }
            }
            catch (Exception ex)
            {
                this.Protocol.LogError($"SingleConnectionSocketServerProxy: Error while trying to close connection: " + ex.Message);
            }

            ConnectionStatus = ConnectionStatus.Closed;
            this.tcpClient = null;
        }
    }
}
