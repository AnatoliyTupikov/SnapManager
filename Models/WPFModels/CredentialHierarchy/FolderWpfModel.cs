using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnapManager.Models.DomainModels.CredentialsHierarchy;

namespace SnapManager.Models.WPFModels.CredentialHierarchy
{
    public class FolderWpfModel : TreeItemWpfModel
    {
        public FolderDModel? CastedDModel 
        { 
            get 
            { 
                var res = DModel as FolderDModel;
                if (res == null) throw new InvalidOperationException("DModel is not of type FolderDModel.");
                return res;
            }
        }

        private string? description;

        public string? Description
        {
            get { return description; }
            set { OnPropertyChanged(ref description, value); }
        }


        public FolderWpfModel()
        {
            DModel = new FolderDModel();
        }
        public override void UpdateValuesToDModel()
        {
            CastedDModel!.Description = Description;
            base.UpdateValuesToDModel();
        }
        public override void PullValuesFromDModel()
        {
            Description = CastedDModel!.Description;
            base.PullValuesFromDModel();
        }

        public override bool IsEqualToDModel()
        {           
            return base.IsEqualToDModel() &&
                   Description == CastedDModel!.Description;
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
