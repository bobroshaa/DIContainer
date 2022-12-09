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

    public T Resolve<T>(Enum? index = null)
    {
        if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition().Name.Contains("IEnumerable"))
        {
            return (T)ResolveEnum(typeof(T));
        }

        if (index != null)
        {
            return (T)Resolve(typeof(T), index);
        }

        return (T)Resolve(typeof(T));
    }

    private object Resolve(Type type, Enum index)
    {
        object inst = null;
        foreach (var implementation in _dependencies.EnumerableServices[type])
        {
            if (!implementation.Index.Equals(index))
                continue;
            if (singletonDependency.ContainsKey(implementation.ImplementationType))
            {
                inst = singletonDependency[implementation.ImplementationType];
                return inst;
            }

            ConstructorInfo constructor = implementation.ImplementationType.GetConstructors()[0];
            ParameterInfo[] parameters = constructor.GetParameters();
            if (parameters.Length == 0)
            {
                inst = Activator.CreateInstance(implementation.ImplementationType);
                if (implementation.TimeToLive == LivingTime.Singleton)
                {
                    singletonDependency[implementation.ImplementationType] = inst;
                }

                return inst;
            }

            List<object> initializedParameters = new List<object>(parameters.Length);
            foreach (var param in parameters)
            {
                initializedParameters.Add(Resolve(param.ParameterType));
            }

            inst = constructor.Invoke(initializedParameters.ToArray());

            if (implementation.TimeToLive == LivingTime.Singleton)
            {
                singletonDependency[implementation.ImplementationType] = inst;
            }
        }
        return inst;
    }

    private object Resolve(Type type)
    {
        if (!_dependencies.Services.TryGetValue(type, out ImplenetationInfo implementation))
        {
            if (type.IsGenericType)
            {
                if (!_dependencies.Services.TryGetValue(type.GetGenericTypeDefinition(),
                        out implementation))
                {
                    return null;
                }
            }
            else
            {
                /*var t = type.GetInterfaces()[0];
                if (!_dependencies.Services.TryGetValue(type.GetInterfaces()[0], out implementation))
                {
                    return null;
                }
                else
                {
                    return type.DeclaringType.MakeGenericType(type.GetInterfaces()[0]);
                }*/
                return null;
            }
        }

        if (singletonDependency.ContainsKey(implementation.ImplementationType))
        {
            return singletonDependency[implementation.ImplementationType];
        }
        var instanceType = implementation.ImplementationType;
        if (instanceType.IsGenericTypeDefinition)
        {
            instanceType = instanceType.MakeGenericType(type.GenericTypeArguments);
        }
        ConstructorInfo constructor = instanceType.GetConstructors()[0];
        ParameterInfo[] parameters = constructor.GetParameters();
        object instance;
        if (parameters.Length == 0)
        {
            instance = Activator.CreateInstance(instanceType);
            if (implementation.TimeToLive == LivingTime.Singleton)
            {
                singletonDependency[instanceType] = instance;
            }

            return instance;
        }

        List<object> initializedParameters = new List<object>(parameters.Length);
        foreach (var param in parameters)
        {
            initializedParameters.Add(Resolve(param.ParameterType));
        }

        
        instance = constructor.Invoke(initializedParameters.ToArray());
        //instance = Activator.CreateInstance(implementation.ImplementationType, initializedParameters.ToArray());

        if (implementation.TimeToLive == LivingTime.Singleton)
        {
            singletonDependency[instanceType] = instance;
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