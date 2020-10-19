using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using EventServer.Data;
using EventServerCore;

namespace EventServer
{
    public class HealthCheckLoop : BaseLoop<Unit>
    {
        public HealthCheckLoop(int interval, string name = "Input")
            :base(interval, name)
        {
        }

        protected override async Task Update(int count)
        {
            var newClients = DataHolder.Instance.Clients;
            Console.WriteLine($"Num clients: {newClients.Count}");
            foreach (var client in newClients)
            {
                var connected = IsConnected(client.Item1.Client);
                if(!connected)
                {
                    var remoteEndPoint = (IPEndPoint) client.Item1.Client.RemoteEndPoint;
                    Console.WriteLine($"Disconnected: [No name] " +
                                      $"({((IPEndPoint) remoteEndPoint).Address}: {((IPEndPoint) remoteEndPoint).Port})");
                    newClients.Remove(client);
                }
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
