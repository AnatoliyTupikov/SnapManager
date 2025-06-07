using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Npgsql;
using SnapManager.Data;
using SnapManager.Models;
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



        public DBSettingsWindowViewModel(DbService dbService, ErrorHandler errorHandler) : base(errorHandler)
        {
            _dbService = dbService;
            ErrorHandler.TryWindowed(() => Initialize(), instance: ErrorHandler);
            
        }
        private void Initialize() 
        {
            foreach (var provider in _dbService.DbSettingsList)
            {

                ProvidersList.Add(DBSettingsWindowDataGridRow.GetDBConfigurationRowCollection(provider, true));
            }
            selectedDbProvider = ProvidersList.First(p => p.Key == _dbService.SelectedDbProvider.Key);
            throw new NotImplementedException("This method is not implemented yet. Please implement the logic to initialize the selected database provider.");
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

        public IEnumerable GetErrors(string? propertyName)
        {
            throw new NotImplementedException();
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
                    obj => !NativeMethods.HasValidationErrors((Window)obj)
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
                        ErrorHandler.TryWindowed(()=> Test(), "Db setting error:", instance: ErrorHandler);

                    }));
            }
        }

        private void Test() => throw new NotImplementedException("Cancel button");

        //private RelayCommand? _dropValidation;

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        

    }
}
