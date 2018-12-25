using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using static BDatos_API.TUsuario;

namespace BDatos_API
{
    /// <summary>
    /// Lógica de interacción para Verificacion.xaml
    /// </summary>
    public partial class Nuevo_usuario : MetroWindow
    {
        Metodos_comunes Controles;
        Metodos_bd metodos_bd;
        public int ESTADO = 0;
        public int INDICADOR = 0;
        int CANTIDAD = 0;
        bool CREAR_TEMPORAL=false;
        public Nuevo_usuario()
        {
            InitializeComponent();
            /*se inician constructores*/
            Controles = new Metodos_comunes();
            metodos_bd = new Metodos_bd();
            /*limpiar toda la forma y variables*/
            LIMPIAR_TODO();
            /*se mandan los controles visuales a una lista para facilitar su manejo*/
            Controles.campos.Add(caja_texto_usuario);
            Controles.campos.Add(caja_contrasena);
            Controles.campos.Add(caja_contrasena1);
            Controles.campos.Add(Combobox_tipo);
            /*se apunta al primer control de la forma*/
            Controles.Inicial.Focus();
            /*Se carga la tabla en la cuadricula DataGrid*/
            CANTIDAD = metodos_bd.LLENAR_DATAGRID(tabla_Principal, NOMBRE_TABLA, RUTA_ENLACE_DATAGRID, ID_USUARIO, NOMBRE, TIPO_USUARIO);
            ACTUALIZAR_TABLA_DATAGRID();
        }

