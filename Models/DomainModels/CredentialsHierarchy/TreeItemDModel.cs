
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
namespace SnapManager.Models.DomainModels.CredentialsHierarchy
{
    [Table("hierarchy_scheme")]
    public abstract class TreeItemDModel
    {        
    
        public int Id { get; set; }        
        public string Name { get; set; }
        public TreeItemDModel? Parent { get; set; }
        public List<TreeItemDModel>? Children { get; set; }
        public DateTime CreationDateUTC { get; set; }
        public DateTime ModificationDateUTC { get; set; }



    }
    

}
