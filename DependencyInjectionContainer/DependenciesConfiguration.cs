using System.Collections.Concurrent;

namespace DependencyInjectionContainer;

public class DependenciesConfiguration
{
    public readonly ConcurrentDictionary<Type,ImplenetationInfo> Services = new ();
    public void Register<TDependency, TImplementation>(LivingTime timeToLive = LivingTime.InstancePerDependency)
    {
        Services[typeof(TDependency)] = new ImplenetationInfo(timeToLive, typeof(TImplementation));
    }
}