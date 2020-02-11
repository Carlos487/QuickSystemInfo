using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace QuickSystemInfo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<String, CustomNetworkInfo> NetworkDictionary { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            NetworkDictionary = new Dictionary<String, CustomNetworkInfo>();
            ComputerNameTextBox.Text = Environment.MachineName;

            foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (networkInterface.OperationalStatus == OperationalStatus.Up)
                {

                    var mac = networkInterface.GetPhysicalAddress().ToString();
                    var regex = "(.{2})(.{2})(.{2})(.{2})(.{2})(.{2})";
                    var replace = "$1:$2:$3:$4:$5:$6";
                    var formattedMac = Regex.Replace(mac, regex, replace);
                    var ipProperties = networkInterface.GetIPProperties();
                    var addresses = ipProperties.UnicastAddresses;
                    var ipAddress = "";
                    foreach (var address in addresses)
                    {
                        var ip = address.Address;
                        if (ip.AddressFamily == AddressFamily.InterNetwork)
                        {
                            ipAddress = ip.ToString();
                            Console.WriteLine("IP: " + ip);
                        }
                    }

                    NetworkCardComboBox.Items.Add(networkInterface.Description);

                    var card = new CustomNetworkInfo();
                    card.NetworkCard = networkInterface.Description;
                    card.MAC = formattedMac;
                    card.IP = ipAddress;
                    NetworkDictionary.Add(card.NetworkCard, card);
                }
            }
            if (NetworkCardComboBox.Items.Count > 0)
            {
                NetworkCardComboBox.SelectedIndex = 0;
                var card = NetworkDictionary[NetworkCardComboBox.SelectedItem.ToString()];
                IPAddressTextBox.Text = card.IP;
                MACAddressTextBox.Text = card.MAC;
            }
        }

        private void NetworkCardComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var card = NetworkDictionary[NetworkCardComboBox.SelectedItem.ToString()];
            IPAddressTextBox.Text = card.IP;
            MACAddressTextBox.Text = card.MAC;
        }

        private void ComputerNameButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetDataObject(ComputerNameTextBox.Text);
        }

        private void IPAddressButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetDataObject(IPAddressTextBox.Text);
        }

        private void MACAddressButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetDataObject(MACAddressTextBox.Text);
        }
    }

    class CustomNetworkInfo
    {
        public string NetworkCard { get; set; }
        public string IP { get; set; }
        public string MAC { get; set; }
    }
}
