using SnapManager.Models;
using SnapManager.Models.WPFModels.Hierarchy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.Primitives;

namespace SnapManager.Models.DomainModels
{
    [Table("hierarchy_scheme")]
    public class TreeItemBaseDModel
    {        
    
        public int Id { get; set; }        
        public string Name { get; set; }
        public TreeItemBaseDModel? Parent { get; set; }
        public List<TreeItemBaseDModel>? Children { get; set; }        

       

    }
    

}
