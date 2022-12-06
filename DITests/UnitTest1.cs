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
}