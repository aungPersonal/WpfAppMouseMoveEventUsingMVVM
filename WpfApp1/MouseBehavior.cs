using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
namespace WpfApp1
{
    public static class MouseBehavior
    {
        public static readonly DependencyProperty MouseMoveCommandProperty =
            DependencyProperty.RegisterAttached(
                "MouseMoveCommand",
                typeof(ICommand),
                typeof(MouseBehavior),
                new PropertyMetadata(null, OnMouseMoveCommandChanged));

        public static void SetMouseMoveCommand(UIElement element, ICommand value)
        {
            element.SetValue(MouseMoveCommandProperty, value);
        }

        public static ICommand GetMouseMoveCommand(UIElement element)
        {
            return (ICommand)element.GetValue(MouseMoveCommandProperty);
        }

        private static void OnMouseMoveCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement element)
            {
                if (e.OldValue is ICommand oldCommand)
                {
                    element.MouseMove -= OnMouseMove;
                }

                if (e.NewValue is ICommand newCommand)
                {
                    element.MouseMove += OnMouseMove;
                }
            }
        }

        private static void OnMouseMove(object sender, MouseEventArgs e)
        {
            var element = sender as UIElement;
            var command = GetMouseMoveCommand(element);

            if (command != null && command.CanExecute(e))
            {
                command.Execute(e);
            }
        }
    }

}
