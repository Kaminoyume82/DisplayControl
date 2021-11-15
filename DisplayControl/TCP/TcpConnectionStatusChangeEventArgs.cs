using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayControl.TCP
{
    /// <summary>
    /// Connection status change event arguments.
    /// </summary>
    public class TcpConnectionStatusChangeEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the old state.
        /// </summary>
        public ConnectionStatus OldStatus { get; set; }

        /// <summary>
        /// Gets or sets the new state.
        /// </summary>
        public ConnectionStatus NewStatus { get; set; }
    }
}
