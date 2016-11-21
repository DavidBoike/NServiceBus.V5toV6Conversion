using System;
using System.Threading.Tasks;
using Billing.Messages.Events;
using Sales.Messages.Events;
using Shipping.Messages.Events;

namespace Shipping
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
            var configuration = new EndpointConfiguration("Shipping");

            var routing = configuration.UseTransport<MsmqTransport>().Routing();

            routing.RegisterPublisher(typeof(OrderPlaced).Assembly, "Sales");
            routing.RegisterPublisher(typeof(OrderBilled).Assembly, "Billing");
            routing.RegisterPublisher(typeof(OrderShipped).Assembly, "Shipping");

            var endpoint = await Endpoint.Start(configuration);

            Console.WriteLine("Press Enter to quit");
            Console.ReadLine();

            await endpoint.Stop();
        }
    }
}
