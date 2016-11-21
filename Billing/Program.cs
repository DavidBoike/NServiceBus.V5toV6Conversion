
using System;
using System.Threading.Tasks;
using Sales.Messages.Events;

namespace Billing
{
    using NServiceBus;

    public class Program
    {
        public static void Main()
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {
            var configuration = new EndpointConfiguration("Billing");

            var routing = configuration.UseTransport<MsmqTransport>().Routing();

            routing.RegisterPublisher(typeof(OrderPlaced).Assembly, "Sales");

            var endpoint = await Endpoint.Start(configuration);

            Console.WriteLine("Press Enter to quit");
            Console.ReadLine();

            await endpoint.Stop();
        }
    }
}
