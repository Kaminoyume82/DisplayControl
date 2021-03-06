using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DisplayControl.ViewModels
{
    public class ConfigurationViewModel : INotifyPropertyChanged
    {
        private int beamer1In;
        private int beamer2In;
        private int stageDisplayIn;
        private int smallHallIn;

        private IpAddressPortViewModel companion;
        private IpAddressPortViewModel beamer1Out;
        private IpAddressPortViewModel beamer2Out;
        private IpAddressPortViewModel stageDisplayOut;
        private IpAddressPortViewModel smallHallOut;

        [JsonIgnore]
        public (int Page, int Bank) Beamer1PageBank { get; private set; }
        [JsonIgnore]
        public (int Page, int Bank) Beamer2PageBank { get; private set; }
        [JsonIgnore]
        public (int Page, int Bank) StageDisplayPageBank { get; private set; }
        [JsonIgnore]
        public (int Page, int Bank) SmallHallPageBank { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public static ConfigurationViewModel Default
        {
            get
            {
                return new ConfigurationViewModel()
                {
                    Companion = IpAddressPortViewModel.Default,

                    Beamer1In = 12345,
                    Beamer1Out = IpAddressPortViewModel.Default,
                    Beamer1Page = 99,
                    Beamer1Bank = 99,

                    Beamer2In = 12345,
                    Beamer2Out = IpAddressPortViewModel.Default,
                    Beamer2Page = 99,
                    Beamer2Bank = 99,

                    StageDisplayIn = 12345,
                    StageDisplayOut = IpAddressPortViewModel.Default,
                    StageDisplayPage = 99,
                    StageDisplayBank = 99,

                    SmallHallIn = 12345,
                    SmallHallOut = IpAddressPortViewModel.Default,
                    SmallHallPage = 99,
                    SmallHallBank = 99,
                };
            }
        }

        public int Beamer1In
        {
            get { return beamer1In; }
            set
            {
                if (beamer1In != value)
                {
                    beamer1In = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Beamer1In)));
                }
            }
        }

        public int Beamer2In
        {
            get { return beamer2In; }
            set
            {
                if (beamer2In != value)
                {
                    beamer2In = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Beamer2In)));
                }
            }
        }

        public int StageDisplayIn
        {
            get { return stageDisplayIn; }
            set
            {
                if (stageDisplayIn != value)
                {
                    stageDisplayIn = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StageDisplayIn)));
                }
            }
        }

        public int SmallHallIn
        {
            get { return smallHallIn; }
            set
            {
                if (smallHallIn != value)
                {
                    smallHallIn = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SmallHallIn)));
                }
            }
        }

        public IpAddressPortViewModel Companion
        {
            get { return companion; }
            set
            {
                if (companion != value)
                {
                    companion = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Companion)));
                }
            }
        }

        public IpAddressPortViewModel Beamer1Out
        {
            get { return beamer1Out; }
            set
            {
                if (beamer1Out != value)
                {
                    beamer1Out = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Beamer1Out)));
                }
            }
        }

        public IpAddressPortViewModel Beamer2Out
        {
            get { return beamer2Out; }
            set
            {
                if (beamer2Out != value)
                {
                    beamer2Out = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Beamer2Out)));
                }
            }
        }

        public IpAddressPortViewModel StageDisplayOut
        {
            get { return stageDisplayOut; }
            set
            {
                if (stageDisplayOut != value)
                {
                    stageDisplayOut = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StageDisplayOut)));
                }
            }
        }

        public IpAddressPortViewModel SmallHallOut
        {
            get { return smallHallOut; }
            set
            {
                if (smallHallOut != value)
                {
                    smallHallOut = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SmallHallOut)));
                }
            }
        }

        public int Beamer1Page
        {
            get { return Beamer1PageBank.Page; }
            set
            {
                if (Beamer1PageBank.Page != value)
                {
                    Beamer1PageBank = (value, Beamer1PageBank.Bank);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Beamer1Page)));
                }
            }
        }

        public int Beamer1Bank
        {
            get { return Beamer1PageBank.Bank; }
            set
            {
                if (Beamer1PageBank.Bank != value)
                {
                    Beamer1PageBank = (Beamer1PageBank.Page, value);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Beamer1Bank)));
                }
            }
        }

        public int Beamer2Page
        {
            get { return Beamer2PageBank.Page; }
            set
            {
                if (Beamer2PageBank.Page != value)
                {
                    Beamer2PageBank = (value, Beamer2PageBank.Bank);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Beamer2Page)));
                }
            }
        }

        public int Beamer2Bank
        {
            get { return Beamer2PageBank.Bank; }
            set
            {
                if (Beamer2PageBank.Bank != value)
                {
                    Beamer2PageBank = (Beamer2PageBank.Page, value);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Beamer2Bank)));
                }
            }
        }

        public int StageDisplayPage
        {
            get { return StageDisplayPageBank.Page; }
            set
            {
                if (StageDisplayPageBank.Page != value)
                {
                    StageDisplayPageBank = (value, StageDisplayPageBank.Bank);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StageDisplayPage)));
                }
            }
        }

        public int StageDisplayBank
        {
            get { return StageDisplayPageBank.Bank; }
            set
            {
                if (StageDisplayPageBank.Bank != value)
                {
                    StageDisplayPageBank = (StageDisplayPageBank.Page, value);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StageDisplayBank)));
                }
            }
        }

        public int SmallHallPage
        {
            get { return SmallHallPageBank.Page; }
            set
            {
                if (SmallHallPageBank.Page != value)
                {
                    SmallHallPageBank = (value, SmallHallPageBank.Bank);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SmallHallPage)));
                }
            }
        }

        public int SmallHallBank
        {
            get { return SmallHallPageBank.Bank; }
            set
            {
                if (SmallHallPageBank.Bank != value)
                {
                    SmallHallPageBank = (SmallHallPageBank.Page, value);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SmallHallBank)));
                }
            }
        }
    }
}
