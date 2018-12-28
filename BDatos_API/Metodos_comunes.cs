using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BDatos_API
{
    public class Metodos_comunes
    {
        public List<Control> campos = new List<Control>();
       

        public Metodos_comunes(){
            
        }

        public  bool Seleccionar_control(bool apuntar)
        {
            TextBox textBox=null;
            PasswordBox passwordBox = null;
            ComboBox comboBox = null;

            foreach (Control a in campos)
            {

                switch (a.GetType().Name)
                {
                    case "TextBox":
                        textBox = a as TextBox;
                        if (string.IsNullOrEmpty(textBox.Text))
                        {
                            Marcar_control(textBox, apuntar);
                            a.Focus();
                            return false;
                        }
                        break;

                    case "PasswordBox":
                        passwordBox = (a as PasswordBox);
                        if (string.IsNullOrEmpty(passwordBox.Password))
                        {
                            Marcar_control(passwordBox, apuntar);
                            a.Focus();
                            return false;
                        }
                        break;
                    case "ComboBox":
                        comboBox = (a as ComboBox);
                        if (comboBox.SelectedItem==null)
                        {
                            Marcar_control(comboBox, apuntar);
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
                control.ClearValue(Control.BackgroundProperty);
            else
                control.Background = Brushes.LightGray;
        }

       public void Limpiar_lista()
        {
            if(campos.Count>0)
            campos.Clear();
        }

        public void Limpiar_controles()
        {
            foreach(Control a in campos)
            {
                switch (a.GetType().Name)
                {
                    case "TextBox":
                        (a as TextBox).Clear();
                        (a as TextBox).ClearValue(Control.BackgroundProperty);
                        break;

                    case "PasswordBox":
                        (a as PasswordBox).Clear();
                        (a as PasswordBox).ClearValue(Control.BackgroundProperty);
                        break;
                    case "ComboBox":
                        (a as ComboBox).SelectedItem = null;
                        (a as ComboBox).ClearValue(Control.BackgroundProperty);
                        break;
                }
            }
        }

        public Control Inicial
        {
            get
            {
                return campos[0];
            }
        }

        public static IEnumerable<Key> seDetectoTecla()
        {
            foreach (Key key in Enum.GetValues(typeof(Key)))
            {
                if (Keyboard.IsKeyDown(key))
                    yield return key;
            }
        }

    }

}
