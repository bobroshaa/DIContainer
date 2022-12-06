using System.Collections.Concurrent;
using System.Reflection;

namespace DependencyInjectionContainer;

public class DependencyProvider
{
    private DependenciesConfiguration _dependencies;
    public readonly ConcurrentDictionary<Type, object> singletonDependency = new();

    public DependencyProvider(DependenciesConfiguration dependencies)
    {
        _dependencies = dependencies;
    }

    public T Resolve<T>()
    {
        return (T)Resolve(typeof(T));
    }

    private object Resolve(Type type)
    {
        ImplenetationInfo implementation = _dependencies.Services[type];
        if (singletonDependency.ContainsKey(implementation.ImplementationType))
        {
            return singletonDependency[implementation.ImplementationType];
        }
        
        ConstructorInfo constructor = implementation.ImplementationType.GetConstructors()[0];
        ParameterInfo[] parameters = constructor.GetParameters();
        if (parameters.Length == 0)
        {
            return Activator.CreateInstance(implementation.ImplementationType);
        }

        List<object> initializedParameters = new List<object>(parameters.Length);
        foreach (var param in parameters)
        {
            initializedParameters.Add(Resolve(param.ParameterType));
        }

        var instance = constructor.Invoke(initializedParameters.ToArray());

        if (implementation.TimeToLive == LivingTime.Singleton)
        {
            singletonDependency[implementation.ImplementationType] = instance;
        }

        return instance;
    }
}