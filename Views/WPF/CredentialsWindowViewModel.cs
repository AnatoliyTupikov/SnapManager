using Microsoft.EntityFrameworkCore;
using Microsoft.Xaml.Behaviors;
using SnapManager.Data;
using SnapManager.Models.WPFModels;
using SnapManager.Models.WPFModels.Hierarchy;
using SnapManager.Services;
using SnapManager.Views.WPF.WPFHelpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace SnapManager.Views.WPF
{
    public class CredentialsWindowViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        private readonly HierarchyService _hierarchyService;

        private readonly DbService _DBService;
		public ObservableCollection<TreeItemWpfModel> CredHierarchy { get; set; } = new ObservableCollection<TreeItemWpfModel>();       

        private TreeItemWpfModel selectedItem;

		public TreeItemWpfModel SelectedItem 
		{
			get { return selectedItem; }
			set { OnPropertyChanged<TreeItemWpfModel>(ref selectedItem, value); }
		}		

		public TreeItemWpfModel SelectedItemTemplateClone { get; set; }

		public CredentialsWindowViewModel( HierarchyService hs, DbService db )
        {
            _hierarchyService = hs;
			_DBService = db;

        }

		private RelayCommand? windowLoaded;

		public RelayCommand WindowLoaded
		{
			get
			{
				return windowLoaded ??
					(windowLoaded = new RelayCommand(obj =>
					{
						ErrorHandler.Try(
							() =>
							{
                                
                                Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>  //откладываем выполнения, иначе будет петля
                                {
                                    
                                    using (var dbContext = _DBService.GetDBContext())
                                    {
                                        CredHierarchy?.Clear();
                                        foreach (var i in _hierarchyService.GetCredsHierarchyFromDB(dbContext))
                                        {
                                            CredHierarchy!.Add(i);
                                        }
                                    }
                                }), DispatcherPriority.Background);
                                								
                            },
                            (Exception ex) => ErrorDialogService.ShowErrorMessage(ex, "Can't get creds hierarchy: \n")

                        );
                    }));
			}

		}

		private RelayCommand? selectedItemChanged;

		public RelayCommand SelectedItemChanged
		{
			get
			{
				return selectedItemChanged ??
					(selectedItemChanged = new RelayCommand(obj =>
					{

						var income = (TreeItemWpfModel)obj;

                        if (income == null)
                        {
                            SelectedItem = null;
                            SelectedItemTemplateClone = null;
                            return;
                        }

                        if (SelectedItemTemplateClone != null && income.Id == SelectedItemTemplateClone.Id ) return;
						
						bool refresh = false;
                        if (SelectedItem != null && (SelectedItem.IsEditing || SelectedItem.IsCreating))
						{
                            string usaveChangesMessage = SelectedItem.IsCreating ? 
							"The current item is creating.\n All not saved changes will be lost.\n Are you sure you want to stop creating this item?" 
							: "The current item is editing.\n All not saved changes will be lost.\n Are you sure you want to stop editing this item?";
                            var result = MessageBox.Show(usaveChangesMessage, "Stop Editing", MessageBoxButton.YesNo, MessageBoxImage.Question);
							if (result == MessageBoxResult.No)
							{
								Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>  //откладываем выполнения, иначе будет петля
								{
									selectedItem.IsSelected = true;
								}), DispatcherPriority.Background);
								return;
							}
							else
							{   
								refresh = true;
                                if (SelectedItem.IsCreating)
								{
									this.RemoveSelectedItem();									
								}
								else undoChanges!.Execute(null);                               
							}
                        }
						this.SelectedItemTemplateClone = income?.Clone()!;
                        
                        this.SelectedItem = income!;

						if(refresh) CollectionViewSource.GetDefaultView(CredHierarchy).Refresh();


                    }));
			}

		}

	
		private RelayCommand? undoChanges;

		public RelayCommand UndoChanges
		{
			get
			{
				return undoChanges ??
					(undoChanges = new RelayCommand(obj =>
					{
						if (SelectedItem != null && SelectedItem.IsCreating) 
						{
                            var result = MessageBox.Show("Are you shure you want to cancel creating?\n All changes will be lost", "Cancel creating", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (result == MessageBoxResult.No) return;
                            this.RemoveSelectedItemAndSelectParent();
							return;
                        }
						selectedItem.CopyValueFrom(SelectedItemTemplateClone);
                        SelectedItem!.IsEditing = false;
                    },
					canExecute: obj =>  SelectedItemTemplateClone != null && SelectedItem != null && !SelectedItem.Equals(SelectedItemTemplateClone) || (SelectedItem != null && SelectedItem.IsCreating)
                    ));
			}

		}

		private RelayCommand? saveChanges;

		public RelayCommand SaveChanges
		{
			get
			{
				return saveChanges ??
					(saveChanges = new RelayCommand(obj =>
					{
						using (var db = this._DBService.GetDBContext())
						{
							SelectedItem.IsEditing = false;
							if ( SelectedItem is CredentialWpfModel cred) cred.ModificationDateUTC = DateTime.UtcNow;
                            db.Attach(SelectedItem);
							db.Entry(SelectedItem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            db.SaveChanges();
						}
                        CollectionViewSource.GetDefaultView(CredHierarchy).Refresh();
                        var tw = obj as TreeView;
						
                    },
					canExecute: obj => SelectedItem != null && !SelectedItem.Equals(SelectedItemTemplateClone) && SelectedItemTemplateClone != null || (SelectedItem != null && SelectedItem.IsCreating)
					));
			}
		}

		private RelayCommand? textChangedCommand;

		public RelayCommand TextChangedCommand
        {
			get
			{
				return textChangedCommand ??
					(textChangedCommand = new RelayCommand(obj =>
					{
						if (SelectedItem == null) return;
                        if (!SelectedItem.Equals(SelectedItemTemplateClone)) SelectedItem.IsEditing = true;
						else SelectedItem.IsEditing = false;
                        ValidateSelectedItem();
                    }));
			}

		}

		private RelayCommand? addMenu;

		public RelayCommand AddMenu
		{
			get
			{
				return addMenu ??
					(addMenu = new RelayCommand(obj => { },
					canExecute: obj => SelectedItem == null ? true : !SelectedItem.IsCreating));
			}

		}


		private RelayCommand? addFolder;

		public RelayCommand AddFolder
		{
			get
			{
				return addFolder ??
					(addFolder = new RelayCommand(obj =>
					{
                        Window? owner = obj as Window;
                        var newFolder = new FolderWithCredentialsWpfModel
						{	
							Name = "New Folder",
                            IsExpanded = true,
							IsSelected = true,
							IsCreating = true,

                        };
						this.AddItem(owner, newFolder);

                    }));
			}

		}

		private RelayCommand? addCredential;

		public RelayCommand AddCredential
		{
			get
			{
				return addCredential ??
					(addCredential = new RelayCommand(obj =>
					{
                        Window? owner = obj as Window;
                        var newCred = new CredentialWpfModel
                        {
                            Name = "New Credential",                            
                            IsSelected = true,
                            IsCreating = true,
							CreationDateUTC = DateTime.UtcNow,
							ModificationDateUTC = DateTime.UtcNow
                        };
                        this.AddItem(owner, newCred);

                    }));
			}

		}


		private void AddItem(Window? owner, TreeItemWpfModel item)
		{
            if (selectedItem == null)
            {
                CredHierarchy.Add(item);
                return;
            }
            else
            {

                if (SelectedItem.Parent == null && SelectedItem is FolderWpfModel)
                {
                    
                    var res = NestedOrRoot.ShowNestedOrRoot(owner);
                    if (res == NestedOrRootType.Cancel) return;
                    if (res == NestedOrRootType.Root)
                    {
                        CredHierarchy.Add(item);
                        return;
                    }
                }
                _hierarchyService.AddChildToParentWithSearchUp(SelectedItem, item);
                if (item.Parent == null) CredHierarchy.Add(item);
                SelectedItem.IsExpanded = true;
                CollectionViewSource.GetDefaultView(CredHierarchy).Refresh();
            }
        }



		private RelayCommand? removeItem;        

        public RelayCommand RemoveItem
		{
			get
			{
				return removeItem ??
					(removeItem = new RelayCommand(obj =>
					{						
                        var result = MessageBox.Show("Are you shure you want to remoe the item?", "Remove the item", MessageBoxButton.YesNo, MessageBoxImage.Question);
						if (result == MessageBoxResult.No) return;
						this.RemoveSelectedItemAndSelectParent();

                    },
					canExecute: obj => SelectedItem != null && !SelectedItem.IsCreating
                    ));
			}

		}


        private void RemoveSelectedItemAndSelectParent()
		{
            if (SelectedItem.IsEditing) UndoChanges.Execute(null);
            var temp = SelectedItem.Parent;
			this.RemoveSelectedItem();
            if (temp != null) temp.IsSelected = true;
            SelectedItemChanged.Execute(temp ?? CredHierarchy.FirstOrDefault() ?? null);
            CollectionViewSource.GetDefaultView(CredHierarchy).Refresh();
        }

		private void RemoveSelectedItem()
		{
            if (!selectedItem.IsCreating)
            {
                using (var db = this._DBService.GetDBContext())
                {
                    _hierarchyService.RemoveItemFromHierarchy(SelectedItem, db);
                }
            }
            else
            {
                if (selectedItem.Parent != null) _hierarchyService.RemoveChildFromParent(SelectedItem);
            }
            CredHierarchy.Remove(SelectedItem);
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
			if (String.IsNullOrEmpty(SelectedItem?.Name)) errors.Add("Name cannot be empty.");
			SetErrors(nameof(SelectedItem.Name), errors);

        }
    }
}
