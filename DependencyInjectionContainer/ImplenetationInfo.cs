using System.Runtime.InteropServices.ComTypes;

namespace DependencyInjectionContainer;

public class ImplenetationInfo
{
    public ImplenetationInfo(LivingTime timeToLive, Type implementationType, Enum? index)
    {
        TimeToLive = timeToLive;
        ImplementationType = implementationType;
        Index = index;
    }
    public Enum? Index { get; set; }
    public LivingTime TimeToLive { get; set; }
    public Type ImplementationType{ get; set; }
}