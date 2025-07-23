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
    public enum NestedOrRootType
    {
        Cancel = -1,
        Root,
        Nested
    }
    /// <summary>
    /// Interaction logic for AddCredsOrFolderWindow.xaml
    /// </summary>
    public partial class NestedOrRoot : Window
    {
        public NestedOrRootType Result { get; private set; }
        public NestedOrRoot()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Result = NestedOrRootType.Cancel;
            this.Close();
        }

        public static NestedOrRootType ShowNestedOrRoot(Window? owner)
        {
            NestedOrRoot NestedOrRoot = new NestedOrRoot();
            NestedOrRoot.Owner = owner;
            NestedOrRoot.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            NestedOrRoot.ShowDialog();
            return NestedOrRoot.Result;
        }

        private void Root_Click(object sender, RoutedEventArgs e)
        {
            Result = NestedOrRootType.Root;
            this.Close();
        }

        private void Nested_Click(object sender, RoutedEventArgs e)
        {
            Result = NestedOrRootType.Nested;
            this.Close();
        }
    }
}
