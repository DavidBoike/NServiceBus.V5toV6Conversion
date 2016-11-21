using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using Sales.Messages.Commands;

namespace Sales
{
    public class Startup : IWantToRunWhenEndpointStartsAndStops
    {
        public async Task Start(IMessageSession session)
        {
            while (true)
            {
                Console.WriteLine("Press P to place an order, or Ctrl+C to quit");

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.P:
                        var cmd = new PlaceOrder {OrderId = Guid.NewGuid().ToString()};

                        Console.WriteLine($"Sending PlaceOrder: {cmd.OrderId}");
                        await session.SendLocal(cmd);
                        break;
                }
            }
        }

        public Task Stop(IMessageSession session)
        {
            Console.WriteLine("Shutting down...");
            return Task.CompletedTask;
        }
    }
}
