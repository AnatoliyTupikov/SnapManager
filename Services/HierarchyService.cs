using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SnapManager.Data;
using SnapManager.Data.DesignTime.Migrations.NpgsqlServer;
using SnapManager.Models.WPFModels;
using SnapManager.Models.WPFModels.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapManager.Services
{
    public class HierarchyService
    {

        
        public List<TreeItemWpfModel> GetCredsHierarchyFromDB(ApplicationDbContext db)
        {
            db.TreeItems.Include("Children").Load();
            List<TreeItemWpfModel> result = db.TreeItems.Where(p => (p.GetType() == typeof(FolderWithCredentialsWpfModel) || p.GetType() == typeof(CredentialWpfModel))).ToList();
            List<TreeItemWpfModel> res = result.Where(p => p.Parent is null).ToList();
            SortHierarchy(res);
            //.OrderBy(p => 
            //{
            //    if (p is Folder) return 0; // Folders should be at the top
            //    if (p is Credential) return 1; // Credentials should be below folders
            //    return 99; // Other items should be at the bottom
            //})
            //.ThenBy(p => p.Name)
            //.ToList();
            return res;
        }

        public void SortHierarchy(List<TreeItemWpfModel> hierarchy)
        {
            var sorted = hierarchy.OrderBy(p =>
            {
                if (p is FolderWpfModel) return 0; // Folders should be at the top
                if (p is CredentialWpfModel) return 1; // Credentials should be below folders
                return 99; // Other items should be at the bottom
            })
            .ThenBy(p => p.Name)
            .ToList();
            hierarchy.Clear();
            hierarchy.AddRange(sorted);
            foreach (var item in hierarchy)
            {
                if (item.Children != null && item.Children.Count > 0)
                {
                    SortHierarchy(item.Children);
                }
            }
        }

        public void WalkThroughHierarchy_IdScope(ICollection<TreeItemWpfModel> hierarchylevel, Action<TreeItemWpfModel> action, params int[] action_scope_id)
        {
            foreach (var item in hierarchylevel)
            {
                if (action_scope_id.Length == 0 || action_scope_id.Contains(item.Id))
                {
                    action(item);
                }
                if (!item.Children.IsNullOrEmpty())
                {
                    WalkThroughHierarchy_IdScope(item.Children!, action, action_scope_id);
                }
            }
        }

        public void WalkThroughHierarchy(ICollection<TreeItemWpfModel> hierarchylevel, Action<TreeItemWpfModel> action, params TreeItemWpfModel[] action_scope_id)
        {
            int[] action_scope_id_int = action_scope_id.Select(x => x.Id).ToArray();
            WalkThroughHierarchy_IdScope(hierarchylevel, action, action_scope_id_int);
        }

        public void AddChildToParent(FolderWpfModel parent, TreeItemWpfModel child)
        {            
            if (parent.Children is null) parent.Children = new List<TreeItemWpfModel>();
            if (child.Parent != null) throw new InvalidOperationException("Child already has a parent");
            parent.Children.Add(child);
            child.Parent = parent;
        }

        public void AddChildToParentWithSearchUp(TreeItemWpfModel parent, TreeItemWpfModel child)
        {
            if (parent is FolderWpfModel folderParent)
            {
                AddChildToParent(folderParent, child);
                return;
            }
            else
            {
                if (parent.Parent is null) {child.Parent = null; return; }
                AddChildToParentWithSearchUp(parent.Parent, child);
            }
        }

        public void RemoveChildFromParent(TreeItemWpfModel child)
        {
            if (child.Parent is null) throw new InvalidOperationException("Child has no parent to remove from");
            child.Parent.Children?.Remove(child);
            child.Parent = null;
        }

        public void ChangeParent(TreeItemWpfModel child, FolderWpfModel newParent)
        {
            
            if (child.Parent != null) RemoveChildFromParent(child);
            AddChildToParent(newParent, child);
        }

        public void ChangeParentWithSearchUp(TreeItemWpfModel child, TreeItemWpfModel newParent)
        {
            if (child.Parent != null) RemoveChildFromParent(child);
            AddChildToParentWithSearchUp(newParent, child);
        }      

        public void RemoveItemFromHierarchy(TreeItemWpfModel item, ApplicationDbContext? db = null)
        {
            var tempChildren = item.Children?.ToList() ?? new List<TreeItemWpfModel>();
            foreach (var child in tempChildren)
            {
                RemoveItemFromHierarchy(child, db);
            }
            if (item.Parent != null)
            {
                item.Parent.Children?.Remove(item);
                item.Parent = null;
            }
            if (db != null)
            {
                db.TreeItems.Remove(item);
                db.SaveChanges();
            }
        }



    }
}
