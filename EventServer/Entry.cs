using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventServer.Data;
using StackExchange.Redis;

namespace EventServer
{
    class Entry
    {
        private static async Task Main(string[] args)
        {
            var listener = TcpListenerFactory.Create();
            listener.Start();
            var acceptingLoop = new AcceptingLoop(listener, 1000);
            acceptingLoop.Run();
            var outputLoop = new OutputLoop(10);
            outputLoop.Run();

            var ctsList = new List<CancellationTokenSource> {acceptingLoop.Cts};
            
            //Redis Setup
            
            // ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
            // ISubscriber sub = redis.GetSubscriber();
            //
            // sub.Subscribe("messages", (channel, message) => {
            //     foreach (var client in DataHolder.Instance.Clients)
            //     {
            //
            //         client.Item2.WriteLine(message);
            //     }
            // });

            while (true)
            {
                var line = Console.ReadLine();
                if (line == "exit!")
                {
                    foreach (var cts in ctsList)
                    {
                        cts.Cancel();
                    }
                    Console.WriteLine("Application stopping. Bye!");
                    await Task.Delay(10);
                    break;
                }
            }
        }
    }
}
