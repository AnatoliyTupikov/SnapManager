using SnapManager.Models.DomainModels;
using SnapManager.Models.WPFModels;
using SnapManager.Models.WPFModels.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapManager.Services.MappingServices.ITreeItemMappingServices
{
    public class TreeItemMappingServices : ITreeItemMappingServices
    {
        private bool _isFirstLoad = true;
        private Dictionary<int, TreeItemWpfModel> _wpfModelsCache = new Dictionary<int, TreeItemWpfModel>();
        private int _selectedItemId = -1;

        //public TreeItemBaseDModel MapToDomainModel(TreeItemWpfModel wpfModel)
        //{
        //    List<TreeItemBaseDModel>? tempChildren = new List<TreeItemBaseDModel>(); ;
        //    if (wpfModel.Children != null) tempChildren = wpfModel.Children.Select(child => child.DModel).ToList();

        //    TreeItemBaseDModel domainModelTemplate = new TreeItemBaseDModel
        //    {
        //        Id = wpfModel.Id,
        //        Name = wpfModel.Name,
        //        Parent = wpfModel.Parent?.DModel,
        //        Children = ,

        //    };

        //    TreeItemBaseDModel domainModel = wpfModel switch
        //    {

        //        FolderWithCredentialsWpfModel folderwithcreds => new FolderWithCredentialsModel
        //        {


        //            CreationDate = folderwithcreds.CreationDate,
        //            ModificationDate = folderwithcreds.ModificationDate,
        //            Description = folderwithcreds.Description
        //        },
        //        FolderWpfModel folder => new FolderDModel
        //        {


        //            CreationDate = folder.CreationDate,
        //            ModificationDate = folder.ModificationDate,
        //            Description = folder.Description
        //        },                
        //        CredentialWpfModel credential => new CredentialDModel
        //        {                    

        //            Username = credential.Username,
        //            Password = credential.Password,
        //            CreationDateUTC = credential.CreationDateUTC,
        //            ModificationDateUTC = credential.ModificationDateUTC,
        //            Description = credential.Description
        //        },
        //        _ => throw new NotSupportedException("Unsupported TreeItemBase type")
        //    };
        //}

        public TreeItemWpfModel MapToWPFModel(TreeItemBaseDModel domainModel)
        {
            if (_isFirstLoad)
            {
                _wpfModelsCache.Clear();
                _selectedItemId = domainModel.Id;
                _isFirstLoad = false;
            }



        }

        private TreeItemBaseDModel SearchRoootElement(TreeItemBaseDModel domainModel)
        {
            if (domainModel.Parent == null)
            {
                return domainModel;
            }
            else
            {
                return SearchRoootElement(domainModel.Parent);
            }
        }

        private TreeItemWpfModel IdentInherited(TreeItemBaseDModel domainModel, TreeItemWpfModel baseObject)
        {
       

            TreeItemWpfModel result = domainModel switch
            {
                CredentialDModel credentialDModel => GetCredentialObject(baseObject, credentialDModel),
                FolderWithCredentialsDModel folderWithCredentialsDModel => GetFolderWithCredentialsObject(baseObject, folderWithCredentialsDModel),
                FolderDModel folderDModel => GetFolderObject(baseObject, folderDModel),
                TreeItemBaseDModel _ => baseObject

            };
            return result;


        }

        private TreeItemWpfModel GetBaseObject(TreeItemBaseDModel domainModel)
        {

            if (!_wpfModelsCache.ContainsKey(domainModel.Id))
            {
                var baseObject = new TreeItemWpfModel
                {
                    DModel = domainModel,
                    Name = domainModel.Name,
                    Parent = domainModel.Parent != null ? _wpfModelsCache.GetValueOrDefault(domainModel.Parent.Id) : null,

                };


                if (domainModel.Children != null && domainModel.Children.Count > 0)
                {
                    domainModel.Children.ForEach(child =>
                    {
                        Result.Children!.Add(GetBaseObject(child));
                    });
                }
                return Result;
            }
            else return _wpfModelsCache[domainModel.Id];
        }

        private CredentialWpfModel GetCredentialObject(TreeItemWpfModel baseObject, CredentialDModel credentialDModel)
        {
            if (baseObject is not CredentialWpfModel) throw new InvalidOperationException("Base object must be of type CredentialWpfModel.");
            var credentialWpfModel = baseObject as CredentialWpfModel;
            credentialWpfModel!.Username = credentialDModel.Username;
            credentialWpfModel.Password = credentialDModel.Password;
            credentialWpfModel.CreationDateUTC = credentialDModel.CreationDateUTC;
            credentialWpfModel.ModificationDateUTC = credentialDModel.ModificationDateUTC;
            credentialWpfModel.Description = credentialDModel.Description;
            return credentialWpfModel;
        }

        private FolderWithCredentialsWpfModel GetFolderWithCredentialsObject(TreeItemWpfModel baseObject, FolderWithCredentialsDModel folderWithCredentialsDModel)
        {
            if (baseObject is not FolderWithCredentialsWpfModel) throw new InvalidOperationException("Base object must be of type FolderWithCredentialsWpfModel.");
            return GetFolderObject(baseObject, folderWithCredentialsDModel) as FolderWithCredentialsWpfModel;
        }

        private FolderWpfModel GetFolderObject(TreeItemWpfModel baseObject, FolderDModel folderDModel)
        {
            if (baseObject is not FolderWpfModel) throw new InvalidOperationException("Base object must be of type FolderWpfModel.");
            var folderWpfModel = baseObject as FolderWpfModel;
            folderWpfModel!.CreationDate = folderDModel.CreationDate;
            folderWpfModel.ModificationDate = folderDModel.ModificationDate;
            folderWpfModel.Description = folderDModel.Description;
            return folderWpfModel;
        } 
    }
}