        private async void maquina_estados()
        {
            switch (ESTADO)
            {

                case GUARDAR:
                    /*¿Todos los campos estan llenos?*/
                    if (Controles.Seleccionar_control(false))
                    {   /*Ambas contraseñas coinciden*/
                        if (verificar_contrasena())
                        {
                            if (caja_contrasena.Password.Length < 4)
                            {
                                await this.ShowMessageAsync(TITULO_MENSAJE, "Minimo 4 caracteres", MessageDialogStyle.Affirmative);
                                return;
                            }
                            else
                            {
                                /*Se busca el nombre del usuario ingresado*/
                                ArrayList resultado = metodos_bd.BUSCAR(NOMBRE_TABLA, NOMBRE, NOMBRE_D, CANTIDAD_COLUMNAS, TODO);
                                if (resultado.Count > 0) /*¿ya existe ese usuario?*/
                                {   /*Mensaje para cambiar nombre*/
                                    await this.ShowMessageAsync(TITULO_MENSAJE, "Nombre no disponible", MessageDialogStyle.Affirmative);
                                    return;
                                }
                                else
                                {   /*¿Crear nuevo usuari?*/
                                    var a = await this.ShowMessageAsync(TITULO_MENSAJE, "¿Crear nuevo usuario?", MessageDialogStyle.AffirmativeAndNegative);
                                    if (a == MessageDialogResult.Affirmative)/*Se guarda el nuevo usuario*/
                                    {
                                        metodos_bd.GUARDAR(NOMBRE_TABLA, (NOMBRE, NOMBRE_D), (CONTRASEÑA, CONTRASEÑA_D), (TIPO_USUARIO, TIPO_USUARIO_D));
                                        await this.ShowMessageAsync(TITULO_MENSAJE, "Usuario creado", MessageDialogStyle.Affirmative);
                                        LIMPIAR_TODO();
                                        ACTUALIZAR_TABLA_DATAGRID();
                                    }
                                    else
                                    {
                                        return;
                                    }
                                    
                                }
                            }
                        }
                        else
                        {
                            await this.ShowMessageAsync(TITULO_MENSAJE, "Las contraseñas no coinciden", MessageDialogStyle.Affirmative);
                        }
                    }
                    ESTADO = GUARDAR;
                    break;

                case APUNTAR:
                    /*se busca el usaurio con el Id que se selecciono al hacer clic sobre el DataGrid*/
                    if (INDICADOR != ULTIMO_USUARIO)
                    {
                        ArrayList resultado2 = metodos_bd.BUSCAR(NOMBRE_TABLA, ID_USUARIO, ID_USUARIO_C, CANTIDAD_COLUMNAS, TODO);
                        /*los datos se respaldan en unas variables auxiliares y se muestran en los componentes correspondientes*/
                        ID_USUARIO_C = resultado2[0].ToString();
                        caja_texto_usuario.Text = NOMBRE_C = resultado2[1].ToString();
                        caja_contrasena.Password = caja_contrasena1.Password = CONTRASEÑA_C = resultado2[2].ToString();
                        Combobox_tipo.Text = TIPO_USUARIO_C = resultado2[3].ToString();

                        boton_guardar.Content = BTN_EDITAR;
                        ESTADO = ACTUALIZAR;
                    }
                    else
                    {
                        ESTADO = ULTIMO_USUARIO;
                        maquina_estados();
                    }
                    break;


                case ACTUALIZAR:
                    /*se verifica que todos los campos esten llenos*/
                    if (Controles.Seleccionar_control(false))
                    {
                        if (verificar_contrasena())
                        {
                            if (caja_contrasena.Password.Length < 4)
                            {
                                await this.ShowMessageAsync(TITULO_MENSAJE, "Minimo 4 caracteres", MessageDialogStyle.Affirmative);
                                return;
                            }
                            else
                            {
                                NOMBRE_D = caja_texto_usuario.Text;
                                CONTRASEÑA_D = caja_contrasena.Password;
                                TIPO_USUARIO_D = Combobox_tipo.Text;
                                ArrayList resultado3 = metodos_bd.BUSCAR(NOMBRE_TABLA, NOMBRE, NOMBRE_D, CANTIDAD_COLUMNAS, TODO);
                                if (resultado3.Count > 0)
                                {
                                    if (resultado3[0].ToString() == ID_USUARIO_C)
                                    {/*si resultado3 me regreso a mi mismo puedo modificar el dato*/
                                        var a2 = await this.ShowMessageAsync(TITULO_MENSAJE, "¿Guardar cambios?", MessageDialogStyle.AffirmativeAndNegative);
                                        if (a2 == MessageDialogResult.Affirmative) LLAMAR_ACTUALIZAR();
                                        else LIMPIAR_TODO();
                                        ACTUALIZAR_TABLA_DATAGRID();
                                    }
                                    else
                                    {/*sino esto quiere decir que es otro usuario y por lo tanto no puedo usar ese nombre*/
                                        await this.ShowMessageAsync(TITULO_MENSAJE, "Nombre no disponible", MessageDialogStyle.Affirmative);
                                        return;
                                    }
                                }
                                else
                                {/*si no se regreso ningun dato de la busque significa que el dato esta disponilbe*/
                                    var a2 = await this.ShowMessageAsync(TITULO_MENSAJE, "¿Guardar cambios?", MessageDialogStyle.AffirmativeAndNegative);
                                    if (a2 == MessageDialogResult.Affirmative) { LLAMAR_ACTUALIZAR(); ACTUALIZAR_TABLA_DATAGRID(); }
                                }
                            }
                        }
                        else
                        {
                            await this.ShowMessageAsync(TITULO_MENSAJE, "Las contraseñas no coinciden", MessageDialogStyle.Affirmative);
                        }
                    }
                    ESTADO = GUARDAR;
                    break;

                case ELIMINAR:
                    var mySettings = new MetroDialogSettings
                    {
                        AffirmativeButtonText = "Yes",
                        ColorScheme = MetroDialogColorScheme.Inverted
                    };
                    var a3 = await this.ShowMessageAsync(TITULO_MENSAJE, "¿Eliminar usuario?", MessageDialogStyle.AffirmativeAndNegative, mySettings);
                    if (a3 == MessageDialogResult.Affirmative)
                    {
                        metodos_bd.ELIMINAR(NOMBRE_TABLA, ID_USUARIO, ID_USUARIO_C);
                        LIMPIAR_TODO();
                        
                        CANTIDAD = metodos_bd.LLENAR_DATAGRID(tabla_Principal, NOMBRE_TABLA, RUTA_ENLACE_DATAGRID, ID_USUARIO, NOMBRE, TIPO_USUARIO);
                        ACTUALIZAR_TABLA_DATAGRID();
                    }
                    
                    ESTADO = GUARDAR;
                    break;

                case ULTIMO_USUARIO:
                    ArrayList resultado4 = metodos_bd.BUSCAR(NOMBRE_TABLA, ID_USUARIO, ID_USUARIO_C, CANTIDAD_COLUMNAS, TODO);
                    /*los datos se respaldan en unas variables auxiliares y se muestran en los componentes correspondientes*/
                    metodos_bd.ACTUALIZAR(NOMBRE_TABLA, ID_USUARIO, resultado4[0].ToString(),(NOMBRE, resultado4[1].ToString()),(CONTRASEÑA, resultado4[2].ToString()),(TIPO_USUARIO, TIPO_ADMINISTRADOR));
                    await this.ShowMessageAsync(TITULO_MENSAJE, "Ultimo usuario se volvio administrador", MessageDialogStyle.Affirmative);
                    ESTADO = GUARDAR;
                    ACTUALIZAR_TABLA_DATAGRID();
                    LIMPIAR_TODO();
                    break;
            }
        }

