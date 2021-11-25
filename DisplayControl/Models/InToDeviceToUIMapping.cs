using DisplayControl.TCP;

namespace DisplayControl.Models
{
    internal class InToDeviceToUIMapping
    {
        public (DeviceStatusColumn Col, DeviceStatusRow Row) DeviceIpThreadStatus;

        public (DeviceStatusColumn Col, DeviceStatusRow Row) DevicePowerStatus;

        public (DeviceStatusColumn Col, DeviceStatusRow Row) IncomingIpThreadStatus;
        public TcpIpThread DeviceIpThread { get; set; }
        public int Id { get; set; }
        public TcpIpThread IncomingIpThread { get; set; }
        public (int Page, int Bank) UIPageBank { get; set; }
    }
}