using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BDatos_API
{
    public static class Navegacion
    {
        static Navegacion()
        {
            pilaNavegacion.Push(Application.Current.MainWindow);
        }

        private static readonly Stack<Window> pilaNavegacion = new Stack<Window>();

        private static Frame _frame;

        public static Frame Frame
        {
            get { return _frame; }
            set { _frame = value; }
        }

        //public static bool NavegarA(Uri sourcePageUri, object extraData = null)
        //{
        //    if (_frame.NavigationService.CurrentSource != sourcePageUri)
        //    {
        //        return _frame.NavigationService.Navigate(sourcePageUri, extraData);
        //    }
        //    return true;
        //}

        public static void NavegarA(Window win)
        {
            if (pilaNavegacion.Count > 0)
                pilaNavegacion.Peek().Hide();
            pilaNavegacion.Push(win);
            win.Show();
        }

        public static bool NavegarA(object content)
        {
            if (_frame.NavigationService.Content != content)
            {
                return _frame.NavigationService.Navigate(content);
            }
            return true;
        }

        //public static void Regresar_frame()
        //{
        //    if (_frame.CanGoBack)
        //    {
        //        _frame.GoBack();
        //    }
        //}

        public static bool NavegarAtras()
        {
            if (pilaNavegacion.Count <= 1)
                return false;

            pilaNavegacion.Pop().Hide();
            pilaNavegacion.Peek().Show();
            return true;
        }

        public static bool sePuedeNavegarAtras()
        {
            return pilaNavegacion.Count > 1;
        }
    }
}
