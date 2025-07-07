using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using SnapManager.Data;
using SnapManager.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
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

        /// <summary>
        /// Initializes the database settings from a connection string builder.
        /// Throw DbConfigurationException if the connection string is invalid or missing required parameters.
        /// </summary>
        /// <param name="connectionstringbuilder"></param>
        /// <exception cref="DBConfigurationException"></exception>
        public override void Initialize(DbConnectionStringBuilder connectionstringbuilder)
        {
                           
            var castedBuilder = (NpgsqlConnectionStringBuilder)connectionstringbuilder;
            bool throwed = false;
            string errorMessage = "";
            if (string.IsNullOrEmpty(castedBuilder.Host))
            {
                throwed = true;
                errorMessage += "Hostname is not specified.\n";
            }                
            if (string.IsNullOrEmpty(castedBuilder.Database))
            {
                throwed = true;
                errorMessage += "Database is not specified.\n";
            }                
            if (string.IsNullOrEmpty(castedBuilder.Username))
            {
                throwed = true;
                errorMessage += "Username is not specified.\n";
            }  
                
            if (throwed) throw new DBConfigurationException(errorMessage, 120, Services.Severity.Warning);
                
            base.IsInitialized = true;


            Hostname = castedBuilder.Host!;
            Port = castedBuilder.Port;
            Database = castedBuilder.Database!;
            Username = castedBuilder.Username!;
            Password = castedBuilder.Password!;
            
        }

        /// <summary>
        /// Initializes the database settings from a connection string.
        /// Throw DbConfigurationException if the connection string is invalid or missing required parameters.
        /// </summary>
        /// <param name="connectionstring"></param>
        /// <exception cref="DBConfigurationException">Если не указан обязательный параметр</exception>"
        /// <exception cref="ArgumentException">Если строка подключения содержит недопустимые параметры.</exception>
        /// <exception cref="FormatException">Если значение параметра имеет неверный формат.</exception>
        public override void Initialize(string? connectionstring) 
        { 
            if (string.IsNullOrEmpty(connectionstring)) throw new DBConfigurationException($"Settings for the \"{this.DisplayProviderName}\" database are not specified.", 120, Services.Severity.Warning);
            
            Initialize(new NpgsqlConnectionStringBuilder(connectionstring)); 
        }

        public override string GetConnectionString()
        {
            return $"Host={Hostname};Port={Port};Database={Database}; Username={Username};Password={Password}; Persist Security Info=True;";
        }

        public override NpgsqlConnectionStringBuilder GetConnectionStringBuilder() => new NpgsqlConnectionStringBuilder(GetConnectionString());

        public override DbContextOptionsBuilder<ApplicationDbContext> GetDbContextOptionsBuilder() 
        {
            //загрузка сборки миграции, так как она не загружается автоматически при запуске приложения из-за того,
            //что у проекта приложения нет зависимости на проект миграции.
            //А сделать зависимость не получится, так как проект миграции сам ссылается на проект приложения и получается циклическая зависимость.
            var migrationAssemblyPath = Path.Combine(AppContext.BaseDirectory, "SnapManager.Migrations.NpgsqlServer.dll");
            if (File.Exists(migrationAssemblyPath))
            {
                Assembly.LoadFrom(migrationAssemblyPath);
            }
            else
            {
                throw new FileNotFoundException("Не найден файл миграции", migrationAssemblyPath);
            }
            var OptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            OptionsBuilder.UseNpgsql(GetConnectionString(), b => b.MigrationsAssembly("SnapManager.Migrations.NpgsqlServer"));
            return OptionsBuilder;
        }
        
    }
}
