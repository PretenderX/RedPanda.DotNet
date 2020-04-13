using System;

namespace RedPanda.Service.Governance
{
    public class NotFoundServiceGovernanceConfigItemException : Exception
    {
        public NotFoundServiceGovernanceConfigItemException(string configItemName)
            : base($"未获取到本地 \"{configItemName}\" 配置项的值！")
        {
        }
    }
}
