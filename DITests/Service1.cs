namespace DITests;
public interface IService1
{
    public void DoSomething();
}
public class Service1 : IService1
{
    public void DoSomething()
    {
        Console.WriteLine("Do something");
    }
}