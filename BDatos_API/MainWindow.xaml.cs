using System;
using System.Collections;
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
      
        private readonly ToastViewModel _vm;
        Metodos_comunes Controles;
        Metodos_bd metodos_bd;
        public int ESTADO = 0;

        public MainWindow()
        {
            ConectorDB.Initialize();
            InitializeComponent();
            Controles = new Metodos_comunes();
            metodos_bd = new Metodos_bd();

            Controles.campos.Add(caja_texto_usuario);  
            Controles.campos.Add(caja_contrasena);
            Controles.Inicial.Focus();
            Controles.Limpiar_controles();
            //Verificacion de conexion a servidor
            _vm = new ToastViewModel();
        }


        private  void maquina_estados()
        {
            switch (ESTADO)
            {
                case InicioSesion.ABRIR:
                    metodo_abrir(InicioSesion.ABRIR);
                    break;
                case InicioSesion.CUENTA_NUEVA:
                    metodo_abrir(InicioSesion.CUENTA_NUEVA);
                    break;
            }
        }

         private async void metodo_abrir(int seleccionar)
        {
            if (Controles.Seleccionar_control(false))
            {
                ArrayList resultado = metodos_bd.BUSCAR(InicioSesion.TABLA_USUARIO, InicioSesion.NOMBRE, InicioSesion.USUARIOdato, 4, InicioSesion.TODO);
                if (resultado.Count > 0) /*si hay mas de un resultado entonces si existe el usuario*/
                {
                    _vm.ShowInformation("Bienvenida(o): " +resultado[1].ToString());
                    if (seleccionar == InicioSesion.ABRIR) { Navegacion.NavigarA(new Ventana_principal()); }
                    if (seleccionar == InicioSesion.CUENTA_NUEVA)
                    {
                        if (resultado[3].ToString() != InicioSesion.TIPO_ADMINISTRADOR)
                        {
                            await this.ShowMessageAsync(InicioSesion.TITULO_MENSAJE, "No es administrador", MessageDialogStyle.Affirmative);
                        }
                        else
                        {
                            Navegacion.NavigarA(new Nuevo_usuario());
                            _vm.ShowInformation("Ajustes de administrador");
                        }
                    }
                }
                else
                {
                    await this.ShowMessageAsync(InicioSesion.TITULO_MENSAJE, "Datos incorrectos", MessageDialogStyle.Affirmative);
                    limpiar();
                }
            }
            else
            {
                await this.ShowMessageAsync(InicioSesion.TITULO_MENSAJE, "Información incompleta", MessageDialogStyle.Affirmative);
            }
        }

        #region Botones y cajas de texto
        //*******************Botones y cajas de texto********************************//

        //Cajas de texto de usuario y contraseña con enfoque al siguiente elemento
        private void Caja_texto_usuario_KeyDown(object sender, KeyEventArgs e)
        {
            InicioSesion.USUARIOdato = caja_texto_usuario.Text;
            Controles.Marcar_control(caja_texto_usuario, true);
            if (e.Key == Key.Enter) caja_contrasena.Focus();
        }

        private void Caja_contrasena_KeyDown(object sender, KeyEventArgs e)
        {
            InicioSesion.CONTRASEÑAdato = caja_contrasena.Password;
            Controles.Marcar_control(caja_contrasena,true);
            if (e.Key == Key.Enter) boton_ingresar.Focus();
        }

        private void Caja_texto_usuario_TextChanged(object sender, TextChangedEventArgs e)
        {
            InicioSesion.USUARIOdato = caja_texto_usuario.Text;
        }

        private void Caja_contrasena_PasswordChanged(object sender, RoutedEventArgs e)
        {
            InicioSesion.CONTRASEÑAdato = caja_contrasena.Password;
        }
        //Boton ingresar con Enter y con Click
        private void Boton_ingresar_Click(object sender, RoutedEventArgs e)
        {
            ESTADO = InicioSesion.ABRIR;
            maquina_estados();
        }

        private void Boton_ingresar_KeyDown(object sender, KeyEventArgs e)
        {
            ESTADO = InicioSesion.ABRIR;
            if (e.Key == Key.Enter) maquina_estados();
        }

        //Boton nueva cuenta con Enter y con Click
        private void Boton_nueva_cuenta_Click(object sender, RoutedEventArgs e)
        {
            ESTADO = InicioSesion.CUENTA_NUEVA;
            maquina_estados();
        }

        private void Boton_nueva_cuenta_KeyDown(object sender, KeyEventArgs e)
        {
            ESTADO = InicioSesion.CUENTA_NUEVA;
            if (e.Key == Key.Enter) { maquina_estados(); }
        }


        private void limpiar()
        {
            Controles.Limpiar_controles();
            Controles.Inicial.Focus();
            ESTADO = InicioSesion.ABRIR;
        }

        #endregion

        private void MetroWindow_Closed(object sender, EventArgs e)
        {
            this.Close();
        }
    } //termina clase
}//termina namespace
