using System.Collections.Concurrent;

namespace DependencyInjectionContainer;

public class DependenciesConfiguration
{
    public readonly ConcurrentDictionary<Type,Type> Services = new ();
    public void Register<TContract, TImplementation>() {
        
    }
}