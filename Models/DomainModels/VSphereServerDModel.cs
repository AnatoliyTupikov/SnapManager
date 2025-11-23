using SnapManager.Models.DomainModels.CredentialsHierarchy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapManager.Models.DomainModels
{
    [Table("vsphere_servers")]
    public class VSphereServerDModel
    {
        public int Id { get; set; }

        [MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(256)]
        public string Host { get; set; }
        public int Port { get; set; }
        public CredentialDModel Credential { get; set; }

        [MaxLength(1024)]
        public string? Description { get; set; }

        public DateTime CreationDateUTC { get; set; }
    }
}
