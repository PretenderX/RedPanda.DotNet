using System;

namespace RedPanda.Service.Governance
{
    public interface IServiceRegistration : IDisposable
    {
        void RegisterSelf();

        void DeregisterSelf();
    }
}
