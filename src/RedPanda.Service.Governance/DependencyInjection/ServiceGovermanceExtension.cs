using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RedPanda.Service.Governance.Common;
using RedPanda.Service.Governance.Configuration;
using RedPanda.Service.Governance.Discovery;
using RedPanda.Service.Governance.Registration;

namespace RedPanda.Service.Governance.DependencyInjection
{
    public static class ServiceGovermanceExtension
    {
        public static IServiceCollection AddServiceGovermance(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            ServiceGovernanceConfig.ServiceGovernanceConfigProvider = new ServiceGovernanceConfigProvider(configuration);
            serviceCollection.AddSingleton<IServiceRegistration, ServiceRegistration>();
            serviceCollection.AddSingleton<IServiceDiscovery, ServiceDiscovery>();
            serviceCollection.AddSingleton<IScopedConfigManager, ScopedConfigManager>();

            return serviceCollection;
        }
    }
}
