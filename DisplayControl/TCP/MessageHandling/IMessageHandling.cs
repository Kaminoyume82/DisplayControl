namespace DisplayControl.TCP.MessageHandling
{
    internal interface IMessageHandling
    {
        string RequestDeviceStatusString { get; }

        void HandleMessage(string message);

        string[] Split(string receivedData);

        void ToggleOnOff();
    }
}