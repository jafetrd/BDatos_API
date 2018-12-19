using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace BDatos_API
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// Este código pertenece a la pantalla de Inicio de sesion ademas de tener metodos
    /// que seran utilizados en otras partes del programa 
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        List<Control> Controles = new List<Control>();
        public static RoutedCommand comandoTecla = new RoutedCommand();
        private bool _sePuedeEjecutar = true;
        private bool _sePuedeEjecutar1 = true;
        private bool boolcontrol = false;
        public MainWindow()
        {
            InitializeComponent();
            Controles.Add(caja_texto_usuario);  //0 caja de texto usuario
            Controles.Add(caja_contrasena);     //1 caja de contraseña
            Controles[0].Focus();
            comandoTecla.InputGestures.Add(new KeyGesture(Key.I, ModifierKeys.Control));
        }



        #region Botones y cajas de texto
        //*******************Botones y cajas de texto********************************//
        
        //Cajas de texto de usuario y contraseña con enfoque al siguiente elemento
        private void Caja_texto_usuario_KeyDown(object sender, KeyEventArgs e)
        {
            _sePuedeEjecutar = true;
            Marcar_control(Controles[0], true);
            if (e.Key == Key.Enter) Controles[1].Focus();
        }

        private void Caja_contrasena_KeyDown(object sender, KeyEventArgs e)
        {
            _sePuedeEjecutar = true;
            Marcar_control(Controles[1], true);
            if (e.Key == Key.Enter) boton_ingresar.Focus();
        }

        //Boton ingresar con Enter y con Click
        private void Boton_ingresar_Click(object sender, RoutedEventArgs e)
        {
           if(boolcontrol==false) botoningresar();
        }

        private void Boton_ingresar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter & boolcontrol==false) botoningresar();
        }

        //Boton comandos rapidos con Enter y con Click
        private async void Boton_comandos_ClickAsync(object sender, RoutedEventArgs e)
        {
            _sePuedeEjecutar1 = false;
            await botoncomandoAsync();
        }

        private async void Boton_comandos_KeyDownAsync(object sender, KeyEventArgs e)
        {
            _sePuedeEjecutar1 = false;
            if(e.Key==Key.Enter) await botoncomandoAsync();
        }

        //Boton nueva cuenta con Enter y con Click
        private void Boton_nueva_cuenta_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Boton_nueva_cuenta_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        #endregion

        #region Metodos_de_operaciones
        //*******************METODOS DE OPERACIONES*********************************//

        private void botoningresar()
        {
            boolcontrol = Seleccionar_control();
            if (boolcontrol)
            {
                new Ventana_principal().Show();
                this.Close();
            }
            else
            {
                _sePuedeEjecutar = false;
                Mensaje_asincrono("Inicio de sesión", "Datos incorrectos");
            }
        }

        //Recibe como parametro la forma a abrir y cierra la actual
       

        //Metodo para enfocar a un elemento que pertenezca a la clase Control
        //si el campo esta vacio retorna falso y no permite las ejecucion siguiente
        private bool Seleccionar_control()
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
                    default: return true;
                }
            }
            return true;
        }
        
        //Metodo para marcar las cajas vacias 
        private void Marcar_control(Control control,Boolean limpiar)
        {
            if (limpiar)
                control.Background = Brushes.White;
            else
                control.Background = Brushes.LightGray;
        }

        //Se muestra un mensaje al estilo METRO 
        private void Mensaje_asincrono(string titulo, string mensaje)
        {
            this.ShowMessageAsync(titulo, mensaje);
        }

        //Muestra mensaje cuando boton comandos
        private async System.Threading.Tasks.Task botoncomandoAsync()
        {
            var res = await this.ShowMessageAsync("Comandos", "Ctrl + I -> Iniciar sesión \nCtrl + N -> Nuevo usuario \nCtrl + H -> Ayuda",MessageDialogStyle.Affirmative);
            if (res == MessageDialogResult.Affirmative)
            {
                _sePuedeEjecutar1 = true;
            }
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _sePuedeEjecutar;
            e.Handled = true;
        }

        private void CommandBinding_CanExecute_1(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _sePuedeEjecutar1;
            e.Handled = true;
        }
    }
    #endregion
}
