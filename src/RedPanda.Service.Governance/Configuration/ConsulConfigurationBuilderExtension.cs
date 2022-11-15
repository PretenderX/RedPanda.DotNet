using Microsoft.Extensions.Configuration;
using System;
using Winton.Extensions.Configuration.Consul;

namespace RedPanda.Service.Governance.Configuration
{
    public static class ConsulConfigurationBuilderExtension
    {
        public static IConfigurationBuilder AddScopedConsulConfiguration(this IConfigurationBuilder builder, string name = "scopesettings.json", IConfiguration configuration = null)
        {
            return AddConsulConfiguration(builder, serviceConfiguration => $"{serviceConfiguration[ServiceGovernanceConsts.ServiceSpace]}/{name}", configuration);
        }

        public static IConfigurationBuilder AddServiceConsulConfiguration(this IConfigurationBuilder builder, string name = "appsettings.json", IConfiguration configuration = null)
        {
            return AddConsulConfiguration(builder, serviceConfiguration => $"{serviceConfiguration[ServiceGovernanceConsts.ServiceSpace]}/{serviceConfiguration[ServiceGovernanceConsts.ServiceName]}/{name}", configuration);
        }

        private static IConfigurationBuilder AddConsulConfiguration(IConfigurationBuilder builder, Func<IConfiguration, string> buildKeyFunc, IConfiguration configuration = null)
        {
            var serviceConfiguration = configuration ?? builder.Build();
            var consulConfigurationKey = buildKeyFunc.Invoke(serviceConfiguration);
            var consulAddress = serviceConfiguration[ServiceGovernanceConsts.ConsulAddress];
            var consulToken = serviceConfiguration[ServiceGovernanceConsts.ConsulToken];

            builder.AddConsul(consulConfigurationKey, default, options =>
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
