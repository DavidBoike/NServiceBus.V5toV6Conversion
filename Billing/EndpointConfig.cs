
using Sales.Messages.Events;

namespace Billing
{
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(EndpointConfiguration configuration)
        {
            var routing = configuration.UseTransport<MsmqTransport>().Routing();

            routing.RegisterPublisher(typeof(OrderPlaced).Assembly, "Sales");
        }
    }
}
