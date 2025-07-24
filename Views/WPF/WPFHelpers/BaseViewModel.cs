using SnapManager.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SnapManager.Views.WPF.WPFHelpers
{
    public class BaseViewModel : INotifyPropertyChanged
    {

        public BaseViewModel() { }



        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged<T>( ref T property, T value, [CallerMemberName] string prop = "") //аттрибут компилятора, кот. передает имя вызваютщего метода или св-ва, вешается только на необязательные параметры
        {
            property = value;
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
