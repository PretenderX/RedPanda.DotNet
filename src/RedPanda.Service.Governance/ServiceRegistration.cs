using Consul;
using System;

namespace RedPanda.Service.Governance
{
    public class ServiceRegistration : IServiceRegistration
    {
        private readonly string serviceId;

        public ServiceRegistration()
        {
            serviceId = Guid.NewGuid().ToString();
        }

        public void RegisterSelf()
        {
            var serviceDescription = SelfServiceDescriptionProvider.Build();
            var serviceCheck = new AgentServiceCheck
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(15),
                Interval = TimeSpan.FromSeconds(10),
                HTTP = $"{serviceDescription.ServiceSchema ?? "http"}://{serviceDescription.Host}:{serviceDescription.Port}{serviceDescription.HealthCheckRoute ?? "/"}",
                Timeout = TimeSpan.FromSeconds(5),
            };
            var serviceRegistration = new AgentServiceRegistration
            {
                Checks = new[] { serviceCheck },
                ID = serviceId,
                Address = serviceDescription.Host,
                Port = serviceDescription.Port == 0 ? 80 : serviceDescription.Port,
            };

            if (string.IsNullOrEmpty(serviceDescription.ServiceSpace))
            {
                serviceRegistration.Name = serviceDescription.ServiceName;
                serviceRegistration.Tags = new[] { $"urlprefix-/{serviceRegistration.Name}" };
            }
            else
            {
                serviceRegistration.Name = $"{serviceDescription.ServiceSpace}.{serviceDescription.ServiceName}";
                serviceRegistration.Tags = new[] { serviceDescription.ServiceSpace, $"urlprefix-/{serviceRegistration.Name}" };
            }

            using (var consulClient = ConsulClientFactory.Create())
            {
                consulClient.Agent.ServiceRegister(serviceRegistration).Wait();
            }
        }

        public void DeregisterSelf()
        {
            using (var consulClient = ConsulClientFactory.Create())
            {
                consulClient.Agent.ServiceDeregister(serviceId).Wait();
            }
        }

        public void Dispose()
        {
            DeregisterSelf();
        }
    }
}
