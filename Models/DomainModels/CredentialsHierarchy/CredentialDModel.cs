
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnapManager.Models.DomainModels.CredentialsHierarchy
{
    [Table("credentials")]
    public class CredentialDModel : TreeItemDModel
    {
        [MaxLength(256)]
        public string Username { get; set; }

        [MaxLength(1024)]
        public string Password { get; set; }        
        
        [MaxLength(1024)]
        public string? Description { get; set; }

        // в data context на прямую не указан dbset для Credential, но из-за этого навигационного свойства EF все равно добвит Credential таблицу в базу данных

    }
}
