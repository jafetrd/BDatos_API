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

        public void ocultarCampos(int[] posicion)
        {
            for(int a = 0; a < posicion.Length; a++)
            {
                campos[posicion[a]].IsEnabled = false;
            }
        }

        public void mostrarCampos(int[] posicion)
        {
            for (int a = 0; a < posicion.Length; a++)
            {
                campos[posicion[a]].IsEnabled = true;
            }
        }
        /// <summary>
        /// Cuando un control en la pantalla esta vacio este lo apunta y retorna un valor 
        /// true si vacio y false si todo esta lleno
        /// </summary>
        /// <param name="apuntar">true=control color original false=color gris</param>
        /// <returns></returns>
        public bool Seleccionar_control(bool apuntar)
        {
            TextBox textBox=null;
            PasswordBox passwordBox = null;
            ComboBox comboBox = null;
            Xceed.Wpf.Toolkit.IntegerUpDown integerUpDown = null;
            Xceed.Wpf.Toolkit.DecimalUpDown decimalUpDown = null;
            Xceed.Wpf.Toolkit.DateTimePicker dateTimePicker = null;
            DotNetKit.Windows.Controls.AutoCompleteComboBox autoCompleteComboBox = null;

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

                    case "IntegerUpDown":
                        integerUpDown = (a as Xceed.Wpf.Toolkit.IntegerUpDown);
                        if (string.IsNullOrEmpty(integerUpDown.Text))
                        {
                            Marcar_control(integerUpDown, apuntar);
                            a.Focus();
                            return false;
                        }
                        break;

                    case "DecimalUpDown":
                        decimalUpDown = (a as Xceed.Wpf.Toolkit.DecimalUpDown);
                        if (string.IsNullOrEmpty(decimalUpDown.Text))
                        {
                            Marcar_control(decimalUpDown, apuntar);
                            a.Focus();
                            return false;
                        }
                        break;

                    case "DateTimePicker":
                        dateTimePicker = (a as Xceed.Wpf.Toolkit.DateTimePicker);
                        if (string.IsNullOrEmpty(dateTimePicker.Text))
                        {
                            Marcar_control(dateTimePicker, apuntar);
                            a.Focus();
                            return false;
                        }
                        break; 

                    case "AutoCompleteComboBox":
                        autoCompleteComboBox = (a as DotNetKit.Windows.Controls.AutoCompleteComboBox);
                        if (string.IsNullOrEmpty(autoCompleteComboBox.Text))
                        {
                            Marcar_control(autoCompleteComboBox, apuntar);
                            a.Focus();
                            return false;
                        }
                        break;
                }
            }
            return true;
        }
        /// <summary>
        /// Cuando un control en la pantalla esta vacio este lo apunta y retorna un valor 
        /// true si vacio y false si todo esta lleno
        /// </summary>
        /// <param name="posicion">un arreglo int que indica que elementos se van a verificar</param>
        /// <param name="apuntar">true=control color original false=color gris</param>
        /// <returns></returns>
        public bool Seleccionar_control(int[] posicion,bool apuntar)
        {
            TextBox textBox = null;
            PasswordBox passwordBox = null;
            ComboBox comboBox = null;
            Xceed.Wpf.Toolkit.IntegerUpDown integerUpDown = null;
            Xceed.Wpf.Toolkit.DecimalUpDown decimalUpDown = null;
            Xceed.Wpf.Toolkit.DateTimePicker dateTimePicker = null;
            DotNetKit.Windows.Controls.AutoCompleteComboBox autoCompleteComboBox = null;
            Control a;
            for(int b=0;b<posicion.Length;b++)
            {
                a = campos[posicion[b]];

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
                        if (comboBox.SelectedItem == null)
                        {
                            Marcar_control(comboBox, apuntar);
                            a.Focus();
                            return false;
                        }
                        break;

                    case "IntegerUpDown":
                        integerUpDown = (a as Xceed.Wpf.Toolkit.IntegerUpDown);
                        if (string.IsNullOrEmpty(integerUpDown.Text))
                        {
                            Marcar_control(integerUpDown, apuntar);
                            a.Focus();
                            return false;
                        }
                        break;

                    case "DecimalUpDown":
                        decimalUpDown = (a as Xceed.Wpf.Toolkit.DecimalUpDown);
                        if (string.IsNullOrEmpty(decimalUpDown.Text))
                        {
                            Marcar_control(decimalUpDown, apuntar);
                            a.Focus();
                            return false;
                        }
                        break;

                    case "DateTimePicker":
                        dateTimePicker = (a as Xceed.Wpf.Toolkit.DateTimePicker);
                        if (string.IsNullOrEmpty(dateTimePicker.Text))
                        {
                            Marcar_control(dateTimePicker, apuntar);
                            a.Focus();
                            return false;
                        }
                        break;

                    case "AutoCompleteComboBox":
                        autoCompleteComboBox = (a as DotNetKit.Windows.Controls.AutoCompleteComboBox);
                        if (string.IsNullOrEmpty(autoCompleteComboBox.Text))
                        {
                            Marcar_control(autoCompleteComboBox, apuntar);
                            a.Focus();
                            return false;
                        }
                        break;
                }
            }
            return true;
        }

        /// <summary>
        /// Marca con gris si limpiar es false
        /// Regresa a su color original si limpiar es true
        /// </summary>
        /// <param name="control">Control que se va a marcar</param>
        /// <param name="limpiar">true=restaurar color original, false=marca con color gris</param>
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
                    case "IntegerUpDown":
                        (a as Xceed.Wpf.Toolkit.IntegerUpDown).Value = null;
                        (a as Xceed.Wpf.Toolkit.IntegerUpDown).ClearValue(Control.BackgroundProperty);
                        break;
                    case "DecimalUpDown":
                        (a as Xceed.Wpf.Toolkit.DecimalUpDown).Value = null;
                        (a as Xceed.Wpf.Toolkit.DecimalUpDown).ClearValue(Control.BackgroundProperty);
                        break;
                    case "DateTimePicker":
                        (a as Xceed.Wpf.Toolkit.DateTimePicker).Value = null;
                        (a as Xceed.Wpf.Toolkit.DateTimePicker).ClearValue(Control.BackgroundProperty);
                        break;
                    case "AutoCompleteComboBox":
                        (a as DotNetKit.Windows.Controls.AutoCompleteComboBox).Text = null;
                        (a as DotNetKit.Windows.Controls.AutoCompleteComboBox).ClearValue(Control.BackgroundProperty);
                        break;
                    case "CheckBox":
                        (a as CheckBox).IsChecked = false;
                        break;
                    case "RadioButton":
                        (a as RadioButton).IsChecked = false;
                        break;
                }
            }
        }

        public void vacio(object sender)
        {
            switch (sender.GetType().Name)
            {
                case "TextBox":
                    if (String.IsNullOrEmpty((sender as TextBox).Text))
                    {
                        (sender as TextBox).Focus();
                    }
                    break;

                case "PasswordBox":
                    if(String.IsNullOrEmpty((sender as PasswordBox).Password))
                    {
                        (sender as PasswordBox).Focus();
                    }
                    break;
                case "ComboBox":
                    if((sender as ComboBox).SelectedItem == null)
                    {
                        (sender as ComboBox).Focus();
                    }
                    break;
                case "IntegerUpDown":
                    if (String.IsNullOrEmpty((sender as Xceed.Wpf.Toolkit.IntegerUpDown).Text))
                    {
                        (sender as Xceed.Wpf.Toolkit.IntegerUpDown).Focus();
                    }
                    break;
                case "DecimalUpDown":
                    if (String.IsNullOrEmpty((sender as Xceed.Wpf.Toolkit.DecimalUpDown).Text))
                    {
                        (sender as Xceed.Wpf.Toolkit.DecimalUpDown).Focus();
                    }
                    break;
                case "DateTimePicker":
                    if (String.IsNullOrEmpty((sender as Xceed.Wpf.Toolkit.DateTimePicker).Text))
                    {
                        (sender as Xceed.Wpf.Toolkit.DateTimePicker).Focus();
                    }
                    break;
                case "AutoCompleteComboBox":
                    if (String.IsNullOrEmpty((sender as DotNetKit.Windows.Controls.AutoCompleteComboBox).Text))
                    {
                        (sender as DotNetKit.Windows.Controls.AutoCompleteComboBox).Focus();
                    }
                    break;
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
