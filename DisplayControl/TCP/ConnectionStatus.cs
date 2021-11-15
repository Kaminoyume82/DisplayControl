using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayControl.TCP
{
    public enum ConnectionStatus
    {
        /// <summary>
        /// Initial status.
        /// </summary>
        None,

        /// <summary>
        /// The BeginCommunication method is called.
        /// </summary>
        Started,

        /// <summary>
        /// Waiting to connect.
        /// </summary>
        WaitingForConnection,

        /// <summary>
        /// The connection is established.
        /// </summary>
        Connected,

        /// <summary>
        /// The connection is closed.
        /// </summary>
        Closed,

        /// <summary>
        /// An unexpected error occured. The connection/thread has terminated.
        /// </summary>
        Error
    }
}
