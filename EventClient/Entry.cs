using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace EventClient
{
    class Program
    {
        private static int count;
        
        private static async Task Main(string[] args)
        {
            var clientList = new List<TcpClient>();
            var ctsList = new List<CancellationTokenSource>();

            while (true)
            {
                var line = Console.ReadLine();
                if (line == "exit!")
                {
                    foreach (var cts in ctsList)
                    {
                        cts.Cancel();
                    }

                    await Task.Delay(500);
                    Console.WriteLine("Application stopping. Bye!");
                    break;
                }

                if (line == "close!")
                {
                    Console.WriteLine("Closing Socket");
                    clientList.ForEach(x => x.Client.Close());
                    await Task.Delay(500);
                }

                if (line == "connect!")
                {
                    var tasks = new List<Task>();
                    for (int i = 0; i < 500; i++)
                    {
                        var tcpClient = new TcpClient();
                        Task task;

                        task = tcpClient.ConnectAsync("localhost", 5588);
                        tasks.Add(task);
                        clientList.Add(tcpClient);
                    }

                    foreach (var task in tasks)
                    {
                        try
                        {
                            await task;
                        }
                        catch (SocketException e)
                        {
                            Console.WriteLine($"Exception times{++count}");
                        }
                    }

                    foreach (var tcpClient in clientList)
                    {
                        var readLoop = new ReadLoop(tcpClient, 10);
                        readLoop.Run();
                        ctsList.Add(readLoop.Cts);
                    }
                }
            }
        }
    }
}
