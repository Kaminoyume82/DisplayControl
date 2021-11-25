using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DisplayControl.Log;

namespace DisplayControl.TCP
{
    /// <summary>
    /// Represents a socket client connection.
    /// </summary>
    public class SingleConnectionSocketClientProxy : ICommunicationProxy
    {
        /// <summary>
        /// The endpoint for the server.
        /// </summary>
        private readonly IPEndPoint server;

        /// <summary>
        /// The TCP client.
        /// </summary>
        private TcpClient tcpClient;

        /// <summary>
        /// The current connection state.
        /// </summary>
        private ConnectionStatus connectionState;

        /// <summary>
        /// Task for a running connection attempt.
        /// </summary>
        private Task connectionAttemptTask;

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleConnectionSocketClientProxy" /> class.
        /// </summary>
        /// <param name="serverToConnectTo">The server endpoint for the connection.</param>
        public SingleConnectionSocketClientProxy(IPEndPoint serverToConnectTo, Logger workerLogger)
        {
            if (serverToConnectTo == null)
            {
                throw new ArgumentNullException(nameof(serverToConnectTo));
            }

            this.server = serverToConnectTo;
            this.Protocol = workerLogger;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleConnectionSocketClientProxy" /> class.
        /// </summary>
        /// <param name="serverToConnectTo">The server endpoint for the connection.</param>
        /// <param name="bindingPort">The port for the client-side binding.</param>
        public SingleConnectionSocketClientProxy(IPEndPoint serverToConnectTo, int bindingPort, Logger workerLogger)
            : this(serverToConnectTo, workerLogger)
        {
            if (bindingPort < IPEndPoint.MinPort || bindingPort > IPEndPoint.MaxPort)
            {
                throw new ArgumentException($"Must be in range {IPEndPoint.MinPort}-{IPEndPoint.MaxPort}", nameof(bindingPort));
            }

            this.BindingPort = bindingPort;
        }

        /// <summary>
        /// This event is fired then the TCP connection state changes.
        /// </summary>
        private event EventHandler<TcpConnectionStatusChangeEventArgs> ConnectionStatusChange;

        /// <summary>
        /// Gets the connection status.
        /// </summary>
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

        /// <summary>
        /// Gets the binding port. Can be set to define a specific port for client communication with server.
        /// </summary>
        public int BindingPort { get; private set; } = -1;

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
                return $"{this.server}";
            }
        }

        /// <summary>
        /// Open a connection if no connection exist.
        /// </summary>
        /// <returns>Connection interface if connection exist. If no connection exist return value is null.</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IDataSendReceive Setup()
        {
            bool isConnected = this.tcpClient != null && this.tcpClient.Client.Connected;
            bool alreadyTryingToConnect = this.connectionAttemptTask != null;

            try
            {
                if (isConnected)
                {
                    return new SocketSendReceive(this.tcpClient.Client);
                }
                else if (!alreadyTryingToConnect)
                {
                    if (this.ConnectionStatus != ConnectionStatus.WaitingForConnection)
                    {
                        this.Protocol.LogInfo($"SingleConnectionSocketClientProxy: Trying to connect with {this.server.Address}:{this.server.Port}.");
                    }

                    this.tcpClient = this.CreateTcpClient();
                    this.connectionAttemptTask = this.tcpClient.ConnectAsync(this.server.Address, this.server.Port);
                    this.connectionAttemptTask.ContinueWith(this.OnConnectionAttemptTaskFinished);
                    this.ConnectionStatus = ConnectionStatus.WaitingForConnection;
                }
            }
            catch (Exception ex)
            {
                this.Protocol.LogError("SingleConnectionSocketClientProxy: Error trying to setup tcp/ip connection: " + ex.Message);
                ConnectionStatus = ConnectionStatus.Error;
            }

            return null;
        }

        /// <summary>
        /// Close the open connection.
        /// </summary>
        public void Close()
        {
            try
            {
                if (this.connectionAttemptTask != null)
                {
                    this.connectionAttemptTask.Wait();
                }

                if (this.tcpClient != null)
                {
                    this.Protocol.LogInfo($"SingleConnectionSocketClientProxy: Closing connection with {server.Address}:{server.Port}.");
                    this.tcpClient.Close();
                }
            }
            catch (Exception ex)
            {
                string exceptionMessage = string.Format(
                    CultureInfo.InvariantCulture,
                    "SingleConnectionSocketClientProxy: Error while trying to close connection. {0} / {1}",
                    ex.Message,
                    ex.InnerException != null ? ex.InnerException.Message : string.Empty);

                this.Protocol.LogError(exceptionMessage);
            }

            ConnectionStatus = ConnectionStatus.Closed;
            this.tcpClient = null;
        }

        /// <summary>
        /// Add an status changed event to the event handler. The event will be removed first if exist before added, so an event will not be add twice.
        /// </summary>
        /// <param name="handler">Status changed event.</param>
        public void RegisterConnectionStatusChangeHandler(EventHandler<TcpConnectionStatusChangeEventArgs> handler)
        {
            this.ConnectionStatusChange -= handler;
            this.ConnectionStatusChange += handler;
        }

        /// <summary>
        /// Fires the connection state changed event.
        /// </summary>
        /// <param name="oldState">The old state.</param>
        /// <param name="newState">The new state.</param>
        private void FireConnectionStatusChange(ConnectionStatus oldState, ConnectionStatus newState)
        {
            this.ConnectionStatusChange?.Invoke(this, new TcpConnectionStatusChangeEventArgs() { OldStatus = oldState, NewStatus = newState });
        }

        /// <summary>
        /// Called then a connection attempt is finished.
        /// </summary>
        /// <param name="connectTask">The finished connection task.</param>
        private void OnConnectionAttemptTaskFinished(Task connectTask)
        {
            if (this.tcpClient.Connected)
            {
                this.Protocol.LogInfo($"SingleConnectionSocketClientProxy: Connected with {this.server.Address}:{this.server.Port}.");
                ConnectionStatus = ConnectionStatus.Connected;
            }
            else
            {
                if (ConnectionStatus != ConnectionStatus.WaitingForConnection)
                {
                    string connectTaskExceptionMessage = connectTask.Exception != null ? connectTask.Exception.Message : "<no message>";
                    string exceptionMessage =
                        $"SingleConnectionSocketClientProxy: Connection attempt failed to complete. {connectTaskExceptionMessage}";

                    if (connectTask.Exception != null && connectTask.Exception.InnerExceptions.Count > 0)
                    {
                        exceptionMessage += " " + string.Join(" / ", connectTask.Exception.InnerExceptions.Select(ex => ex.Message));
                    }

                    this.Protocol.LogWarning(exceptionMessage);
                    ConnectionStatus = ConnectionStatus.WaitingForConnection;
                }
            }

            this.connectionAttemptTask = null;
        }

        /// <summary>
        /// Creates a TCP client.
        /// </summary>
        /// <returns>Returns the newly created TCP client.</returns>
        private TcpClient CreateTcpClient()
        {
            if (this.BindingPort > -1)
            {
                IPEndPoint localbinding = new IPEndPoint(IPAddress.Any, this.BindingPort);
                return new TcpClient(localbinding);
            }

            return new TcpClient();
        }
    }
}
