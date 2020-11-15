using Common;
using Common.Kafka;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            int i = 0;
            ConfluentProducer producer = new ConfluentProducer();
            while (true)
            {
                LogModel log = new LogModel { };
                log.Message = $"Welcome to Kafka {i++}";
                log.Timestamp = DateTime.UtcNow;
                
                await producer.PublishAsync("test-topic", JsonConvert.SerializeObject(log));
                Thread.Sleep(1000);
            }
        }
    }
}
