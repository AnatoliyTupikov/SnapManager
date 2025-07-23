using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Npgsql;
using SnapManager.Exceptions;
using SnapManager.Models;
using SnapManager.Models.WPFModels;
using SnapManager.Services;
using SnapManager.Views.WPF.WPFHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace SnapManager.Data
{

    public class DbService 
    {
        private Dictionary<string, DBSettingsBase> _dbSettingsList = new Dictionary<string, DBSettingsBase>
        {
            [typeof(NpgDBSettings).GetCustomAttribute<DisplayAttribute>()?.Name ?? typeof(NpgDBSettings).Name]  = new NpgDBSettings()
        };
        /// <summary>
        /// Свойство для получения списка допустимых провайдеров баз данных. А также их конфигураций, которые были получены из appsettings.json при инициализации сервиса.
        /// Каждый раз создает новый экземпляр KeyValuePair, чтобы избежать проблем с изменением словаря.
        /// </summary>
        public Dictionary<string, DBSettingsBase> DbSettingsList => new(_dbSettingsList);


        private KeyValuePair<string, DBSettingsBase> _selectedDbProvider;

        /// <summary>
        /// Свойство для получения выбранного провайдера базы данных.
        /// Каждый раз создает новый экземпляр KeyValuePair, чтобы избежать проблем с изменением словаря.
        /// </summary>
        public KeyValuePair<string, DBSettingsBase> SelectedDbProvider => new(_selectedDbProvider.Key, _selectedDbProvider.Value);


        private readonly IConfiguration configuration;
        //public AlloWedDBTypes SelectedDbProvider { get => GetSelectedDbProviderFromConfig(); } 

        public DbService(IConfiguration configuration)
        {
            this.configuration = configuration;// получение конфигураций из файла appsettings.json
            InitializeDbConfigurationsFromAppSett();
            //dboption = new DbContextOptions<ApplicationDbContext>();


        }

        /// <summary>
        /// Инициализация конфигураций базы данных из appsettings.json.
        /// </summary>
        private void InitializeDbConfigurationsFromAppSett()
        {
            var RootConf = (ConfigurationRoot)configuration;
            RootConf.Reload();
            //Вызов RootConf.Reload(); обновляет конфигурацию, загруженную из источников, таких как файл appsettings.json.
            //Это полезно, если конфигурация была изменена во время выполнения приложения, и эти изменения вступили в силу без необходимости перезапуска этого приложения.
            GetSavedProviderConfigurationsFromAppSett();
            ErrorHandler.Try(
                () => GetSelectedProviderFromAppSett(),
                (Exception ex) => ErrorDialogService.ShowErrorMessage((ex)));



        }

        /// <summary>
        /// Получаем сохранненные конфигурации для каждого провайдера DB из appsettings.json и заполняем их в словарь _dbSettingsList.
        /// </summary>
        private void GetSavedProviderConfigurationsFromAppSett()
        {
            foreach (var s in _dbSettingsList)
            {
                string? cs = configuration.GetConnectionString(s.Key);
                //Невалидная строка: передаст как есть
                //Отсутствие нода (как конкретного по провайдеру, так и основного "ConnectionStrings") или в принципе файла: возвращает null
                ErrorHandler.Try(
                    () => s.Value?.Initialize(cs),
                    (Exception ex) => ErrorDialogService.ShowErrorMessage(ex, $"Configuration for \"{s.Key}\" provider can't be upload from Application Settings: \n "));


            }

        }

        /// <summary>
        /// Сервис получает выбранного провайдера из appsettings.json и проверяет его на поддержку.
        /// </summary>
        private void GetSelectedProviderFromAppSett()
        {
            string? selected = configuration["DBType"];
            //Невалидная строка: передаст как есть
            //Отсутствие нода или впринципе файла: возвращает null
            try
            {
                _selectedDbProvider = _dbSettingsList.First(p => p.Key == selected);
            }
            catch (InvalidOperationException ex)
            {
                _selectedDbProvider = _dbSettingsList.First(p => p.Key == (typeof(NpgDBSettings).GetCustomAttribute<DisplayAttribute>()?.Name ?? typeof(NpgDBSettings).Name));
                if (string.IsNullOrWhiteSpace(selected)) throw new DBConfigurationException($"There is no database type specified in appsettings.json. The {_selectedDbProvider.Key} provider was specified as selected", 100, Severity.Warning); //110 - there is no db type
                if (!_dbSettingsList.ContainsKey(selected)) throw new DBConfigurationException($"\"{selected}\" is not supported database type. The {_selectedDbProvider.Key} provider was specified as selected ", 110, Severity.Warning); //111 - specified the db type is not supproted
                throw ex;
            }

        }
        

        private int SaveDbSettingsToAppConfig(DBSettingsBase dBSettingsobj) 
        {
            string dbProvidername = dBSettingsobj.DisplayProviderName;

            var semaphore = new SemaphoreSlim(1, 1);
            semaphore.Wait();
            try
            {
                var jsonText = File.ReadAllText(Program.appsettingsPath);
                var json = JsonNode.Parse(jsonText)!;
                json["DBType"] = dbProvidername;

                if (json["ConnectionStrings"] is not JsonObject) json["ConnectionStrings"] = new JsonObject();
                json["ConnectionStrings"]![dbProvidername] = dBSettingsobj.GetConnectionString();
                jsonText = json.ToJsonString();
                File.WriteAllText(Program.appsettingsPath, jsonText);
                return 0;
            }
            finally { semaphore.Release(); }

        }

        public ApplicationDbContext GetDBContext()
        {
            if (!_selectedDbProvider.Value.IsInitialized) 
            {

                throw new DBConfigurationException($"The database settings are not configured. Please, specify it.", 130, Severity.Warning);

            };
            return new ApplicationDbContext(SelectedDbProvider.Value.GetDbContextOptionsBuilder().Options);
        }
        private ApplicationDbContext GetDBContextFromSettings(DBSettingsBase dBSettingsobj) => new ApplicationDbContext(dBSettingsobj.GetDbContextOptionsBuilder().Options);

        public int CheckCurrentConnection()
        {            
            using var dbc = GetDBContext();
            return CheckConnection(dbc);
        }

        public int CheckConnection(ApplicationDbContext dbContext)
        {

            try
            {
                dbContext.Database.OpenConnection();
                //var qwe = dbContext.Database.GetDbConnection(); получаем объект коннекции, который поможет в отслеживании статуса коннекции
                
                return 0;
            }
            catch (Exception ex)
            {

                var vrEx = new Exception("Database connection faild!", ex);
                
                throw vrEx;
            }
            finally 
            {
                dbContext.Database.CloseConnection();
            }
        }

        public int CheckConnection(DBSettingsBase dBSettingsobj) 
        {

            using var dbContext = GetDBContextFromSettings(dBSettingsobj);
            CheckConnection(dbContext);
            return 0;
        }

        public int SetNewDbConfiguration(DBSettingsBase dBSettingsobj, bool createDbIfNotExist = true) 
        {
            if (!createDbIfNotExist) CheckConnection(dBSettingsobj);
            using var dbCont = GetDBContextFromSettings(dBSettingsobj);
            dbCont.Database.Migrate();
            SaveDbSettingsToAppConfig(dBSettingsobj);
            

            InitializeDbConfigurationsFromAppSett();
            return 0;
        }
    }
}
