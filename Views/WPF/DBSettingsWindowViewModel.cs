using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Npgsql;
using SnapManager.Data;
using SnapManager.Models;
using SnapManager.Services;
using SnapManager.Views.WPF.WPFHelpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SnapManager.Views.WPF
{
    public class DBSettingsWindowViewModel : BaseViewModel
    {
        private bool _valid;

        public bool IsValid
        {
            get { return _valid; }
            set { OnPropertyChanged<bool>(ref _valid, value); }
        }

        public ObservableCollection<KeyValuePair<string, ObservableCollection<DBSettingsWindowDataGridRow>>> ProvidersList { get; set; } = new ();

        private readonly DbService _dbService;

        private KeyValuePair<string, ObservableCollection<DBSettingsWindowDataGridRow>> selectedDbProvider;               

        public KeyValuePair<string, ObservableCollection<DBSettingsWindowDataGridRow>> SelectedDbProvider
        {
            get { return selectedDbProvider; }
            set { OnPropertyChanged <KeyValuePair<string, ObservableCollection<DBSettingsWindowDataGridRow>>>(ref selectedDbProvider, value); }
        }



        public DBSettingsWindowViewModel(DbService dbService)
        {
            _dbService = dbService;
            Initialize();

        }
        private void Initialize() 
        {
            foreach (var provider in _dbService.DbSettingsList)
            {
                ProvidersList.Add(DBSettingsWindowDataGridRow.GetDBConfigurationRowCollection(provider, true));
            }
            selectedDbProvider = ProvidersList.First(p => p.Key == _dbService.SelectedDbProvider.Key);            
        }

        private string _log;

        public string Log
        {
            get { return _log; }
            set { OnPropertyChanged<string>(ref _log, value); }
        }

        private bool _createDbIfNotExist = true;

        public bool CreateDbIfNotExist
        {
            get { return _createDbIfNotExist; }
            set { OnPropertyChanged<bool>(ref _createDbIfNotExist, value); }
        }


        private void Logging(string message)
        {
            Log += ($"[{DateTime.Now:HH:mm:ss}] {message}\n");           
        }

        

        private RelayCommand? makeConnection;
        public RelayCommand MakeConnection
        {
            get
            {
                return makeConnection ??
                    (makeConnection = new RelayCommand(obj =>
                    {

                        
                        Log = "";
                        foreach (var c in SelectedDbProvider.Value)
                        {

                            
                            string t = $"Test2: {Convert.ToString(c.Column2)}";
                            Logging(t);

                        }
                        Logging(CreateDbIfNotExist.ToString());


                        var settingobj = DBSettingsWindowDataGridRow.ReturnDBConfiguration(selectedDbProvider.Value);
                        try 
                        {
                           _dbService.SetNewDbConfiguration(settingobj, CreateDbIfNotExist);                           
                            MessageBox.Show("Connected!");
                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message); }
                        
                    },
                    canExecute: obj => !NativeMethods.HasValidationErrors((Window)obj)
                    ));


            }

        }

        private RelayCommand cancel;

        public RelayCommand Cancel
        {
            get 
            { return cancel ??
                    (cancel = new RelayCommand(obj =>
                    {
                        var window = (Window)obj;
                        window.Close();

                    }));
            }
        }


        //private RelayCommand? _dropValidation;

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        

    }
}
