using DependencyInjectionContainer;

namespace DITests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var dependencies = new DependenciesConfiguration();
        dependencies.Register<IService1, Service1>();
        dependencies.Register<AbstractService2, Service2>();
 
        var provider = new DependencyProvider(dependencies);
        var service1 = provider.Resolve<IService1>();
        Assert.That(service1.GetType().ToString(), Is.EqualTo("DITests.Service1"));
    }
    
    [Test]
    public void Test2()
    {
        var dependencies = new DependenciesConfiguration();
        dependencies.Register<IService, ServiceImpl>();
        dependencies.Register<IRepository, RepositoryImpl>();
 
        var provider = new DependencyProvider(dependencies);
        
        var service1 = provider.Resolve<IService>();
        Assert.That(service1.GetType().ToString(), Is.EqualTo("DITests.ServiceImpl"));
    }

    [Test]
    public void Test3()
    {
        var dependencies = new DependenciesConfiguration();
        dependencies.Register<IService1, Service1>(LivingTime.Singleton);
 
        var provider = new DependencyProvider(dependencies);
        var service1 = provider.Resolve<IService1>();
        service1.SomeData = "singleton";
        var newService1 = provider.Resolve<IService1>();
        Assert.That(newService1.SomeData, Is.EqualTo("singleton"));
    }
    
    [Test]
    public void Test4()
    {
        var dependencies = new DependenciesConfiguration();

        dependencies.Register<IService1, Service1>();
        dependencies.Register<IService1, Service3>();
        var provider = new DependencyProvider(dependencies);
        IEnumerable<IService1> services = provider.Resolve<IEnumerable<IService1>>();
        Assert.That(services.Count(), Is.EqualTo(2));
    }
    
    interface IService5<TRepository> where TRepository : IRepository
    {
            
    }

    class ServiceImpl5<TRepository> : IService5<TRepository> 
        where TRepository : IRepository
    {
        public ServiceImpl5(TRepository repository)
        {
               
        }
    }
    
    [Test]
    public void Test5()
    {
        var dependencies = new DependenciesConfiguration();

        dependencies.Register<IRepository, RepositoryImpl>();
        dependencies.Register<IService5<IRepository>, ServiceImpl5<IRepository>>();
        var provider = new DependencyProvider(dependencies);
        var services = provider.Resolve<IService5<IRepository>>();
    }
}