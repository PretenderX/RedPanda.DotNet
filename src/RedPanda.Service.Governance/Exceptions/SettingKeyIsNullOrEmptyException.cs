using System;

namespace RedPanda.Service.Governance.Exceptions
{
    public class SettingKeyIsNullOrEmptyException : Exception
    {
        public SettingKeyIsNullOrEmptyException()
            : base($"操作的配置项的Key不能为Null或空！")
        {
        }
    }
}
