﻿using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static BDatos_API.nombresPatioFerrocarril;
using static BDatos_API.Maquina_estados;
using System.Collections;

namespace BDatos_API.VISTAS
{
    /// <summary>
    /// Lógica de interacción para Patio_Ferrocarril.xaml
    /// </summary>
    public partial class Patio_Ferrocarril : Page
    {
        #region variables 
        DateTime fechaE = DateTime.Now;
        DateTime fechaS = DateTime.Now;
        Metodos_comunes metodos_Comunes;
        Metodos_bd metodos_Bd;
       
        configMetroDialog configMetro;
        int[] desbloqueado;
        int[] bloqueado;
        int ESTADO_LOCAL=0;
        string fecha;
        modeloPatioFerrocarril modeloPatioFerrocarril;


        //Propiedades para la tabla numero es la columna 1 y presentacion es la columna 2 
        //con combobox 
        public class fila
        {
            public string iniciales { get; set; }
            public string numero { get; set; }
            public List<string> presentacion { get; set; }
            public string seleccionado { get; set; }
        }
        //La coleccion es de tipo fila por lo que cada fila lleva dos elementos 
        //el numero y la lista de presentaciones
        public ObservableCollection<fila> datos { get; set; }
        //esta lista se usa para obtener los datos de la base SQL y despues los pasa
        //a la lista interna de la coleccion datos 
        public List<string> presentaciones;
        int cont = 0;
        string[] a;

        #endregion
        ActualizarAutoCompletado autoCompletado;
        private int _estado;
        private string _regimen;
        public Patio_Ferrocarril(int estado,string regimen)
        { 
            InitializeComponent();

            //constructores
            _estado = estado;
            _regimen = regimen;
            constructores();
            autoCompletado = new ActualizarAutoCompletado();

            cliente_Combobox.DataContext = autoCompletado;
            producto_textbox.DataContext = autoCompletado;
            buque_Combobox.DataContext = autoCompletado;

            cliente_Combobox.ItemsSource = autoCompletado.GetClientes;
            producto_textbox.ItemsSource = autoCompletado.GetProductos;
            buque_Combobox.ItemsSource = autoCompletado.GetBuques;

            a = new string[3] { "UUFF", "Contenedores", "Carga suelta" };
        }

        #region metodos de carga iniciales
   
        public void constructores()
        {
            modeloPatioFerrocarril = null;
            modeloPatioFerrocarril = new modeloPatioFerrocarril();
            this.DataContext = modeloPatioFerrocarril;
            _ESTADO = _estado;
            modeloPatioFerrocarril.REGIMEN = _regimen;
            if (configMetro == null) configMetro = new configMetroDialog();
            if (metodos_Bd == null) metodos_Bd = new Metodos_bd();
            if (presentaciones == null) presentaciones = new List<string>();
          
            if (metodos_Comunes == null) metodos_Comunes = new Metodos_comunes();
            if (datos == null) datos = new ObservableCollection<fila>();
            // if (comboBuqueDatos == null) comboBuqueDatos = new ObservableCollection<comboBuque>();
            llenarPresentaciones();
        }

        public void elegirFase()
        {
            switch (_ESTADO)
            {
                case Entrada:
                    switch (modeloPatioFerrocarril.REGIMEN)
                    {
                        case IMPORTACION:
                            mostrarImportacion();
                            break;
                        case EXPORTACION:
                            mostrarExportacion();
                            break;
                    }
                    modeloPatioFerrocarril.FECHA_ENTRADA = DateTime.Now.ToShortDateString();
                    break;
                case Salida:
                 
                    switch (modeloPatioFerrocarril.REGIMEN)
                    {
                        case IMPORTACION:
                            mostrarImportacion2();
                            break;
                        case EXPORTACION:
                            mostrarExportacion2();
                            break;
                    }
                    char[] caracteres = DateTime.Now.Year.ToString().ToCharArray();
                    //primeros 4 digitos del numero de pedimento 08 es la aduana
                    fecha = caracteres[2].ToString() + caracteres[3].ToString() + "08";
                    modeloPatioFerrocarril.PEDIMENTO = fecha;
                    mostrarImportacion2();
                    break;
            }
        }

