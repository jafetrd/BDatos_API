using MahApps.Metro.Controls.Dialogs;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

using static BDatos_API.TUsuario;

namespace BDatos_API
{
    /// <summary>
    /// Lógica de interacción para auxiliar.xaml
    /// </summary>
    public partial class auxiliar : Window
    {
        Metodos_bd metodos_bd;
        public auxiliar()
        {
            InitializeComponent();
            metodos_bd = new Metodos_bd();
        }

 

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            verificar_contrasena();
        }

        private void PasswordBox_Copy_PasswordChanged(object sender, RoutedEventArgs e)
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
                caja_contrasena1.Background = Brushes.LightGreen;
                caja_contrasena.Background = Brushes.LightGreen;
                return true;
            }
        }

        private void Button_KeyDown(object sender, KeyEventArgs e)
        {
            if (verificar_contrasena())
            {
                CONTRASEÑA_D = caja_contrasena.Password.Trim();
                NOMBRE_D = "Administrador";
                TIPO_USUARIO_D = "Administrador";
                metodos_bd.GUARDAR(NOMBRE_TABLA, (NOMBRE, NOMBRE_D), (CONTRASEÑA, CONTRASEÑA_D), (TIPO_USUARIO, TIPO_USUARIO_D));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (verificar_contrasena())
            {
                CONTRASEÑA_D = caja_contrasena.Password.Trim();
                NOMBRE_D = "Administrador";
                TIPO_USUARIO_D = "Administrador";
                metodos_bd.GUARDAR(NOMBRE_TABLA, (NOMBRE, NOMBRE_D), (CONTRASEÑA, CONTRASEÑA_D), (TIPO_USUARIO, TIPO_USUARIO_D));
                Navegacion.NavegarAtras();
            }
        }

        private void Caja_contrasena_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) caja_contrasena1.Focus();
        }

        private void Caja_contrasena1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) button.Focus();
        }
    }
}
