using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace QuickSystemInfo_CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var computerName = Environment.MachineName;
            var networkCardList = new List<String>();
            var NetworkDictionary =  new Dictionary<String, CustomNetworkInfo> ();
            Console.WriteLine($"Computer Name: {computerName}");
            Console.WriteLine("==============================================");
            foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces()){
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
        }
    }
    class CustomNetworkInfo
    {
        public string NetworkCard { get; set; }
        public string IP { get; set; }
        public string MAC { get; set; }
    }
}
