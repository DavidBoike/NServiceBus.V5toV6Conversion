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
        public void Handle(OrderShipped message)
        {
            Console.WriteLine($"Received OrderShipped {message.OrderId}");
        }
    }
}
