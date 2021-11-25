using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DisplayControl.Log;
using DisplayControl.Models;
using DisplayControl.Properties;
using DisplayControl.TCP;
using DisplayControl.ViewModels;

namespace DisplayControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ConfigurationViewModel configurationViewModel;
        private DeviceStatusViewModel deviceStatusViewModel;
        private List<InToDeviceToUIMapping> inToDeviceToUIMappings;
        private Logger logger;

        public MainWindow()
        {
            inToDeviceToUIMappings = new List<InToDeviceToUIMapping>();
            logger = new Logger();

            InitializeComponent();
            LoadSettings();
            InitUIContext();
            InitConnectionMappings();
            Application.Current.Exit += App_Exit;
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            StopThreads();

            Settings.Default.InvisibleOnStart = (bool)checkBoxStartInvisible.IsChecked;
            Settings.Default.ConfigurationJson = JsonSerializer.Serialize(this.configurationViewModel);
            Settings.Default.Save();
        }

        private void ButtonMakeStartUpEntry_Click(object sender, RoutedEventArgs e)
        {
            RegistryHelper.AddStartUpAppEntry();
        }

        private (int[] PortsForIncomingConnections, IPEndPoint[] EndpointsForDevices, (int Page, int Bank)[] PagesAndBanksForUI) GetConfigurations()
        {
            (int[] PortsForIncomingConnections, IPEndPoint[] EndpointsForDevices, (int Page, int Bank)[] PagesAndBanksForUI) retVal;

            retVal.PortsForIncomingConnections = new[]
            {
                configurationViewModel.Beamer1In,
                configurationViewModel.Beamer2In,
                configurationViewModel.StageDisplayIn,
                configurationViewModel.SmallHallIn
            };

            retVal.EndpointsForDevices = new[]
            {
                configurationViewModel.Beamer1Out.IPEndPoint,
                configurationViewModel.Beamer2Out.IPEndPoint,
                configurationViewModel.StageDisplayOut.IPEndPoint,
                configurationViewModel.SmallHallOut.IPEndPoint,
            };

            retVal.PagesAndBanksForUI = new[]
            {
                configurationViewModel.Beamer1PageBank,
                configurationViewModel.Beamer2PageBank,
                configurationViewModel.StageDisplayPageBank,
                configurationViewModel.SmallHallPageBank,
            };

            return retVal;
        }

        private void InitConfigurationUIContext()
        {
            this.ipAddressPortCompanion.DataContext = configurationViewModel.Companion;

            this.textBoxBeamer1Port.DataContext = configurationViewModel;
            this.ipAddressPortBeamer1.DataContext = configurationViewModel.Beamer1Out;
            this.textBoxBeamer1Page.DataContext = configurationViewModel;
            this.textBoxBeamer1Bank.DataContext = configurationViewModel;

            this.textBoxBeamer2Port.DataContext = configurationViewModel;
            this.ipAddressPortBeamer2.DataContext = configurationViewModel.Beamer2Out;
            this.textBoxBeamer2Page.DataContext = configurationViewModel;
            this.textBoxBeamer2Bank.DataContext = configurationViewModel;

            this.textBoxStagePort.DataContext = configurationViewModel;
            this.ipAddressPortStage.DataContext = configurationViewModel.StageDisplayOut;
            this.textBoxStagePage.DataContext = configurationViewModel;
            this.textBoxStageBank.DataContext = configurationViewModel;

            this.textBoxSmallHallPort.DataContext = configurationViewModel;
            this.ipAddressPortSmallHall.DataContext = configurationViewModel.SmallHallOut;
            this.textBoxSmallHallPage.DataContext = configurationViewModel;
            this.textBoxSmallHallBank.DataContext = configurationViewModel;
        }

        private void InitConnectionMappings()
        {
            (int[] PortsForIncomingConnections, IPEndPoint[] EndpointsForDevices, (int Page, int Bank)[] PagesAndBanksForUI) configurations = GetConfigurations();

            Dictionary<int, TcpIpThread> inThreads = new Dictionary<int, TcpIpThread>();
            for (int i = 0; i <= 3; i++)
            {
                TcpIpThread inTcpIpThread;
                if (!inThreads.TryGetValue(configurations.PortsForIncomingConnections[i], out inTcpIpThread))
                {
                    inTcpIpThread = new TcpIpThread(new SingleConnectionSocketServerProxy(configurations.PortsForIncomingConnections[i], true, this.logger), "Companion");
                    inThreads.Add(configurations.PortsForIncomingConnections[i], inTcpIpThread);
                }

                InToDeviceToUIMapping inToDeviceToUIMapping = new InToDeviceToUIMapping()
                {
                    Id = i,
                    IncomingIpThread = inTcpIpThread,
                    DeviceIpThread = new TcpIpThread(new SingleConnectionSocketClientProxy(configurations.EndpointsForDevices[i], this.logger), "Device"),
                    IncomingIpThreadStatus = (DeviceStatusColumn.Companion, (DeviceStatusRow)i),
                    DeviceIpThreadStatus = (DeviceStatusColumn.Device, (DeviceStatusRow)i),
                    DevicePowerStatus = (DeviceStatusColumn.Power, (DeviceStatusRow)i),
                    UIPageBank = configurations.PagesAndBanksForUI[i],
                };

                inToDeviceToUIMapping.IncomingIpThread.RegisterConnectionStatusChangeHandler((sender, args) =>
                {
                    InvokeSetDeviceStatusColor(inToDeviceToUIMapping.IncomingIpThreadStatus, GetColorByConnectionStatus(args.NewStatus));
                });

                inToDeviceToUIMapping.IncomingIpThread.MessageReceived += IncomingIpThread_MessageReceived;

                inToDeviceToUIMapping.DeviceIpThread.RegisterConnectionStatusChangeHandler((sender, args) =>
                {
                    InvokeSetDeviceStatusColor(inToDeviceToUIMapping.DeviceIpThreadStatus, GetColorByConnectionStatus(args.NewStatus));
                    if (args.NewStatus != ConnectionStatus.Connected)
                    {
                        InvokeSetDeviceStatusColor(inToDeviceToUIMapping.DevicePowerStatus, Brushes.Gray);
                    }
                });

                inToDeviceToUIMappings.Add(inToDeviceToUIMapping);
            }
        }

        private void IncomingIpThread_MessageReceived(object sender, string message)
        {
            IEnumerable<InToDeviceToUIMapping> correspondingMappings = GetMappingsForIncomingIpThread((TcpIpThread)sender);

            foreach (var correspondingMapping in correspondingMappings)
            {
                if (message.ToUpperInvariant() == PredefinedMessages.TOGGLE_ON_OFF)
                {
                    correspondingMapping.DeviceIpThread.ToggleOnOff();
                }
                else
                {
                    correspondingMapping.DeviceIpThread.SendData(message);
                }
            }
        }

        private IEnumerable<InToDeviceToUIMapping> GetMappingsForIncomingIpThread(TcpIpThread tcpIpThread)
        {
            foreach (InToDeviceToUIMapping mapping in this.inToDeviceToUIMappings)
            {
                if (mapping.IncomingIpThread == tcpIpThread)
                {
                    yield return mapping;
                }
            }
        }

        SolidColorBrush GetColorByConnectionStatus(ConnectionStatus connectionStatus)
        {
            switch (connectionStatus)
            {
                case ConnectionStatus.WaitingForConnection:
                    return Brushes.Yellow;
                case ConnectionStatus.Connected:
                    return Brushes.LightGreen;
                case ConnectionStatus.Closed:
                    return Brushes.Red;
                case ConnectionStatus.Error:
                    return Brushes.Red;
                default:
                    return Brushes.Gray;
            }
        }

        private void InitDeviceStatusUIContext()
        {
            this.deviceStatusViewModel = DeviceStatusViewModel.Default;

            this.labelBeamer1TCPInColor.DataContext = this.deviceStatusViewModel;
            this.labelBeamer1TCPOutColor.DataContext = this.deviceStatusViewModel;
            this.labelBeamer1PowerColor.DataContext = this.deviceStatusViewModel;

            this.labelBeamer2TCPInColor.DataContext = this.deviceStatusViewModel;
            this.labelBeamer2TCPOutColor.DataContext = this.deviceStatusViewModel;
            this.labelBeamer2PowerColor.DataContext = this.deviceStatusViewModel;

            this.labelStageDisplayTCPInColor.DataContext = this.deviceStatusViewModel;
            this.labelStageDisplayTCPOutColor.DataContext = this.deviceStatusViewModel;
            this.labelStageDisplayPowerColor.DataContext = this.deviceStatusViewModel;

            this.labelSmallHallTCPInColor.DataContext = this.deviceStatusViewModel;
            this.labelSmallHallTCPOutColor.DataContext = this.deviceStatusViewModel;
            this.labelSmallHallPowerColor.DataContext = this.deviceStatusViewModel;
        }

        private void InitUIContext()
        {
            InitConfigurationUIContext();
            InitDeviceStatusUIContext();
        }

        private void InvokeSetDeviceStatusColor((DeviceStatusColumn Col, DeviceStatusRow Row) targetCell, SolidColorBrush colorBrush)
        {
            this.Dispatcher.Invoke(() => SetDeviceStatusColor(targetCell, colorBrush));
        }

        private void LoadSettings()
        {
            checkBoxStartInvisible.IsChecked = Settings.Default.InvisibleOnStart;
            if (Settings.Default.InvisibleOnStart)
            {
                Visibility = Visibility.Hidden;
            }
            else
            {
                Visibility = Visibility.Visible;
            }

            if (String.IsNullOrWhiteSpace(Settings.Default.ConfigurationJson))
            {
                configurationViewModel = ConfigurationViewModel.Default;
            }
            else
            {
                try
                {
                    configurationViewModel = JsonSerializer.Deserialize<ConfigurationViewModel>(Settings.Default.ConfigurationJson);
                }
                catch (Exception ex)
                {
                    configurationViewModel = ConfigurationViewModel.Default;
                    MessageBox.Show(ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SetDeviceStatusColor((DeviceStatusColumn Col, DeviceStatusRow Row) targetCell, SolidColorBrush colorBrush)
        {
            switch (targetCell)
            {
                case { Col: DeviceStatusColumn.Companion, Row: DeviceStatusRow.Beamer1 }:
                    this.deviceStatusViewModel.Beamer1TCPInColor = colorBrush;
                    break;

                case { Col: DeviceStatusColumn.Companion, Row: DeviceStatusRow.Beamer2 }:
                    this.deviceStatusViewModel.Beamer2TCPInColor = colorBrush;
                    break;

                case { Col: DeviceStatusColumn.Companion, Row: DeviceStatusRow.StageDisplay }:
                    this.deviceStatusViewModel.StageDisplayTCPInColor = colorBrush;
                    break;

                case { Col: DeviceStatusColumn.Companion, Row: DeviceStatusRow.SmallHall }:
                    this.deviceStatusViewModel.SmallHallTCPInColor = colorBrush;
                    break;

                case { Col: DeviceStatusColumn.Device, Row: DeviceStatusRow.Beamer1 }:
                    this.deviceStatusViewModel.Beamer1TCPOutColor = colorBrush;
                    break;

                case { Col: DeviceStatusColumn.Device, Row: DeviceStatusRow.Beamer2 }:
                    this.deviceStatusViewModel.Beamer2TCPOutColor = colorBrush;
                    break;

                case { Col: DeviceStatusColumn.Device, Row: DeviceStatusRow.StageDisplay }:
                    this.deviceStatusViewModel.StageDisplayTCPOutColor = colorBrush;
                    break;

                case { Col: DeviceStatusColumn.Device, Row: DeviceStatusRow.SmallHall }:
                    this.deviceStatusViewModel.SmallHallTCPOutColor = colorBrush;
                    break;

                case { Col: DeviceStatusColumn.Power, Row: DeviceStatusRow.Beamer1 }:
                    this.deviceStatusViewModel.Beamer1PowerColor = colorBrush;
                    break;

                case { Col: DeviceStatusColumn.Power, Row: DeviceStatusRow.Beamer2 }:
                    this.deviceStatusViewModel.Beamer2PowerColor = colorBrush;
                    break;

                case { Col: DeviceStatusColumn.Power, Row: DeviceStatusRow.StageDisplay }:
                    this.deviceStatusViewModel.StageDisplayPowerColor = colorBrush;
                    break;

                case { Col: DeviceStatusColumn.Power, Row: DeviceStatusRow.SmallHall }:
                    this.deviceStatusViewModel.SmallHallPowerColor = colorBrush;
                    break;

                default:
                    throw new NotImplementedException();
            }
        }
        private void StartThreads()
        {
            foreach (InToDeviceToUIMapping mapping in this.inToDeviceToUIMappings)
            {
                mapping.IncomingIpThread.Start();
                mapping.DeviceIpThread.Start();
            }
        }

        private void StopThreads()
        {
            foreach (InToDeviceToUIMapping mapping in this.inToDeviceToUIMappings)
            {
                mapping.IncomingIpThread.Stop();
                mapping.DeviceIpThread.Stop();
            }
        }

        private void SystemTray_Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void SystemTray_Hide(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private void SystemTray_Show(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Visible;
        }

        private void TextBoxNumbersOnly(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.StartThreads();
        }
    }
}