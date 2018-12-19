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
using MahApps.Metro.Controls;


namespace BDatos_API
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        List<Control> controles = new List<Control>();
       
        public MainWindow()
        {
            InitializeComponent();
            caja_texto_usuario.Focus();
            controles.Add(caja_texto_usuario);
            controles.Add(caja_contrasena);
        }

        private void Caja_texto_usuario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) caja_contrasena.Focus();
        }

        private void Caja_contrasena_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) boton_ingresar.Focus();
        }

        private void Boton_ingresar_Click(object sender, RoutedEventArgs e)
        {
            Boton_ingresar_KeyDown(null, null);
        }

        private void Boton_ingresar_KeyDown(object sender, KeyEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(caja_texto_usuario.Text.Trim()))
            {

                Glow_TextBox_Border(caja_texto_usuario);
                caja_texto_usuario.Focus();
                MessageBox.Show("Usuario no valido");
                return;
            }
            if (String.IsNullOrWhiteSpace(caja_contrasena.Password.ToString()))
            {
                Glow_TextBox_Border(caja_contrasena);
                caja_contrasena.Focus();
                MessageBox.Show("Contraseña no valida");
                return;
            }
        }

        private void MetroWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.LeftCtrl | Key.I:
                    {
                        Boton_ingresar_KeyDown(null, null);
                    }
                break;
                case Key.LeftCtrl | Key.N:
                    { }
                break;
                case Key.LeftCtrl | Key.H:
                    { }
                break;
            }
        }

        private void Glow_TextBox_Border(Control control)
        {
            control.BorderThickness = new Thickness(2, 2, 2, 2);
            control.BorderBrush = Brushes.Red;
            control.Background = Brushes.Beige;
        }
    }
}
