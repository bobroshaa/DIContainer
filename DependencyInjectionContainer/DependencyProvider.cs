using System.Collections;
using System.Collections.Concurrent;
using System.ComponentModel.Design.Serialization;
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
        if (typeof(T).GetGenericTypeDefinition().Name.Contains("IEnumerable"))
        {
            return (T)ResolveEnum(typeof(T));
        }

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
        object instance;
        if (parameters.Length == 0)
        {
            instance = Activator.CreateInstance(implementation.ImplementationType);
            if (implementation.TimeToLive == LivingTime.Singleton)
            {
                singletonDependency[implementation.ImplementationType] = instance;
            }

            return instance;
        }

        List<object> initializedParameters = new List<object>(parameters.Length);
        foreach (var param in parameters)
        {
            initializedParameters.Add(Resolve(param.ParameterType));
        }

        instance = constructor.Invoke(initializedParameters.ToArray());

        if (implementation.TimeToLive == LivingTime.Singleton)
        {
            singletonDependency[implementation.ImplementationType] = instance;
        }

        return instance;
    }


    // TODO Вынести в отдельный метод, тк повтор идет
    private IEnumerable<object> ResolveEnum(Type type)
    {
        var implementations = createList(type.GenericTypeArguments[0]);
        foreach (var implementation in _dependencies.EnumerableServices[type.GenericTypeArguments[0]])
        {
            object instance;
            if (singletonDependency.ContainsKey(implementation.ImplementationType))
            {
                implementations.Add(singletonDependency[implementation.ImplementationType]);
            }

            ConstructorInfo constructor = implementation.ImplementationType.GetConstructors()[0];
            ParameterInfo[] parameters = constructor.GetParameters();
            if (parameters.Length == 0)
            {
                instance = Activator.CreateInstance(implementation.ImplementationType);
                if (implementation.TimeToLive == LivingTime.Singleton)
                {
                    singletonDependency[implementation.ImplementationType] = instance;
                }

                implementations.Add(instance);
                continue;
            }

            List<object> initializedParameters = new List<object>(parameters.Length);
            foreach (var param in parameters)
            {
                initializedParameters.Add(Resolve(param.ParameterType));
            }

            instance = constructor.Invoke(initializedParameters.ToArray());

            if (implementation.TimeToLive == LivingTime.Singleton)
            {
                singletonDependency[implementation.ImplementationType] = instance;
            }

            implementations.Add(instance);
        }

        return (IEnumerable<object>)implementations;
    }
    
    public IList createList(Type myType)
    {
        Type genericListType = typeof(List<>).MakeGenericType(myType);
        return (IList)Activator.CreateInstance(genericListType);
    }
}