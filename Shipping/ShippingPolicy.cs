using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Billing.Messages.Events;
using NServiceBus;
using NServiceBus.Sagas;
using Sales.Messages.Events;
using Shipping.Messages.Events;

namespace Shipping
{
    public class ShippingPolicy : Saga<ShippingPolicyData>,
        IAmStartedByMessages<OrderPlaced>,
        IAmStartedByMessages<OrderBilled>,
        IHandleTimeouts<WaitBeforeShipping>
    {

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShippingPolicyData> mapper)
        {
            mapper.ConfigureMapping<OrderPlaced>(msg => msg.OrderId).ToSaga(saga => saga.OrderId);
            mapper.ConfigureMapping<OrderBilled>(msg => msg.OrderId).ToSaga(saga => saga.OrderId);
        }

        public async Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            Data.OrderId = message.OrderId;
            Data.Placed = true;

            Console.WriteLine($"Received OrderPlaced {message.OrderId} - should we ship yet?");

            await CheckForCompletion(context);
        }

        public async Task Handle(OrderBilled message, IMessageHandlerContext context)
        {
            Data.OrderId = message.OrderId;
            Data.Billed = true;

            Console.WriteLine($"Received OrderBilled {message.OrderId} - should we ship yet?");

            await CheckForCompletion(context);
        }

        async Task CheckForCompletion(IMessageHandlerContext context)
        {
            Console.WriteLine("");
            if (Data.Placed && Data.Billed)
            {
                Console.WriteLine($"Order Placed & Billed - Shipping {Data.OrderId} in 10s...");
                await this.RequestTimeout<WaitBeforeShipping>(context, TimeSpan.FromSeconds(10));
            }
            else
            {
                Console.WriteLine("Nope, don't ship yet");
            }
        }

        public async Task Timeout(WaitBeforeShipping state, IMessageHandlerContext context)
        {
            Console.WriteLine($"Now shipping OrderId {Data.OrderId}");

            await context.Publish(new OrderShipped {OrderId = Data.OrderId});

            this.MarkAsComplete();
        }
    }

    public class ShippingPolicyData : ContainSagaData
    {
        public virtual string OrderId { get; set; }
        public virtual bool Placed { get; set; }
        public virtual bool Billed { get; set; }
    }

    public class WaitBeforeShipping { }
}
