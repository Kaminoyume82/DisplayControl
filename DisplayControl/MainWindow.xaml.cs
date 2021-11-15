using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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

namespace DisplayControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            checkBoxStartInvisible.IsChecked = Settings.Default.InvisibleOnStart;
            if (Settings.Default.InvisibleOnStart)
            {
                Visibility = Visibility.Hidden;
            }
            else
            {
                Visibility = Visibility.Visible;
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

        private void window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
        }

        private void checkBoxStartInvisible_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (checkBoxStartInvisible.IsChecked != null && checkBoxStartInvisible.IsChecked != Settings.Default.InvisibleOnStart)
            {
                Settings.Default.InvisibleOnStart = (bool)checkBoxStartInvisible.IsChecked;
                Settings.Default.Save();
            }
        }

        private void buttonMakeStartUpEntry_Click(object sender, RoutedEventArgs e)
        {
            RegistryHelper.AddStartUpAppEntry();
        }

        private void textBoxNumbersOnly(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
