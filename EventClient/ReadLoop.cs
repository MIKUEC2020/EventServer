using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using EventServerCore;

namespace EventClient
{
    public class ReadLoop : BaseLoop<Unit>
    {
        private readonly TcpClient _tcpClient;

        public ReadLoop(TcpClient tcpClient, int interval, string name = "ReadLoop")
            :base(interval, name)
        {
            _tcpClient = tcpClient;
        }

        protected override async Task Update(int count)
        {
            var networkStream = _tcpClient.GetStream();
            var reader = new StreamReader(networkStream, System.Text.Encoding.UTF8);
            //networkStream.ReadTimeout = 10;
            while (!reader.EndOfStream)
            {
                var str = await reader.ReadLineAsync();
            }
        }
    }
}
