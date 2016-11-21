using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using Sales.Messages.Commands;

namespace Sales
{
    public class Startup : IWantToRunWhenBusStartsAndStops
    {
        public IBus Bus { get; set; }

        public void Start()
        {
            while (true)
            {
                Console.WriteLine("Press P to place an order, or ESC to quit.");

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.P:
                        var cmd = new PlaceOrder {OrderId = Guid.NewGuid().ToString()};

                        Console.WriteLine($"Sending PlaceOrder: {cmd.OrderId}");
                        Bus.SendLocal(cmd);
                        break;

                    case ConsoleKey.Escape:
                        Bus.Dispose();
                        return;
                }
            }
        }

        public void Stop()
        {
            Console.WriteLine("Shutting down...");
        }
    }
}
