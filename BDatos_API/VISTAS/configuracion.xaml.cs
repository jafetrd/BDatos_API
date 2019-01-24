using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Windows.Input;

namespace BDatos_API.VISTAS
{
    /// <summary>
    /// Lógica de interacción para configuracion.xaml
    /// </summary>
    public partial class configuracion : Page
    {
        private class BUQUE
        {
            public string buque { get; set; }
            public string viaje { get; set; }
        }

        private class CLIENTE
        {
            public string cliente { get; set; } 
        }

        private class PRODUCTOS
        {
            public string producto { get; set; }
        }

        private ObservableCollection<BUQUE> listaBuques;
        private ObservableCollection<CLIENTE> listaCliente;
        private ObservableCollection<PRODUCTOS> listaProductos;

        public configuracion()
        {
            InitializeComponent();

            listaBuques = new ObservableCollection<BUQUE>();
            tabla_buques.ItemsSource = listaBuques;
            getBuques();
            listaCliente = new ObservableCollection<CLIENTE>();
            tabla_clientes.ItemsSource = listaCliente;
            getClientes();
            listaProductos = new ObservableCollection<PRODUCTOS>();
            tabla_productos.ItemsSource = listaProductos;
            getProductos();
        }

        private void getBuques()
        {
            using (var sqlConnection = new SqlConnection(ConectorDB.datos))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "SELECT * FROM [buques]";

                    
                    using (var sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        if (sqlDataReader.HasRows)
                            while (sqlDataReader.Read())
                            {
                                listaBuques.Add(new BUQUE
                                {
                                    buque = sqlDataReader.SafeGetString("BUQUE"),
                                    viaje = sqlDataReader.SafeGetString("VIAJE")
                                });
                            }
                    }
                }
            }
        }

        private void getClientes()
        {
            using (var sqlConnection = new SqlConnection(ConectorDB.datos))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "SELECT * FROM [clientes]";
                    using (var sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        if (sqlDataReader.HasRows)
                            while (sqlDataReader.Read())
                            {
                                listaCliente.Add(new CLIENTE
                                {
                                    cliente = sqlDataReader.SafeGetString("PRODUCTO"),
                                });
                            }
                    }
                }
            }
        }

        private void getProductos()
        {
            using (var sqlConnection = new SqlConnection(ConectorDB.datos))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "SELECT * FROM [productos]";
                    using (var sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        if (sqlDataReader.HasRows)
                            while (sqlDataReader.Read())
                            {
                                listaProductos.Add(new PRODUCTOS 
                                {
                                    producto = sqlDataReader.SafeGetString("PRODUCTO"),
                                });
                            }
                    }
                }
            }
        }

        private void DataGridCell_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }

    public static class extensionDataRead
    {
        public static string SafeGetString(this SqlDataReader reader, string Columna)
        {
            int columIndex = reader.GetOrdinal(Columna);
            if (!reader.IsDBNull(columIndex))
            {
                return reader.GetString(columIndex);
            }
            else
            {
                return string.Empty;
            }
        }
    }

}
