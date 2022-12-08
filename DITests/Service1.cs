namespace DITests;
public interface IService1
{
    public string SomeData { get; set; }
}
public class Service1 : IService1
{
    public string SomeData { get; set; }
    public void DoSomething()
    {
        Console.WriteLine("Do something");
    }
}

public class Service3 : IService1
{
    public string SomeData { get; set; }
}