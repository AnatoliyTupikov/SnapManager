using Microsoft.Extensions.DependencyInjection;
using SnapManager.Models.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SnapManager.Views.WPF
{
    /// <summary>
    /// Interaction logic for CredentialsWindow.xaml
    /// </summary>
    public partial class CredentialsWindow : Window
    {
        public bool IsExpand { get; set; } = false;
        public CredentialsWindow()
        {
            InitializeComponent();

            DataContext = ActivatorUtilities.CreateInstance<CredentialsWindowViewModel>(Program.AppHost.Services);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var t = base.DataContext as CredentialsWindowViewModel;
            var item = qwe.SelectedItem as TreeItemBase;
            item.IsExpanded = true;
            item?.Children?.Add(new TreeItemBase() { Name = "New Credential", IsEditing = true, IsSelected = true });
            
            CollectionViewSource.GetDefaultView(item.Children).Refresh();
            

            //t.CredHierarchy.Add(new TreeItemBase() { Name = "New Credential", IsEditing = true, IsExpanded = false });
        }
    }
}
