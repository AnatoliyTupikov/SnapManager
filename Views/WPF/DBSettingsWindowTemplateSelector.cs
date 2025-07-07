using SnapManager.Views.WPF.WPFHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SnapManager.Views.WPF
{
    public class DBSettingsWindowTemplateSelector : DataTemplateSelector
    {
        public DataTemplate IntProp { get; set; }
        public DataTemplate StringProp { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {

            if (item is DBSettingsWindowDataGridRow row)
            {
                if (row.Column2 is int) return IntProp;
                if (row.Column2 is string) return StringProp;
            }

            return null;
        }
    }
}
