using System.Collections.Concurrent;

namespace DependencyInjectionContainer;

public class DependenciesConfiguration
{
    public readonly ConcurrentDictionary<Type, ImplementationInfo> Services = new();
    public readonly ConcurrentDictionary<Type, List<ImplementationInfo>> EnumerableServices = new();

    public void Register<TDependency, TImplementation>(LivingTime timeToLive = LivingTime.InstancePerDependency, Enum? index = null )
    {
        Register(typeof(TDependency), typeof(TImplementation), timeToLive, index);
    }
    
    public void Register(Type TDependency, Type TImplementation, LivingTime timeToLive = LivingTime.InstancePerDependency, Enum? index = null )
    {
        if (Services.ContainsKey(TDependency))
            EnumerableServices[TDependency].Add(new ImplementationInfo(timeToLive, TImplementation, index));
        else
        {
            EnumerableServices[TDependency] = new List<ImplementationInfo>()
                { new (timeToLive, TImplementation, index) };
            Services[TDependency] = new ImplementationInfo(timeToLive, TImplementation, index);
        }
    }
}