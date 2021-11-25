using System.ComponentModel;
using System.Net;
using System.Text.Json.Serialization;

namespace DisplayControl.ViewModels
{
    public class IpAddressPortViewModel : INotifyPropertyChanged
    {
        private int ipPart1;
        private int ipPart2;
        private int ipPart3;
        private int ipPart4;
        private int port;

        public event PropertyChangedEventHandler PropertyChanged;

        public IpAddressPortViewModel()
        {
            IPEndPoint = new IPEndPoint(IPAddress.Broadcast, IPEndPoint.MaxPort);
        }

        public IpAddressPortViewModel(IPEndPoint endPoint)
        {
            IPEndPoint = endPoint;
        }

        [JsonIgnore]
        public static IpAddressPortViewModel Default
        {
            get
            {
                return new IpAddressPortViewModel();
            }
        }

        [JsonIgnore]
        public IPEndPoint IPEndPoint
        {
            get
            {
                string ipAdressString = $"{ipPart1}.{ipPart2}.{ipPart3}.{ipPart4}";
                IPAddress ipAddress = IPAddress.Parse(ipAdressString);
                return new IPEndPoint(ipAddress, port);
            }
            private set
            {
                if (!IPEndPoint.Equals(value))
                {
                    var parts = value.Address.ToString().Split('.');
                    IpPart1 = int.Parse(parts[0]);
                    IpPart2 = int.Parse(parts[1]);
                    IpPart3 = int.Parse(parts[2]);
                    IpPart4 = int.Parse(parts[3]);
                    Port = value.Port;
                }
            }
        }

        public int IpPart1
        {
            get { return ipPart1; }
            set
            {
                if (ipPart1 != value)
                {
                    ipPart1 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IpPart1)));
                }
            }
        }

        public int IpPart2
        {
            get { return ipPart2; }
            set
            {
                if (ipPart2 != value)
                {
                    ipPart2 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IpPart2)));
                }
            }
        }

        public int IpPart3
        {
            get { return ipPart3; }
            set
            {
                if (ipPart3 != value)
                {
                    ipPart3 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IpPart3)));
                }
            }
        }

        public int IpPart4
        {
            get { return ipPart4; }
            set
            {
                if (ipPart4 != value)
                {
                    ipPart4 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IpPart4)));
                }
            }
        }

        public int Port
        {
            get { return port; }
            set
            {
                if (port != value)
                {
                    port = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Port)));
                }
            }
        }
    }
}
