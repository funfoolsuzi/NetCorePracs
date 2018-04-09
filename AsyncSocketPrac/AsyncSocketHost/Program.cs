using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace AsyncSocketHost
{
    class Program
    {
        static void Main(string[] args)
        {
            StartHost();
        }

        static void StartHost() {

            using (Socket host = new Socket(
                AddressFamily.InterNetwork, 
                SocketType.Stream, 
                ProtocolType.Tcp
            )) {
                Mutex mut = new Mutex();
                IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("0.0.0.0"), 9999);
                host.Bind(endpoint);
                host.Listen(5);
                Console.WriteLine("Listening on {0}", endpoint);
                Boolean stop = false;
                List<Socket> connections = new List<Socket>();
                Thread listenThread = new Thread((state) => {
                    // c# lambda functions do form closure!
                    while(!stop) {
                        if(!host.Poll(3000000, SelectMode.SelectRead)) continue;
                        host.BeginAccept((ar)=>{
                            Socket newConnection = host.EndAccept(ar);
                            Console.WriteLine(newConnection.RemoteEndPoint);
                            mut.WaitOne();
                            connections.Add(newConnection);
                            mut.ReleaseMutex();
                        }, null);
                    }
                });
                listenThread.Start();

                String response;
                while(!(response = Console.ReadLine().ToLower()).Equals("quit")) {
                    if (response.Equals("list")) {
                        for(int idx = 0; idx < connections.Count; idx++) {
                            Console.WriteLine(
                                "{0} {1} Available:{2}",
                                idx,
                                connections[idx].RemoteEndPoint,
                                connections[idx].Available
                            );
                        }
                    } else if (response.Equals("receive")) {
                        Console.Write("connection id:");
                        try {
                            Socket con = connections[Int32.Parse(Console.ReadLine())];
                            byte[] buf = new byte[2096];
                            con.ReceiveAsync(buf, SocketFlags.None).ContinueWith((byteReceived)=>{
                                Console.WriteLine(Encoding.Default.GetString(buf));
                            });
                        } catch (Exception e) {
                            Console.WriteLine("invalid connection id.");
                        }
                    }
                }
                stop = true;
            } // end of using 'Socket host'
            Console.WriteLine("Ending program. Bye.");
        } // end of StartHost()
    }
}
