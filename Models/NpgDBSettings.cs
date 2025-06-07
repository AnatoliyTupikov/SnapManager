using Microsoft.EntityFrameworkCore;
using Npgsql;
using SnapManager.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapManager.Models
{
    [Display(Name = "PostgreSQL")]
    public class NpgDBSettings : DBSettingsBase
    {
        [Required]
        [Range(1024, 65535, ErrorMessage = "Invalid port range")]
        [Display(Name = "Port", Order =2)]        
        public int Port { get; set; }

        [Required]
        [Display(Name = "Username", Order = 4)]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Password", Order = 5)]
        public string Password { get; set; }

        public NpgDBSettings()
        {
            Hostname = "";
            Port = 5432; // Default PostgreSQL port
            Database = "";
            Username = "";
            Password = "";
        }

        public override void FillUp(DbConnectionStringBuilder connectionstringbuilder)
        {
            var castedBuilder = (NpgsqlConnectionStringBuilder)connectionstringbuilder;
            Hostname = castedBuilder.Host ?? "";
            Port = castedBuilder.Port;
            Database = castedBuilder.Database ?? "";
            Username = castedBuilder.Username ?? "";
            Password = castedBuilder.Password ?? "";



        }

        public override void FillUp(string? connectionstring) => FillUp(new NpgsqlConnectionStringBuilder(connectionstring));

        public override string GetConnectionString()
        {
            return $"Host={Hostname};Port={Port};Database={Database}; Username={Username};Password={Password}; Persist Security Info=True;";
        }

        public override NpgsqlConnectionStringBuilder GetConnectionStringBuilder() => new NpgsqlConnectionStringBuilder(GetConnectionString());

        public override DbContextOptionsBuilder<ApplicationDbContext> GetDbContextOptionsBuilder() 
        {
            var OptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            OptionsBuilder.UseNpgsql(GetConnectionString());
            return OptionsBuilder;
        }
        
    }
}
