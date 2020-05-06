using System;
using System.Collections.Generic;
using System.Text;
using Avalonia;
using ReactiveUI;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace QuickSystemInfo.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
        }
        public MainWindowViewModel(Application app)
        {
            ComputerName = Environment.MachineName;
            UserName = Environment.UserName;
            NetworkList = ObtainNetworkingInformation();
            ApplicationReference = app;
        }

        public Application ApplicationReference { get; set; }
        public string ComputerName { get; set; }
        public string UserName { get; set; }

        private string ipAddress;
        public string IpAddress
        {
            get { return ipAddress; }
            set
            {
                this.RaiseAndSetIfChanged(ref ipAddress, value);
            }
        }

        private string macAddress;
        public string MacAddress
        {
            get { return macAddress; }
            set
            {
                this.RaiseAndSetIfChanged(ref macAddress, value);
            }
        }

        private string selectedNetwork;
        public string SelectedNetwork
        {
            get { return selectedNetwork; }
            set
            {
                this.RaiseAndSetIfChanged(ref selectedNetwork, value);
                if (selectedNetwork != null)
                {
                    var item = NetworkDictionary[selectedNetwork];
                    IpAddress = item.IP;
                    MacAddress = item.MAC;
                }
            }
        }

        private List<string> networkList;
        public List<string> NetworkList
        {
            get { return networkList; }
            set
            {
                networkList = value;
                if (networkList.Count > 0)
                {
                    SelectedNetwork = networkList[0];
                }
            }
        }

        public Dictionary<String, CustomNetworkInfo> NetworkDictionary { get; set; }

        //public ReactiveCommand<Unit, Unit> DoTheThing { get; }
        public string Greeting => "Hello World!";
        public string Test { get; set; }

        void RunTheThing()
        {
            var c = ApplicationReference.Clipboard;
            c.SetTextAsync("Hola mundo");
        }

        void CopyData(string value)
        {
            ApplicationReference.Clipboard.SetTextAsync(value);
        }

        List<string> ObtainNetworkingInformation()
        {
            var networkCardList = new List<String>();
            NetworkDictionary = new Dictionary<String, CustomNetworkInfo>();
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
                            //Console.WriteLine("IP: " + ip);
                        }
                    }

                    networkCardList.Add(networkInterface.Description);

                    var card = new CustomNetworkInfo();
                    card.NetworkCard = networkInterface.Description;
                    Console.WriteLine($"Network Card: {networkInterface.Description}");
                    Console.WriteLine($"MAC: {formattedMac}");
                    Console.WriteLine($"IP: {ipAddress}");

                    card.MAC = formattedMac;
                    card.IP = ipAddress;
                    NetworkDictionary.Add(card.NetworkCard, card);
                    Console.WriteLine("==============================================");
                }
            }
            return networkCardList;
        }
    }
    public class CustomNetworkInfo
    {
        public string NetworkCard { get; set; }
        public string IP { get; set; }
        public string MAC { get; set; }
    }
}
