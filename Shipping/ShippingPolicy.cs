using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Billing.Messages.Events;
using NServiceBus.Saga;
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

        public void Handle(OrderPlaced message)
        {
            Data.OrderId = message.OrderId;
            Data.Placed = true;

            Console.WriteLine($"Received OrderPlaced {message.OrderId} - should we ship yet?");

            CheckForCompletion();
        }

        public void Handle(OrderBilled message)
        {
            Data.OrderId = message.OrderId;
            Data.Billed = true;

            Console.WriteLine($"Received OrderBilled {message.OrderId} - should we ship yet?");

            CheckForCompletion();
        }

        void CheckForCompletion()
        {
            Console.WriteLine("");
            if (Data.Placed && Data.Billed)
            {
                Console.WriteLine($"Order Placed & Billed - Shipping {Data.OrderId} in 10s...");
                this.RequestTimeout<WaitBeforeShipping>(TimeSpan.FromSeconds(10));
            }
            else
            {
                Console.WriteLine("Nope, don't ship yet");
            }
        }

        public void Timeout(WaitBeforeShipping state)
        {
            Console.WriteLine($"Now shipping OrderId {Data.OrderId}");

            Bus.Publish(new OrderShipped {OrderId = Data.OrderId});

            this.MarkAsComplete();
        }
    }

    public class ShippingPolicyData : ContainSagaData
    {
        [Unique]
        public virtual string OrderId { get; set; }
        public virtual bool Placed { get; set; }
        public virtual bool Billed { get; set; }
    }

    public class WaitBeforeShipping { }
}
