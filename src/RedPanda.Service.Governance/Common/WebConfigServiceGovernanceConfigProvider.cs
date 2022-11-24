using System.Configuration;
using RedPanda.Service.Governance.Configuration;
using RedPanda.Service.Governance.Exceptions;

namespace RedPanda.Service.Governance.Common
{
    public class WebConfigServiceGovernanceConfigProvider : IServiceGovernanceConfigProvider
    {
        public string GetAppSetting(string name, bool isOptional = true)
        {
            var value = ConfigurationManager.AppSettings.Get(name);

            if (!isOptional && string.IsNullOrEmpty(value))
            {
                throw new NotFoundServiceGovernanceConfigItemException(name);
            }

            return value;
        }

        public T GetAppSetting<T>(string name)
        {
            return GetAppSetting(name, default(T));
        }

        public T GetAppSetting<T>(string name, T defaultValue)
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
