
using Billing.Messages.Events;
using Sales.Messages.Events;
using Shipping.Messages.Events;

namespace Shipping
{
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(EndpointConfiguration configuration)
        {
            var routing = configuration.UseTransport<MsmqTransport>().Routing();

            routing.RegisterPublisher(typeof(OrderPlaced).Assembly, "Sales");
            routing.RegisterPublisher(typeof(OrderBilled).Assembly, "Billing");
            routing.RegisterPublisher(typeof(OrderShipped).Assembly, "Shipping");
        }
    }
}
