using Microsoft.Extensions.Configuration;
﻿using RedPanda.Service.Governance.Common;
using System;

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

        /// <summary>
        /// 默认10秒
        /// </summary>
        public static TimeSpan ServiceCheckInterval
        {
            get
            {
                var configValue = ServiceGovernanceConfigProvider.GetAppSetting(ServiceGovernanceConsts.ServiceCheckInterval);

                if (!int.TryParse(configValue, out var interval))
                {
                    interval = 10;
                }


                return TimeSpan.FromSeconds(interval);
            }
        }

        /// <summary>
        /// 默认30秒
        /// </summary>
        public static TimeSpan ServiceCheckTimeout
        {
            get
            {
                var configValue = ServiceGovernanceConfigProvider.GetAppSetting(ServiceGovernanceConsts.ServiceCheckTimeout);

                if (int.TryParse(configValue, out var interval))
                {
                    interval = 30;
                }

                return TimeSpan.FromSeconds(interval);
            }
        }

        /// <summary>
        /// 默认60秒
        /// </summary>
        public static TimeSpan DeregisterCriticalServiceAfter
        {
            get
            {
                var configValue = ServiceGovernanceConfigProvider.GetAppSetting(ServiceGovernanceConsts.DeregisterCriticalServiceAfter);

                if (!int.TryParse(configValue, out var interval))
                {
                    interval = 60;
                }

                return TimeSpan.FromSeconds(interval);
            }
        }
    }
}
