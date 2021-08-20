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
            var registeringServiceAddress = $"{serviceSchema}://{serviceDescription.Host}:{serviceDescription.Port}";
            var virtualDirectory = serviceDescription.VirtualDirectory.Trim('/');
            var serviceMeta = new Dictionary<string, string>
            {
                { ServiceGovernanceConsts.ServiceSchema, serviceSchema }
            };

            if (appendMetaAction != null)
            {
                appendMetaAction.Invoke(serviceMeta);
            }

            if (!string.IsNullOrEmpty(virtualDirectory))
            {
                serviceMeta.Add(ServiceGovernanceConsts.ServiceVirtualDirectory, serviceDescription.VirtualDirectory);
                registeringServiceAddress = $"{registeringServiceAddress}/{serviceDescription.VirtualDirectory}";
            }

            var serviceCheck = new AgentServiceCheck
            {
                Interval = ServiceGovernanceConfig.ServiceCheckInterval,
                HTTP = $"{registeringServiceAddress}/{serviceDescription.HealthCheckRoute.Trim('/') ?? string.Empty}",
                Timeout = ServiceGovernanceConfig.ServiceCheckTimeout,
            };

            if (!IsDevelopmentEnvironment())
            {
                serviceCheck.DeregisterCriticalServiceAfter = ServiceGovernanceConfig.DeregisterCriticalServiceAfter;
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

                    foreach (var service in existingServices)
                    {
                        var registeredServiceAddress = service.GetRegisteredServiceAddress();
                        if (registeredServiceAddress == registeringServiceAddress)
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
