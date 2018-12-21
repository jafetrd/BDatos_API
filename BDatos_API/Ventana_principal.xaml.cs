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
using System.Windows.Shapes;

namespace BDatos_API
{
    /// <summary>
    /// Lógica de interacción para Ventana_principal.xaml
    /// </summary>
    public partial class Ventana_principal : Window
    {
        public Ventana_principal()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Navegacion.NavegarAtras();
        }
    }
}
