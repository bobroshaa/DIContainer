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
    
    interface IService5<T>
    {
            
    }

    class ServiceImpl5<T> : IService5<T>
    {
        public ServiceImpl5(T repository)
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
        var service = provider.Resolve<IService5<IRepository>>();
        Assert.That(service.GetType().ToString(), Is.EqualTo("DITests.Tests+ServiceImpl5`1[DITests.IRepository]"));
    }
    
    [Test]
    public void Test6()
    {
        var dependencies = new DependenciesConfiguration();

        dependencies.Register(typeof(IService5<>), typeof(ServiceImpl5<>));
        dependencies.Register<IRepository, RepositoryImpl>();
        var provider = new DependencyProvider(dependencies);
        var service = provider.Resolve<IService5<IRepository>>();
        Assert.That(service.GetType().ToString(), Is.EqualTo("DITests.Tests+ServiceImpl5`1[DITests.IRepository]"));
    }

}