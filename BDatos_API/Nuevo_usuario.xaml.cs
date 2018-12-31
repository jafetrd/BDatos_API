using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
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
        configMetroDialog configMetro;
        DataSet dataSet = null;
        public int ESTADO = 0;

        public Nuevo_usuario()
        {
            InitializeComponent();
            /*se inician constructores*/
            Controles = new Metodos_comunes();
            metodos_bd = new Metodos_bd();
            configMetro = new configMetroDialog();

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
            dataSet = metodos_bd.LLENAR_DATAGRID(tabla_Principal, NOMBRE_TABLA, RUTA_ENLACE_DATAGRID, ID_USUARIO, NOMBRE, TIPO_USUARIO);
            tabla_Principal.DataContext = dataSet;
        }

        public struct ultimoDato
        {
            public int id_usuario { set; get; }
            public string nombre { set; get; }
            public string tipo { set; get; }

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
                                await this.ShowMessageAsync(TITULO_MENSAJE, "Minimo 4 caracteres", MessageDialogStyle.Affirmative,configMetro.mensajeNormal);
                                return;
                            }
                            else
                            {
                                /*Se busca el nombre del usuario ingresado*/
                                ArrayList resultado = metodos_bd.BUSCAR(NOMBRE_TABLA, NOMBRE, NOMBRE_D, CANTIDAD_COLUMNAS, TODO);
                                if (resultado.Count > 0 ) /*¿ya existe ese usuario?*/
                                {   /*Mensaje para cambiar nombre*/
                                    await this.ShowMessageAsync(TITULO_MENSAJE, "Nombre no disponible", MessageDialogStyle.Affirmative,configMetro.mensajeNormal);
                                    return;
                                }
                                else
                                {   /*¿Crear nuevo usuario?*/
                                    var a = await this.ShowMessageAsync(TITULO_MENSAJE, "¿Crear nuevo usuario?", MessageDialogStyle.AffirmativeAndNegative,configMetro.mensajeAcentuado);
                                    if (a == MessageDialogResult.Affirmative)/*Se guarda el nuevo usuario*/
                                    {
                                        metodos_bd.GUARDAR(NOMBRE_TABLA, (NOMBRE, NOMBRE_D), (CONTRASEÑA, CONTRASEÑA_D), (TIPO_USUARIO, TIPO_USUARIO_D));
                                        await this.ShowMessageAsync(TITULO_MENSAJE, "Usuario creado", MessageDialogStyle.Affirmative, configMetro.mensajeNormal);
                                        LIMPIAR_TODO();
                                        int ultimooID = metodos_bd.ULTIMO_REGISTRO(NOMBRE_TABLA, ID_USUARIO);
                                        ArrayList userLocal = metodos_bd.BUSCAR(NOMBRE_TABLA, ID_USUARIO, ultimooID.ToString(), CANTIDAD_COLUMNAS, TODO);                                 
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
                            await this.ShowMessageAsync(TITULO_MENSAJE, "Las contraseñas no coinciden", MessageDialogStyle.Affirmative,configMetro.mensajeNormal);
                        }
                    }
                    ESTADO = GUARDAR;
                    break;

                case APUNTAR:
                        ArrayList resultado2 = metodos_bd.BUSCAR(NOMBRE_TABLA, ID_USUARIO, ID_USUARIO_C, CANTIDAD_COLUMNAS, TODO);
                        /*los datos se respaldan en unas variables auxiliares y se muestran en los componentes correspondientes*/
                        ID_USUARIO_C = resultado2[0].ToString();
                        caja_texto_usuario.Text = NOMBRE_C = resultado2[1].ToString();
                        caja_contrasena.Password = caja_contrasena1.Password = CONTRASEÑA_C = resultado2[2].ToString();
                        Combobox_tipo.Text = TIPO_USUARIO_C = resultado2[3].ToString();
                        boton_borrar.Visibility = Visibility.Visible;
                        boton_guardar.Content = BTN_EDITAR;
                        ESTADO = ACTUALIZAR;
                    break;


                case ACTUALIZAR:
                    /*se verifica que todos los campos esten llenos*/
                    if (Controles.Seleccionar_control(false))
                    {
                        if (verificar_contrasena())
                        {
                            if (caja_contrasena.Password.Length < 4)
                            {
                                await this.ShowMessageAsync(TITULO_MENSAJE, "Minimo 4 caracteres", MessageDialogStyle.Affirmative,configMetro.mensajeNormal);
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
                                    if (resultado3[1].ToString() == TIPO_ADMINISTRADOR)
                                    {
                                        var a2 = await this.ShowMessageAsync(TITULO_MENSAJE, "¿Guardar cambios?", MessageDialogStyle.AffirmativeAndNegative,configMetro.mensajeAcentuado);
                                        if (a2 == MessageDialogResult.Affirmative) LLAMAR_ACTUALIZAR();
                                        else LIMPIAR_TODO();
                                    }
                                    else
                                    {
                                        if (resultado3[0].ToString() == ID_USUARIO_C)
                                        {/*si resultado3 me regreso a mi mismo puedo modificar el dato*/
                                            var a2 = await this.ShowMessageAsync(TITULO_MENSAJE, "¿Guardar cambios?", MessageDialogStyle.AffirmativeAndNegative,configMetro.mensajeAcentuado);
                                            if (a2 == MessageDialogResult.Affirmative) LLAMAR_ACTUALIZAR();
                                            else LIMPIAR_TODO();
                                        }
                                        else
                                        {/*sino esto quiere decir que es otro usuario y por lo tanto no puedo usar ese nombre*/
                                            await this.ShowMessageAsync(TITULO_MENSAJE, "Nombre no disponible", MessageDialogStyle.Affirmative,configMetro.mensajeNormal);
                                            return;
                                        }
                                    }
                                }
                                else
                                {/*si no se regreso ningun dato de la busque significa que el dato esta disponilbe*/
                                    var a2 = await this.ShowMessageAsync(TITULO_MENSAJE, "¿Guardar cambios?", MessageDialogStyle.AffirmativeAndNegative,configMetro.mensajeAcentuado);
                                    if (a2 == MessageDialogResult.Affirmative) { LLAMAR_ACTUALIZAR(); }
                                }
                            }
                        }
                        else
                        {
                            await this.ShowMessageAsync(TITULO_MENSAJE, "Las contraseñas no coinciden", MessageDialogStyle.Affirmative,configMetro.mensajeNormal);
                        }
                    }
                    tabla_Principal.Items.Refresh();
                    ESTADO = GUARDAR;
                    break;

                case ELIMINAR:
                   
                    var a3 = await this.ShowMessageAsync(TITULO_MENSAJE, "¿Eliminar usuario?", MessageDialogStyle.AffirmativeAndNegative, configMetro.mensajeBorrar);
                    if (a3 == MessageDialogResult.Affirmative)
                    {
                        metodos_bd.ELIMINAR(NOMBRE_TABLA, ID_USUARIO, ID_USUARIO_C);
                        tabla_Principal.Items.Refresh();
                    }
                    LIMPIAR_TODO();
                    ESTADO = GUARDAR;
                    break;
            }
        }

        #region datagrid

        private async void LLAMAR_ACTUALIZAR()
        {
            metodos_bd.ACTUALIZAR(NOMBRE_TABLA, ID_USUARIO, ID_USUARIO_C,(NOMBRE, NOMBRE_D),(CONTRASEÑA, CONTRASEÑA_D),(TIPO_USUARIO, TIPO_USUARIO_D));
            await this.ShowMessageAsync(TITULO_MENSAJE, "Usuario actualizado", MessageDialogStyle.Affirmative,configMetro.mensajeNormal);
            LIMPIAR_TODO();
        }

        private void dataGridMetodo()
        {
            if (tabla_Principal.SelectedItems.Count == 1)
            {
                DataRowView row = null;
                try
                {
                    row = (DataRowView)tabla_Principal.SelectedItems[0];
                    ID_USUARIO_C = (row[0]).ToString();
                }
                catch (Exception) { }

                if (row[1].ToString() != TIPO_ADMINISTRADOR)
                {
                    ESTADO = APUNTAR;
                }
                else
                {
                    LIMPIAR_TODO();
                }
            }
            maquina_estados();
        }
        
        #endregion

        #region eventos_controles

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
           // Navegacion.GoBack();
            Controles.Limpiar_lista();
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (DispatcherOperationCallback)delegate (object o)
            {
                Navegacion.NavegarAtras();
                return null;
            }, null);

            e.Cancel = true;
        }

        private void DataGridCell_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) dataGridMetodo();
        }

        private void DataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            dataGridMetodo();
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
                caja_contrasena.Background = Brushes.LightSkyBlue;
                caja_contrasena1.Background = Brushes.LightSkyBlue;
                return false;
            }
            else
            {
                CONTRASEÑA_D = caja_contrasena.Password.Trim();
                caja_contrasena1.ClearValue(Control.BackgroundProperty);
                caja_contrasena.ClearValue(Control.BackgroundProperty);
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

        private void Boton_limpiar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) LIMPIAR_TODO();
        }

        private void Boton_limpiar_Click(object sender, RoutedEventArgs e)
        {
            LIMPIAR_TODO();
        }
        #endregion


        private void LIMPIAR_TODO()
        {
            boton_borrar.Visibility = Visibility.Hidden;
            boton_guardar.Content = BTN_GUARDAR;
            ESTADO = GUARDAR;
            LimpiarDatos();
            LimpiarCopias();
            Controles.Limpiar_controles();
        }

    }
}
