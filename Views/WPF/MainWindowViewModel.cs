using Microsoft.EntityFrameworkCore;
using SnapManager.Data;
using SnapManager.Models;
using SnapManager.Models.DomainModels;
using SnapManager.Models.WPFModels;
using SnapManager.Models.WPFModels.Hierarchy;
using SnapManager.Services;
using SnapManager.Views.WPF.WPFHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SnapManager.Views.WPF.WPFViewModels
{
    public class MainWindowViewModel : BaseViewModel
	{

        private readonly DbService _dbService;
        private readonly WpfHierarchyService _hierarchyService;

        private Point windowCenterPosition;

        public Point WindowCenterPosition
        {
            get { return windowCenterPosition; }
            set { OnPropertyChanged<Point>(ref windowCenterPosition, value); }
        }
        
        public MainWindowViewModel(DbService dbService, WpfHierarchyService hService) 
        {
            _dbService = dbService;
            _hierarchyService = hService;

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
                                _dbService.CheckCurrentConnection();
                                _dbService.GetDBContext().Database.Migrate();

                                //var testFolder = new FolderWithCredentialsDModel() { Children = new List<TreeItemDModel>(), CreationDateUTC = DateTime.UtcNow, ModificationDateUTC = DateTime.UtcNow, Name = "Test Folder", Description = "Check!" };
                                ////var testCredential1 = new CredentialDModel() { Children = null, CreationDateUTC = DateTime.UtcNow, ModificationDateUTC = DateTime.UtcNow, Name = "Test Credential 1", Description = "Check!", Username = "testuser", Password = "testpassword" };
                                ////var testCredential2 = new CredentialDModel() { Children = null, CreationDateUTC = DateTime.UtcNow, ModificationDateUTC = DateTime.UtcNow, Name = "Test Credential 2", Description = "Check!", Username = "testuser", Password = "testpassword" };
                                
                                //var testFolder2 = new FolderDModel() { Children = new List<TreeItemDModel>(), CreationDateUTC = DateTime.UtcNow, ModificationDateUTC = DateTime.UtcNow, Name = "Test Folder2", Description = "Check!" };
                                //var testCredential21 = new CredentialDModel() { Children = null, CreationDateUTC = DateTime.UtcNow, ModificationDateUTC = DateTime.UtcNow, Name = "Test Credential 21", Description = "Check!", Username = "testuser", Password = "testpassword" };
                                //var testCredential22 = new CredentialDModel() { Children = null, CreationDateUTC = DateTime.UtcNow, ModificationDateUTC = DateTime.UtcNow, Name = "Test Credential 22", Description = "Check!", Username = "testuser", Password = "testpassword" };
                                //testFolder2.Children.Add(testCredential21);
                                //testFolder2.Children.Add(testCredential22);
                                ////testFolder.Children.Add(testFolder2);
                                ////testFolder.Children.Add(testCredential1);
                                ////testFolder.Children.Add(testCredential2);

                                //var testFolderA = new FolderWithCredentialsDModel() { Children = new List<TreeItemDModel>(), CreationDateUTC = DateTime.UtcNow, ModificationDateUTC = DateTime.UtcNow, Name = "Test FolderA", Description = "Check!" };
                                //var testCredentialA = new CredentialDModel() { Children = null, CreationDateUTC = DateTime.UtcNow, ModificationDateUTC = DateTime.UtcNow, Name = "Test Credential A", Description = "Check!", Username = "testuser", Password = "testpassword" };
                                //var testFolderB1 = new FolderWithCredentialsDModel() { Children = new List<TreeItemDModel>(), CreationDateUTC = DateTime.UtcNow, ModificationDateUTC = DateTime.UtcNow, Name = "Test FolderB1", Description = "Check!" };
                                //var testCredentialA1 = new CredentialDModel() { Children = null, CreationDateUTC = DateTime.UtcNow, ModificationDateUTC = DateTime.UtcNow, Name = "Test Credential A1", Description = "Check!", Username = "testuser", Password = "testpassword" };
                                //var testCredentialB1 = new CredentialDModel() { Children = null, CreationDateUTC = DateTime.UtcNow, ModificationDateUTC = DateTime.UtcNow, Name = "Test Credential B1", Description = "Check!", Username = "testuser", Password = "testpassword" };
                                //testFolderA.Children.Add(testCredentialA);
                                //testFolderB1.Children.AddRange(new List<CredentialDModel>() { testCredentialA1, testCredentialB1 });
                                //testFolderA.Children.Add(testFolderB1);

                                //var testCredentialB = new CredentialDModel() { Children = null, CreationDateUTC = DateTime.UtcNow, ModificationDateUTC = DateTime.UtcNow, Name = "Test Credential B", Description = "Check!", Username = "testuser", Password = "testpassword" };


                                ////var testFolderNoC = new FolderDModel() { Children = new List<TreeItemDModel>(), CreationDateUTC = DateTime.UtcNow, ModificationDateUTC = DateTime.UtcNow, Name = "Test FolderNoC", Description = "Check!" };
                                ////var testCredentialNoC1 = new Credential() { Children = null, CreationDate = DateTime.UtcNow, ModificationDate = DateTime.UtcNow, Name = "Test Credential NoC1", Description = "Check!", Username = "testuser", Password = "testpassword" };
                                ////var testCredentialNoC2 = new Credential() { Children = null, CreationDate = DateTime.UtcNow, ModificationDate = DateTime.UtcNow, Name = "Test Credential NoC2", Description = "Check!", Username = "testuser", Password = "testpassword" };


                                
                                //using (var db = _dbService.GetDBContext())
                                //{

                                //    //var forRemoveFolders = db.Folders.Where(p => p.Name == "Test Folder");
                                //    //_hierarchyService.RemoveItemFromHierarchy(forRemoveFolders.First()!, db);

                                //    //db.Credentials.ForEachAsync(cred =>
                                //    //{
                                //    //    _hierarchyService.RemoveItemFromHierarchy(cred);
                                //    //}).Wait();

                                //    //var forRemoveCreds = db.Credentials.FirstOrDefault();
                                //    //db.TreeItems.Remove(forRemoveCreds!);

                                //    db.Folders.Add(testFolder);
                                //    db.Folders.Add(testFolderA);
                                //    db.Credentials.Add(testCredentialB);
                                //    //db.Folders.Add(testFolderNoC);
                                //    db.SaveChanges();
                                //}
                            },
                            (Exception ex) => ErrorDialogService.ShowErrorMessage(ex, "Main message: \n"));

                        
                    }));
            }

        }

        private RelayCommand? configurationDBShow;
        public RelayCommand ConfigurationDBShow
		{
			get
			{
				return configurationDBShow ??
					(configurationDBShow = new RelayCommand(obj =>
					{
                        

                        var newWind = new DBSettings();
                        //newWind.ShowDialog(); модальное окно перехватывает поток здесь, следовательно код дальше не выполнится, пока окно не закроется.
                                              //Поэтому присваиваение координат окну нужно делать до этого метода


                        newWind.Loaded += (s, e) =>
                        {
                            newWind.Top = (WindowCenterPosition.Y - newWind.ActualHeight / 2d);
                            newWind.Left = (WindowCenterPosition.X - newWind.ActualWidth / 2d);
                            NativeMethods.RemoveMinimizeButton(newWind);
                        };

                        newWind.ShowDialog();



                    }
					));


			}

		}

        private RelayCommand? credentialsWizardShow;

        public RelayCommand CredentialsWizardShow
        {
            get
            {
                return credentialsWizardShow ??
                    (credentialsWizardShow = new RelayCommand(obj =>
                    {
                        if (Application.Current.Windows.OfType<CredentialsWindow>().Any())
                        {
                            var oldWind = Application.Current.Windows.OfType<CredentialsWindow>().First();
                            oldWind.WindowState = WindowState.Normal; //если окно свернуто, то разворачиваем его
                            oldWind.Top = (WindowCenterPosition.Y - oldWind.ActualHeight / 2d);
                            oldWind.Left = (WindowCenterPosition.X - oldWind.ActualWidth / 2d);
                            oldWind.Activate();                            
                            return;
                        }
                        var newWind = new CredentialsWindow();
                        newWind.Loaded += (s, e) =>
                        {
                            newWind.Top = (WindowCenterPosition.Y - newWind.ActualHeight / 2d);
                            newWind.Left = (WindowCenterPosition.X - newWind.ActualWidth / 2d);
                            
                        };
                        newWind.Show();
                    }));
            }

        }


    }
}
