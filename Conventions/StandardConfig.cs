using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence;

namespace Conventions
{
    public class StandardConfig : INeedInitialization
    {
        public void Customize(EndpointConfiguration configuration)
        {
            configuration.UseTransport<MsmqTransport>();

            configuration.UsePersistence<NHibernatePersistence>()
                .ConnectionString(@"Server=.\sqlexpress;Database=V5toV6Demo;Trusted_Connection=true;");

            configuration.UseSerialization<XmlSerializer>();

            configuration.SendFailedMessagesTo("error");
            configuration.AuditProcessedMessagesTo("audit");
        }
    }
}
