using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MySql.Data.MySqlClient;



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
        //public static RoutedCommand comandoTecla = new RoutedCommand();
        private bool _sePuedeEjecutar = true;   //Para ejecutar una sola vez el mensaje tipo METRO
        private bool _sePuedeEjecutar1 = true;  //para el boton de ingresar y el de atajos
        private bool boolcontrol = false;       //para que seleccioncontrol solo se ejecute una vez
        private readonly ToastViewModel _vm;
        private bool nuevo = false;
        
        public MainWindow()
        {
            ConectorDB.Initialize();
            InitializeComponent();
            Controles.Add(caja_texto_usuario);  //0 caja de texto usuario
            Controles.Add(caja_contrasena);     //1 caja de contraseña
            Controles[0].Focus();
            //Verificacion de conexion a servidor
            abrir();
            ConectorDB.CerrarConexion();
            _vm = new ToastViewModel();            
        }

        public async System.Threading.Tasks.Task verificar_bdAsync()
        {
            var res = await this.ShowMessageAsync("Error base de datos", "No se conecto servidor. Contactar administrador\n jafetrd@gmail.com" +
                "\nDatos registrados: \nServidor: " + ConectorDB.server + "\nBase de datos: " + ConectorDB.database, MessageDialogStyle.Affirmative);
            if (res == MessageDialogResult.Affirmative)
            {
                this.Close();
            }
        }

        public void abrir()
        {
            if (ConectorDB.ObtenerConexion() == false)
            {
                var task = verificar_bdAsync();
            }
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
           if (boolcontrol==false) botoningresar();
        }

        private void Boton_ingresar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter & boolcontrol==false) botoningresar();
        }

        //Boton comandos rapidos con Enter y con Click
        private async void Boton_comandos_ClickAsync(object sender, RoutedEventArgs e)
        {
            _sePuedeEjecutar1 = false;
            await botoncomandoAsync("Comandos", "Ctrl + I -> Iniciar sesión \nCtrl + N -> Nuevo usuario \nCtrl + H -> Ayuda");
        }

        private async void Boton_comandos_KeyDownAsync(object sender, KeyEventArgs e)
        {
            _sePuedeEjecutar1 = false;
            if(e.Key==Key.Enter) await botoncomandoAsync("Comandos", "Ctrl + I -> Iniciar sesión \nCtrl + N -> Nuevo usuario \nCtrl + H -> Ayuda");
        }

        //Boton nueva cuenta con Enter y con Click
        private void Boton_nueva_cuenta_Click(object sender, RoutedEventArgs e)
        {
            nuevo = true;
            if (boolcontrol == false) botoningresar();
        }

        private void Boton_nueva_cuenta_KeyDown(object sender, KeyEventArgs e)
        {
            nuevo = true;
            if (e.Key == Key.Enter & boolcontrol == false) botoningresar();
        }

        #endregion

        #region Metodos_de_operaciones
        //*******************METODOS DE OPERACIONES*********************************//

        private void botoningresar()
        {
            boolcontrol = Seleccionar_control();
            if (boolcontrol)
            {
                abrir();
                VerificarAsync();
                ConectorDB.CerrarConexion();
                caja_texto_usuario.Focus();
            }
            else
            {
                _sePuedeEjecutar = false;
                this.ShowMessageAsync("Inicio de sesión", "Campos en blanco");
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

        //Muestra mensaje cuando boton comandos
        private async System.Threading.Tasks.Task botoncomandoAsync(string a, string b)
        {
            var res = await this.ShowMessageAsync(a,b,MessageDialogStyle.Affirmative);
            if (res == MessageDialogResult.Affirmative)
            {
                _sePuedeEjecutar1 = true; //solo cuando se oprimi el mensaje se puede volver a mostrar
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

        private void limpiar()
        {
            caja_texto_usuario.Clear();
            caja_contrasena.Clear();
            _sePuedeEjecutar = true;   
            _sePuedeEjecutar1 = true;   
            nuevo = false;
            boolcontrol = Seleccionar_control();
        }
        #endregion

        #region base_datos
        private async void VerificarAsync()
        {
            /*Creamos la sentencia SQL que podra realizar la consulta que nesesitamos*/
          
            string SQL = String.Format(
                "SELECT * FROM tabla_usuario  where nombre_Usuario ='{0}' and contraseña_Usuario='{1}'",
                caja_texto_usuario.Text.Trim(), caja_contrasena.Password.Trim());
            MySqlDataReader reader = ConectorDB.Consultas(SQL);

            /*Comprobamos que el usuario exista*/
            bool local = comprobarID(reader);
            ConectorDB.CerrarConexion();
            reader.Dispose();
            if (local)
            {
                /*Si todo a salido bien se abrira el formulario principal y muestra notificacion*/
                if (nuevo == true)
                {
                    if (Usuario.TIPO_USUARIO == 1)
                    { //si es administrador
                        Navegacion.NavigarA(new Nuevo_usuario());
                    }
                    else
                    {
                        await botoncomandoAsync("Registro denegado", "No es administrador");
                    }
                }
                else
                {//inicio de sesion normal
                    _vm.ShowSuccess("Sesion iniciada");
                    Navegacion.NavigarA(new Ventana_principal());
                }
            }
            else
            {
                await botoncomandoAsync("Inicio de sesión", "Usuario no encontrado");
            }
            limpiar();
        }


        private bool comprobarID(MySqlDataReader reader)
        {
            while (reader.Read())
            {
                /*Si existe el usuario de forma correcta mandara un id_usuario mayor a 0*/
                Usuario.ID_USUARIO = reader.GetInt32(0);
                if (Usuario.ID_USUARIO > 0)
                {
                    /*Guardamos los datos del usuario en las propiedades de la clase USUARIO*/
                    Usuario.ID_USUARIO = reader.GetInt16(0);
                    Usuario.USUARIO = reader.GetString(1);
                    Usuario.CONTRASEÑA = reader.GetString(2);
                    Usuario.TIPO_USUARIO = reader.GetInt16(3);
                    return true;
                }
            }
            return false;
        }
        #endregion

    } //termina clase
}//termina namespace
