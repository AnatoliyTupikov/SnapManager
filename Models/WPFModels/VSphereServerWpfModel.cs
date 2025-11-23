using SnapManager.Models.DomainModels;
using SnapManager.Models.WPFModels.CredentialHierarchy;
using SnapManager.Views.WPF.WPFHelpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapManager.Models.WPFModels
{
    public class VSphereServerWpfModel : BaseViewModel, INotifyDataErrorInfo
    {
        public int Id { get { return DModel.Id; } }
        private string name;

        public string Name
        {
            get { return name; }
            set { OnPropertyChanged<string>(ref name, value); }
        }

        private string host;

        public string Host
        {
            get { return host; }
            set { OnPropertyChanged<string>(ref host, value); }
        }

        private int port;

        public int Port
        {
            get { return port; }
            set { OnPropertyChanged<int>(ref port, value); }
        }

        private CredentialWpfModel? credential;

        public CredentialWpfModel? Credential
        {
            get { return credential; }
            set { OnPropertyChanged<CredentialWpfModel?>(ref credential, value); }
        }

        public DateTime CreationDateUTC { get; set; }

        private string? description;

        public string? Description
        {
            get { return description; }
            set { OnPropertyChanged<string>(ref description, value); }
        }

        private bool isCreating = false;

        public bool IsCreating
        {
            get { return isCreating; }
            set { OnPropertyChanged<bool>(ref isCreating, value); }
        }

        private bool isEditing = false;

        public bool IsEditing
        {
            get { return isEditing = false; }
            set { OnPropertyChanged<bool>(ref isEditing, value); }
        }

        private bool isSelected = false;

        public bool IsSelected
        {
            get { return isSelected; }
            set { OnPropertyChanged<bool>(ref isSelected, value); }
        }


        public VSphereServerDModel DModel { get; init; }

        public VSphereServerWpfModel()
        {
            DModel = new VSphereServerDModel();
            Name = string.Empty;
            Host = string.Empty;
            Port = 443; // Default port for vSphere
            Credential = null;
            Description = null;
            CreationDateUTC = DateTime.UtcNow;
        }




        //---------------------------------------------------------Validation Logic---------------------------------------------------------//
        public bool HasErrors => throw new NotImplementedException();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors(string? propertyName)
        {
            throw new NotImplementedException();
        }
    }
}
