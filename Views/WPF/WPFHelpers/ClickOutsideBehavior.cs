using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace SnapManager.Views.WPF.WPFHelpers
{
    public class ClickOutsideBehavior : Behavior<FrameworkElement>
    {
        //для регистрации св-ва зависимостей нужно, чтобы путь наслдедования класса,
        //в кот. будет/ут регистрироваться св-во(а), привел к DependencyObject

        

        public Storyboard Animation
        {
            get { return (Storyboard)GetValue(AnimationProperty); }
            set { SetValue(AnimationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnimationProperty =
            DependencyProperty.Register("Animation", typeof(Storyboard), typeof(ClickOutsideBehavior));        


        protected override void OnAttached()
        {
            base.OnAttached();
            var window = Window.GetWindow(AssociatedObject);
            window.PreviewMouseLeftButtonDown += OnElemetnMouseDown;            

        }

        protected override void OnDetaching()
        {
            var window = Window.GetWindow(AssociatedObject);
            window.PreviewMouseLeftButtonDown -= OnElemetnMouseDown;
            base.OnDetaching();
        }

        private void OnElemetnMouseDown(object? sender, EventArgs e)
        {
            if(AssociatedObject.IsMouseOver) return;  // Если клик был внутри элемента, то ничего не делаем       

            Animation?.Begin(); // Запускаем анимацию, если она задана
            
        }

        
    }
}
