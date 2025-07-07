using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SnapManager.Data;
using SnapManager.Models;
using SnapManager.Models.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapManager.Services
{
    public class HierarchyService
    {

        public void RemoveItemFromHierarchy(TreeItemBase hierarchyItem, ApplicationDbContext db)
        {
            db.TreeItems.Include("Children").Load();

            while (!hierarchyItem.Children.IsNullOrEmpty())
            {
                // Recursively remove all children first
                RemoveItemFromHierarchy(hierarchyItem.Children![0], db);
            }


            hierarchyItem.Parents?.ForEach(parent => parent.Children?.Remove(hierarchyItem));


            db.TreeItems.Remove(hierarchyItem);
            db.SaveChanges();

        }

        public List<TreeItemBase> GetCredsHierarchy(ApplicationDbContext db)
        {
            db.TreeItems.Include("Children").Load();
            List<TreeItemBase> result = db.TreeItems.Where(p => (p.GetType() == typeof(FolderWithCredentials) || p.GetType() == typeof(Credential))).ToList();
            List<TreeItemBase> res = result.Where(p => p.Parents.IsNullOrEmpty()).ToList();
            return res;
        }

        public void WalkThroughHierarchy_IdScope(ICollection<TreeItemBase> hierarchylevel, Action<TreeItemBase> action, params int[] action_scope_id)
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

        public void WalkThroughHierarchy(ICollection<TreeItemBase> hierarchylevel, Action<TreeItemBase> action, params TreeItemBase[] action_scope_id)
        {
            int[] action_scope_id_int = action_scope_id.Select(x => x.Id).ToArray();
            WalkThroughHierarchy_IdScope(hierarchylevel, action, action_scope_id_int);
        }

        public void SaveHierarchyState(ICollection<TreeItemBase> hierarchy, ApplicationDbContext db)
        {
            Action<TreeItemBase> action = item =>
            {
                db.TreeItems.Attach(item);
                db.Entry(item).Property(i => i.IsExpanded).IsModified = true;
                db.Entry(item).Property(i => i.IsSelected).IsModified = true;
            };

            WalkThroughHierarchy(hierarchy, action);
        }

    }
}
