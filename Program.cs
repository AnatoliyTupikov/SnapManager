using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnapManager.Data;
using Microsoft.Extensions.Configuration;
using System.IO;
using SnapManager.Views.WPF;
using System.Collections.ObjectModel;
using SnapManager.Views.WPF.WPFHelpers;

namespace SnapManager
{
    
    internal static class Program
    {
        public const string   appsettingsPath = "appsettings.json";
       
            
        public static IHost AppHost { get; private set; }
        [STAThread]
        public static void Main()
        {
            ErrorHandler.TryWindowed(()=> { Go(); }, "Error while starting app:");
        }

        public static void Go()
        {
            AppHost = Host
                // создаем хост приложения
                .CreateDefaultBuilder()
                .ConfigureWebHostDefaults(configure =>
                {
                    configure.UseUrls("http://localhost123:5005");
                    configure.UseStartup<WebServerStartup>();

                })

                // внедряем сервисы
                .ConfigureServices(services =>
                {
                    services.AddSingleton<App>();
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton<DbService>();

                })
                .ConfigureAppConfiguration(config =>
                {
                    config.AddJsonFile(appsettingsPath, optional: true, reloadOnChange: true);
                })
                .Build();
            // получаем сервис - объект класса App
            var app = AppHost.Services.GetService<App>();

            AppHost.Start();

            app?.Run();

            // запускаем приложения
        }
    }
}
