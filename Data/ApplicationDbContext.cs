using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using SnapManager.Models.WPFModels;
using SnapManager.Models.WPFModels.Hierarchy;

namespace SnapManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        //В C# 8.0+ включена поддержка nullable reference types. Компилятор требует, чтобы все не-nullable свойства были инициализированы. Но DbSet<T> инициализируется внутри DbContext базовым классом, а не в вашем конструкторе.
        //Чтобы избежать предупреждения, используется = null!; — это говорит компилятору: "Я знаю, что это не будет null во время выполнения".
        //	Влияет ли это на миграции или очистку таблиц? Нет.Это только для компилятора. Миграции инициализируют и изменяют структуру базы данных, но не очищают таблицы из-за этой записи.
        public DbSet<TreeItemWpfModel> TreeItems { get; set; } = null!;
        public DbSet<FolderWpfModel> Folders { get; set; } = null!;
        public DbSet<FolderWithCredentialsWpfModel> FoldersWithCredentials { get; set; } = null!;
        public DbSet<CredentialWpfModel> Credentials { get; set; } = null!;
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
           
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TreeItemWpfModel>().UseTptMappingStrategy();    // устанавливаем подход TPT
        }





    }
}