        #region datagrid
        private void ACTUALIZAR_TABLA_DATAGRID()
        {
            if (CANTIDAD == 1)
            {
                DataRowView row=null;
                try
                {
                    row = (DataRowView)tabla_Principal.Items[0];
                    ID_USUARIO_C = (row[0]).ToString();
                }
                catch (Exception) { }
                Navegacion.NavigarA(new auxiliar());
              
            }
            else
            {
                if (tabla_Principal.SelectedItems.Count>0)
                {
                    boton_borrar.Visibility = Visibility.Visible;
                    try
                    {
                        DataRowView row = (DataRowView)tabla_Principal.SelectedItems[0];
                        ID_USUARIO_C = (row[0]).ToString();
                    }
                    catch (Exception) { }
                    ESTADO = APUNTAR;
                }
                else
                {
                    ESTADO = GUARDAR;
                }
            }
            maquina_estados();
            CANTIDAD = metodos_bd.LLENAR_DATAGRID(tabla_Principal, NOMBRE_TABLA, RUTA_ENLACE_DATAGRID, ID_USUARIO, NOMBRE, TIPO_USUARIO);
        }

        private async void LLAMAR_ACTUALIZAR()
        {
            metodos_bd.ACTUALIZAR(NOMBRE_TABLA, ID_USUARIO, ID_USUARIO_C,(NOMBRE, NOMBRE_D),(CONTRASEÑA, CONTRASEÑA_D),(TIPO_USUARIO, TIPO_USUARIO_D));
            await this.ShowMessageAsync(TITULO_MENSAJE, "Usuario actualizado", MessageDialogStyle.AffirmativeAndNegative);
            LIMPIAR_TODO();
            ACTUALIZAR_TABLA_DATAGRID();
            ESTADO = GUARDAR;
        }

        private void DataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ACTUALIZAR_TABLA_DATAGRID();
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
            if (e.Key == Key.Enter) caja_contrasena.Focus();
        }

        private void Caja_texto_usuario_TextChanged(object sender, TextChangedEventArgs e)
        {
            NOMBRE_D = caja_texto_usuario.Text.Trim();
        }

        private void Caja_contrasena_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) caja_contrasena1.Focus();
        }

        private void Caja_contrasena1_KeyDown(object sender, KeyEventArgs e)
        {
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
                caja_contrasena1.Background = Brushes.LightSalmon;
                caja_contrasena.Background = Brushes.LightSalmon;
                return false;
            }
            else
            {
                CONTRASEÑA_D = caja_contrasena.Password.Trim();
                caja_contrasena1.Background = Brushes.LightGreen;
                caja_contrasena.Background = Brushes.LightGreen;
                return true;
            }
        }

        private void Combobox_tipo_KeyDown(object sender, KeyEventArgs e)
        {

            if (Combobox_tipo.IsDropDownOpen==true)
            {
                if (Combobox_tipo.SelectedItem != null)
                {
                    if (e.Key == Key.Enter)
                    {
                        TIPO_USUARIO_D = Combobox_tipo.Text;
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
            if (Combobox_tipo.SelectedItem != null)
            {
                TIPO_USUARIO_D = Combobox_tipo.Text;
                boton_guardar.Focus();
            }
            else
            {
                Combobox_tipo.Focus();
            }
        }

        private void Boton_guardar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) maquina_estados();
           
        }

        private void Boton_guardar_Click(object sender, RoutedEventArgs e)
        {
            maquina_estados();
        }

        private void Boton_borrar_Click(object sender, RoutedEventArgs e)
        {
                ESTADO = ELIMINAR;
                maquina_estados();
        }

        private void Boton_borrar_KeyDown(object sender, KeyEventArgs e)
        {
            ESTADO = ELIMINAR;
            maquina_estados();
        }
        #endregion
        private void LIMPIAR_TODO()
        {
            CANTIDAD = 0;
            ESTADO = GUARDAR;
            boton_borrar.Visibility = Visibility.Hidden;
            boton_guardar.Content = BTN_GUARDAR;
            Combobox_tipo.IsEnabled = true;
            LimpiarDatos();
            LimpiarCopias();
            Controles.Limpiar_controles();
        }
    }
}
