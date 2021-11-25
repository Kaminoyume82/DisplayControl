using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DisplayControl.Log;

namespace DisplayControl.TCP
{
    public interface ICommunicationProxy
    {
        string Id { get; }

        Logger Protocol { get; set; }

        ConnectionStatus ConnectionStatus { get; }

        IDataSendReceive Setup();

        void Close();

        void RegisterConnectionStatusChangeHandler(EventHandler<TcpConnectionStatusChangeEventArgs> handler);
    }
}
