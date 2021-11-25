using System;
using System.Threading;
using System.Threading.Tasks;
using DisplayControl.Log;
using DisplayControl.TCP.MessageHandling;

namespace DisplayControl.TCP
{
    /// <summary>
    ///  Simple class that manages a single tcp/ip connection.
    ///  It can send messages over a socket as ASCII encoded string and it sends events if a message is received.
    /// </summary>
    internal class TcpIpThread
    {
        private readonly CancellationTokenSource cts;
        private readonly string description;
        private readonly IMessageHandling deviceSpecificMessageHandling;
        private readonly Task mainTask;
        private readonly Logger protocol;
        private readonly ICommunicationProxy proxy;

        public TcpIpThread(ICommunicationProxy proxy, string decription, IMessageHandling deviceSpecificMessageHandling)
        {
            this.cts = new CancellationTokenSource();
            this.mainTask = new Task(Main, cts.Token, TaskCreationOptions.LongRunning);
            this.proxy = proxy;
            this.protocol = proxy.Protocol;
            this.description = $"{decription}({proxy.Id})";
            this.deviceSpecificMessageHandling = deviceSpecificMessageHandling;
            this.MessageReceived += TcpIpThread_MessageReceived;
        }

        public event EventHandler<string> MessageReceived;

        public string Id => proxy.Id;

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

        public void ToggleOnOff()
        {
            deviceSpecificMessageHandling.ToggleOnOff();
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
                        string[] splittedMessages = this.deviceSpecificMessageHandling.Split(receivedData);

                        foreach (string message in splittedMessages)
                        {
                            FireMessageReceivedAsync(message)
                                .ContinueWith(FireMessageReceivedHandled);
                        }
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
                    this.SendRequestDeviceStatus();
                    this.FireDataReceivedIfAvailable();

                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                this.protocol.LogError($"{description} thread ended uncondionally. ErrorMessage={ex.Message}");
            }
        }

        private void SendRequestDeviceStatus()
        {
            if (string.IsNullOrWhiteSpace(deviceSpecificMessageHandling.RequestDeviceStatusString))
                return;

            IDataSendReceive dataSendReceive = proxy.Setup();

            if (dataSendReceive != null)
            {
                try
                {
                    dataSendReceive.SendData(deviceSpecificMessageHandling.RequestDeviceStatusString);
                }
                catch (Exception ex)
                {
                    this.protocol.LogError($"{description} thread: Error while sending device status request. ErrorMessage={ex.Message}");
                }
            }
        }

        private void TcpIpThread_MessageReceived(object sender, string message)
        {
            deviceSpecificMessageHandling.HandleMessage(message);
        }
    }
}