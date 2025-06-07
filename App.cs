using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnapManager.Views.WPF;

namespace SnapManager
{
    public partial class App : Application
    {
        //readonly MainWindow mainWindow;

        // через систему внедрения зависимостей получаем объект главного окна
        public App(MainWindow mainWindow)
        {
            base.MainWindow = mainWindow;

            this.DispatcherUnhandledException += (sender, e) =>
            {
                // обработка исключений из UI потока, не перехваченных в коде приложения
                MessageBox.Show($"Generic UI error: {e.Exception.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Handled = true; // предотвращаем дальнейшую обработку исключения, что бы приложение не завершилось
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                // обработка исключений не из UI потока, не перехваченных в коде приложения
                MessageBox.Show($"Critical error: {e.ExceptionObject}", "Critical error", MessageBoxButton.OK, MessageBoxImage.Error);
                // Здесь нельзя "подавить" завершение — программа все равно закроется, но пользователь увидит сообщение
            };

            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                // обработка исключений из асинк задач, не перехваченных в коде приложения
                MessageBox.Show($"Generic error of task: {e.Exception.Message}", "Task error", MessageBoxButton.OK, MessageBoxImage.Error);
                e.SetObserved(); // предотвращаем дальнейшую обработку исключения, что бы приложение не завершилось
            };
        }

        
        protected override void OnStartup(StartupEventArgs e)
        {
            //base.MainWindow = mainWindow;
            base.ShutdownMode = ShutdownMode.OnMainWindowClose;
            base.MainWindow.Show();  // отображаем главное окно на экране
            
            base.OnStartup(e);
        }
    }
}
