using SnapManager.Data;
using SnapManager.Models;
using SnapManager.Views.WPF.WPFHelpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SnapManager.Views.WPF
{
    public class DBSettingsWindowDataGridRow : BaseViewModel, IDataErrorInfo, INotifyDataErrorInfo
    {
        public Type TypeOfCastedObj { get; private set; }

        private List<Attribute>? _attributes;

        public List<Attribute>? Attributes
        {
            get { return _attributes; }
            private set { _attributes = value; }
        }
        
        private dynamic? _column1;

        public dynamic? Column1
        {
            get { return _column1; }
            set { OnPropertyChanged<dynamic>(ref _column1, value); }
        }


        

        //private ColumClass _column1;

        //public ColumClass Test1
        //{
        //    get { return _column1; }
        //    set { OnPropertyChanged(ref _column1, value); }
        //}

        private dynamic? _column2;

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public dynamic? Column2
        {
            get { return _column2; }
            set 
            {
                
                OnPropertyChanged<dynamic>(ref _column2, value);
                
            }
        }

        //private ColumClass _column2;

        //public ColumClass Test2
        //{
        //    get { return _column2; }
        //    set { OnPropertyChanged(ref _column2, value); }
        //}

        private bool _offValidation = false;

        private Dictionary<string, List<string>> _errors = new();
        public string Error => null;

        public bool HasErrors => _errors.Any();

        public IEnumerable GetErrors(string? propertyName)
        {
            if (propertyName != null && _errors.ContainsKey(propertyName))
                return _errors[propertyName];
            return null;
        }

        public string this[string columnName] 
        {
            get
            {
                _errors.Remove(columnName);
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(columnName));

                if (_offValidation) 
                {
                    this._offValidation = false;
                    return null;
                }
                    

                if (columnName == nameof(Column2))
                {
                    if (_attributes.Exists(p => p.GetType() == typeof(RequiredAttribute)) && (Column2 is null || string.IsNullOrEmpty(Column2?.ToString()))) 
                    {
                        const string errorMessage = "Value cannot be null";
                        _errors.Add(columnName, new List<string> { errorMessage });
                        return errorMessage; 
                    }
                }
                return null;
            }
        }

        public static ObservableCollection<DBSettingsWindowDataGridRow> GetDBConfigurationCollection(object decomposedObj, bool offFirstBindingValidation = false) 
        {
            ObservableCollection<DBSettingsWindowDataGridRow> returnedCollection = new();
            
            Type type = decomposedObj.GetType();
            var properties = type.GetProperties();
            var propquery = properties.Where(p => p.GetCustomAttributes(typeof(DisplayAttribute), true).Length > 0)?
                                      .OrderBy(p => p.GetCustomAttribute<DisplayAttribute>(true)?.Order);

            foreach (var property in propquery) 
            {
                DBSettingsWindowDataGridRow gridRow = new();
                gridRow._offValidation = offFirstBindingValidation; // отключение валидации при первой привязке, чтобы не вызывать ошибку валидации сразу при загрузке грида
                var DispAttr = property.GetCustomAttribute<DisplayAttribute>();
                gridRow.Column1 = DispAttr.Name;
                gridRow.Column2 = property.GetValue(decomposedObj) ?? "";
                gridRow.Attributes = property.GetCustomAttributes(true).Cast<Attribute>().ToList();
                gridRow.TypeOfCastedObj = type;
                returnedCollection.Add(gridRow);
            }
            return returnedCollection;
        }

        public static KeyValuePair<string, ObservableCollection<DBSettingsWindowDataGridRow>> GetDBConfigurationRowCollection(KeyValuePair<string, DBSettingsBase> provider, bool offFirstBindingValidation = false) => new KeyValuePair<string, ObservableCollection<DBSettingsWindowDataGridRow>>(provider.Key, DBSettingsWindowDataGridRow.GetDBConfigurationCollection(provider.Value, offFirstBindingValidation));

        public static DBSettingsBase ReturnDBConfiguration (ObservableCollection<DBSettingsWindowDataGridRow> DBConfigurationRowCollection) 
        {
            Type type = DBConfigurationRowCollection[0].TypeOfCastedObj;
            object? objinstance = Activator.CreateInstance(type);
            DBSettingsBase instance = (DBSettingsBase)objinstance;

            
            var properties = type.GetProperties();
            var propquery = properties.Where(p => p.GetCustomAttributes(typeof(DisplayAttribute), true).Length > 0)?
                                      .OrderBy(p => p.GetCustomAttribute<DisplayAttribute>(true)?.Order);
            foreach (var row in DBConfigurationRowCollection)
            {
                var property = propquery.First(p => p.GetCustomAttribute< DisplayAttribute>().Name == row.Column1);
                property.SetValue(instance, row.Column2);
            }
            return instance;


        }

       
        
    }
}
