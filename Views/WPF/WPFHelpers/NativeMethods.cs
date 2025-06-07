using System;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Media;

namespace SnapManager.Views.WPF.WPFHelpers
{
    public static class NativeMethods
    {
        [DllImport("user32.dll")]
        public static extern uint GetDpiForWindow(nint hWnd);

        [DllImport("user32.dll")]
        public static extern long GetWindowLongA(nint hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern long SetWindowLongA(nint hWnd, int nIndex, long dwNewLong);

        public static Point GetPointToScreenDPI(Window window, Point? LocalPoint = null)
        {
            LocalPoint = LocalPoint ?? new Point(0, 0);
            Point NotNullPoint = (Point)LocalPoint;

            var pointOnScreen = window.PointToScreen(NotNullPoint);
            var hwnd = new WindowInteropHelper(window).Handle;

            if (hwnd != nint.Zero)
            {
                uint dpi = GetDpiForWindow(hwnd);
                double scale = dpi / 96.0;
                pointOnScreen.X /= scale;
                pointOnScreen.Y /= scale;
            }

            return pointOnScreen;
        }

        public static void RemoveMinimizeButton(Window window)
        {
            var hwnd = new WindowInteropHelper(window).Handle;
            if (hwnd != nint.Zero)
            {
                int GWL_STYLE = -16; //код, указывающий на то, битовую маску какого элемента я хочу получить. В данном случае style окна
                var style = GetWindowLongA(hwnd, GWL_STYLE); //получаем эту маску
                style &= ~0x00020000L; //это битовая маска кнопки minimize ( 1 бит на 16 позиции, все остальные 0) в 16-ричном формате 
                SetWindowLongA(hwnd, GWL_STYLE, style); //сетим уже исправленную битовую маску <style> в стиль <GWL_STYLE>
                                                        //для того же идентификатора окна <hwnd> 

            }
        }

        public static bool HasValidationErrors(DependencyObject parent)
        {
            if (parent == null)
                return false;

            // Проверить текущий элемент
            if (Validation.GetHasError(parent))
                return true;

            // Рекурсивно проверить дочерние элементы
            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (HasValidationErrors(child))
                    return true;
            }

            return false;
        }


    }
}
