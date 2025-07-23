using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using SnapManager.Data;
using System.Reflection;

namespace SnapManager.Models
{
    
    public abstract  class DBSettingsBase
    {
        [Required]
        [Display(Name = "Hostname", Order = 1)]
        public virtual string Hostname { get; set; }

        [Required]
        [Display(Name = "Database", Order = 3)]
        public virtual string Database { get; set; }

        public virtual bool IsInitialized { get; protected set; } = false;
        public virtual string DisplayProviderName 
        {
            get => GetType().GetCustomAttribute<DisplayAttribute>()?.Name ?? GetType().Name; 
        }

        public abstract void Initialize(string? connectionstring);
        public abstract void Initialize(DbConnectionStringBuilder connectionstring);

        public abstract string GetConnectionString();
        public abstract DbConnectionStringBuilder GetConnectionStringBuilder();

        public abstract DbContextOptionsBuilder<ApplicationDbContext> GetDbContextOptionsBuilder();

       
        

    }
}
