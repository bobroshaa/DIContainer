namespace DITests;

interface IService {}
class ServiceImpl : IService
{
    public ServiceImpl(IRepository repository) // ServiceImpl зависит от IRepository
    {
       
    }
}

interface IRepository{}
class RepositoryImpl : IRepository
{
    public RepositoryImpl(){} // может иметь свои зависимости, опустим для простоты
}