using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Npgsql;
using SnapManager.Exceptions;
using SnapManager.Models;
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
            [typeof(NpgDBSettings).GetCustomAttribute<DisplayAttribute>().Name] = new NpgDBSettings()
        };

        public Dictionary<string, DBSettingsBase> DbSettingsList => new(_dbSettingsList);


        private KeyValuePair<string, DBSettingsBase> _selectedDbProvider;

        public KeyValuePair<string, DBSettingsBase> SelectedDbProvider => new(_selectedDbProvider.Key, _selectedDbProvider.Value);


        private readonly IConfiguration configuration;
        //public AlloWedDBTypes SelectedDbProvider { get => GetSelectedDbProviderFromConfig(); } 

        public DbService(IConfiguration configuration)
        {
            this.configuration = configuration;// получение конфигураций из файла appsettings.json
            InitializeDbConfigurationsFromAppSett();
            //dboption = new DbContextOptions<ApplicationDbContext>();


        }

        private void InitializeDbConfigurationsFromAppSett()
        {
            var RootConf = (ConfigurationRoot)configuration;
            RootConf.Reload();
            GetSavedProviderConfigurationsFromAppSett();
            GetSelectedProviderFromAppSett();

        }

        private void GetSavedProviderConfigurationsFromAppSett()
        {
            foreach (var s in _dbSettingsList)
            {
                s.Value.FillUp(GetConnectionStringFromAppSett(s.Key));
            }

        }

        private void GetSelectedProviderFromAppSett()
        {
            string selected = configuration["DBType"];
            if (selected is null) _selectedDbProvider = _dbSettingsList.First(p => p.Key == typeof(NpgDBSettings).GetCustomAttribute<DisplayAttribute>().Name);
            IsSupportedDbProvider(selected);
            _selectedDbProvider = _dbSettingsList.First(p => p.Key == selected);


        }

        private bool IsSupportedDbProvider(string? dbProvider)
        {
            if (string.IsNullOrWhiteSpace(dbProvider)) throw new DBConfigurationException("There is no database type specified in appsettings.json.", 110);
            if (!_dbSettingsList.ContainsKey(dbProvider)) throw new DBConfigurationException($"\"{dbProvider}\" is not supported database type.", 111);
            else return true;
        }

        private string? GetConnectionStringFromAppSett(string dbProvider) => GetConnectionStringFromAppSett(out var catchedEx, dbProvider);

        private string? GetConnectionStringFromAppSett(out DBConfigurationException? catchedEx, string dbProvider)
        {
            try
            {
                string? result = configuration.GetConnectionString(dbProvider ?? throw new DBConfigurationException($"Connection string \"{dbProvider}\" not specified in appsetting.", 120));
                catchedEx = null;
                return result;
            }
            catch (DBConfigurationException ex) when (new[] { 120 }.Contains(ex.ErrorCode))
            {
                catchedEx = ex;
                return null;
            }

        }

        private int SaveDbSettingsToAppConfig(DBSettingsBase dBSettingsobj) 
        {
            string dbProvidername = dBSettingsobj.GetType().GetCustomAttribute<DisplayAttribute>().Name;

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

        public ApplicationDbContext GetDBContext() => new ApplicationDbContext(SelectedDbProvider.Value.GetDbContextOptionsBuilder().Options);
        public ApplicationDbContext GetDBContext(DBSettingsBase dBSettingsobj) => new ApplicationDbContext(dBSettingsobj.GetDbContextOptionsBuilder().Options);

        public int CheckConnection(ApplicationDbContext dbContext)
        {
            dbContext.Database.OpenConnection();
            dbContext.Database.CloseConnection();
            return 0;
        }

        public int CheckConnection(DBSettingsBase dBSettingsobj) 
        {

            using var dbContext = GetDBContext(dBSettingsobj);
            CheckConnection(dbContext);
            return 0;
        }

        public int SetNewDbConfiguration(DBSettingsBase dBSettingsobj, bool createDbIfNotExist = true) 
        {
            if (!createDbIfNotExist) CheckConnection(dBSettingsobj);
            using var dbCont = GetDBContext(dBSettingsobj);
            dbCont.Database.Migrate();
            SaveDbSettingsToAppConfig(dBSettingsobj);
            

            InitializeDbConfigurationsFromAppSett();
            return 0;
        }
        //============================================================================================================================================================================

        //   

        //    


        //    private string GetNpgConnectionString() => configuration.GetConnectionString(Program.AllowedDBTypesCollections[AlloWedDBTypes.Npg] ?? throw new DBConfigurationException("Connection string \"PostgreSQL\" not found or not specified in appsetting.", 120));
        //    private string GetMSConnectionString() => configuration.GetConnectionString(Program.AllowedDBTypesCollections[AlloWedDBTypes.MS] ?? throw new DBConfigurationException("Connection string \"MSSQLS\" not found or not specified in appsetting.", 120));

        //    private AlloWedDBTypes GetSelectedDbProviderFromConfig() => Program.AllowedDBTypesCollections.FirstOrDefault(p => p.Value == configuration["DBType"]).Key;

        //    public NpgDBSettings GetNpgSetting() 
        //    {

        //        var stringBuilder = new NpgsqlConnectionStringBuilder(GetNpgConnectionString());
        //        NpgDBSettings settings = new ();
        //        settings.Hostname = stringBuilder.Host;
        //        settings.Port = stringBuilder.Port;
        //        settings.Database = stringBuilder.Database;
        //        settings.Username = stringBuilder.Username;
        //        settings.Password = stringBuilder.Password;

        //        return settings;

        //    }

        //    public NpgDBSettings GetMSSetting() 
        //    {
        //        var stringBuilder = new NpgsqlConnectionStringBuilder(GetNpgConnectionString());
        //        NpgDBSettings settings = new ();
        //        settings.Hostname = stringBuilder.Host;
        //        settings.Port = stringBuilder.Port;
        //        settings.Database = stringBuilder.Database;
        //        settings.Username = stringBuilder.Username;
        //        settings.Password = stringBuilder.Password;

        //        return settings;

        //    }

        //    public async Task SetNpgSetting(NpgsqlConnectionStringBuilder strBuilder) 
        //    {

        //        var semaphore = new SemaphoreSlim(1,1);
        //        await semaphore.WaitAsync();
        //        try
        //        {
        //            var jsonText = await File.ReadAllTextAsync(Program.appsettingsPath);
        //            var json = JsonNode.Parse(jsonText)!;
        //            json["DBType"] = Program.AllowedDBTypesCollections[AlloWedDBTypes.Npg];

        //            if (json["ConnectionStrings"] is not JsonObject) json["ConnectionStrings"] = new JsonObject();
        //            json["ConnectionStrings"]!["PostgreSQL"] = strBuilder.ConnectionString;

        //        }
        //        finally { semaphore.Release(); }
        //    }

        //    public async Task SetNpgSetting(NpgDBSettings settings) 
        //    {
        //        var stringBuilder = new NpgsqlConnectionStringBuilder();
        //        stringBuilder.Host = settings.Hostname;
        //        stringBuilder.Port = settings.Port;
        //        stringBuilder.Database = settings.Database;
        //        stringBuilder.Username = settings.Username;
        //        stringBuilder.Password = settings.Password;

        //        await SetNpgSetting(stringBuilder);

        //    }
    }
}
