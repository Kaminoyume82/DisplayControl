using DisplayControl.ViewModels;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DisplayControl.Controls
{
    /// <summary>
    /// Interaction logic for IpAddressPort.xaml
    /// </summary>
    public partial class IpAddressPort : UserControl
    {
        public IpAddressPort()
        {
            InitializeComponent();
        }

        public IpAddressPortViewModel ViewModel
        {
            get;
            set;
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
