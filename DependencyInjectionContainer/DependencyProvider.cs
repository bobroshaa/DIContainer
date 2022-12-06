using System.Reflection;

namespace DependencyInjectionContainer;

public class DependencyProvider
{
    private DependenciesConfiguration _dependencies;

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
        Type implementation = _dependencies.Services[type];
        ConstructorInfo constructor = implementation.GetConstructors()[0];
        ParameterInfo[] parameters = constructor.GetParameters();
        if (parameters.Length == 0)
        {
            return Activator.CreateInstance(implementation);
        }

        List<object> initializedParameters = new List<object>(parameters.Length);
        foreach (var param in parameters)
        {
            initializedParameters.Add(Activator.CreateInstance(param.ParameterType));
        }
        return constructor.Invoke(initializedParameters.ToArray());
    }
    
    // TODO: add Resolve<IEnumerable<T>>
}