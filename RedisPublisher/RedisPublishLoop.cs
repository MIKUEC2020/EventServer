using System.Net.Sockets;
using System.Threading.Tasks;
using EventServerCore;
using StackExchange.Redis;

namespace RedisPublisher
{
    public class RedisPublishLoop : BaseLoop<Unit>
    {
        private readonly ISubscriber _sub;

        public RedisPublishLoop(ISubscriber sub, int interval, string name = "ReadLoop")
            :base(interval, name)
        {
            _sub = sub;
        }

        protected override async Task Update(int count)
        {
            _sub.Publish("messages", "hello");
        }
    }
}
