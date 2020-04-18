using System;

namespace RedPanda.Service.Governance.Registration
{
    public interface IServiceRegistration : IDisposable
    {
        void RegisterSelf();

        void DeregisterSelf();
    }
}
