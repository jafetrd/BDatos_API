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
        private bool boolcontrol=true;
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
                        if (verificar_contrasena())
                        {
                            ArrayList resultado = metodos_bd.obtener_por_criterio(TablaUsuario.TABLA_USUARIO, TablaUsuario.NOMBRE, TablaUsuario.USUARIOdato, 4, TablaUsuario.TODO);
                            if (resultado.Count > 0) /*si hay mas de un resultado entonces si existe el usuario*/
                            {
                                await this.ShowMessageAsync(TablaUsuario.TITULO_MENSAJE, "Nombre no disponible", MessageDialogStyle.Affirmative);
                            }
                            else /*sino hay resultado entonces podemos guardar al nuevo usuario*/
                            {
                                var a = await this.ShowMessageAsync(TablaUsuario.TITULO_MENSAJE, "¿Crear nuevo usuario?", MessageDialogStyle.AffirmativeAndNegative);
                                if (a == MessageDialogResult.Affirmative)/*se confirma si o no se quiere guardar*/
                                {
                                    metodos_bd.guardarenSQL(TablaUsuario.TABLA_USUARIO, (TablaUsuario.NOMBRE, TablaUsuario.USUARIOdato), (TablaUsuario.CONTRASEÑA, TablaUsuario.CONTRASEÑAdato), (TablaUsuario.TIPO_USUARIO, TablaUsuario.TIPO_USUARIOdato));
                                    await this.ShowMessageAsync(TablaUsuario.TITULO_MENSAJE, "Usuario creado", MessageDialogStyle.Affirmative);
                                    limpiar();
                                    cargar_datagrid();
                                }
                            }
                        }
                        else
                        {
                            await this.ShowMessageAsync(TablaUsuario.TITULO_MENSAJE, "Las contraseñas no coinciden", MessageDialogStyle.Affirmative);
                        }
                    }
                    else
                    {
                        await this.ShowMessageAsync(TablaUsuario.TITULO_MENSAJE, "Información incompleta", MessageDialogStyle.Affirmative);
                    }

                    ESTADO = TablaUsuario.CREAR;
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
                        if (verificar_contrasena())
                        {
                            TablaUsuario.USUARIOdato = caja_texto_usuario.Text;
                            TablaUsuario.CONTRASEÑAdato = caja_contrasena.Password;
                            TablaUsuario.TIPO_USUARIOdato = Combobox_tipo.Text;
                            ArrayList resultado3 = metodos_bd.obtener_por_criterio(TablaUsuario.TABLA_USUARIO, TablaUsuario.NOMBRE, TablaUsuario.USUARIOdato, 4, TablaUsuario.TODO);
                            if (resultado3.Count > 0)
                            {
                                if (resultado3[0].ToString() == TablaUsuario.ID_USUARIOcopia)
                                {/*si resultado3 me regreso a mi mismo puedo modificar el dato*/
                                    var a2 = await this.ShowMessageAsync(TablaUsuario.TITULO_MENSAJE, "¿Guardar cambios?", MessageDialogStyle.AffirmativeAndNegative);
                                    if (a2 == MessageDialogResult.Affirmative) { editar(); }
                                }
                                else
                                {/*sino esto quiere decir que es otro usuario y por lo tanto no puedo usar ese nombre*/
                                    await this.ShowMessageAsync(TablaUsuario.TITULO_MENSAJE, "Nombre no disponible", MessageDialogStyle.Affirmative);
                                }
                            }
                            else
                            {/*si no se regreso ningun dato de la busque significa que el dato esta disponilbe*/
                                var a2 = await this.ShowMessageAsync(TablaUsuario.TITULO_MENSAJE, "¿Guardar cambios?", MessageDialogStyle.AffirmativeAndNegative);
                                if (a2 == MessageDialogResult.Affirmative) { editar(); }
                            }
                        }
                        else
                        {
                            await this.ShowMessageAsync(TablaUsuario.TITULO_MENSAJE, "Las contraseñas no coinciden", MessageDialogStyle.Affirmative);
                        }
                    }
                    ESTADO = TablaUsuario.CREAR;
                    break;

                case TablaUsuario.BORRAR:
                    var mySettings = new MetroDialogSettings
                    {
                        AffirmativeButtonText = "Yes",
                        ColorScheme = MetroDialogColorScheme.Inverted
                    };
                    var a3 = await this.ShowMessageAsync(TablaUsuario.TITULO_MENSAJE, "¿Eliminar usuario?", MessageDialogStyle.AffirmativeAndNegative, mySettings);
                    if (a3 == MessageDialogResult.Affirmative)
                    {
                        metodos_bd.eliminar(TablaUsuario.TABLA_USUARIO, TablaUsuario.ID_USUARIO, TablaUsuario.ID_USUARIOcopia);
                        limpiar();
                        cargar_datagrid();
                    }
                    ESTADO = TablaUsuario.CREAR;
                    break;

                case TablaUsuario.ULTIMO_USUARIO:
                    ArrayList resultado4 = metodos_bd.obtener_por_criterio(TablaUsuario.TABLA_USUARIO, TablaUsuario.ID_USUARIO, TablaUsuario.ID_USUARIOcopia, 4, TablaUsuario.TODO);
                    /*los datos se respaldan en unas variables auxiliares y se muestran en los componentes correspondientes*/
                    TablaUsuario.ID_USUARIOcopia = resultado4[0].ToString();
                    caja_texto_usuario.Text = TablaUsuario.USUARIOcopia = resultado4[1].ToString();
                    caja_contrasena.Password = caja_contrasena1.Password = TablaUsuario.CONTRASEÑAcopia= resultado4[2].ToString();
                    Combobox_tipo.Text = TablaUsuario.TIPO_USUARIOcopia = resultado4[3].ToString();
                    boton_borrar.Visibility = Visibility.Hidden;

                    if (TablaUsuario.TIPO_USUARIOcopia != TablaUsuario.TIPO_ADMINISTRADOR)
                    {
                        await this.ShowMessageAsync(TablaUsuario.TITULO_MENSAJE, "El usuario se cambiara a administrador", MessageDialogStyle.Affirmative);
                        TablaUsuario.TIPO_USUARIOdato = TablaUsuario.TIPO_ADMINISTRADOR;
                        editar();
                    }
                    else
                    {
                        await this.ShowMessageAsync(TablaUsuario.TITULO_MENSAJE, "Ultimo usuario", MessageDialogStyle.Affirmative);
                    }
                  
                    break;
            }
        }

        #region datagrid
        private void cargar_datagrid()
        {
            metodos_bd.popular_tabla(tabla_Principal, TablaUsuario.TABLA_USUARIO, "cargarUsuarios", TablaUsuario.ID_USUARIO, TablaUsuario.NOMBRE, TablaUsuario.TIPO_USUARIO);
            
            DataGridCell_MouseDoubleClick(null, null);
        }

        private async void editar()
        {
            metodos_bd.actualizar(TablaUsuario.TABLA_USUARIO, TablaUsuario.ID_USUARIO,TablaUsuario.ID_USUARIOcopia,
                                      (TablaUsuario.NOMBRE, TablaUsuario.USUARIOdato),
                                      (TablaUsuario.CONTRASEÑA, TablaUsuario.CONTRASEÑAdato),
                                      (TablaUsuario.TIPO_USUARIO, TablaUsuario.TIPO_USUARIOdato));
            await this.ShowMessageAsync(TablaUsuario.TITULO_MENSAJE, "Usuario actualizado", MessageDialogStyle.AffirmativeAndNegative);
            cargar_datagrid();
            
            limpiar();
            ESTADO = TablaUsuario.CREAR;
        }

        private void DataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (tabla_Principal.Items.Count == 1)
            {
                ESTADO = TablaUsuario.ULTIMO_USUARIO;
                DataRowView fila = (DataRowView)tabla_Principal.Items[0];
                TablaUsuario.ID_USUARIOcopia = (fila[0]).ToString();
                maquina_estados();
            }
            else
            {
                if (tabla_Principal.SelectedItems.Count == 1)
                {
                    limpiar();
                    try
                    {
                        DataRowView row = (DataRowView)tabla_Principal.SelectedItems[0];
                        TablaUsuario.ID_USUARIOcopia = (row[0]).ToString();
                    }
                    catch (Exception) { }

                    ESTADO = TablaUsuario.APUNTAR;
                    maquina_estados();
                }
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

        private bool verificar_contrasena()
        {
            if (caja_contrasena.Password != caja_contrasena1.Password)
            {
                caja_contrasena1.Background = Brushes.LightYellow;
                caja_contrasena.Background = Brushes.LightYellow;
                return false;
            }
            else
            {
                TablaUsuario.CONTRASEÑAdato = caja_contrasena.Password.Trim();
                caja_contrasena1.Background = Brushes.LightGreen;
                caja_contrasena.Background = Brushes.LightGreen;
                return true;
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
            ESTADO = TablaUsuario.BORRAR;
            maquina_estados();
        }

        private void Boton_borrar_KeyDown(object sender, KeyEventArgs e)
        {
            ESTADO = TablaUsuario.BORRAR;
            maquina_estados();
        }
        #endregion
        private void limpiar()
        {
            ESTADO = TablaUsuario.CREAR;
            boton_borrar.Visibility = Visibility.Hidden;
            boton_guardar.Content = TablaUsuario.BTN_GUARDAR;
            boolcontrol = false;
            Controles.Limpiar_controles();
        }
    }
}
