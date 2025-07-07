using SnapManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SnapManager.Models.Hierarchy
{
    [Table("TreeItems")]
    public class TreeItemBase : IEquatable<TreeItemBase>, ICopy<TreeItemBase>, INotifyPropertyChanged
    {        
    
        public int Id { get; set; }

        [NotMapped]
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
                }
            }
        }

        //public string Name { get; set; }

        [NotMapped]
        private bool isExpanded = false;
        [NotMapped]
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
        //[NotMapped]
        //public bool IsSelected { get; set; } = false;
        [NotMapped]
        private bool isSelected = false;
        [NotMapped]
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

        [NotMapped]
        private bool isEditing;

        
        public event PropertyChangedEventHandler? PropertyChanged;

        [NotMapped]
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

        [NotMapped]
        public bool IsCreating { get; set; } = false;
        


        public List<TreeItemBase>? Parents { get; set; }
        public List<TreeItemBase>? Children { get; set; }



        public TreeItemBase Clone()
        {
           return this.MemberwiseClone() as TreeItemBase ?? throw new InvalidOperationException("Failed to clone TreeItemBase.");
        }

        public virtual void CopyValueFrom(TreeItemBase source)
        {
            this.Name = source.Name;            
            this.IsEditing = source.IsEditing;
            this.IsCreating = source.IsCreating;
            this.Parents = source.Parents;
            this.Children = source.Children;
        }

        public virtual bool Equals(TreeItemBase? other)
        {
            if (other == null) return false;
            return Name == other.Name;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
    

}
