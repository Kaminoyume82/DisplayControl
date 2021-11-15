using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DisplayControl.Controls
{
    /// <summary>
    /// Interaction logic for IpAddressPort.xaml
    /// </summary>
    public partial class IpAddressPort : UserControl
    {
        public IpAddressPortViewModel ViewModel { get; private set; }

        public IpAddressPort()
        {
            InitializeComponent();

            ViewModel = IpAddressPortViewModel.Default;
            IPAdressPort.DataContext = ViewModel;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox focusedTextBox = (TextBox)sender;
            focusedTextBox.SelectAll();
        }

        private void textBoxNumbersOnly(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
