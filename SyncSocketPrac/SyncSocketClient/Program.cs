using System;
using System.Net;
using System.Net.Sockets;

namespace SyncSocketClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Client Begin");
            RunClient();
        }
        static void RunClient() {
            Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            soc.Connect(new IPEndPoint(IPAddress.Parse("192.168.0.18"), 4000));
            Console.WriteLine("Write a message");
            String msg = Console.ReadLine();
            soc.Send(System.Text.Encoding.ASCII.GetBytes(msg));
            soc.Close();
        }
    }
}
