using System.Net;
using System.Net.Sockets;

namespace EventServer
{
    public static class TcpListenerFactory
    {
        public static TcpListener Create()
        {
            var localEndpoint = new IPEndPoint(IPAddress.Any, 5588);
            return new TcpListener(localEndpoint);
        }
    }
}
