using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace AsyncSocketClient
{
    class Program
    {
        static void Main(string[] args)
        {
            StartClient();
        }

        static void StartClient() {

            // get dest ip address
            String ipString;
            IPAddress dest_ip;
            do {
                Console.WriteLine("Enter Destination IP Address");
                ipString = Console.ReadLine();
                if(ipString.Equals(String.Empty))
                    ipString = "0.0.0.0";
            } while (!IPAddress.TryParse(ipString, out dest_ip)) ;

            // get dest port number
            String portString;
            int port = 0;
            do {
                Console.WriteLine("Enter Destination port number.");
                portString = Console.ReadLine();
            } while (!Int32.TryParse(portString, out port));

            IPEndPoint endpoint = new IPEndPoint(dest_ip, port);
            Socket client = new Socket(
                AddressFamily.InterNetwork, 
                SocketType.Stream,
                ProtocolType.Tcp
            );
            client.Connect(endpoint);
            Console.WriteLine("Connected to {0}", endpoint);

            // ... receiving thread taking off

            while(true) {
                Console.Write("messege:");
                String messege = Console.ReadLine();

                if(messege.ToLower().Equals("quit"))
                    break;
                client.Send(System.Text.Encoding.ASCII.GetBytes(messege));
            }
        }
    }
}
