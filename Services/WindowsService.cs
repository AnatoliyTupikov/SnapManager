using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SnapManager.Services
{
    public class WindowsService
    {
        public WindowCollection OpenedWindows { get; private set; } = new WindowCollection();

        
    }
}
