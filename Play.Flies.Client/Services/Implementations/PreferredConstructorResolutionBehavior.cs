using System;
using System.Linq;
using System.Reflection;
using SimpleInjector.Advanced;

namespace Play.Flies.Client.Services.Implementations
{
    internal class PreferredConstructorResolutionBehavior : IConstructorResolutionBehavior
    {
        public ConstructorInfo GetConstructor(Type serviceType, Type implementationType)
        {
            var constructors = implementationType.GetConstructors();
            return constructors.Length > 1
                ? constructors.First(ctor => ctor.GetCustomAttribute<PreferredConstructorAttribute>() != null)
                : constructors.First();
        }
    }
}