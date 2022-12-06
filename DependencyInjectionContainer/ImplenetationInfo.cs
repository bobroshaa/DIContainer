namespace DependencyInjectionContainer;

public class ImplenetationInfo
{
    public ImplenetationInfo(LivingTime timeToLive, Type implementationType)
    {
        TimeToLive = timeToLive;
        ImplementationType = implementationType;
    }
    public LivingTime TimeToLive { get; set; }
    public Type ImplementationType{ get; set; }
}