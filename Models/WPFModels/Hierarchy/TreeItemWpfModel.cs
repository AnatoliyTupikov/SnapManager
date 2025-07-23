using SnapManager.Models.DomainModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.Primitives;

namespace SnapManager.Models.WPFModels.Hierarchy
{
    [Table("TreeItems")]
    public class TreeItemWpfModel : IEquatable<TreeItemWpfModel>, ICopy<TreeItemWpfModel>, INotifyPropertyChanged, INotifyDataErrorInfo
    {
        
        public TreeItemBaseDModel DModel { get; init; }

        public int Id { get => DModel.Id; }

        private string name;

        public string Name
        {
            get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(name));
                    ValidateSelectedItem();
                }
            }
        }
        public TreeItemWpfModel? Parent { get; set; }
        public List<TreeItemWpfModel>? Children { get; set; }


        private bool isExpanded = false;
        public bool IsExpanded
        {
            get => isExpanded;
            set
            {
                if (isExpanded != value)
                {
                    isExpanded = value;
                    OnPropertyChanged(nameof(isExpanded)); 
                }
            }
        }

        private bool isSelected = false;
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged(nameof(isSelected));
                }
            }
        }


        private bool isEditing = false;      
        
        public bool IsEditing
        {
            get => isEditing;
            set
            {
                if (isEditing != value)
                {
                    isEditing = value;
                    OnPropertyChanged(nameof(IsEditing));  // уведомляем UI
                }
            }
        }

        private bool isCreating = false;

        public bool IsCreating
        {
            get => isCreating;
            set
            {
                if (isCreating != value)
                {
                    isCreating = value;
                    OnPropertyChanged(nameof(IsCreating));  // уведомляем UI
                }
            }
        } 
        

        public TreeItemWpfModel()
        {
            DModel = new TreeItemBaseDModel();
        }



        public TreeItemWpfModel Clone()
        {
           return MemberwiseClone() as TreeItemWpfModel ?? throw new InvalidOperationException("Failed to clone TreeItemBase.");
        }

        public virtual void CopyValueFrom(TreeItemWpfModel source)
        {
            Name = source.Name;            
            IsEditing = source.IsEditing;
            IsCreating = source.IsCreating;
            Parent = source.Parent;
            Children = source.Children;
        }

        public virtual bool Equals(TreeItemWpfModel? other)
        {
            if (other == null) return false;
            return Name == other.Name;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly Dictionary<string, List<string>> _errors = new();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public bool HasErrors => _errors.Any();


        public IEnumerable GetErrors(string? propertyName)
        {
            return _errors.TryGetValue(propertyName, out var errors) ? errors : null;
        }
        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private void SetErrors(string propertyName, List<string> errors)
        {
            bool changed = !_errors.ContainsKey(propertyName) || !_errors[propertyName].SequenceEqual(errors);

            if (errors.Any())
                _errors[propertyName] = errors;
            else
                _errors.Remove(propertyName);

            if (changed)
                OnErrorsChanged(propertyName);
        }

        private void ValidateSelectedItem()
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(Name) ) errors.Add("Name cannot be empty.");
            SetErrors(nameof(Name), errors);

        }

    }
    

}
