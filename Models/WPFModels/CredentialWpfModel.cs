using SnapManager.Models.DomainModels;
using SnapManager.Models.WPFModels.Hierarchy;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnapManager.Models.WPFModels
{
    [Table("Credentials")]
    public class CredentialWpfModel : TreeItemWpfModel
    {
        public CredentialDModel? CastedDModel
        {
            get
            {
                var res = base.DModel as CredentialDModel;
                if (res == null) throw new InvalidOperationException("DModel is not of type CredentialDModel.");
                return res;
            }
        }

        private string username;

        [Required(ErrorMessage = "Username is required.")]
        public string Username
        {
            get { return username; }
            set { OnPropertyChanged<string>(ref username, value); ValidateProperty(value, nameof(Username)); }
        }


        private string password;
        [Required(ErrorMessage = "Password is required.")]
        public string Password
        {
            get { return password; }
            set { OnPropertyChanged<string>(ref password, value); ValidateProperty(value, nameof(Password)); }
        }

        private string? description;

        public string? Description
        {
            get { return description; }
            set { OnPropertyChanged<string?>(ref description, value); }
        }

        public CredentialWpfModel()
        {
            username = string.Empty;
            password = string.Empty;
            DModel = new CredentialDModel();
        }

        public override void UpdateValuesToDModel()
        {

            CastedDModel!.Username = Username;
            CastedDModel!.Password = Password;
            CastedDModel!.Description = Description;
            base.UpdateValuesToDModel();
        }

        public override void PullValuesFromDModel()
        {
            Username = CastedDModel!.Username;
            Password = CastedDModel!.Password;
            Description = CastedDModel!.Description;
            base.PullValuesFromDModel();
        }

        public override bool IsEqualToDModel()
        {
            return base.IsEqualToDModel() &&
                   Username == CastedDModel!.Username &&
                   Password == CastedDModel!.Password &&
                   Description == CastedDModel!.Description;
        }
    }
}
