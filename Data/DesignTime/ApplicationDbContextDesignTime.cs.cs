using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SnapManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapManager.Data.DesignTime
{
    internal class ApplicationDbContextDesignTime : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // Укажите строку подключения вручную
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=test; Username=ADO;Password=123; Persist Security Info=True;");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
