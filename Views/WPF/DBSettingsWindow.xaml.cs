using Microsoft.Extensions.DependencyInjection;
using SnapManager.Views.WPF.WPFHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
    /// Interaction logic for DBSettings.xaml
    /// </summary>
    public partial class DBSettings : Window
    {

        public DBSettings()
        {
            InitializeComponent();
            DataContext = ActivatorUtilities.CreateInstance<DBSettingsWindowViewModel>(Program.AppHost.Services, new ErrorHandler(this));

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show(this, "This is a test message", "Test", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
