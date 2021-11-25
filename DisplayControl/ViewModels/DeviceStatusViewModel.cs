using System.ComponentModel;
using System.Windows.Media;

namespace DisplayControl.ViewModels
{
    public class DeviceStatusViewModel : INotifyPropertyChanged
    {
        private SolidColorBrush beamer1TCPInColor;
        private SolidColorBrush beamer1TCPOutColor;
        private SolidColorBrush beamer1PowerColor;

        private SolidColorBrush beamer2TCPInColor;
        private SolidColorBrush beamer2TCPOutColor;
        private SolidColorBrush beamer2PowerColor;

        private SolidColorBrush stageDisplayTCPInColor;
        private SolidColorBrush stageDisplayTCPOutColor;
        private SolidColorBrush stageDisplayPowerColor;

        private SolidColorBrush smallHallTCPInColor;
        private SolidColorBrush smallHallTCPOutColor;
        private SolidColorBrush smallHallPowerColor;

        public event PropertyChangedEventHandler PropertyChanged;

        public static DeviceStatusViewModel Default
        {
            get
            {
                return new DeviceStatusViewModel()
                {
                    Beamer1TCPInColor = Brushes.White,
                    Beamer1TCPOutColor = Brushes.White,
                    Beamer1PowerColor = Brushes.White,
                    Beamer2TCPInColor = Brushes.White,
                    Beamer2TCPOutColor = Brushes.White,
                    Beamer2PowerColor = Brushes.White,
                    StageDisplayTCPInColor = Brushes.White,
                    StageDisplayTCPOutColor = Brushes.White,
                    StageDisplayPowerColor = Brushes.White,
                    SmallHallTCPInColor = Brushes.White,
                    SmallHallTCPOutColor = Brushes.White,
                    SmallHallPowerColor = Brushes.White,
                };
            }
        }

        public SolidColorBrush Beamer1TCPInColor
        {
            get { return beamer1TCPInColor; }
            set
            {
                if (beamer1TCPInColor != value)
                {
                    beamer1TCPInColor = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Beamer1TCPInColor)));
                }
            }
        }

        public SolidColorBrush Beamer1TCPOutColor
        {
            get { return beamer1TCPOutColor; }
            set
            {
                if (beamer1TCPOutColor != value)
                {
                    beamer1TCPOutColor = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Beamer1TCPOutColor)));
                }
            }
        }
        public SolidColorBrush Beamer1PowerColor
        {
            get { return beamer1PowerColor; }
            set
            {
                if (beamer1PowerColor != value)
                {
                    beamer1PowerColor = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Beamer1PowerColor)));
                }
            }
        }

        public SolidColorBrush Beamer2TCPInColor
        {
            get { return beamer2TCPInColor; }
            set
            {
                if (beamer2TCPInColor != value)
                {
                    beamer2TCPInColor = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Beamer2TCPInColor)));
                }
            }
        }

        public SolidColorBrush Beamer2TCPOutColor
        {
            get { return beamer2TCPOutColor; }
            set
            {
                if (beamer2TCPOutColor != value)
                {
                    beamer2TCPOutColor = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Beamer2TCPOutColor)));
                }
            }
        }
        public SolidColorBrush Beamer2PowerColor
        {
            get { return beamer2PowerColor; }
            set
            {
                if (beamer2PowerColor != value)
                {
                    beamer2PowerColor = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Beamer2PowerColor)));
                }
            }
        }

        public SolidColorBrush StageDisplayTCPInColor
        {
            get { return stageDisplayTCPInColor; }
            set
            {
                if (stageDisplayTCPInColor != value)
                {
                    stageDisplayTCPInColor = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StageDisplayTCPInColor)));
                }
            }
        }

        public SolidColorBrush StageDisplayTCPOutColor
        {
            get { return stageDisplayTCPOutColor; }
            set
            {
                if (stageDisplayTCPOutColor != value)
                {
                    stageDisplayTCPOutColor = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StageDisplayTCPOutColor)));
                }
            }
        }
        public SolidColorBrush StageDisplayPowerColor
        {
            get { return stageDisplayPowerColor; }
            set
            {
                if (stageDisplayPowerColor != value)
                {
                    stageDisplayPowerColor = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StageDisplayPowerColor)));
                }
            }
        }

        public SolidColorBrush SmallHallTCPInColor
        {
            get { return smallHallTCPInColor; }
            set
            {
                if (smallHallTCPInColor != value)
                {
                    smallHallTCPInColor = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SmallHallTCPInColor)));
                }
            }
        }

        public SolidColorBrush SmallHallTCPOutColor
        {
            get { return smallHallTCPOutColor; }
            set
            {
                if (smallHallTCPOutColor != value)
                {
                    smallHallTCPOutColor = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SmallHallTCPOutColor)));
                }
            }
        }
        public SolidColorBrush SmallHallPowerColor
        {
            get { return smallHallPowerColor; }
            set
            {
                if (smallHallPowerColor != value)
                {
                    smallHallPowerColor = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SmallHallPowerColor)));
                }
            }
        }
    }
}
