using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapManager.Models.DomainModels.CredentialsHierarchy
{
    [Table("folders")]
    public class FolderDModel : TreeItemDModel
    {
        [MaxLength(1024)]
        public string? Description { get; set; }
    }

    [Table("folders_with_credentials")]
    public class FolderWithCredentialsDModel : FolderDModel
    {

    }

    
} 
