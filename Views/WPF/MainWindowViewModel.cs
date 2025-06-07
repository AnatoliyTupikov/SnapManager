using SnapManager.Data;
using SnapManager.Views.WPF;
using SnapManager.Views.WPF.WPFHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SnapManager.Views.WPF.WPFViewModels
{
    public class MainWindowViewModel : BaseViewModel
	{
        private DbService _dbService;



        private Point windowCenterPosition;

        public Point WindowCenterPosition
        {
            get { return windowCenterPosition; }
            set { OnPropertyChanged<Point>(ref windowCenterPosition, value); }
        }
        
        public MainWindowViewModel(DbService dbService) 
        {
            _dbService = dbService;

        }




        private RelayCommand? configurationDBShow;
        public RelayCommand ConfigurationDBShow
		{
			get
			{
				return configurationDBShow ??
					(configurationDBShow = new RelayCommand(obj =>
					{

                        
                        var newWind = new DBSettings();
                        //newWind.ShowDialog(); модальное окно перехватывает поток здесь, следовательно код дальше не выполнится, пока окно не закроется.
                                              //Поэтому присваиваение координат окну нужно делать до этого метода


                        newWind.Loaded += (s, e) =>
                        {
                            newWind.Top = (WindowCenterPosition.Y - newWind.ActualHeight / 2d);
                            newWind.Left = (WindowCenterPosition.X - newWind.ActualWidth / 2d);
                            NativeMethods.RemoveMinimizeButton(newWind);
                        };

                        newWind.ShowDialog();



                    }
					));


			}

		}
        
    }
}
