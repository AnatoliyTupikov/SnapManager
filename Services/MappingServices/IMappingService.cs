

namespace SnapManager.Services.MappingServices
{
    public interface IMappingService<S,T> 
        where S : class 
        where T : class
    {
        T MapToWievModel(S domainModel);

        //TreeItemBaseDModel MapToDomainModel(TreeItemWpfModel wpfModel);
    }
}
