using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BDatos_API
{
    public static class Navegacion
    {
        static Navegacion()
        {
            pilaNavegacion.Push(Application.Current.MainWindow);
        }

        private static readonly Stack<Window> pilaNavegacion = new Stack<Window>();

        public static void NavigarA(Window win)
        {
            if (pilaNavegacion.Count > 0)
                pilaNavegacion.Peek().Hide();
            pilaNavegacion.Push(win);
            win.Show();
        }
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
