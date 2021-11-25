namespace DisplayControl.Models
{
    static class BeamerStatusMessages
    {
        public const string REQUEST_STATUS = "CR0\r";
        public const string SET_POWER_ON = "C00\r";
        public const string SET_POWER_OFF = "C01\r";

        public const char POWER_ON = (char)0x00;
        public const char STANDBY = (char)0x80;
        public const char COUNTDOWN_IN_PROCESS = (char)0x40;
        public const char COOLING_DOWN_IN_PROCESS = (char)0x20;
        public const char POWER_FAILURE = (char)0x10;
        public const char COOLING_DOWN_IN_PROCESS_DUE_ABNORMAL_TEMPERATURE = (char)0x28;
        public const char STANDBY_AFTER_COOLING_DOWN_DUE_ABNORMAL_TEMPERATURE = (char)0x88;
        public const char POWER_SAVE_COOLING_DOWN_IN_PROCESS = (char)0x24;
        public const char POWER_SAVE = (char)0x04;
        public const char COOLING_DOWN_IN_PROCESS_AFTER_OFF_DUE_LAMP_FAILURE = (char)0x21;
        public const char STANDBY_AFTER_COOLING_DOWN_DUE_LAMP_FAILURE = (char)0x81;
        public const char COOLING_DOWN_IN_PROCESS_AFTER_OFF_DUE_TO_SHUTTER_MANAGEMENT = (char)0x2C;
        public const char STANDBY_AFTER_COOLING_DOWN_DUE_TO_SHUTTER_MANAGEMENT = (char)0x8C;
    }
}