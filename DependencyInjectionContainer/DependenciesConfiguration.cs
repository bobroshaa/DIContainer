using System.Collections.Concurrent;

namespace DependencyInjectionContainer;

public class DependenciesConfiguration
{
    public readonly ConcurrentDictionary<Type, ImplenetationInfo> Services = new();
    public readonly ConcurrentDictionary<Type, List<ImplenetationInfo>> EnumerableServices = new();

    public void Register<TDependency, TImplementation>(LivingTime timeToLive = LivingTime.InstancePerDependency)
    {
        if (Services.ContainsKey(typeof(TDependency)))
            EnumerableServices[typeof(TDependency)] = new List<ImplenetationInfo>()
                { Services[typeof(TDependency)], new ImplenetationInfo(timeToLive, typeof(TImplementation)) };
        else
            Services[typeof(TDependency)] = new ImplenetationInfo(timeToLive, typeof(TImplementation));
    }
}