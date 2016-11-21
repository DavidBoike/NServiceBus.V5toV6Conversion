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
        private Task startupTask;

        public Task Start(IMessageSession session)
        {
            startupTask = Task.Run(async () =>
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
            });

            return Task.CompletedTask;
        }

        public async Task Stop(IMessageSession session)
        {
            Console.WriteLine("Shutting down...");
            await startupTask;
        }
    }
}
