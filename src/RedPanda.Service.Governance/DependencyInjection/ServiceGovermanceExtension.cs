using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RedPanda.Service.Governance.Common;
using RedPanda.Service.Governance.Configuration;
using RedPanda.Service.Governance.Discovery;
using RedPanda.Service.Governance.Registration;
using System;
using System.Collections.Generic;

namespace RedPanda.Service.Governance.DependencyInjection
{
    public static class ServiceGovermanceExtension
    {
        public static IServiceCollection AddServiceGovermance(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IServiceRegistration, ServiceRegistration>();
            serviceCollection.AddSingleton<IServiceDiscovery, ServiceDiscovery>();
            serviceCollection.AddSingleton<IScopedConfigManager, ScopedConfigManager>();

            return serviceCollection;
        }

        public static IServiceProvider UseServiceGovermance(this IServiceProvider serviceProvider, Action<Dictionary<string, string>> appendMetaAction = null)
        {
            ServiceGovernanceConfig.ServiceGovernanceConfigProvider = new ServiceGovernanceConfigProvider(serviceProvider.GetRequiredService<IConfiguration>());

            var hostApplicationLifetime = serviceProvider.GetService<IHostApplicationLifetime>();

            hostApplicationLifetime?.ApplicationStarted.Register(() => serviceProvider.GetService<IServiceRegistration>()?.RegisterSelf(appendMetaAction));
            hostApplicationLifetime?.ApplicationStopping.Register(() => serviceProvider.GetService<IServiceRegistration>()?.DeregisterSelf());

            return serviceProvider;
        }
    }
}
