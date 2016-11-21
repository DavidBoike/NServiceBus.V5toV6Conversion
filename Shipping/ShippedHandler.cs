using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using Shipping.Messages.Events;

namespace Shipping
{
    public class ShippedHandler : IHandleMessages<OrderShipped>
    {
        public Task Handle(OrderShipped message, IMessageHandlerContext context)
        {
            Console.WriteLine($"Received OrderShipped {message.OrderId}");
            return Task.CompletedTask;
        }
    }
}
