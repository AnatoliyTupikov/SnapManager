using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Npgsql;

namespace SnapManager.Data
{
    internal class DBConnector
    {
        private static NpgsqlConnection instance;

        public static NpgsqlConnection Instance { get => instance; }

        private DBConnector() { }

        public static NpgsqlConnection CreateDBConnection(string connectionString, bool exeptionIsExisting = true)
        {
            NpgsqlConnectionStringBuilder connectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString);
            string? passTemp = connectionStringBuilder.Password;
            connectionStringBuilder.Password = null;
            if (instance?.ConnectionString == connectionStringBuilder.ConnectionString && exeptionIsExisting) throw new ApplicationException("The Data Base connection already exist!");
            connectionStringBuilder.Password = passTemp;

            var TempDBConnection = new NpgsqlConnection(connectionStringBuilder.ConnectionString);

            TempDBConnection.Open();
            if (instance != null) instance.Close();
            instance = TempDBConnection;

            return instance;
        }

        public static NpgsqlConnection CreateDBConnectionFromAppConfig(string? connectionString, bool exeptionIsExisting = true)
        {
            if (connectionString == null) throw new ArgumentNullException("Impossible to create Data Base connestion. App.config doesn't contain Data Base configurations");
            return CreateDBConnectionFromAppConfig(connectionString, exeptionIsExisting);
        }

        public static void SaveDBConnectionToAppConfig()
        {
            if (instance == null) throw new ApplicationException("Cannot save DBConnection to app config. Nothing to save!");
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.ConnectionStrings.ConnectionStrings.Remove("DBConnect.MainConnectionString");
            config.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("DBConnect.MainConnectionString", instance.ConnectionString, "Npgsql"));
            config.Save();
            ConfigurationManager.RefreshSection("connectionStrings");
        }

        public static string? LoadConnectionStringFromAppConfig()
        {
            ConnectionStringSettings LoadedconnectionString = ConfigurationManager.ConnectionStrings["DBConnect.MainConnectionString"];
            if (LoadedconnectionString == null) return null;
            return LoadedconnectionString.ConnectionString;
        }
    }
}
