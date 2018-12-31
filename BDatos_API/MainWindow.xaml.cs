using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro.Controls;
using static BDatos_API.InicioSesion;
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
        Ventana_principal ventana_Principal = null;
        Nuevo_usuario nuevo_Usuario = null;
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
            ArrayList resultado2 = metodos_bd.BUSCAR(NOMBRE_TABLA, NOMBRE, TIPO_ADMINISTRADOR, CANTIDAD_COLUMNAS, TODO);
            if (resultado2.Count == 0)
            {
                Navegacion.NavegarA(new auxiliar());
                //Navegacion.Navigate("Nuevo_usuario.xaml");
                _vm.ShowInformation("Creando cuenta administrador");
            }

        }


        private  void maquina_estados()
        {
            switch (ESTADO)
            {
                case ABRIR:
                    metodo_abrir(ABRIR);
                    break;
                case CUENTA_NUEVA:
                    metodo_abrir(CUENTA_NUEVA);
                    break;
            }
        }

         private async void metodo_abrir(int seleccionar)
        {
            if (Controles.Seleccionar_control(false))
            {
                ArrayList resultado = metodos_bd.BUSCAR(NOMBRE_TABLA, NOMBRE, USUARIOdato, CANTIDAD_COLUMNAS, TODO);
                if (resultado.Count > 0) /*si hay mas de un resultado entonces si existe el usuario*/
                {
                    if (resultado[2].ToString() == caja_contrasena.Password)
                    {
                        _vm.ShowInformation("Bienvenida(o): " + resultado[1].ToString());
                        if (seleccionar == ABRIR) {
                            if (ventana_Principal == null) ventana_Principal = new Ventana_principal();
                                Navegacion.NavegarA(ventana_Principal);
                           // Navegacion.Navigate(new Uri("Ventana_principal.xaml",UriKind.RelativeOrAbsolute));
                        }
                        if (seleccionar == CUENTA_NUEVA)
                        {
                            if (resultado[3].ToString() != TIPO_ADMINISTRADOR)
                            {
                                await this.ShowMessageAsync(TITULO_MENSAJE, "No es administrador", MessageDialogStyle.Affirmative);
                            }
                            else
                            {
                                if (nuevo_Usuario == null) nuevo_Usuario = new Nuevo_usuario();
                                Navegacion.NavegarA(nuevo_Usuario);
                                //Navegacion.Navigate(new Uri("Nuevo_usuario.xaml",UriKind.RelativeOrAbsolute));
                                _vm.ShowInformation("Ajustes de administrador");
                            }
                        }
                    }
                    else
                    {
                        await this.ShowMessageAsync(TITULO_MENSAJE, "Contraseña incorrecta", MessageDialogStyle.Affirmative);
                        limpiar();
                    }
                }
                else
                {
                    ArrayList resultado2 = metodos_bd.BUSCAR(NOMBRE_TABLA, NOMBRE, TIPO_ADMINISTRADOR, CANTIDAD_COLUMNAS, TODO);
                    if (resultado2.Count > 0)
                    {
                        await this.ShowMessageAsync(TITULO_MENSAJE, "Datos incorrectos", MessageDialogStyle.Affirmative);
                        limpiar();
                    }
                    else
                    {
                        Navegacion.NavegarA(new auxiliar());
                        //Navegacion.Navigate("auxiliar.xaml");
                        _vm.ShowInformation("Creando cuenta administrador");
                    }
                }
            }
            else
            {
                await this.ShowMessageAsync(TITULO_MENSAJE, "Información incompleta", MessageDialogStyle.Affirmative);
            }
        }

        #region Botones y cajas de texto
        //*******************Botones y cajas de texto********************************//

        //Cajas de texto de usuario y contraseña con enfoque al siguiente elemento
        private void Caja_texto_usuario_KeyDown(object sender, KeyEventArgs e)
        {
            USUARIOdato = caja_texto_usuario.Text;
            Controles.Marcar_control(caja_texto_usuario, true);
            if (e.Key == Key.Enter) caja_contrasena.Focus();
        }

        private void Caja_contrasena_KeyDown(object sender, KeyEventArgs e)
        {
            CONTRASEÑAdato = caja_contrasena.Password;
            Controles.Marcar_control(caja_contrasena,true);
            if (e.Key == Key.Enter) boton_ingresar.Focus();
        }

        private void Caja_texto_usuario_TextChanged(object sender, TextChangedEventArgs e)
        {
            USUARIOdato = caja_texto_usuario.Text;
        }

        private void Caja_contrasena_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CONTRASEÑAdato = caja_contrasena.Password;
        }
        //Boton ingresar con Enter y con Click
        private void Boton_ingresar_Click(object sender, RoutedEventArgs e)
        {
            ESTADO = ABRIR;
            maquina_estados();
        }

        private void Boton_ingresar_KeyDown(object sender, KeyEventArgs e)
        {
            ESTADO = ABRIR;
            if (e.Key == Key.Enter) maquina_estados();
        }

        //Boton nueva cuenta con Enter y con Click
        private void Boton_nueva_cuenta_Click(object sender, RoutedEventArgs e)
        {
            ESTADO = CUENTA_NUEVA;
            maquina_estados();
        }

        private void Boton_nueva_cuenta_KeyDown(object sender, KeyEventArgs e)
        {
            ESTADO = CUENTA_NUEVA;
            if (e.Key == Key.Enter) { maquina_estados(); }
        }


        private void limpiar()
        {
            Controles.Limpiar_controles();
            Controles.Inicial.Focus();
            ESTADO = ABRIR;
        }

        #endregion

        private void MetroWindow_Closed(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    } //termina clase
}//termina namespace
