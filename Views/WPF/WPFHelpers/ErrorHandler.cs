using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SnapManager.Views.WPF.WPFHelpers
{
    public class ErrorHandler
    {
        public Window Owner { get; set; }

        public ErrorHandler(Window owner)
        {
            Owner = owner;
        }

        public static T? Try<T>(Func<T> action, string? messageBeforeError = null , Action<Exception, string?>? onError = null) where T : class // where - это ограничение для T 
        {
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                
                onError?.Invoke(ex,messageBeforeError);

                return null;
            }
        } 
        public static void Try(Action action, string? messageBeforeError = null , Action<Exception, string?>? onError = null) // where - это ограничение для T 
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {                
                onError?.Invoke(ex,messageBeforeError);
            }
        } 

        public static T? TryWindowed<T>(Func<T> action, string? messageBeforeError = null, MessageBoxImage severity = MessageBoxImage.Error, Window? owner = null, Action<Exception, string?>? onError = null) where T : class // where - это ограничение для T 
        {
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                
                ShowDialogMessage(ex, messageBeforeError, severity, owner);
                onError?.Invoke(ex, messageBeforeError);

                return null;
            }
        }
        private void IstanceTryWindowed(Action action, string? messageBeforeError = null, MessageBoxImage severity = MessageBoxImage.Error, Action<Exception, string?>? onError = null) 
            => ClassTryWindowed(action, messageBeforeError, severity, Owner, onError);



        private static void ClassTryWindowed(Action action, string? messageBeforeError = null, MessageBoxImage severity = MessageBoxImage.Error, Window? owner = null,  Action<Exception, string?>? onError = null)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                
                ShowDialogMessage(ex, messageBeforeError, severity, owner);
                onError?.Invoke(ex, messageBeforeError);

            }
        }

        public static void TryWindowed(Action action, string? messageBeforeError = null, MessageBoxImage severity = MessageBoxImage.Error, Window? owner = null, Action<Exception, string?>? onError = null, ErrorHandler? instance = null ) 
        {
            try
            {
                if (instance is not null && instance.Owner is not null)
                {
                    instance.IstanceTryWindowed(action, messageBeforeError, severity, onError);
                }
                else
                {
                    ClassTryWindowed(action, messageBeforeError, severity, owner, onError);
                }
            }
            catch (Exception)
            {

                ClassTryWindowed(action, messageBeforeError, severity, owner, onError);
            }
        }

        public static void ShowDialogMessage(Exception ex, string? messageBeforeError, System.Windows.MessageBoxImage severity = MessageBoxImage.Error, Window? owner = null)
        {
            string message = messageBeforeError + "\n" + ex.Message;            
            owner ??=  Application.Current?.Windows.OfType<Window>().FirstOrDefault(p => p.IsActive == true, Program.AppHost.Services.GetService(typeof(MainWindow)) as MainWindow);
            if (owner is null) System.Windows.MessageBox.Show(message, "Error!", System.Windows.MessageBoxButton.OK, severity);
            else System.Windows.MessageBox.Show(owner, message, "Error!", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            
        }
    }
}
