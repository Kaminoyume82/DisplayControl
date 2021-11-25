using System;
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
        private readonly CancellationTokenSource cts;
        private readonly string description;
        private readonly Task mainTask;
        private readonly Logger protocol;
        private readonly ICommunicationProxy proxy;

        public TcpIpThread(ICommunicationProxy proxy, string decription)
        {
            this.cts = new CancellationTokenSource();
            this.mainTask = new Task(Main, cts.Token, TaskCreationOptions.LongRunning);
            this.proxy = proxy;
            this.protocol = proxy.Protocol;
            this.description = $"{decription}({proxy.Id})";
        }

        public event EventHandler<string> MessageReceived;

        public string Id => proxy.Id;

        public string RequestDeviceStatusData { get; set; }

        public void RegisterConnectionStatusChangeHandler(EventHandler<TcpConnectionStatusChangeEventArgs> handler)
        {
            this.proxy.RegisterConnectionStatusChangeHandler(handler);
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

        public void Start()
        {
            this.mainTask.Start();
        }

        public void Stop()
        {
            this.cts.Cancel();
        }
        private void FireDataReceivedIfAvailable()
        {
            IDataSendReceive dataSendReceive = proxy.Setup();

            if (dataSendReceive != null)
            {
                if (dataSendReceive.DataAvailable)
                {
                    try
                    {
                        string receivedData = dataSendReceive.ReceiveData();
                        FireMessageReceivedAsync(dataSendReceive.ReceiveData())
                            .ContinueWith(FireMessageReceivedHandled);
                    }
                    catch (Exception ex)
                    {
                        this.protocol.LogError($"{description} thread: Error while receiving data. ErrorMessage={ex.Message}");
                    }
                }
            }
        }

        private async Task FireMessageReceivedAsync(string message)
        {
            await Task.Run(() =>
            {
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
                this.protocol.LogWarning($"{description} error while firing message event. ErrorMessage={t.Exception.Message}");
            }
        }

        private void Main()
        {
            try
            {
                Thread.CurrentThread.Name = description;

                while (!this.cts.IsCancellationRequested)
                {
                    this.FireDataReceivedIfAvailable();
                    this.SendRequestDeviceStatus();

                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                this.protocol.LogError($"{description} thread ended uncondionally. ErrorMessage={ex.Message}");
            }
        }
        private void SendRequestDeviceStatus()
        {
            if (string.IsNullOrWhiteSpace(RequestDeviceStatusData))
                return;

            IDataSendReceive dataSendReceive = proxy.Setup();

            if (dataSendReceive != null)
            {
                try
                {
                    dataSendReceive.SendData(RequestDeviceStatusData);
                }
                catch (Exception ex)
                {
                    this.protocol.LogError($"{description} thread: Error while sending device status request. ErrorMessage={ex.Message}");
                }                
            }
        }

        internal void ToggleOnOff()
        {
            throw new NotImplementedException();
        }
    }
}
