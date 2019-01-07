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
    /// Lógica de interacción para Patio_Ferrocarril.xaml
    /// </summary>
    public partial class Patio_Ferrocarril : Page
    {
        public Patio_Ferrocarril()
        {
            InitializeComponent();
        }

        private void Buque_Combobox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) viaje_textbox.Focus();
        }

        private void Viaje_textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Altura_combobox.Focus();
        }

        private void Altura_combobox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) cantidad_TextBox.Focus();
        }

        private void Cantidad_TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Presentacion_combobox.Focus();
        }
        private void Presentacion_combobox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Sigla_textbox.Focus();
        }

        private void Sigla_textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Numero_integer.Focus();
        }

        private void Numero_integer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Fechaentrada_textbox.Focus();
        }

        private void Fechaentrada_textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Peso_integer.Focus();
        }

        private void Peso_integer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) kg_rbutton.Focus();

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
        }

        private void Mineral_checkbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                mineral_checkbox.IsChecked = !mineral_checkbox.IsChecked;
                cliente_Combobox.Focus();
            }
        }

        private void Cliente_Combobox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) pedimento_textbox.Focus();
        }

        private void Pedimento_textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) valorcomercial_updown.Focus();
        }

        private void Valorcomercial_updown_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) fecha_autorizacion.Focus();
        }

        private void Fecha_autorizacion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) fecha_salida.Focus();
        }

        private void Fecha_salida_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) boton_guardar.Focus();
        }

    }
}
