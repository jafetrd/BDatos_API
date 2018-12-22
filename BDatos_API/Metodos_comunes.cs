using MahApps.Metro.Controls.Dialogs;
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
    public class Metodos_comunes
    {
        public List<Control> campos = new List<Control>();


        public  bool Seleccionar_control(bool apuntar)
        {
            foreach (Control a in campos)
            {
                switch (a.GetType().Name)
                {
                    case "TextBox":
                        if (string.IsNullOrEmpty((a as TextBox).Text))
                        {
                            Marcar_control(a, apuntar);
                            a.Focus();
                            return false;
                        }
                        break;

                    case "PasswordBox":
                        if (string.IsNullOrEmpty((a as PasswordBox).Password.ToString()))
                        {
                            Marcar_control(a, apuntar);
                            a.Focus();
                            return false;
                        }
                        break;
                    case "ComboBox":
                        if ((a as ComboBox).SelectedValue==null)
                        {
                            Marcar_control(a, apuntar);
                            a.Focus();
                            return false;
                        }
                        break;
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
            if(campos.Count>0)
            campos.Clear();
        }

        public Control Inicial
        {
            get
            {
                return campos[0];
            }
        }
    }
}
