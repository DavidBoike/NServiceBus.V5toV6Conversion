using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using Sales.Messages.Commands;
using Sales.Messages.Events;

namespace Sales
{
    public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
    {
        public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            Console.WriteLine($"Received PlaceOrder: {message.OrderId}, publishing OrderPlaced");

            var evt = new OrderPlaced() {OrderId = message.OrderId};

            await context.Publish(evt);
        }
    }
}
