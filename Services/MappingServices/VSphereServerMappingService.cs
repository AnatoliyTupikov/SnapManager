using SnapManager.Models.DomainModels;
using SnapManager.Models.WPFModels;
using SnapManager.Models.WPFModels.CredentialHierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapManager.Services.MappingServices
{
    internal class VSphereServerMappingService : IMappingService<VSphereServerDModel, VSphereServerWpfModel>
    {
        public VSphereServerWpfModel MapToWievModel(VSphereServerDModel domainModel)
        {
            var result = new VSphereServerWpfModel
            {
                Name = domainModel.Name,
                Host = domainModel.Host,
                Port = domainModel.Port,
                Credential = new TreeItemMappingService().MapToWievModel(domainModel.Credential) as CredentialWpfModel,
                Description = domainModel.Description,
                CreationDateUTC = domainModel.CreationDateUTC
            };
            return result;
        }
    }
}
