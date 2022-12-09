using System.Collections.Concurrent;
using System.Reflection;

namespace DependencyInjectionContainer;

public class DependenciesConfiguration
{
    public readonly ConcurrentDictionary<Type, ImplenetationInfo> Services = new();
    public readonly ConcurrentDictionary<Type, List<ImplenetationInfo>> EnumerableServices = new();

    public void Register<TDependency, TImplementation>(LivingTime timeToLive = LivingTime.InstancePerDependency, Enum? index = null )
    {
        if (Services.ContainsKey(typeof(TDependency)))
            EnumerableServices[typeof(TDependency)].Add(new ImplenetationInfo(timeToLive, typeof(TImplementation), index));
        else
        {
            EnumerableServices[typeof(TDependency)] = new List<ImplenetationInfo>()
                { new (timeToLive, typeof(TImplementation), index) };
            Services[typeof(TDependency)] = new ImplenetationInfo(timeToLive, typeof(TImplementation), index);
        }
    }
    
    public void Register(Type TDependency, Type TImplementation, LivingTime timeToLive = LivingTime.InstancePerDependency, Enum? index = null )
    {
        if (Services.ContainsKey(TDependency))
            EnumerableServices[TDependency].Add(new ImplenetationInfo(timeToLive, TImplementation, index));
        else
        {
            EnumerableServices[TDependency] = new List<ImplenetationInfo>()
                { new (timeToLive, TImplementation, index) };
            Services[TDependency] = new ImplenetationInfo(timeToLive, TImplementation, index);
        }
    }
}