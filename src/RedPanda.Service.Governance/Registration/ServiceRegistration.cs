using Consul;
using RedPanda.Service.Governance.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedPanda.Service.Governance.Registration
{
    public class ServiceRegistration : IServiceRegistration
    {
        private readonly IDictionary<string, string> serviceIds;

        public ServiceRegistration()
        {
            serviceIds = new Dictionary<string, string>();
        }

        public void RegisterSelf(Action<Dictionary<string, string>> appendMetaAction = null)
        {
            RegisterSelfAsync(appendMetaAction).Wait();
        }

        public async Task RegisterSelfAsync(Action<Dictionary<string, string>> appendMetaAction = null)
        {
            var serviceDescription = LocalServiceDescriptionProvider.Build();
            var serviceSchema = serviceDescription.ServiceSchema ?? "http";
            var serviceMeta = new Dictionary<string, string>
            {
                { ServiceGovernanceConsts.ServiceSchema, serviceSchema }
            };
            var healthCheckVirtualDirectory = string.Empty;

            if (appendMetaAction != null)
            {
                appendMetaAction.Invoke(serviceMeta);
            }

            if (!string.IsNullOrEmpty(serviceDescription.VirtualDirectory) && serviceDescription.VirtualDirectory != "/")
            {
                serviceMeta.Add(ServiceGovernanceConsts.ServiceVirtualDirectory, serviceDescription.VirtualDirectory);
                healthCheckVirtualDirectory = $"/{serviceDescription.VirtualDirectory}/";
            }

            var serviceCheck = new AgentServiceCheck
            {
                DeregisterCriticalServiceAfter = ServiceGovernanceConfig.DeregisterCriticalServiceAfter,
                Interval = ServiceGovernanceConfig.ServiceCheckInterval,
                HTTP = $"{serviceSchema}://{serviceDescription.Host}:{serviceDescription.Port}{healthCheckVirtualDirectory}{serviceDescription.HealthCheckRoute ?? string.Empty}",
                Timeout = ServiceGovernanceConfig.ServiceCheckTimeout,
            };

            var serviceNames = new List<string> { serviceDescription.ServiceName };

            if (serviceDescription.ServiceAliases != null && serviceDescription.ServiceAliases.Contains(","))
            {
                foreach (var serviceAlias in serviceDescription.ServiceAliases.Split(','))
                {
                    var trimmedAlias = serviceAlias.Trim();

                    if (!string.IsNullOrEmpty(trimmedAlias))
                    {
                        serviceNames.Add(trimmedAlias);
                    }
                }
            }

            using (var consulClient = ConsulClientFactory.Create())
            {
                foreach (var serviceName in serviceNames)
                {
                    var serviceRegistration = new AgentServiceRegistration
                    {
                        Checks = new[] { serviceCheck },
                        ID = Guid.NewGuid().ToString(),
                        Address = serviceDescription.Host,
                        Port = serviceDescription.Port == 0 ? 80 : serviceDescription.Port,
                        Meta = serviceMeta,
                    };

                    if (string.IsNullOrEmpty(serviceDescription.ServiceSpace))
                    {
                        serviceRegistration.Name = serviceName;
                        serviceRegistration.Tags = new[] { $"urlprefix-/{serviceRegistration.Name}" };
                    }
                    else
                    {
                        serviceRegistration.Name = $"{serviceDescription.ServiceSpace}.{serviceName}";
                        serviceRegistration.Tags = new[] { serviceDescription.ServiceSpace, $"urlprefix-/{serviceRegistration.Name}" };
                    }

                    var existingServices = (await consulClient.Catalog.Service(serviceRegistration.Name)).Response;

                    foreach (var svc in existingServices)
                    {
                        if (svc.ServiceAddress == serviceRegistration.Address && svc.ServicePort == serviceRegistration.Port)
                        {
                            await consulClient.Agent.ServiceDeregister(svc.ServiceID);
                        }
                    }

                    var result = await consulClient.Agent.ServiceRegister(serviceRegistration);

                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        serviceIds.Add(serviceRegistration.Name, serviceRegistration.ID);
                    }
                }
            }
        }

        public void DeregisterSelf()
        {
            using (var consulClient = ConsulClientFactory.Create())
            {
                foreach (var serviceName in serviceIds.Keys)
                {
                    consulClient.Agent.ServiceDeregister(serviceIds[serviceName]).Wait();
                }
            }
        }

        public void Dispose()
        {
            DeregisterSelf();
        }
    }
}
