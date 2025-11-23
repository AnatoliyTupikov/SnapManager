using SnapManager.Models.DomainModels.CredentialsHierarchy;
using SnapManager.Views.WPF.WPFHelpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.Primitives;

namespace SnapManager.Models.WPFModels.CredentialHierarchy
{
    public class TreeItemWpfModel : BaseViewModel, INotifyDataErrorInfo
    {
        
        public TreeItemDModel DModel { get; init; }

        public int Id { get => DModel.Id; }


        private string name;

        [Required(ErrorMessage = "Name can not be emty.")]
        public string Name
        {
            get { return name; }
            set { OnPropertyChanged(ref name, value); ValidateProperty(value, nameof(Name)); }
        }

        public TreeItemWpfModel? Parent { get; set; }
        public List<TreeItemWpfModel>? Children { get; set; }

        public DateTime CreationDateUTC { get; set; }

        public DateTime CreationDateLocal
        {
            get => CreationDateUTC.ToLocalTime();            
        }

        public DateTime ModificationDateUTC { get; set; }

        public DateTime ModificationDateLocal
        {
            get => ModificationDateUTC.ToLocalTime();            
        }

        private bool isExpanded = false;

        public bool IsExpanded
        {
            get { return isExpanded; }
            set { OnPropertyChanged(ref isExpanded, value); }
        }

        private bool isSelected = false;

        public bool IsSelected
        {
            get { return isSelected; }
            set { OnPropertyChanged(ref isSelected, value); }
        }

        private bool isEditing = false;

        public bool IsEditing
        {
            get { return isEditing; }
            set { OnPropertyChanged(ref isEditing, value); }
        }

        private bool isCreating = false;

        public bool IsCreating
        {
            get { return isCreating; }
            set { OnPropertyChanged(ref isCreating, value); }
        }


        
        

        public TreeItemWpfModel()
        {
            Name = string.Empty;
            DModel = new TreeItemDModel();
            CreationDateUTC = DateTime.UtcNow;
            ModificationDateUTC = DateTime.UtcNow;
        }

        

        public void UpdateHirerarchyToDModel()
        {            
            DModel.Parent = Parent?.DModel;
            DModel.Children = Children?.Select(c => c.DModel).ToList() ?? new List<TreeItemDModel>();
        }

        public void CreateDModel() 
        {
            UpdateHirerarchyToDModel();
            UpdateValuesToDModel();
            DModel.CreationDateUTC = CreationDateUTC;
        }
        public virtual void UpdateValuesToDModel()
        {
            DModel.Name = Name;
            DModel.ModificationDateUTC = ModificationDateUTC;
        }

        public virtual void PullValuesFromDModel()
        {
            Name = DModel.Name;
            CreationDateUTC = DModel.CreationDateUTC;
            ModificationDateUTC = DModel.ModificationDateUTC;            
        }

        public virtual bool IsEqualToDModel()
        {
            if (DModel == null) return false;
            return Name == DModel.Name &&
                   CreationDateUTC == DModel.CreationDateUTC &&
                   ModificationDateUTC == DModel.ModificationDateUTC;
        }


        //--------------------------------------------------Validadation logic--------------------------------------------------//


        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly Dictionary<string, List<string>> _errors = new();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public bool HasErrors => _errors.Any();

        public IEnumerable GetErrors(string propertyName)
        {
            return _errors.ContainsKey(propertyName) ? _errors[propertyName] : null;
        }


        protected void ValidateProperty(object value, string propertyName)
        {
            ClearErrors(propertyName);

            var context = new ValidationContext(this)
            {
                MemberName = propertyName
            };

            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateProperty(value, context, results);

            if (!isValid)
            {
                foreach (var validationResult in results)
                {
                    AddError(propertyName, validationResult.ErrorMessage);
                }
            }
        }

        public void ValidateAllProperties()
        {
            // Очистим все старые ошибки
            var propertyNames = _errors.Keys.ToList();
            foreach (var propertyName in propertyNames)
                ClearErrors(propertyName);

            var context = new ValidationContext(this);
            var results = new List<ValidationResult>();

            // Проверка всех свойств по атрибутам
            Validator.TryValidateObject(this, context, results, validateAllProperties: true);

            foreach (var validationResult in results)
            {
                foreach (var memberName in validationResult.MemberNames)
                {
                    AddError(memberName, validationResult.ErrorMessage);
                }
            }
        }


        protected void AddError(string propertyName, string error)
        {
            if (!_errors.ContainsKey(propertyName))
                _errors[propertyName] = new List<string>();

            if (!_errors[propertyName].Contains(error))
            {
                _errors[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        protected void ClearErrors(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
            {
                _errors.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }
    
        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        

    }
    

}
