using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DisplayControl.Extensions;
using DisplayControl.Properties;
using DisplayControl.StreamDeck;
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

        public MainWindow()
        {
            InitializeComponent();
            LoadSettings();
            InitContext();
            Application.Current.Exit += Current_Exit;
        }

        private void InitContext()
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

        private void SystemTray_Show(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Visible;
        }

        private void SystemTray_Hide(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private void SystemTray_Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
        }

        private void ButtonMakeStartUpEntry_Click(object sender, RoutedEventArgs e)
        {
            RegistryHelper.AddStartUpAppEntry();
        }

        private void TextBoxNumbersOnly(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Current_Exit(object sender, ExitEventArgs e)
        {
            Settings.Default.InvisibleOnStart = (bool)checkBoxStartInvisible.IsChecked;
            Settings.Default.ConfigurationJson = JsonSerializer.Serialize(this.configurationViewModel);
            Settings.Default.Save();
        }
    }
}
