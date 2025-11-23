using Microsoft.IdentityModel.Tokens;
using SnapManager.Models.DomainModels.CredentialsHierarchy;
using SnapManager.Models.WPFModels.CredentialHierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapManager.Services.MappingServices
{
    public class TreeItemMappingService : IMappingService<TreeItemDModel, TreeItemWpfModel>
    {
        private int _itemId;
        private TreeItemWpfModel _globalResult;


        public TreeItemWpfModel MapToWievModel(TreeItemDModel domainModel)
        {
            _itemId = domainModel.Id;
            var rootElement = SearchRoootElement(domainModel);
            FromDModelToWpfModel(rootElement);
            return _globalResult;
        }

        private TreeItemDModel SearchRoootElement(TreeItemDModel domainModel)
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

        private TreeItemWpfModel IdentInherited(TreeItemDModel domainModel)
        {
            TreeItemWpfModel result = domainModel switch
            {
                CredentialDModel credentialDModel => GetCredentialObject(credentialDModel),
                FolderWithCredentialsDModel folderWithCredentialsDModel => GetFolderWithCredentialsObject(folderWithCredentialsDModel),
                FolderDModel folderDModel => GetFolderObject(folderDModel),
                TreeItemDModel _ => new TreeItemWpfModel
                {
                    DModel = domainModel,
                    Name = domainModel.Name,
                    CreationDateUTC = domainModel.CreationDateUTC,
                    ModificationDateUTC = domainModel.ModificationDateUTC,
                    Parent = null
                }
            };
            return result;


        }

        private TreeItemWpfModel FromDModelToWpfModel(TreeItemDModel domainModel)
        {           
            
            var Result = IdentInherited(domainModel);

            if (!domainModel.Children.IsNullOrEmpty())
            {
                domainModel.Children!.ForEach(child =>
                {  
                    var wpfChild = FromDModelToWpfModel(child);
                    var hs = new WpfHierarchyService();
                    hs.AddChildToParent(Result, wpfChild);
                    
                });
            }
            if (Result.Id == _itemId) _globalResult = Result;
            return Result;          
        }

        private CredentialWpfModel GetCredentialObject(CredentialDModel credentialDModel)
        {
            return new CredentialWpfModel()
            {
                DModel = credentialDModel,
                Name = credentialDModel.Name,                               
                Username = credentialDModel.Username,
                Password = credentialDModel.Password,
                CreationDateUTC = credentialDModel.CreationDateUTC,
                ModificationDateUTC = credentialDModel.ModificationDateUTC,
                Description = credentialDModel.Description
            };
        }

        private FolderWithCredentialsWpfModel GetFolderWithCredentialsObject(FolderWithCredentialsDModel folderWithCredentialsDModel)
        {           
            return new FolderWithCredentialsWpfModel()
            {
                DModel = folderWithCredentialsDModel,
                Name = folderWithCredentialsDModel.Name,
                CreationDateUTC = folderWithCredentialsDModel.CreationDateUTC,
                ModificationDateUTC = folderWithCredentialsDModel.ModificationDateUTC,
                Description = folderWithCredentialsDModel.Description
            };
        }

        private FolderWpfModel GetFolderObject(FolderDModel folderDModel)
        {            
            return new FolderWpfModel()
            {
                DModel = folderDModel,
                Name = folderDModel.Name,
                CreationDateUTC = folderDModel.CreationDateUTC,
                ModificationDateUTC = folderDModel.ModificationDateUTC,
                Description = folderDModel.Description
            };
        } 
    }
}
