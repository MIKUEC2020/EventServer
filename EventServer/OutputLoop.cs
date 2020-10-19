using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using EventServer.Data;
using EventServerCore;

namespace EventServer
{
    public class OutputLoop : BaseLoop<Unit>
    {
        public OutputLoop(int interval, string name = "Input")
            :base(interval, name)
        {
        }

        protected override async Task Update(int count)
        {
            var tasks = new List<Task>();
            
            foreach (var clients in DataHolder.Instance.Clients)
            {
                try
                {
                    var task = clients.Item2.FlushAsync();
                    tasks.Add(task);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            await Task.WhenAll(tasks);
        }
    }
}
