using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayControl.TCP
{
    public interface IDataSendReceive
    {
        Encoding Encoding { get; set; }

        bool DataAvailable { get; }

        string ReceiveData();

        void SendData(string data);
    }
}
