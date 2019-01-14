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
    /// Lógica de interacción para Principal.xaml
    /// </summary>
    public partial class Principal : Page
    {
        modeloPrincipal modelo;
        public Principal()
        {
            InitializeComponent();
            if (modelo == null) modelo = new modeloPrincipal();
            this.DataContext = modelo;
        }

        private void DataGridCell_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
