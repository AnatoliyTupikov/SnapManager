using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnapManager.Models.WPFModels.Hierarchy;

namespace SnapManager.Models.DomainModels
{
    [Table("folders")]
    public class FolderDModel : TreeItemBaseDModel
    {
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }

        [MaxLength(1024)]
        public string? Description { get; set; }
    }

    [Table("folders_with_credentials")]
    public class FolderWithCredentialsDModel : FolderDModel
    {

    }

    
} 
