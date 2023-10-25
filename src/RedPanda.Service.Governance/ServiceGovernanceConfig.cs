using Microsoft.Extensions.Configuration;
﻿using RedPanda.Service.Governance.Common;

namespace RedPanda.Service.Governance
{
    public static class ServiceGovernanceConfig
    {
        public static void UseDotNetConfiguration(IConfiguration configuration)
        {
            ServiceGovernanceConfigProvider = new ServiceGovernanceConfigProvider(configuration);
        }

        public static IServiceGovernanceConfigProvider ServiceGovernanceConfigProvider { get; set; } = new WebConfigServiceGovernanceConfigProvider();

        public static IJsonProvider JsonProvider { get; set; } = new SystemTextJsonProvider();

        public static bool ConfigurationSectionEnabled { get; private set; }

        public static string ConfigurationSectionName { get; private set; }

        public static void EnableConfigurationSection(string sectionName = ServiceDescription.SectionName)
        {
            ConfigurationSectionEnabled = true;
            ConfigurationSectionName = sectionName;
        }

        public static void DisableConfigurationSection()
        {
            ConfigurationSectionEnabled = false;
            ConfigurationSectionName = null;
        }

        public static string GetSettingName(string originalName) => ConfigurationSectionEnabled ? $"{ConfigurationSectionName}:{originalName}" : originalName;

        public static string ConsulAddress => ServiceGovernanceConfigProvider.GetAppSetting(ServiceGovernanceConsts.ConsulAddress, ServiceGovernanceConsts.DefaultConsulAddress);

        public static string ConsulToken => ServiceGovernanceConfigProvider.GetAppSetting(ServiceGovernanceConsts.ConsulToken);


    }
}
