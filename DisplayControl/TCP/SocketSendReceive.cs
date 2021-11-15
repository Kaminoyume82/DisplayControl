using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DisplayControl.TCP
{
    /// <summary>
    /// Class manage the data transfer over a socket.
    /// </summary>
    public class SocketSendReceive : IDataSendReceive
    {
        /// <summary>
        /// The socket for the communication.
        /// </summary>
        private readonly Socket socket;

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketSendReceive"/> class.
        /// </summary>
        /// <param name="socket">Socket for communication.</param>
        public SocketSendReceive(Socket socket)
        {
            if (socket == null)
            {
                throw new ArgumentNullException(nameof(socket));
            }

            this.socket = socket;
            ////Encoding = Encoding.GetEncoding("iso-8859-1");
            Encoding = Encoding.ASCII;
        }

        /// <summary>
        /// Gets a value indicating whether data are available at the socket.
        /// </summary>
        public bool DataAvailable
        {
            get
            {
                return this.socket.Poll(0, SelectMode.SelectRead);
            }
        }

        /// <summary>
        /// Gets or sets the encoding for the communication.
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// Receive data over the socket.
        /// </summary>
        /// <returns>Received data.</returns>
        public string ReceiveData()
        {
            StringBuilder strBytesRead = new StringBuilder(64);
            int numberOfBytesAvailable;

            do
            {
                numberOfBytesAvailable = this.socket.Available;
                if (numberOfBytesAvailable == 0)
                {
                    throw new SocketException(10057);  // Disconnected
                }

                byte[] buffer = new byte[numberOfBytesAvailable];
                this.socket.Receive(buffer);
                strBytesRead.Append(Encoding.GetString(buffer));
            }
            while (this.socket.Available > 0);

            return strBytesRead.ToString();
        }

        /// <summary>
        /// Send data over the socket.
        /// </summary>
        /// <param name="data">Data to send.</param>
        public void SendData(string data)
        {
            int bytesSend = 0;
            int result;
            byte[] messageBuffer = Encoding.GetBytes(data);

            while (bytesSend < messageBuffer.Length)
            {
                result = this.socket.Send(messageBuffer, bytesSend, messageBuffer.Length - bytesSend, SocketFlags.None);
                if (result <= 0)
                {
                    throw new SocketException(10057);   // Disconnected
                }

                bytesSend += result;
            }
        }
    }
}
