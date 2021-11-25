using DisplayControl.Models;

namespace DisplayControl.TCP.MessageHandling
{
    internal class CompanionMessageHandling : IMessageHandling
    {
        private TcpIpThread deviceThread;

        public CompanionMessageHandling(TcpIpThread deviceThread)
        {
            this.deviceThread = deviceThread;
        }

        public string RequestDeviceStatusString => string.Empty;

        public void HandleMessage(string message)
        {
            if (message == PredefinedMessages.TOGGLE_ON_OFF)
            {
                deviceThread.ToggleOnOff();
            }
            else
            {
                deviceThread.SendData(message);
            }
        }

        public string[] Split(string receivedData)
        {
            return new[] { receivedData };
        }

        public void ToggleOnOff()
        {
        }
    }
}