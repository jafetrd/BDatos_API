using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BDatos_API.VISTAS
{
    /// <summary>
    /// Lógica de interacción para Bodega.xaml
    /// </summary>
    public partial class Bodega : Page
    {
        DateTime fechaE = DateTime.Now;
        DateTime fechaS = DateTime.Now;
        Metodos_comunes metodos_Comunes;

        List<dato> presentacion_lista;

        struct dato
        {
            public string numero { get; set; }
            public string presentacion { get; set; }
        }

        List<string> listaPresentaciones;

        public Bodega()
        {
            InitializeComponent();

            metodos_Comunes = new Metodos_comunes();
            metodos_Comunes.campos.Add(buque_Combobox);
            metodos_Comunes.campos.Add(viaje_textbox);
            metodos_Comunes.campos.Add(Altura_combobox);
            metodos_Comunes.campos.Add(Presentacion_combobox);
            metodos_Comunes.campos.Add(numContenedor_textbox);
            metodos_Comunes.campos.Add(Fechaentrada_textbox);
            metodos_Comunes.campos.Add(Peso_integer);
            metodos_Comunes.campos.Add(producto_textbox);

            if (String.IsNullOrEmpty(Fechaentrada_textbox.Text))
            {
                Fechaentrada_textbox.Text = DateTime.Now.ToString();
            }
            Presentacion_combobox.Visibility = Visibility.Hidden;
            Etiqueta_contenedor.Visibility = Visibility.Hidden;
            numContenedor_textbox.Visibility = Visibility.Hidden;
            Etiqueta_presentacion.Visibility = Visibility.Hidden;

            listaPresentaciones = new List<string>();

            listaPresentaciones.Add("30");
            listaPresentaciones.Add("40");

            presentacion_lista = new List<dato>
            {
                new dato{ numero="1234" , presentacion="20"}
            };
            tabla_Principal.ItemsSource = presentacion_lista;
        }

        private void Buque_Combobox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) viaje_textbox.Focus();
            metodos_Comunes.vacio(sender);
        }

        private void Viaje_textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Altura_combobox.Focus();
            metodos_Comunes.vacio(sender);
        }

        private void Altura_combobox_KeyDown(object sender, KeyEventArgs e)
        {
            if (Altura_combobox.SelectedItem == null)
            {
                Altura_combobox.IsDropDownOpen = true;
            }
            else
            {
                if (e.Key == Key.Enter) Presentacion_combobox.Focus();
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
                if (e.Key == Key.Enter) numContenedor_textbox.Focus();
            }
            metodos_Comunes.vacio(sender);
        }

        private void Sigla_textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Fechaentrada_textbox.Focus();
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
            if (e.Key == Key.Enter) kg_rbutton.Focus();
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
                    fecha_salida_fisica.Text = DateTime.Now.ToString();
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
                    fecha_autorizacion.Text = DateTime.Now.ToString();
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

        private void DataGridCell_KeyDown(object sender, KeyEventArgs e)
        {

        }

    }
}
