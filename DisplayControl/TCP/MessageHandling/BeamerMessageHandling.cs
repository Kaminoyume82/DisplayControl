using System;
using System.Windows.Media;
using DisplayControl.Models;

namespace DisplayControl.TCP.MessageHandling
{
    internal class BeamerMessageHandling : IMessageHandling
    {
        private Action<SolidColorBrush> UpdateCellColor;

        public BeamerMessageHandling(Action<SolidColorBrush> updateCellColor)
        {
            this.UpdateCellColor = updateCellColor;
        }

        public string RequestDeviceStatusString => BeamerStatusMessages.REQUEST_STATUS;

        public void HandleMessage(string message)
        {
            char statusCode;
            if (message.Length == 2 && message[1] == '\r')
            {
                statusCode = message[0];

                if (IsOperational(statusCode))
                {
                    this.UpdateCellColor(Brushes.LightGreen);
                }
                else if (IsShuttingDown(statusCode))
                {
                    this.UpdateCellColor(Brushes.Yellow);
                }
                else
                {
                    this.UpdateCellColor(Brushes.Red);
                }
            }
        }

        public string[] Split(string receivedData)
        {
            return receivedData.Split('\r', StringSplitOptions.RemoveEmptyEntries);
        }

        public void ToggleOnOff()
        {
        }

        private bool IsOperational(char statusCode)
        {
            return statusCode == BeamerStatusMessages.POWER_ON;
        }

        private bool IsShuttingDown(char statusCode)
        {
            return statusCode == BeamerStatusMessages.COUNTDOWN_IN_PROCESS ||
                statusCode == BeamerStatusMessages.COOLING_DOWN_IN_PROCESS ||
                statusCode == BeamerStatusMessages.POWER_SAVE_COOLING_DOWN_IN_PROCESS ||
                statusCode == BeamerStatusMessages.COOLING_DOWN_IN_PROCESS_AFTER_OFF_DUE_LAMP_FAILURE ||
                statusCode == BeamerStatusMessages.COOLING_DOWN_IN_PROCESS_AFTER_OFF_DUE_TO_SHUTTER_MANAGEMENT ||
                statusCode == BeamerStatusMessages.COOLING_DOWN_IN_PROCESS_DUE_ABNORMAL_TEMPERATURE;
        }
    }
}