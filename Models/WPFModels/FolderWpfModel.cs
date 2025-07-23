using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnapManager.Models.DomainModels;
using SnapManager.Models.WPFModels.Hierarchy;

namespace SnapManager.Models.WPFModels
{
    public class FolderWpfModel : TreeItemWpfModel
    {
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }

        public string? Description { get; set; }

        public FolderWpfModel()
        {
            DModel = new FolderDModel();
        }

        public override bool Equals(TreeItemWpfModel? other)
        {
            if (other == null) return false;
            if (other is not FolderWpfModel otherFolder) return false;
            return base.Equals(other) && Description == otherFolder.Description;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Description);
        }

        public override void CopyValueFrom(TreeItemWpfModel source)
        {
            if (source is not FolderWpfModel folderSource)
            {
                throw new InvalidOperationException("Source must be of type Folder.");
            }            
            CreationDate = folderSource.CreationDate;
            ModificationDate = folderSource.ModificationDate;
            Description = folderSource.Description;
            base.CopyValueFrom(source);
        }

    }
    public class FolderWithCredentialsWpfModel : FolderWpfModel
    {
        public FolderWithCredentialsWpfModel() 
        {
            DModel = new FolderWithCredentialsDModel();
        }

    }

    
} 
