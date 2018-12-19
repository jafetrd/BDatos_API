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
using MahApps.Metro.Controls.Dialogs;

namespace BDatos_API
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        List<Control> Controles = new List<Control>();

        public MainWindow()
        {
            InitializeComponent();
            Controles.Add(caja_texto_usuario);  //0 caja de texto usuario
            Controles.Add(caja_contrasena);     //1 caja de contraseña
            Controles.Add(boton_ingresar);      //2 boton ingresar
            Controles[0].Focus();
        }

        private void Caja_texto_usuario_KeyDown(object sender, KeyEventArgs e)
        {
            Marcar_control(Controles[0], true);
            if (e.Key == Key.Enter) Controles[1].Focus();
        }

        private void Caja_contrasena_KeyDown(object sender, KeyEventArgs e)
        {
            Marcar_control(Controles[1], true);
            if (e.Key == Key.Enter) Controles[2].Focus();
        }

        private void Boton_ingresar_Click(object sender, RoutedEventArgs e)
        {
            Boton_ingresar_KeyDown(null, null);
        }

        private void Boton_ingresar_KeyDown(object sender, KeyEventArgs e)
        {
            if (Seleccionar_control())
            {
                Ventana_principal ventana_Principal = new Ventana_principal();
                ventana_Principal.Show();
                this.Hide();
            }
            else
            {
                this.ShowMessageAsync("Inicio de sesión", "Datos incorrectos");
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

        private bool Seleccionar_control()
        {
            //bool valor=true;
            foreach (Control a in Controles)
            {
                switch (a.GetType().Name)
                {
                    case "TextBox":
                        if (string.IsNullOrEmpty((a as TextBox).Text))
                        {
                            Marcar_control(a, false);
                            a.Focus();
                            return false;
                        }
                        break;

                    case "PasswordBox":
                        if (string.IsNullOrEmpty((a as PasswordBox).Password.ToString()))
                        {
                            Marcar_control(a, false);
                            a.Focus();
                            return false;
                        }
                        break;
                }
            }
            return true;
        }

        private void Marcar_control(Control control,Boolean limpiar)
        {
            if (limpiar)
                control.Background = Brushes.White;
            else
                control.Background = Brushes.LightGray;
        }

       
    }
}
