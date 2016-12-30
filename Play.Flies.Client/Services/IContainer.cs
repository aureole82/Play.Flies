using System;

namespace Play.Flies.Client.Services
{
    public interface IContainer : IDisposable
    {
        void Register<TImplementation>() where TImplementation : class;
        void Register<TService, TImplementation>() where TService : class where TImplementation : class, TService;
        void RegisterAll<TService>() where TService : class;
        TService Resolve<TService>() where TService : class;
        void Verify();
    }
}