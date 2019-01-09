using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;
using static BDatos_API.nombresPatioFerrocarril;
using static BDatos_API.Maquina_estados;

namespace BDatos_API.VISTAS
{
    /// <summary>
    /// Lógica de interacción para Patio_Ferrocarril.xaml
    /// </summary>
    public partial class Patio_Ferrocarril : Page
    {

        DateTime fechaE = DateTime.Now;
        DateTime fechaS = DateTime.Now;
        Metodos_comunes metodos_Comunes;
        Metodos_bd metodos_Bd;
        DataTable data;
        configMetroDialog configMetro;
        int[] desbloqueado;
        int[] bloqueado;
        //ultimos dos caracteres del año 2019 -> 19
        string fecha; 
        //modelo de las variables del formulario
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

        public Patio_Ferrocarril()
        { 
            InitializeComponent();
            cargar_basico();
            if(fase==Entrada)
            MostarFase1();
        }

        private void cargar_basico()
        {
            char[] caracteres = DateTime.Now.Year.ToString().ToCharArray();
            //primeros 4 digitos del numero de pedimento 08 es la aduana
            fecha = caracteres[2].ToString() + caracteres[3].ToString() + "08";
            pedimento_textbox.Text = fecha;
            //creacion de objetos solo si es necesario 
            configMetro = new configMetroDialog();

            modeloPatioFerrocarril = new modeloPatioFerrocarril();
            this.DataContext = modeloPatioFerrocarril;

            if (metodos_Bd == null) metodos_Bd = new Metodos_bd();
            if (presentaciones == null) llenarPresentaciones();
            if (String.IsNullOrEmpty(Fechaentrada_textbox.Text))
            {
                modeloPatioFerrocarril.FechaEntrada = DateTime.Now.ToShortDateString();
            }
            //inserta la primera fila en tabla
            datos.Add(new fila() { numero = "", presentacion = presentaciones });
            agregarControles();
        }

        private void llenarPresentaciones()
        {
            presentaciones = new List<string>();
            data = metodos_Bd.REGRESAR_TODO(NOMBRE_TABLA_2, TABLA2_PRESENTACIONES);
            foreach (DataRow row in data.Rows)
            {
                presentaciones.Add(row[0].ToString());
            }
            if (datos == null) datos = new ObservableCollection<fila>();
            tabla_Principal.ItemsSource = datos;
        }

        private void agregarControles()
        {
            if (metodos_Comunes == null) metodos_Comunes = new Metodos_comunes();
            //primera fase, los primeros 4 elementos
            metodos_Comunes.campos.Add(buque_Combobox);
            metodos_Comunes.campos.Add(viaje_textbox);
            metodos_Comunes.campos.Add(Regimen_combobox);
            metodos_Comunes.campos.Add(Fechaentrada_textbox);
            metodos_Comunes.campos.Add(Presentacion_combobox);
            metodos_Comunes.campos.Add(iniciales_textbox);
            metodos_Comunes.campos.Add(numero_textbox);
            metodos_Comunes.campos.Add(Peso_integer);
            metodos_Comunes.campos.Add(Tn_rbutton);
            metodos_Comunes.campos.Add(kg_rbutton);
            metodos_Comunes.campos.Add(producto_textbox);
            metodos_Comunes.campos.Add(mineral_checkbox);
            metodos_Comunes.campos.Add(cliente_Combobox);
            metodos_Comunes.campos.Add(pedimento_textbox);
            metodos_Comunes.campos.Add(valorcomercial_updown);
            metodos_Comunes.campos.Add(fecha_autorizacion);
            metodos_Comunes.campos.Add(fecha_salida_fisica);
        }

        private void MostarFase1()
        {
            desbloqueado = new int[] { 0, 1, 2, 3 };
            metodos_Comunes.mostrarCampos(desbloqueado);
            tabla_Principal.Visibility = Visibility.Visible;
            Quitarfila.Visibility = Visibility.Visible;
            Agregarfila.Visibility = Visibility.Visible;
            bloqueado = new int[] { 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            metodos_Comunes.ocultarCampos(bloqueado);
            boton_salida_fisica.IsEnabled = false;
            boton_autorizacion.IsEnabled = false;
            boton_actualizar.IsEnabled = false;
            boton_eliminar.IsEnabled = false;
        }

        private void ocultarFase2()
        {
           
        }
        private void Buque_Combobox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) viaje_textbox.Focus();
            metodos_Comunes.vacio(sender);
        }