        private void llenarPresentaciones()
        {

            DataTable data; 
            if (presentaciones.Count==0)
            {
                data = metodos_Bd.REGRESAR_TODO(NOMBRE_TABLA_2, TABLA2_PRESENTACIONES);
                foreach (DataRow row in data.Rows)
                {
                    presentaciones.Add(row[0].ToString());
                }
                tabla_Principal.ItemsSource = datos;
            }
            data = null;

            //inserta la primera fila en tabla
            datos.Add(new fila() { numero = "", presentacion = presentaciones });

            agregarControles();
            elegirFase();
        }


        private void agregarControles()
        {
            //primera fase, los primeros 4 elementos
            metodos_Comunes.campos.Add(Regimen_combobox);
            metodos_Comunes.campos.Add(buque_Combobox);
            metodos_Comunes.campos.Add(viaje_textbox);
            metodos_Comunes.campos.Add(Fechaentrada_textbox);
            metodos_Comunes.campos.Add(Peso_integer);
            metodos_Comunes.campos.Add(Tn_rbutton);
            metodos_Comunes.campos.Add(kg_rbutton);
            metodos_Comunes.campos.Add(producto_textbox);
            metodos_Comunes.campos.Add(mineral_checkbox);
            metodos_Comunes.campos.Add(cliente_Combobox);
            metodos_Comunes.campos.Add(pedimento_textbox);
            metodos_Comunes.campos.Add(valorcomercial_updown);
            metodos_Comunes.campos.Add(agenteTextbox);
            metodos_Comunes.campos.Add(fecha_salida_fisica);
        }

        private void mostrarImportacion()
        {
            desbloqueado = new int[] { 0, 1, 2, 3 };
            metodos_Comunes.mostrarCampos(desbloqueado);
            tabla_Principal.Visibility = Visibility.Visible;
            Quitarfila.Visibility = Visibility.Visible;
            Agregarfila.Visibility = Visibility.Visible;
            boton_guardar.IsEnabled = true;
            tabla_Principal.IsEnabled = true;

            bloqueado = new int[] { 4, 5, 6, 7, 8, 9, 10, 11, 12,13};
            metodos_Comunes.ocultarCampos(bloqueado);
            boton_salida_fisica.IsEnabled = false;
            boton_actualizar.IsEnabled = false;
            boton_eliminar.IsEnabled = false;
        }

        private void mostrarImportacion2()
        {
            bloqueado = new int[] { 0, 1, 2, 3 };
            metodos_Comunes.ocultarCampos(bloqueado);
            tabla_Principal.Visibility = Visibility.Visible;
            Quitarfila.Visibility = Visibility.Hidden;
            Agregarfila.Visibility = Visibility.Hidden;
            boton_guardar.IsEnabled = false;
            tabla_Principal.IsEnabled = false;

            desbloqueado = new int[] { 4, 5, 6, 7, 8, 9, 10, 11, 12,13};
            metodos_Comunes.mostrarCampos(desbloqueado);
            boton_salida_fisica.IsEnabled = true;
            boton_actualizar.IsEnabled = true;
            boton_eliminar.IsEnabled = true;
        }

        private void mostrarExportacion()
        {
            desbloqueado = new int[] { 0,3 };
            metodos_Comunes.mostrarCampos(desbloqueado);
            tabla_Principal.Visibility = Visibility.Visible;
            Quitarfila.Visibility = Visibility.Visible;
            Agregarfila.Visibility = Visibility.Visible;

            bloqueado = new int[] { 1, 2, 4, 5, 6, 7, 8, 9, 10, 11, 12,13};
            metodos_Comunes.ocultarCampos(bloqueado);
            boton_salida_fisica.IsEnabled = false;
            boton_actualizar.IsEnabled = false;
            boton_eliminar.IsEnabled = false;
        }

