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
            var serviceMeta = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(serviceDescription.ServiceSchema))
            {
                serviceMeta.Add(ServiceGovernanceConsts.ServiceSchema, serviceDescription.ServiceSchema);
            }

            if (!string.IsNullOrEmpty(serviceDescription.ServiceVirtualDirectory))
            {
                serviceMeta.Add(ServiceGovernanceConsts.ServiceVirtualDirectory, serviceDescription.ServiceVirtualDirectory);
            }

            appendMetaAction?.Invoke(serviceMeta);

            var serviceCheck = new AgentServiceCheck
            {
                Interval = TimeSpan.FromSeconds(serviceDescription.ServiceCheckInterval),
                HTTP = serviceDescription.HealthCheckUrl,
                Timeout = TimeSpan.FromSeconds(serviceDescription.ServiceCheckTimeout),
            };

            if (serviceDescription.DeregisterCriticalServiceAfter.HasValue && serviceDescription.DeregisterCriticalServiceAfter.Value > 0)
            {
                serviceCheck.DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(serviceDescription.DeregisterCriticalServiceAfter.Value);
            }

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
                    var fullServiceName = $"{serviceDescription.ServiceSpace}.{serviceName}";
                    var serviceRegistration = new AgentServiceRegistration
                    {
                        Checks = new[] { serviceCheck },
                        ID = Guid.NewGuid().ToString(),
                        Name = fullServiceName,
                        Address = serviceDescription.ServiceHost,
                        Port = serviceDescription.ServicePort,
                        Meta = serviceMeta,
                        Tags = new[] { serviceDescription.ServiceSpace, $"urlprefix-/{fullServiceName}" },
                    };
                    var existingServices = (await consulClient.Catalog.Service(serviceRegistration.Name)).Response;

                    foreach (var service in existingServices)
                    {
                        var registeredServiceAddress = service.GetRegisteredServiceAddress();
                        if (registeredServiceAddress == serviceDescription.ServiceAddress)
                        {
                            await consulClient.Agent.ServiceDeregister(service.ServiceID);
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
            DeregisterSelfAsync().Wait();
        }

        public async Task DeregisterSelfAsync()
        {
            using (var consulClient = ConsulClientFactory.Create())
            {
                foreach (var serviceName in serviceIds.Keys)
                {
                    await consulClient.Agent.ServiceDeregister(serviceIds[serviceName]);
                }
            }
        }

        public void Dispose()
        {
            DeregisterSelf();
        }

        private bool IsDevelopmentEnvironment()
        {
            var environment = Environment.GetEnvironmentVariable(ServiceGovernanceConsts.ENVIRONMENT);

            return environment != null && environment.Equals(ServiceGovernanceConsts.DevelopmentEnvironment, StringComparison.OrdinalIgnoreCase);
        }
    }
}
