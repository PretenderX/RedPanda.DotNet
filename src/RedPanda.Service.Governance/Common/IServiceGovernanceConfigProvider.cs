namespace RedPanda.Service.Governance.Common
{
    public interface IServiceGovernanceConfigProvider
    {
        string GetAppSetting(string name, bool isOptional = true);

        T GetAppSetting<T>(string name);

        T GetAppSetting<T>(string name, T defaultValue);
    }
}
