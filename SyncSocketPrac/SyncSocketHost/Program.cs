using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SyncSocketHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Server begin");
            AsyncPrac();
            RunHost();
        }

        static void RunHost() {
            
            Socket soc = new Socket(
                AddressFamily.InterNetwork, 
                SocketType.Stream, 
                ProtocolType.Tcp
            );

            // get all available addrs
            IPAddress[] ip_addr = Dns.GetHostAddresses(Dns.GetHostName()); 
            IPAddress ip4_addr = ip_addr[0];
            Console.WriteLine("Listing all available addresses:");
            for(int idx = 0; idx<ip_addr.Length; idx++) {
                if(ip_addr[idx].AddressFamily == AddressFamily.InterNetwork)
                    ip4_addr = ip_addr[idx];
                Console.WriteLine(string.Format(
                    "{0} {1}", 
                    ip_addr[idx].AddressFamily, 
                    ip_addr[idx]
                ));
            }

            // Creating an end point
            IPEndPoint endpoint = new IPEndPoint(ip4_addr, 4000);
            soc.Bind(endpoint);
            soc.Listen(5);
            
            Boolean stop = false;
            while(!stop) {
                Console.WriteLine(String.Format("Listening {0}", endpoint));
                Socket con = soc.Accept();
                Byte[] dataReceived = new Byte[1024];
                int x = con.Receive(dataReceived);
                Console.WriteLine(String.Format(
                    "Received {0} bytes. Message: {1}", 
                    x, 
                    System.Text.Encoding.ASCII.GetString(dataReceived)
                ));
                Console.WriteLine("Do you want to continue?");
                String cmd = Console.ReadLine();
                if(cmd.ToUpper()[0]!='Y')
                {
                    stop = true;
                    con.Close();
                }
            }
        }
        static void AsyncPrac() {
            Task atask = new Task(() => {
                Console.WriteLine("Async 2secs.");
            });
            Func<int> myfunc = () => {
                return 10;
            };
            Task<int> intTask = new Task<int>(myfunc);
            Task.Delay(2000).ContinueWith(t => {
                atask.Start();
            });
            intTask.ContinueWith((r) => {
                Console.WriteLine(r.GetType());
            });
        }
    }
}
