using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnapManager.Models.Hierarchy;

namespace SnapManager.Models
{
    [Table("Folders")]
    public class Folder : TreeItemBase
    {
        //public int FolderId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }

        [MaxLength(1024)]
        public string? Description { get; set; }

        public override bool Equals(TreeItemBase? other)
        {
            if (other == null) return false;
            if (other is not Folder otherFolder) return false;
            return base.Equals(other) && Description == otherFolder.Description;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Description);
        }

        public override void CopyValueFrom(TreeItemBase source)
        {
            if (source is not Folder folderSource)
            {
                throw new InvalidOperationException("Source must be of type Folder.");
            }            
            this.CreationDate = folderSource.CreationDate;
            this.ModificationDate = folderSource.ModificationDate;
            this.Description = folderSource.Description;
            base.CopyValueFrom(source);
        }

    }
    [Table("FoldersWithCredentials")]
    public class FolderWithCredentials : Folder
    {

    }

    
} 
