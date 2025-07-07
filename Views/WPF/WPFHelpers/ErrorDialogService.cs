using SnapManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SnapManager.Views.WPF.WPFHelpers
{
    public static class ErrorDialogService
    {
        public static void ShowErrorMessage(Exception ex, string? messageBeforeError = null, bool suppressOriginalMessage = false, Severity? severity = null, Window? owner = null)
        {
            string? message = messageBeforeError;
            if (!suppressOriginalMessage) 
            {
                message += ex.Message;
                while (ex.InnerException is not null)
                {                   
                    ex = ex.InnerException;
                    message += "\n";
                    message += ex.Message;
                    
                }
                 
            }
            severity ??= ex.Data.Contains("Severity") && ex.Data["Severity"] is not null? ex.Data["Severity"] as Severity? ?? Severity.Error : Severity.Error;
            var winSev = (MessageBoxImage)(int)severity;
            
            owner ??= Application.Current?.Windows.OfType<Window>().FirstOrDefault(p => p.IsActive == true, Program.AppHost.Services.GetService(typeof(MainWindow)) as MainWindow);
            if (owner is null) MessageBox.Show(message, "Error!", MessageBoxButton.OK, winSev);
            else MessageBox.Show(owner, message, "Error!", MessageBoxButton.OK, winSev);

        }

    }
}
