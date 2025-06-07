using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapManager.Views.WPF.WPFHelpers
{
    public class ColumClass : BaseViewModel
    {
		private dynamic _value;

		public dynamic Value
		{
			get { return _value; }
			set { OnPropertyChanged<dynamic>(ref _value, value); }
		}

		public ColumClass(dynamic value) => _value = value;
	}
}
