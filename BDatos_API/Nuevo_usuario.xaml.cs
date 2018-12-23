using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
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
    /// Lógica de interacción para Verificacion.xaml
    /// </summary>
    public partial class Nuevo_usuario : MetroWindow
    {
        private bool boolcontrol=false;
        private bool contrasena_igual = false;
        Metodos_comunes Controles;
        Metodos_bd metodos_bd;
        public int ESTADO = 0;
  
        public Nuevo_usuario()
        {
            InitializeComponent();
            /*se inician constructores*/
            Controles = new Metodos_comunes();
            metodos_bd = new Metodos_bd();
            /*limpiar toda la forma y variables*/
            limpiar();
            /*se mandan los controles visuales a una lista para facilitar su manejo*/
            Controles.campos.Add(caja_texto_usuario);
            Controles.campos.Add(caja_contrasena);
            Controles.campos.Add(caja_contrasena1);
            Controles.campos.Add(Combobox_tipo);
            /*se apunta al primer control de la forma*/
            Controles.Inicial.Focus();
            /*Se carga la tabla en la cuadricula DataGrid*/
            cargar_datagrid();
        }

        private async void maquina_estados()
        {
            switch (ESTADO)
            {

                case TablaUsuario.CREAR:
                    /*se verifica que todos los campos esten llenos*/
                    if (Controles.Seleccionar_control(false))
                    {   /*Se verifica que no exista un usuario con el mismo nombre*/
                        ArrayList resultado = metodos_bd.obtener_por_criterio(TablaUsuario.TABLA_USUARIO, TablaUsuario.NOMBRE, TablaUsuario.USUARIOdato,4,TablaUsuario.TODO);
                        if (resultado.Count > 0) /*si hay mas de un resultado entonces si existe el usuario*/
                        {
                            await this.ShowMessageAsync("Administrador de usuarios", "Nombre no disponible", MessageDialogStyle.Affirmative);
                        }
                        else /*sino hay resultado entonces podemos guardar al nuevo usuario*/
                        {
                            var a = await this.ShowMessageAsync("Administrador de usuarios", "¿Crear nuevo usuario?", MessageDialogStyle.AffirmativeAndNegative);
                            if (a == MessageDialogResult.Affirmative)/*se confirma si o no se quiere guardar*/
                            {
                                metodos_bd.guardarenSQL(TablaUsuario.TABLA_USUARIO, (TablaUsuario.NOMBRE, TablaUsuario.USUARIOdato), (TablaUsuario.CONTRASEÑA, TablaUsuario.CONTRASEÑAdato), (TablaUsuario.TIPO_USUARIO, TablaUsuario.TIPO_USUARIOdato));
                                await this.ShowMessageAsync("Administrador de usuarios", "Usuario creado", MessageDialogStyle.Affirmative);
                                limpiar();
                                cargar_datagrid();
                            }
                        }
                    }
                    else
                    {
                        await this.ShowMessageAsync("Administrador de usuarios", "Información incompleta", MessageDialogStyle.Affirmative);
                    }
                    break;

                case TablaUsuario.APUNTAR:
                    /*se busca el usaurio con el Id que se selecciono al hacer clic sobre el DataGrid*/
                    ArrayList resultado2 = metodos_bd.obtener_por_criterio(TablaUsuario.TABLA_USUARIO, TablaUsuario.ID_USUARIO, TablaUsuario.ID_USUARIOcopia, 4, TablaUsuario.TODO);
                    /*los datos se respaldan en unas variables auxiliares y se muestran en los componentes correspondientes*/
                    TablaUsuario.ID_USUARIOcopia = resultado2[0].ToString();
                    caja_texto_usuario.Text = TablaUsuario.USUARIOcopia = resultado2[1].ToString();
                    caja_contrasena.Password = caja_contrasena1.Password = TablaUsuario.CONTRASEÑAcopia = resultado2[2].ToString();
                    Combobox_tipo.Text = TablaUsuario.TIPO_USUARIOcopia = resultado2[3].ToString();

                    boton_borrar.Visibility = Visibility.Visible;
                    boton_guardar.Content = TablaUsuario.BTN_EDITAR;

                    ESTADO = TablaUsuario.ACTUALIZAR;
                    break;

                case TablaUsuario.ACTUALIZAR:
                    /*se verifica que todos los campos esten llenos*/
                    if (Controles.Seleccionar_control(false))
                    {
                        TablaUsuario.USUARIOdato = caja_texto_usuario.Text;
                        TablaUsuario.CONTRASEÑAdato = caja_contrasena.Password;
                        TablaUsuario.TIPO_USUARIOdato = Combobox_tipo.Text;
                        ArrayList resultado3 = metodos_bd.obtener_por_criterio(TablaUsuario.TABLA_USUARIO, TablaUsuario.NOMBRE, TablaUsuario.USUARIOdato, 4, TablaUsuario.TODO);
                        if (resultado3.Count > 0)
                        {
                            if (resultado3[0].ToString() == TablaUsuario.ID_USUARIOcopia)
                            {/*si resultado3 me regreso a mi mismo puedo modificar el dato*/
                                var a2 =await this.ShowMessageAsync("Administrador de usuarios", "¿Guardar cambios?", MessageDialogStyle.AffirmativeAndNegative);
                                if (a2 == MessageDialogResult.Affirmative) { editar(); }
                            }
                            else
                            {/*sino esto quiere decir que es otro usuario y por lo tanto no puedo usar ese nombre*/
                                await this.ShowMessageAsync("Administrador de usuarios", "Nombre no disponible", MessageDialogStyle.Affirmative);
                            }
                        }
                        else
                        {/*si no se regreso ningun dato de la busque significa que el dato esta disponilbe*/
                            var a2 = await this.ShowMessageAsync("Administrador de usuarios", "¿Guardar cambios?", MessageDialogStyle.AffirmativeAndNegative);
                            if (a2 == MessageDialogResult.Affirmative) { editar(); }
                        }
                    }
                        break;

                case TablaUsuario.SELECCIONAR:

                    break;
            }
        }

        #region datagrid
        private void cargar_datagrid()
        {
        metodos_bd.popular_tabla(tabla_Principal, TablaUsuario.TABLA_USUARIO, "cargarUsuarios",TablaUsuario.ID_USUARIO,TablaUsuario.NOMBRE,TablaUsuario.TIPO_USUARIO);  
        }

        private async void editar()
        {
            metodos_bd.actualizar(TablaUsuario.TABLA_USUARIO, TablaUsuario.ID_USUARIO,TablaUsuario.ID_USUARIOcopia,
                                      (TablaUsuario.NOMBRE, TablaUsuario.USUARIOdato),
                                      (TablaUsuario.CONTRASEÑA, TablaUsuario.CONTRASEÑAdato),
                                      (TablaUsuario.TIPO_USUARIO, TablaUsuario.TIPO_USUARIOdato));
            await this.ShowMessageAsync("Administrador de usuarios", "Usuario actualizado", MessageDialogStyle.AffirmativeAndNegative);
            cargar_datagrid();
            limpiar();
            ESTADO = TablaUsuario.CREAR;
        }

        private void DataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (tabla_Principal.SelectedItems.Count == 1)
            {
                limpiar();
                try
                {
                    DataRowView row = (DataRowView)tabla_Principal.SelectedItems[0];
                    TablaUsuario.ID_USUARIOcopia = (row[0]).ToString();
                }
                catch (Exception){}
                ESTADO = TablaUsuario.APUNTAR;
                maquina_estados();
            }
        }
        #endregion

        #region eventos_controles

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Navegacion.NavegarAtras();
            Controles.Limpiar_lista();
        }

        private void Caja_texto_usuario_KeyDown(object sender, KeyEventArgs e)
        {
            Controles.Marcar_control(caja_texto_usuario, true);
            if (e.Key == Key.Enter) caja_contrasena.Focus();
        }

        private void Caja_texto_usuario_TextChanged(object sender, TextChangedEventArgs e)
        {
            TablaUsuario.USUARIOdato = caja_texto_usuario.Text.Trim();
        }

        private void Caja_contrasena_KeyDown(object sender, KeyEventArgs e)
        {
            Controles.Marcar_control(caja_contrasena, true);
            if (e.Key == Key.Enter) caja_contrasena1.Focus();
        }

        private void Caja_contrasena1_KeyDown(object sender, KeyEventArgs e)
        {
            Controles.Marcar_control(caja_contrasena1, true);
            if (e.Key == Key.Enter) Combobox_tipo.Focus();
        }

        private void Caja_contrasena_PasswordChanged(object sender, RoutedEventArgs e)
        {
            verificar_contrasena();
        }

        private void Caja_contrasena1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            verificar_contrasena();
        }

        void verificar_contrasena()
        {
            if (caja_contrasena.Password != caja_contrasena1.Password)
            {
                caja_contrasena1.Background = Brushes.LightYellow;
                caja_contrasena.Background = Brushes.LightYellow;
                contrasena_igual = false;
            }
            else
            {
                TablaUsuario.CONTRASEÑAdato = caja_contrasena.Password.Trim();
                caja_contrasena1.Background = Brushes.LightGreen;
                caja_contrasena.Background = Brushes.LightGreen;
                contrasena_igual = true;
            }
        }

        private void Combobox_tipo_KeyDown(object sender, KeyEventArgs e)
        {
            Controles.Marcar_control(Combobox_tipo, true);
            if (Combobox_tipo.IsDropDownOpen==true)
            {
                if (Combobox_tipo.SelectedItem != null)
                {
                    if (e.Key == Key.Enter)
                    {
                        TablaUsuario.TIPO_USUARIOdato = Combobox_tipo.Text;
                        boton_guardar.Focus();
                    }
                }
            }
            else
            {
                Combobox_tipo.IsDropDownOpen = true;
            }
        }

        private void Combobox_tipo_DropDownClosed(object sender, EventArgs e)
        {
            Controles.Marcar_control(Combobox_tipo, true);
            if (Combobox_tipo.SelectedItem != null)
            {
                TablaUsuario.TIPO_USUARIOdato = Combobox_tipo.Text;
                boton_guardar.Focus();
            }
            else
            {
                Combobox_tipo.Focus();
            }
        }

        private void Boton_guardar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter & boolcontrol == false) maquina_estados();
           
        }

        private void Boton_guardar_Click(object sender, RoutedEventArgs e)
        {
            if (boolcontrol == false) maquina_estados();
        }

        private void Boton_borrar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Boton_borrar_KeyDown(object sender, KeyEventArgs e)
        {

        }
        #endregion
        private void limpiar()
        {
            ESTADO = TablaUsuario.CREAR;
            contrasena_igual = false;
            boton_borrar.Visibility = Visibility.Hidden;
            boton_guardar.Content = TablaUsuario.BTN_GUARDAR;
            boolcontrol = false;
            Controles.Limpiar_controles();
        }
    }
}
