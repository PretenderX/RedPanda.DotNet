using System;
using System.Collections.Generic;

namespace RedPanda.Service.Governance.Registration
{
    public interface IServiceRegistration : IDisposable
    {
        void RegisterSelf(Action<Dictionary<string, string>> appendMetaAction = null);

        void DeregisterSelf();
    }
}
