using System.Threading.Tasks;

namespace RedPanda.Service.Governance
{
    public interface IScopedConfigManager
    {
        /// <summary>
        /// 使用本地配置的ServiceSpace的值作为Scope上传配置值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> PutSettingByLocalSpaceAsync(string key, string value);

        /// <summary>
        /// 使用指定的ServiceSpace的值作为Scope上传配置值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="serviceSpace"></param>
        /// <returns></returns>
        Task<bool> PutSettingAsync(string key, string value, string serviceSpace = null);

        /// <summary>
        /// 使用本地配置的ServiceSpace的值作为Scope获取配置值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> GetSettingByLocalSpaceAsync(string key);

        /// <summary>
        /// 使用指定的ServiceSpace的值作为Scope获取配置值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="serviceSpace"></param>
        /// <returns></returns>
        Task<string> GetSettingAsync(string key, string serviceSpace = null);

        /// <summary>
        /// 使用本地配置的ServiceSpace的值作为Scope获取配置值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="serviceScope"></param>
        /// <returns></returns>
        Task<T> GetSettingByLocalSpaceAsync<T>(string key);

        /// <summary>
        /// 使用指定的ServiceSpace的值作为Scope获取配置值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="serviceSpace"></param>
        /// <returns></returns>
        Task<T> GetSettingAsync<T>(string key, string serviceSpace = null);

        /// <summary>
        /// 使用本地配置的ServiceSpace的值作为Scope删除配置值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> DeleteSettingByLocalSpaceAsync(string key);

        /// <summary>
        /// 使用指定的ServiceSpace的值作为Scope删除配置值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="serviceSpace"></param>
        /// <returns></returns>
        Task<bool> DeleteSettingAsync(string key, string serviceSpace = null);

        /// <summary>
        /// 使用本地配置的ServiceSpace的值作为Scope删除其中所有的配置值
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<bool> DeleteAllSettingsOfLocalSpace(string path = null);

        /// <summary>
        /// 使用本地配置的ServiceSpace的值作为Scope删除其中所有的配置值
        /// </summary>
        /// <param name="serviceSpace"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<bool> DeleteAllSettings(string serviceSpace, string path = null);
    }
}
