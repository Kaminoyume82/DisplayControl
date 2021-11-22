using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayControl.ViewModels
{
    public class DeviceStatusViewModel : INotifyPropertyChanged
    {
        private Brush beamer1TCPInColor;
        private Brush beamer1TCPOutColor;
        private Brush beamer1PowerColor;

        private Brush beamer2TCPInColor;
        private Brush beamer2TCPOutColor;
        private Brush beamer2PowerColor;

        private Brush stageDisplayTCPInColor;
        private Brush stageDisplayTCPOutColor;
        private Brush stageDisplayPowerColor;

        private Brush smallHallTCPInColor;
        private Brush smallHallTCPOutColor;
        private Brush smallHallPowerColor;

        public event PropertyChangedEventHandler PropertyChanged;

        public Brush Beamer1TCPInColor
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

        public Brush Beamer1TCPOutColor
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
        public Brush Beamer1PowerColor
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

        public Brush Beamer2TCPInColor
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

        public Brush Beamer2TCPOutColor
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
        public Brush Beamer2PowerColor
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

        public Brush StageDisplayTCPInColor
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

        public Brush StageDisplayTCPOutColor
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
        public Brush StageDisplayPowerColor
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

        public Brush SmallHallTCPInColor
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

        public Brush SmallHallTCPOutColor
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
        public Brush SmallHallPowerColor
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
