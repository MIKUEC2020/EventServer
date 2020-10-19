using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace RedisPublisher
{
    class Entry
    {
        static async Task Main(string[] args)
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
            ISubscriber sub = redis.GetSubscriber();
            RedisPublishLoop redisPublishLoop = new RedisPublishLoop(sub, 1);
            redisPublishLoop.Run();
            
            while (true)
            {
                var line = Console.ReadLine();
                if (line == "exit!")
                {
                    Console.WriteLine("Application stopping. Bye!");
                    await Task.Delay(10);
                    break;
                }
            }
        }
    }
}
