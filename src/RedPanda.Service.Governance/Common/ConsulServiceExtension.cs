using Consul;
using System.Collections.Generic;

namespace RedPanda.Service.Governance.Common
{
    public static class ConsulServiceExtension
    {
        public static string GetRegisteredServiceAddress(this CatalogService catalogService)
        {
            var serviceAdress = BuildServiceAddress(catalogService.ServiceAddress, catalogService.ServicePort, catalogService.ServiceMeta);

            return serviceAdress;
        }

        public static string GetHealthyServiceAddress(this AgentService service)
        {
            var serviceAdress = BuildServiceAddress(service.Address, service.Port, service.Meta);

            return serviceAdress;
        }

        public static string BuildServiceAddress(string address, int port, IDictionary<string, string> meta)
        {
            if (!meta.TryGetValue(ServiceGovernanceConsts.ServiceSchema, out var serviceSchema) ||
                string.IsNullOrEmpty(serviceSchema))
            {
                serviceSchema = ServiceGovernanceConsts.DefaultServiceSchema;
            }

            var serviceAddress = $"{serviceSchema}://{address}";
            var servicePort = port == default ? ServiceGovernanceConsts.DefaultServicePort : port;

            if (servicePort > 0 && servicePort != 80 && servicePort != 443)
            {
                serviceAddress = $"{serviceAddress}:{servicePort}";
            }

            if (meta.TryGetValue(ServiceGovernanceConsts.ServiceVirtualDirectory, out var virtualDirectory))
            {
                virtualDirectory = virtualDirectory?.Trim('/');

                if (!string.IsNullOrEmpty(virtualDirectory))
                {
                    serviceAddress = $"{serviceAddress}/{virtualDirectory}";
                }
            }

            return serviceAddress;
        }
    }
}
