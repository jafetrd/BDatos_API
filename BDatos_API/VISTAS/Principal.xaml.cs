using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using static BDatos_API.nombresPrincipal;
using static BDatos_API.Maquina_estados;

namespace BDatos_API.VISTAS
{
    /// <summary>
    /// Lógica de interacción para Principal.xaml
    /// </summary>
    public partial class Principal : Page
    {
        modeloPrincipal modelo;
        Metodos_bd metodos_Bd;
        public  Principal()
        {
            InitializeComponent();

            if (modelo == null) modelo = new modeloPrincipal();
            this.DataContext = modelo;

            metodos_Bd = new Metodos_bd();

        }


       


        private void DataGridCell_KeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine(modelo.datosSeleccionados.PATIO);
        }
    }
}