        private void mostrarExportacion2()
        {
            bloqueado = new int[] { 0, 3 };
            metodos_Comunes.ocultarCampos(bloqueado);
            tabla_Principal.Visibility = Visibility.Visible;
            Quitarfila.Visibility = Visibility.Hidden;
            Agregarfila.Visibility = Visibility.Hidden;
            boton_guardar.IsEnabled = false;
            tabla_Principal.IsEnabled = false;

            desbloqueado = new int[] { 1, 2, 4, 5, 6, 7, 8, 9, 10, 11, 12,13 };
            metodos_Comunes.mostrarCampos(desbloqueado);
            boton_salida_fisica.IsEnabled = true;
            boton_actualizar.IsEnabled = true;
            boton_eliminar.IsEnabled = true;
        }

        private bool verificarTabla()
        {
            bool vacio = true;
            foreach (var data in datos)
            {
                vacio = false;
                if (string.IsNullOrEmpty(data.numero)) { vacio = true; break; }
                if (string.IsNullOrEmpty(data.iniciales)) { vacio = true; break; }
                if (string.IsNullOrEmpty(data.seleccionado)) { vacio = true; break; }
            }
            return vacio;
        }

        #endregion


        #region metodos de controles
        private void Buque_Combobox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) {siguiente_Key(sender, e); }
            metodos_Comunes.vacio(sender);
        }

        private void Viaje_textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { siguiente_Key(sender, e); }
            metodos_Comunes.vacio(sender);
        }

        private void Altura_combobox_KeyDown(object sender, KeyEventArgs e)
        {
            if (Regimen_combobox.SelectedItem == null)
            {
                Regimen_combobox.IsDropDownOpen = true;
            }
            else
            {
                if (e.Key == Key.Enter)
                {
                    siguiente_Key(sender, e);
                }
            }
            metodos_Comunes.vacio(sender);
        }

        private void Regimen_combobox_DropDownClosed(object sender, EventArgs e)
        {
            if (modeloPatioFerrocarril.REGIMEN == EXPORTACION)
            {
                buque_Combobox.Text = null;
                viaje_textbox.Text = null;
            }

            siguiente_Key(sender,enter());
            elegirFase();
        }

        private void Iniciales_textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { siguiente_Key(sender, e); }
            metodos_Comunes.vacio(sender);
        }

        private void Numero_textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { siguiente_Key(sender, e); }
            metodos_Comunes.vacio(sender);
        }

        private void Fechaentrada_textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (!String.IsNullOrEmpty(Fechaentrada_textbox.Text))
                if (e.Key == Key.Enter) { siguiente_Key(sender, e); }
            metodos_Comunes.vacio(sender);
        }

        private void Peso_integer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Tn_rbutton.Focus();
            metodos_Comunes.vacio(sender);
        }

        private void Kg_rbutton_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                kg_rbutton.IsChecked = true;
                Tn_rbutton.IsChecked = false;
                producto_textbox.Focus();
            }
        }

        private void Tn_rbutton_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                kg_rbutton.IsChecked = false;
                Tn_rbutton.IsChecked = true;
                producto_textbox.Focus();
            }
        }

        private void Producto_textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { mineral_checkbox.Focus(); }
            metodos_Comunes.vacio(sender);
        }

        private void Mineral_checkbox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.S:
                    mineral_checkbox.IsChecked = !mineral_checkbox.IsChecked;
                    mineral_checkbox.Focus();
                    break;
                case Key.Enter:
                    cliente_Combobox.Focus();
                    break;
            }
        }

        private void Cliente_Combobox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) siguiente_Key(sender, e);
            metodos_Comunes.vacio(sender);
        }

        private void Pedimento_textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) siguiente_Key(sender, e);
            metodos_Comunes.vacio(sender);
        }

        private void Valorcomercial_updown_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) siguiente_Key(sender, e);
            metodos_Comunes.vacio(sender);
        }

        private void AgenteTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) siguiente_Key(sender, e);
            metodos_Comunes.vacio(sender);
        }

        private void Fecha_salida_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            periodo_almacenaje();
        }

        private void Fecha_salida_fisica_KeyDown(object sender, KeyEventArgs e)
        {
            if (!String.IsNullOrEmpty(fecha_salida_fisica.Text))
            {
                if (e.Key == Key.Enter) siguiente_Key(sender, e);
                periodo_almacenaje();
            }
            metodos_Comunes.vacio(sender);
        }

        private void DataGridCell_KeyDown(object sender, KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            var uiElement = e.OriginalSource as UIElement;
            if (e.Key == Key.Enter && uiElement != null)
            {
                var row = tabla_Principal.Items.IndexOf(tabla_Principal.CurrentItem);
                uiElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                if (uiElement.GetType() == typeof(ComboBox))
                    if ((uiElement as ComboBox).SelectedItem == null)
                        (uiElement as ComboBox).IsDropDownOpen = true;
                e.Handled = true;
            }
        }

        private void Presentacion_datagrid_DropDownOpened(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            comboBox.Focus();
        }

        private void Presentacion_datagrid_DropDownClosed(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;

            if (comboBox.SelectedIndex > -1)
            {
                if (verificarTabla() == false)
                {
                    datos.Add(new fila() { numero = "", presentacion = presentaciones });
                    try
                    {
                        DataGridRow dataGridRow = (DataGridRow)tabla_Principal.ItemContainerGenerator.ContainerFromIndex(tabla_Principal.SelectedIndex);
                        dataGridRow.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
                        tabla_Principal.SelectedIndex++;
                    }
                    catch { }
                }
            }
        }

        private void siguiente_Key(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Enter)
            {
                TraversalRequest tRequest = new TraversalRequest(FocusNavigationDirection.Next);
                UIElement keyboardFocus = Keyboard.FocusedElement as UIElement;
                if (keyboardFocus != null)
                {
                    keyboardFocus.MoveFocus(tRequest);
                }
                e.Handled = true;
            }
        }

        private KeyEventArgs enter()
        {
            // InputManager.Current.ProcessInput(new KeyEventArgs
            //(Keyboard.PrimaryDevice,Keyboard.PrimaryDevice.ActiveSource,0, Key.Enter){RoutedEvent = Keyboard.KeyDownEvent});
            KeyEventArgs keyEventArgs=null;
            if (keyEventArgs == null)
            {
                keyEventArgs = new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Enter) { RoutedEvent = Keyboard.KeyDownEvent };
            }
            return keyEventArgs;
        }

        private void Buque_Combobox_DropDownClosed(object sender, EventArgs e)
        {
            if (autoCompletado.buqueSeleccionado != null)
                modeloPatioFerrocarril.VIAJE = (int.Parse(autoCompletado.buqueSeleccionado.VIAJE) + 1).ToString();
        }
        #endregion

        #region botones

        private void Agregarfila_Click(object sender, RoutedEventArgs e)
        {
            datos.Add(new fila() { numero = "", presentacion = presentaciones });
            tabla_Principal.Focus();
            tabla_Principal.BeginEdit();
        }

        private void Quitarfila_Click(object sender, RoutedEventArgs e)
        {
            fila a;
            for (int i = tabla_Principal.SelectedItems.Count - 1; i >= 0; i--)
            {
                a = tabla_Principal.SelectedItem as fila;
                if (a != null)
                {
                    datos.Remove(a);
                }
            }
        }

        private async void Boton_salida_Click(object sender, RoutedEventArgs e)
        {
            var a = await this.TryFindParent<MetroWindow>().ShowMessageAsync("Patio de contenedores", "¿Dar salida al contenedor?", MessageDialogStyle.AffirmativeAndNegative);
            if (a == MessageDialogResult.Affirmative)
            {
                if (String.IsNullOrEmpty(fecha_salida_fisica.Text))
                    fecha_salida_fisica.Text = DateTime.Now.ToShortDateString();
            }
            else
            {
                return;
            }
        }

        private void Boton_guardar_Click(object sender, RoutedEventArgs e)
        {
            ESTADO_LOCAL = GUARDAR;
            Maquina_Estados();
        }

        private void Boton_limpiar_Click(object sender, RoutedEventArgs e)
        {
            ESTADO_LOCAL = LIMPIAR;
            Maquina_Estados();
        }
       
        private void periodo_almacenaje()
        {

        }

        #endregion

        public async void Maquina_Estados()
        {
            switch (_ESTADO)
            {
                case Entrada:
                    switch (ESTADO_LOCAL)
                    {
                        case GUARDAR:
                            if (!metodos_Comunes.Seleccionar_control(desbloqueado, false))
                            {
                                await this.TryFindParent<MetroWindow>().ShowMessageAsync(TITULO_MENSAJE, "Faltan datos", MessageDialogStyle.Affirmative, configMetro.mensajeNormal);
                                return;
                            }
                                int contenedores = tabla_Principal.Items.Count;
                            if (contenedores < 1)
                            {
                                await this.TryFindParent<MetroWindow>().ShowMessageAsync(TITULO_MENSAJE, "No hay elementos en la tabla", MessageDialogStyle.Affirmative, configMetro.mensajeNormal);
                                return;
                            }
                                    bool vacio = verificarTabla();
                            if (vacio == false)
                            {
                                await this.TryFindParent<MetroWindow>().ShowMessageAsync(TITULO_MENSAJE, "Campos vacios en la tabla", MessageDialogStyle.Affirmative, configMetro.mensajeNormal);
                                return;
                            }
                            modeloPatioFerrocarril.conFormatoSQL();
                            string msj = "BUQUE: " + autoCompletado.BUQUE + "\n" +
                                            "VIAJE: " + modeloPatioFerrocarril.VIAJE + "\n" +
                                            "REGIMEN: " + modeloPatioFerrocarril.REGIMEN + "\n" +
                                            "FECHA DE ENTRADA: " + modeloPatioFerrocarril._FECHA_ENTRADA+ "\n"+
                                            "CANTIDAD CONTENEDORES: " + contenedores + "\n";
                            for (int a = 0; a < datos.Count; a++)
                            {
                                msj += datos[a].iniciales + datos[a].numero + "   " + datos[a].seleccionado+"\n";
                            }
                                      
                            var res = await this.TryFindParent<MetroWindow>().ShowMessageAsync("Datos a guardar:",msj,MessageDialogStyle.AffirmativeAndNegative, configMetro.PREVIEW);
                            if (!(res == MessageDialogResult.Affirmative)) return;

                            for (int a = 0; a < contenedores; a++)
                            {
                                metodos_Bd.GUARDAR(NOMBRE_TABLA,
                                    (BUQUE_, autoCompletado.BUQUE),
                                    (VIAJE_, modeloPatioFerrocarril._VIAJE),
                                    (REGIMEN_, modeloPatioFerrocarril._REGIMEN),
                                    (FECHA_ENTRADA_, modeloPatioFerrocarril._FECHA_ENTRADA),
                                    (PRESENTACION_, datos[a].seleccionado),
                                    (INICIALES_, datos[a].iniciales),
                                    (NUMERO_, datos[a].numero),
                                    (ESTADO_,modeloPatioFerrocarril._ESTADO),
                                    (SESION_ENTRADA_,modeloPatioFerrocarril._SESION_ENTRADA));
                            }

                            int ID = metodos_Bd.ULTIMO_REGISTRO(NOMBRE_TABLA, ID_) - contenedores;
                            for (int a = 0; a < contenedores; a++)
                            {
                                metodos_Bd.GUARDAR(nombresPatioFerrocarril.TABLA_TEMPORAL,
                                    (ID_, (ID + a).ToString()),
                                    (BUQUE_, autoCompletado.BUQUE),
                                    (VIAJE_, modeloPatioFerrocarril._VIAJE),
                                    (REGIMEN_, modeloPatioFerrocarril._REGIMEN),
                                    (FECHA_ENTRADA_, modeloPatioFerrocarril._FECHA_ENTRADA),
                                    (INICIALES_, datos[a].iniciales),
                                    (NUMERO_, datos[a].numero),
                                    (ESTADO_, modeloPatioFerrocarril._ESTADO),
                                    (ALMACEN_, nombresPatioFerrocarril.PFERROCARRIL));
                            }
                            if(modeloPatioFerrocarril._REGIMEN==nombresPatioFerrocarril.IMPO_SQL)
                            metodos_Bd.GUARDAR(nombresPatioFerrocarril.TABLA_TEMPORAL2,
                                (BUQUE_, autoCompletado.BUQUE),
                                (VIAJE_, modeloPatioFerrocarril._VIAJE),
                                (ALMACEN_, nombresPatioFerrocarril.PFERROCARRIL),
                                (REGIMEN_,modeloPatioFerrocarril._REGIMEN));

                            if (autoCompletado.BUQUE == null) return;
                                            
                            ArrayList arrayList = metodos_Bd.BUSCAR(tablaBuque.NOMBRE_TABLA, BUQUE_,autoCompletado.buqueSeleccionado.BUQUE, 3, TODO);
                            if (arrayList.Count == 0)
                            {
                                metodos_Bd.GUARDAR(tablaBuque.NOMBRE_TABLA,
                                    (tablaBuque.BUQUE_, autoCompletado.buqueSeleccionado.BUQUE),
                                    (tablaBuque.VIAJE_, modeloPatioFerrocarril._VIAJE));
                            }
                            else
                            {
                                metodos_Bd.ACTUALIZAR(tablaBuque.NOMBRE_TABLA, tablaBuque.ID_, arrayList[0].ToString(),
                                    (tablaBuque.VIAJE_, modeloPatioFerrocarril._VIAJE));
                            }

                            if (autoCompletado.CLIENTE == null) return;
                            arrayList.Clear();
                            arrayList = metodos_Bd.BUSCAR(tablaCliente.NOMBRE_TABLA, CLIENTE_, autoCompletado.CLIENTE, tablaCliente.CANTIDAD_COLUMNAS_, TODO);
                            if (arrayList.Count == 0)
                            {
                                metodos_Bd.GUARDAR(tablaCliente.NOMBRE_TABLA,
                                    (tablaCliente.CLIENTE_, autoCompletado.CLIENTE));
                            }

                            if (autoCompletado.PRODUCTO == null) return;
                            arrayList.Clear();
                            arrayList = metodos_Bd.BUSCAR(tablaProducto.NOMBRE_TABLA, PRODUCTO_, autoCompletado.PRODUCTO, tablaProducto.CANTIDAD_COLUMNAS_, TODO);
                            if (arrayList.Count == 0)
                            {
                                metodos_Bd.GUARDAR(tablaProducto.NOMBRE_TABLA,(tablaProducto.PRODUCTO_, autoCompletado.PRODUCTO));
                            }
                                            
                            await this.TryFindParent<MetroWindow>().ShowMessageAsync(TITULO_MENSAJE, "Se crearon: " + contenedores + " registros",
                                MessageDialogStyle.Affirmative, configMetro.PREVIEW);
                            ESTADO_LOCAL = LIMPIAR;
                            Maquina_Estados();
                            break;

                        case LIMPIAR:
                            metodos_Comunes.Limpiar_controles();
                            datos.Clear();
                            modeloPatioFerrocarril.limpiarPropiedades();
                            constructores();
                            break;
                        case NINGUNO:
                            break;
                            
                    }
                    break;
                case Autorizacion:

                    break;
                case Salida:

                    break;
            }
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            //e.Handled = true;
        }

        private void EstadoPresentacion_Click(object sender, RoutedEventArgs e)
        {
            int ultimo = tabla_Principal.SelectedIndex;
            if (ultimo < 0) return;
            datos.Remove(tabla_Principal.SelectedItem as fila);
            cont++;
            if (cont == 3) cont = 0;
            estadoPresentacion.Content = a[cont];
            DataTable data = null;
            if (cont == 0)
                data = metodos_Bd.REGRESAR_TODO(nombresPatioFerrocarril.NOMBRE_TABLA_2, nombresPatioFerrocarril.TABLA2_PRESENTACIONES);
            if (cont == 1)
                data = metodos_Bd.REGRESAR_TODO(nombresPatioContenedor.NOMBRE_TABLA_2, nombresPatioContenedor.TABLA2_PRESENTACIONES);
            if (cont == 2)
                data = metodos_Bd.REGRESAR_TODO(nombresBodegaC.NOMBRE_TABLA_2, nombresBodegaC.TABLA2_PRESENTACIONES);
            presentaciones = new List<string>();
            foreach (DataRow row in data.Rows)
            {
                presentaciones.Add(row[0].ToString());
            }
            datos.Insert(ultimo, new fila() { numero = "", presentacion = presentaciones });

            tabla_Principal.SelectedIndex = ultimo;
        }
    }
}
