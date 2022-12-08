using System.Collections.Concurrent;
using System.Reflection;

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
    
    public void Register(Type TDependency, Type TImplementation, LivingTime timeToLive = LivingTime.InstancePerDependency)
    {
        if (Services.ContainsKey(TDependency))
            EnumerableServices[TDependency] = new List<ImplenetationInfo>()
                { Services[TDependency], new ImplenetationInfo(timeToLive, TImplementation) };
        else
            Services[TDependency] = new ImplenetationInfo(timeToLive, TImplementation);
    }
}