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
        public CredentialDModel? CustedDModel { get => base.DModel as CredentialDModel; }
        public string? Username { get; set; }

        public string? Password { get; set; }

        public string? Description { get; set; }

        public DateTime? CreationDateUTC { get; set; }

        public DateTime? CreationDateLocal { 
            get
            {   if (CreationDateUTC == null) return null;             
                return CreationDateUTC?.ToLocalTime();
            } }

        public DateTime? ModificationDateUTC { get; set; }

        public DateTime? ModificationDateLocal
        {
            get
            {
                if (ModificationDateUTC == null) return null;
                return ModificationDateUTC?.ToLocalTime();
            }
        }           

        public CredentialWpfModel()
        {
            DModel = new CredentialDModel();
        }
        public override bool Equals(TreeItemWpfModel? other)
        {
            if (other == null) return false;
            if (other is not CredentialWpfModel otherCredential) return false;
            return base.Equals(other) && Username == otherCredential.Username && Password == otherCredential.Password && Description == otherCredential.Description;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Username, Password);
        }

        public override void CopyValueFrom(TreeItemWpfModel source)
        {
            if (source is not CredentialWpfModel credentialSource)
            {
                throw new InvalidOperationException("Source must be of type Credential.");
            }
            Username = credentialSource.Username;
            Password = credentialSource.Password;
            CreationDateUTC = credentialSource.CreationDateUTC;
            ModificationDateUTC = credentialSource.ModificationDateUTC;
            Description = credentialSource.Description;
            base.CopyValueFrom(source);
        }


    }
}
