using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DisplayControl.Log;

namespace DisplayControl.TCP
{

    /// <summary>
    ///  Simple class that manages a single tcp/ip connection.
    ///  It can send messages over a socket as ASCII encoded string and it sends events if a message is received.
    /// </summary>
    class TcpIpThread
    {
        private readonly Task mainTask;
        private readonly CancellationTokenSource cts;
        private readonly ICommunicationProxy proxy;
        private readonly Logger protocol;
        private readonly string description;
        public event EventHandler<string> MessageReceived;

        public TcpIpThread(ICommunicationProxy proxy, string decription)
        {
            this.cts = new CancellationTokenSource();
            this.mainTask = new Task(Main, cts.Token, TaskCreationOptions.LongRunning);
            this.proxy = proxy;
            this.protocol = proxy.Protocol;
            this.description = decription;
        }

        public void Start()
        {
            this.mainTask.Start();
        }

        public void Stop()
        {
            this.cts.Cancel();
        }

        public void SendData(string data)
        {
            IDataSendReceive dataSendReceive = proxy.Setup();
            if (dataSendReceive != null)
            {
                try
                {
                    dataSendReceive.SendData(data);
                }
                catch (Exception ex)
                {
                    this.protocol.LogWarning($"{description} can't send data {data}. ErrorMessage={ex.Message}");
                }
            }
        }

        private void Main()
        {
            try
            {
                while (true)
                {
                    IDataSendReceive dataSendReceive = proxy.Setup();

                    if (dataSendReceive != null && dataSendReceive.DataAvailable)
                    {
                        FireMessageReceivedAsync(dataSendReceive.ReceiveData())
                            .ContinueWith(FireMessageReceivedHandled);
                    }
                }
            }
            catch (Exception ex)
            {
                this.protocol.LogError($"{description} thread ended uncondionally. ErrorMessage={ex.Message}");
            }
        }

        private async Task FireMessageReceivedAsync(string message)
        {
            await Task.Run(() => {
                if (MessageReceived != null)
                {
                    MessageReceived(this, message);
                }
            });
        }

        private void FireMessageReceivedHandled(Task t)
        {
            if (t.IsFaulted)
            {
                this.protocol.LogWarning($"{description} error while firing message event.");
            }
        }
    }
}
