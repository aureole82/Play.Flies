using System;
using System.Linq;
using SimpleInjector;

namespace Play.Flies.Client.Services.Implementations
{
    internal class SimpleInjectorContainer : IContainer
    {
        private readonly Container _container;

        public SimpleInjectorContainer()
        {
            _container = new Container
            {
                Options =
                {
                    ConstructorResolutionBehavior = new PreferredConstructorResolutionBehavior(),
                    DefaultLifestyle = Lifestyle.Singleton
                }
            };
        }

        public void Dispose()
        {
            _container?.Dispose();
        }

        public void Register<TImplementation>() where TImplementation : class
        {
            _container.Register<TImplementation>();
        }

        public void Register<TService, TImplementation>() where TService : class where TImplementation : class, TService
        {
            _container.Register<TService, TImplementation>();
        }

        public void RegisterAll<TService>() where TService : class
        {
            var baseType = typeof(TService);
            var allTypes = _container
                .GetTypesToRegister(baseType, new[] {baseType.Assembly})
                .ToArray();

            var registrations = allTypes
                .Select(type => Lifestyle.Singleton.CreateRegistration(type, _container))
                .ToArray();

            _container.RegisterCollection<TService>(registrations);
        }

        public TService Resolve<TService>() where TService : class
        {
            return _container.GetRegistration(typeof(TService)) != null
                ? _container.GetInstance<TService>()
                : default(TService);
        }

        public void Verify()
        {
            throw new NotImplementedException();
        }
    }
}