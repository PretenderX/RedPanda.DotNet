using Microsoft.Extensions.Configuration;
using RedPanda.Service.Governance.Common;
using System;
using Winton.Extensions.Configuration.Consul;

namespace RedPanda.Service.Governance.Configuration
{
    public static class ConfigurationBuilderExtension
    {
        private static bool serviceGovernanceConfigProviderInitialized = false;

        public static IConfigurationBuilder InitialServiceGovernanceConfigProvider(this IConfigurationBuilder builder, IConfiguration configuration = null)
        {
            if (!serviceGovernanceConfigProviderInitialized)
            {
                var serviceConfiguration = configuration ?? builder.Build();
                ServiceGovernanceConfig.UseDotNetConfiguration(serviceConfiguration);
               
                serviceGovernanceConfigProviderInitialized = true;
            }

            return builder;
        }

        public static IConfigurationBuilder AddScopedConsulConfiguration(this IConfigurationBuilder builder, string name = "scopesettings.json", IConfiguration configuration = null)
        {
            return AddConsulConfiguration(builder, () => $"{LocalServiceDescriptionProvider.Value.ServiceSpace}/{name}", configuration);
        }

        public static IConfigurationBuilder AddServiceConsulConfiguration(this IConfigurationBuilder builder, string name = "appsettings.json", IConfiguration configuration = null)
        {
            return AddConsulConfiguration(builder, () => $"{LocalServiceDescriptionProvider.Value.ServiceSpace}/{LocalServiceDescriptionProvider.Value.ServiceName}/{name}", configuration);
        }

        private static IConfigurationBuilder AddConsulConfiguration(IConfigurationBuilder builder, Func<string> buildKeyFunc, IConfiguration configuration = null)
        {
            builder.InitialServiceGovernanceConfigProvider(configuration);

            var consulConfigurationKey = buildKeyFunc.Invoke();
            var consulAddress = ServiceGovernanceConfig.ConsulAddress;
            var consulToken = ServiceGovernanceConfig.ConsulToken;

            builder.AddConsul(consulConfigurationKey, options =>
            {
                options.ConsulConfigurationOptions = consulOptions =>
                {
                    consulOptions.Address = new Uri(consulAddress);

                    if (!string.IsNullOrEmpty(consulToken))
                    {
                        consulOptions.Token = consulToken;
                    }
                };
                options.Optional = true;
                options.ReloadOnChange = true;
                options.OnLoadException = exceptionContext =>
                {
                    exceptionContext.Ignore = true;
                };
            });

            return builder;
        }
    }
}
