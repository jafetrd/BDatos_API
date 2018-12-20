using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace BDatos_API
{
    public  class Metodos_comunes
    {
        public  List<Control> Controles = new List<Control>();

        public bool Seleccionar_control()
        {
            foreach (Control a in Controles)
            {
                switch (a.GetType().Name)
                {
                    case "TextBox":
                        if (string.IsNullOrEmpty((a as TextBox).Text))
                        {
                            Marcar_control(a, false);
                            a.Focus();
                            return false;
                        }
                        break;

                    case "PasswordBox":
                        if (string.IsNullOrEmpty((a as PasswordBox).Password.ToString()))
                        {
                            Marcar_control(a, false);
                            a.Focus();
                            return false;
                        }
                        break;
                    case "ComboBox":
                        if (string.IsNullOrEmpty((a as ComboBox).SelectedItem.ToString()))
                        {
                            Marcar_control(a, false);
                            a.Focus();
                            return false;
                        }
                        break;
                    default: return true;
                }
            }
            return true;
        }

        public void Marcar_control(Control control, Boolean limpiar)
        {
            if (limpiar)
                control.Background = Brushes.White;
            else
                control.Background = Brushes.LightGray;
        }

       public void Limpiar_lista()
        {
            Controles.Clear();
        }
    }
}
