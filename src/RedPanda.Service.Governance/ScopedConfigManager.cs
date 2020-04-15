using Consul;
using System.Text;
using System.Threading.Tasks;

namespace RedPanda.Service.Governance
{
    public class ScopedConfigManager : ConfigManagerBase, IScopedConfigManager
    {
        public Task<bool> PutSettingByLocalSpaceAsync(string key, string value)
        {
            string localServiceScope = GetLocalServiceScope();

            return PutSettingAsync(key, value, localServiceScope);
        }

        public async Task<bool> PutSettingAsync(string key, string value, string serviceSpace = null)
        {
            string scopedKey = GetScopedKey(key, serviceSpace);
            var putPair = new KVPair(scopedKey)
            {
                Value = Encoding.UTF8.GetBytes(value)
            };

            using (var consulClient = ConsulClientFactory.Create())
            {
                var result = await consulClient.KV.Put(putPair);

                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return result.Response;
                }

                return false;
            }
        }

        public Task<string> GetSettingByLocalSpaceAsync(string key)
        {
            string localServiceScope = GetLocalServiceScope();

            return GetSettingAsync(key, localServiceScope);
        }

        public async Task<string> GetSettingAsync(string key, string serviceSpace = null)
        {
            string scopedKey = GetScopedKey(key, serviceSpace);

            using (var consulClient = ConsulClientFactory.Create())
            {
                var result = await consulClient.KV.Get(scopedKey);

                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return Encoding.UTF8.GetString(result.Response.Value, 0, result.Response.Value.Length);
                }

                return null;
            }
        }

        public async Task<T> GetSettingByLocalSpaceAsync<T>(string key)
        {
            var value = await GetSettingByLocalSpaceAsync(key);

            return GetReturnValue<T>(value);
        }

        public async Task<T> GetSettingAsync<T>(string key, string serviceSpace = null)
        {
            var value = await GetSettingAsync(key, serviceSpace);

            return GetReturnValue<T>(value);
        }

        public Task<bool> DeleteSettingByLocalSpaceAsync(string key)
        {
            string localServiceScope = GetLocalServiceScope();

            return DeleteSettingAsync(key, localServiceScope);
        }

        public async Task<bool> DeleteSettingAsync(string key, string serviceSpace = null)
        {
            string scopedKey = GetScopedKey(key, serviceSpace);

            using (var consulClient = ConsulClientFactory.Create())
            {
                var result = await consulClient.KV.Delete(scopedKey);

                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return result.Response;
                }

                return false;
            }
        }

        public Task<bool> DeleteAllSettingsOfLocalSpace(string path = null)
        {
            string localServiceScope = GetLocalServiceScope();

            return DeleteAllSettings(localServiceScope, path);
        }

        public async Task<bool> DeleteAllSettings(string serviceSpace, string path = null)
        {
            string scopedKey = GetScopedKey(path, serviceSpace);

            using (var consulClient = ConsulClientFactory.Create())
            {
                var result = await consulClient.KV.DeleteTree(scopedKey);

                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return result.Response;
                }

                return false;
            }
        }

        private static string GetLocalServiceScope()
        {
            var serviceDescription = LocalServiceDescriptionProvider.Build();
            var serviceScope = string.IsNullOrEmpty(serviceDescription.ServiceSpace) ? null : $"{serviceDescription.ServiceSpace}";

            return serviceScope;
        }

        private static string GetScopedKey(string key, string serviceScope)
        {
            return string.IsNullOrEmpty(serviceScope) ? key : $"{serviceScope}/{key}";
        }
    }
}
