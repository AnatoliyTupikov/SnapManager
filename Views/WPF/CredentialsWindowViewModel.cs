using SnapManager.Data;
using SnapManager.Models;
using SnapManager.Models.Hierarchy;
using SnapManager.Services;
using SnapManager.Views.WPF.WPFHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class CredentialsWindowViewModel : BaseViewModel
    {
		private const string _usaveChangesMessage = "The current item is editing.\n All not saved changes will be lost.\n Are you sure you want to stop editing this item?";
        private readonly HierarchyService _hierarchyService;

        private readonly DbService _DBService;

		public ObservableCollection<TreeItemBase> CredHierarchy { get; set; } = new ObservableCollection<TreeItemBase>();
		
		

		private TreeItemBase selectedItem;

		public TreeItemBase SelectedItem 
		{
			get { return selectedItem; }
			set { OnPropertyChanged<TreeItemBase>(ref selectedItem, value); }
		}
		private bool isEditing;

		public bool IsEditing
		{
			get { return isEditing; }
			set { OnPropertyChanged<bool>(ref isEditing, value); }
		}

		public TreeItemBase SelectedItemTemplateClone { get; set; }

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
								using (var dbContext = _DBService.GetDBContext())
								{
									_hierarchyService.SaveHierarchyState(CredHierarchy, dbContext);
                                    CredHierarchy?.Clear();
                                    foreach ( var i in _hierarchyService.GetCredsHierarchy(dbContext))
									 {
										CredHierarchy.Add(i);
									 }
                                    

                                }
								
                            },
                            (Exception ex) => ErrorDialogService.ShowErrorMessage(ex, "Can't get creds hierarchy: \n")

                        );
                        //SelectedItem = CredHierarchy.First();

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
						
						var income = (TreeItemBase)obj;

						if (income.IsEditing) return;

						if (SelectedItem != null && SelectedItem.IsEditing)
						{
                            var result = MessageBox.Show(_usaveChangesMessage, "Stop Editing", MessageBoxButton.YesNo, MessageBoxImage.Question);
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
                                undoChanges!.Execute(null);                               
							}
                        }
						this.SelectedItemTemplateClone = income?.Clone()!;
                        
                        this.SelectedItem = income!;
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
						selectedItem.CopyValueFrom(SelectedItemTemplateClone);
                        //var clone = SelectedItemTemplateClone.Clone();
                        //this.SelectedItem = clone;
                        //CredHierarchy.Remove(SelectedItem);
                        //CredHierarchy.Add(clone);
                        SelectedItem.IsEditing = false;
                    },
					canExecute: obj =>  SelectedItemTemplateClone != null && !SelectedItem.Equals(SelectedItemTemplateClone)
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
							if ( SelectedItem is Credential cred) cred.ModificationDateUTC = DateTime.UtcNow;
                            db.Attach(SelectedItem);
							db.Entry(SelectedItem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            db.SaveChanges();
						}
                        CollectionViewSource.GetDefaultView(CredHierarchy).Refresh();
                        var tw = obj as TreeView;
						
                    },
					canExecute: obj => SelectedItem != null && !SelectedItem.Equals(SelectedItemTemplateClone) && SelectedItemTemplateClone != null
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
						if (!SelectedItem.Equals(SelectedItemTemplateClone)) SelectedItem.IsEditing = true;
						else SelectedItem.IsEditing = false;
                    }));
			}

		}

	}
}
