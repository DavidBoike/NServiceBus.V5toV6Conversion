
using System;
using System.Threading.Tasks;
using Sales.Messages.Events;

namespace Sales
{
    using Messages.Commands;
    using NServiceBus;

    public class Program
    {
        public static void Main()
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {
            var configuration = new EndpointConfiguration("Sales");

            var endpoint = await Endpoint.Start(configuration);

            bool running = true;

            while (running)
            {
                Console.WriteLine("Press P to place an order, or Esc to quit");

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.P:
                        var cmd = new PlaceOrder { OrderId = Guid.NewGuid().ToString() };

                        Console.WriteLine($"Sending PlaceOrder: {cmd.OrderId}");
                        await endpoint.SendLocal(cmd);
                        break;

                    case ConsoleKey.Escape:
                        Console.WriteLine("Shutting down...");
                        running = false;
                        break;
                }
            }

            await endpoint.Stop();
        }
    }
}
