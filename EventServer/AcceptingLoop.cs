using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using EventServer.Data;
using EventServerCore;

namespace EventServer
{
    public class AcceptingLoop : BaseLoop<Unit>
    {
        private readonly TcpClient _tcpClient;
        private readonly TcpListener _listener;

        public AcceptingLoop(TcpListener listener, int interval, string name = "ReadLoop")
            :base(interval, name)
        {
            _listener = listener;
        }

        protected override async Task Update(int count)
        {
            Console.WriteLine("Health");
            var tasks = new List<Task>();
            while (_listener.Pending())
            {
                try
                {
                    var task = _listener.AcceptTcpClientAsync();
                    tasks.Add(task);
                }
                catch (SocketException e)
                {
                    Console.WriteLine(e);
                }
            }
            await Task.WhenAll(tasks);
            var newClients = new HashSet<Tuple<TcpClient, StreamWriter>>(DataHolder.Instance.Clients);
            foreach (var task in tasks)
            {
                var client = ((Task<TcpClient>) task).Result;
                var remoteEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;
                Console.WriteLine($"Connected: [No name] " +
                                  $"({remoteEndPoint.Address}: {remoteEndPoint.Port})");
                newClients.Add(new Tuple<TcpClient, StreamWriter>(client , new StreamWriter(client.GetStream(), System.Text.Encoding.UTF8)));
            }
            DataHolder.Instance.Clients = newClients;
            HealthCheck();
        }
        
        private void HealthCheck()
        {
            var newClients = new HashSet<Tuple<TcpClient, StreamWriter>>(DataHolder.Instance.Clients);
            var removeList = new List<Tuple<TcpClient, StreamWriter>>();
            Console.WriteLine($"Num clients: {newClients.Count}");
            foreach (var client in newClients)
            {
                var connected = IsConnected(client.Item1.Client);
                if(!connected)
                {
                    var remoteEndPoint = (IPEndPoint) client.Item1.Client.RemoteEndPoint;
                    Console.WriteLine($"Disconnected: [No name] " +
                                      $"({((IPEndPoint) remoteEndPoint).Address}: {((IPEndPoint) remoteEndPoint).Port})");
                    removeList.Add(client);
                }
            }

            foreach (var toRemoveClient in removeList)
            {
                newClients.Remove(toRemoveClient);
            }
            DataHolder.Instance.Clients = newClients;
        }
        private static bool IsConnected(Socket socket)
        {
            try
            {
                return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
            }
            catch (SocketException) { return false; }
        }
    }
}
