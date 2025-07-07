using SnapManager.Models.Hierarchy;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnapManager.Models
{
    [Table("Credentials")]
    public class Credential : TreeItemBase
    {
        //public int CredentialId { get; set; }

        [MaxLength(256)]
        public string Username { get; set; }

        [MaxLength(1024)]
        public string Password { get; set; }

        public DateTime CreationDateUTC { get; set; }

        [NotMapped]
        public DateTime? CreationDateLocal { 
            get 
            {
                if (CreationDateUTC == null) return null;
                return CreationDateUTC.ToLocalTime();
            } }

        public DateTime ModificationDateUTC { get; set; }

        [NotMapped]
        public DateTime? ModificationDateLocal
        {
            get
            {
                if (CreationDateUTC == null) return null;
                return ModificationDateUTC.ToLocalTime();
            }
        }

        [MaxLength(1024)]
        public string? Description { get; set; }

        public override bool Equals(TreeItemBase? other)
        {
            if (other == null) return false;
            if (other is not Credential otherCredential) return false;
            return base.Equals(other) && Username == otherCredential.Username && Password == otherCredential.Password && Description == otherCredential.Description;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Username, Password);
        }

        public override void CopyValueFrom(TreeItemBase source)
        {
            if (source is not Credential credentialSource)
            {
                throw new InvalidOperationException("Source must be of type Credential.");
            }
            this.Username = credentialSource.Username;
            this.Password = credentialSource.Password;
            this.CreationDateUTC = credentialSource.CreationDateUTC;
            this.ModificationDateUTC = credentialSource.ModificationDateUTC;
            this.Description = credentialSource.Description;
            base.CopyValueFrom(source);
        }

        // в data context на прямую не указан dbset для Folder, но из-за этого навигационного свойства EF все равно добвит Folder таблицу в базу данных
        //public Folder? Folder { get; set; }

    }
}