        private void Viaje_textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Regimen_combobox.Focus();
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
                    if (Presentacion_combobox.IsVisible == true) {
                        Presentacion_combobox.Focus();
                    }
                    else
                    {
                        Agregarfila.Focus();
                    } 
                }
            }
            metodos_Comunes.vacio(sender);
        }

        private void Presentacion_combobox_KeyDown(object sender, KeyEventArgs e)
        {
            if (Presentacion_combobox.SelectedItem == null)
            {
                Presentacion_combobox.IsDropDownOpen = true;
            }
            else
            {
                if (e.Key == Key.Enter) iniciales_textbox.Focus();
            }
            metodos_Comunes.vacio(sender);
        }

        private void Iniciales_textbox_KeyDown(object sender, KeyEventArgs e)
        {
           if (e.Key == Key.Enter) numero_textbox.Focus();
            metodos_Comunes.vacio(sender);
        }

        private void Numero_textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Peso_integer.Focus();
            metodos_Comunes.vacio(sender);
        }

        private void Fechaentrada_textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (!String.IsNullOrEmpty(Fechaentrada_textbox.Text))
                if (e.Key == Key.Enter) Peso_integer.Focus();
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
            if (e.Key == Key.Enter) pedimento_textbox.Focus();
            metodos_Comunes.vacio(sender);
        }

        private void Pedimento_textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) valorcomercial_updown.Focus();
            metodos_Comunes.vacio(sender);
        }

        private void Valorcomercial_updown_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) boton_guardar.Focus();
            metodos_Comunes.vacio(sender);
        }

        private void Fecha_autorizacion_KeyDown(object sender, KeyEventArgs e)
        {
            if (!String.IsNullOrEmpty(fecha_autorizacion.Text))
                if (e.Key == Key.Enter) fecha_salida_fisica.Focus();
            metodos_Comunes.vacio(sender);

        }

        private void Fecha_salida_fisica_KeyDown(object sender, KeyEventArgs e)
        {
            if (!String.IsNullOrEmpty(fecha_salida_fisica.Text))
            {
                if (e.Key == Key.Enter) boton_guardar.Focus();
                periodo_almacenaje();
            }
            metodos_Comunes.vacio(sender);
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

        private async void Boton_autorizacion_Click(object sender, RoutedEventArgs e)
        {
            var a = await this.TryFindParent<MetroWindow>().ShowMessageAsync("Patio de contenedores", "¿Dar autorización de salida?", MessageDialogStyle.AffirmativeAndNegative);
            if (a == MessageDialogResult.Affirmative)
            {
                if (String.IsNullOrEmpty(fecha_autorizacion.Text))
                    fecha_autorizacion.Text = DateTime.Now.ToShortDateString();
            }
            else
            {
                return;
            }
        }

        private void periodo_almacenaje()
        {

        }

        private void Fecha_salida_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            periodo_almacenaje();
        }

        private void Agregarfila_Click(object sender, RoutedEventArgs e)
        {
            datos.Add(new fila() { numero = "", presentacion = presentaciones });
            tabla_Principal.Focus();
           // tabla_Principal.SelectedIndex = tabla_Principal.Items.Count - 1;
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

        private void DobleClick(object sender,RoutedEventArgs e)
        {
  
        }

        private async void Boton_guardar_Click(object sender, RoutedEventArgs e)
        {
            if (metodos_Comunes.Seleccionar_control(desbloqueado,false))
            {
                int contenedores = tabla_Principal.Items.Count;
                if (contenedores > 0)
                {
                    bool vacio = verificarTabla();
                    if (vacio == false)
                    {
                        int registros = tabla_Principal.Items.Count;
                        for(int a = 0; a<registros;a++)
                        {
                           
                        }
                        metodos_Comunes.Limpiar_controles();
                        
                    }
                    else
                    {
                        await this.TryFindParent<MetroWindow>().ShowMessageAsync(TITULO_MENSAJE, "Campos vacios en la tabla", MessageDialogStyle.Affirmative, configMetro.mensajeNormal);
                    }
                }
                else
                {
                    await this.TryFindParent<MetroWindow>().ShowMessageAsync(TITULO_MENSAJE, "No hay elementos en la tabla", MessageDialogStyle.Affirmative, configMetro.mensajeNormal);
                }
            }
            else
            {
                await this.TryFindParent<MetroWindow>().ShowMessageAsync(TITULO_MENSAJE, "Faltan datos", MessageDialogStyle.Affirmative, configMetro.mensajeNormal);
            }

            
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
                    DataGridRow dataGridRow = (DataGridRow)tabla_Principal.ItemContainerGenerator.ContainerFromIndex(tabla_Principal.SelectedIndex);
                    dataGridRow.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
                    tabla_Principal.SelectedIndex++;
                }
            }
        }

        private void Boton_limpiar_Click(object sender, RoutedEventArgs e)
        {
            metodos_Comunes.Limpiar_controles();
        }
    }
}
