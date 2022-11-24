using System.Configuration;
using RedPanda.Service.Governance.Configuration;
using RedPanda.Service.Governance.Exceptions;

namespace RedPanda.Service.Governance.Common
{
    public class WebConfigServiceGovernanceConfigProvider : IServiceGovernanceConfigProvider
    {
        public virtual string GetAppSetting(string name, bool isOptional = true)
        {
            var settingName = ServiceGovernanceConfig.GetSettingName(name);
            var value = ConfigurationManager.AppSettings.Get(settingName);

            if (!isOptional && string.IsNullOrEmpty(value))
            {
                throw new NotFoundServiceGovernanceConfigItemException(name);
            }

            return value;
        }

        public virtual T GetAppSetting<T>(string name)
        {
            return GetAppSetting(name, default(T));
        }

        public virtual T GetAppSetting<T>(string name, T defaultValue)
        {
            var value = GetAppSetting(name);

            if (value == null)
            {
                return defaultValue;
            }

            return value.As<T>();
        }
    }
}
