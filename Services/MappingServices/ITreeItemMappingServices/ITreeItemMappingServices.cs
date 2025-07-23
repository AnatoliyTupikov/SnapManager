using SnapManager.Models.DomainModels;
using SnapManager.Models.WPFModels.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapManager.Services.MappingServices.ITreeItemMappingServices
{
    public interface ITreeItemMappingServices
    {
        TreeItemWpfModel MapToWPFModel(TreeItemBaseDModel domainModel);

        //TreeItemBaseDModel MapToDomainModel(TreeItemWpfModel wpfModel);
    }
}
