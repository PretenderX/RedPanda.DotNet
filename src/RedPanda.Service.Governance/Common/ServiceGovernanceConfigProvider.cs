using Microsoft.Extensions.Configuration;
using RedPanda.Service.Governance.Exceptions;

namespace RedPanda.Service.Governance.Common
{
    public class ServiceGovernanceConfigProvider : IServiceGovernanceConfigProvider
    {
        protected readonly IConfiguration Configuration;

        public ServiceGovernanceConfigProvider(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public virtual string GetAppSetting(string name, bool isOptional = true)
        {
            var settingName = ServiceGovernanceConfig.GetSettingName(name);
            var value = Configuration[settingName];

            if (!isOptional && string.IsNullOrEmpty(value))
            {
                throw new NotFoundServiceGovernanceConfigItemException(settingName);
            }

            return value;
        }

        public virtual T GetAppSetting<T>(string name)
        {
            var settingName = ServiceGovernanceConfig.GetSettingName(name);

            return Configuration.GetValue<T>(settingName);
        }

        public virtual T GetAppSetting<T>(string name, T defaultValue)
        {
            var settingName = ServiceGovernanceConfig.GetSettingName(name);

            return Configuration.GetValue(settingName, defaultValue);
        }
    }
}
