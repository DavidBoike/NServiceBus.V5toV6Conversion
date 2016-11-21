using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Billing.Messages.Events;
using NServiceBus;
using Sales.Messages.Events;

namespace Billing
{
    public class OrderPlacedHandler : IHandleMessages<OrderPlaced>
    {
        public async Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            Console.WriteLine($"Received OrderPlaced: {message.OrderId}, publishing OrderBilled");

            var evt = new OrderBilled {OrderId = message.OrderId};

            await context.Publish(evt);
        }
    }
}
