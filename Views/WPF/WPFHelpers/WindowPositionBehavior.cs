using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace SnapManager.Views.WPF.WPFHelpers
{
    /// <summary>
    /// Поведение, которое можно присвоить классу (в данном случае Window) за счет событий. 
    /// Т.е. мы имеем доступ к классу, к кот. добавляем поведение.
    /// Создаем новое св-во, кот. изменяется в зависимости от других св-в таргетного класса 
    /// и пересчитывается (обновляется) по триггеру иветнов того же класса( когда срабатывает событие).
    /// Чтобы на новое св-во можно было забиндится, повесить анимацию и пр. это св-во делается зависимым. (см. xaml)  
    /// </summary>
    public class WindowPositionBehavior : Behavior<Window>
    //для регистрации св-ва зависимостей нужно, чтобы путь наслдедования класса,
    //в кот. будет/ут регистрироваться св-во(а), привел к DependencyObject
    {
        public Point Position
        {
            get { return (Point)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
            //при создании экземпляра этого класса, естественно срабатывает и конструктор DependencyObject класса, т.к.
            //текущ. класс наследуется от него. В этом конструкторе и создается служеб. таблица,
            //где и хранятся эти зависимые св-ва
        }

        //регистрируем свойство выше, чтобы WPF мог изменяет его через Binding
        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register(
                "Position",        //имя свойства
                typeof(Point),    // тип кот. хранит св-во 
                typeof(WindowPositionBehavior), //класс, кот. пренадлежит св-во
                new PropertyMetadata(new Point(0, 0))); //класс, кот. содержит в себе метаданные:              //делегат ValidateValueCallback,                                                        
                                                        // - дефолтное значение                               //кот. вызвается ПЕРЕД присваиванием значения
                                                        // - делегат, кот. будет вызываться                   // true - ok, false - throw ArgumentException
                                                        //   каждый раз, когда значение уже поменялось (ивент)//Необязательный параметр (перегрузка метода)
                                                        // - делегат, кот. будет вызываться                   //поэтому он тут отсутсвует
                                                        //   при присваивании значения. Например:
                                                        //   если приходит >100, присвоить 100
                                                        //Необязательный параметр (перегрузка метода)



        public Point CenterPosition
        {
            get { return (Point)GetValue(CenterPositionProperty); }
            set { SetValue(CenterPositionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WindowCenterPosition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CenterPositionProperty =
            DependencyProperty.Register("CenterPosition", typeof(Point), typeof(WindowPositionBehavior), new PropertyMetadata(new Point(0, 0)));


        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnWindowChangePosition;
            AssociatedObject.LocationChanged += OnWindowChangePosition;
            AssociatedObject.SizeChanged += OnWindowChangePosition;
            Update();

        }

        protected override void OnDetaching()
        {

            AssociatedObject.Loaded -= OnWindowChangePosition;
            AssociatedObject.LocationChanged -= OnWindowChangePosition;
            AssociatedObject.SizeChanged -= OnWindowChangePosition;
            base.OnDetaching();
        }

        private void OnWindowChangePosition(object? sender, EventArgs e)
        {
            Update();
        }

        void Update()
        {
            var source = PresentationSource.FromVisual(AssociatedObject);
            if (source != null)
            {
                //var transform = source.CompositionTarget.TransformToDevice;
                //var dpiX = transform.M11;
                //var dpiY = transform.M22;

                //var location = AssociatedObject.PointToScreen(new Point(0, 0));

                Position = NativeMethods.GetPointToScreenDPI(AssociatedObject); //new Point(location.X / dpiX, location.Y / dpiY);

                Point localCenter = new Point(AssociatedObject.ActualWidth / 2d, AssociatedObject.ActualHeight / 2d);

                CenterPosition = NativeMethods.GetPointToScreenDPI(AssociatedObject, localCenter);


            }

        }


    }
}
