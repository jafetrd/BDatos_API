using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Windows.Threading;
using BDatos_API.servicioBuques;
using BDatos_API.servicioClientes;
using BDatos_API.servicioProductos;
using DotNetKit.Windows.Controls;

namespace BDatos_API.VISTAS
{
    public class ActualizarAutoCompletado : IBuquesCallback, IClientesCallback, IProductosCallback
    {
        public ObservableCollection<Buques> GetBuques;
        public ObservableCollection<Clientes> GetClientes;
        public ObservableCollection<Productos> GetProductos;

        public BuquesClient buquesClient;
        public ClientesClient clientesClient;
        public ProductosClient productosClient;

        public AutoCompleteComboBox clientes,productos,buques;

        public Buques buqueSeleccionado { get; set; }
        public Productos productoSeleccionado { get; set; }
        public Clientes clienteSeleccionado { get; set; }

        public ActualizarAutoCompletado(AutoCompleteComboBox _clientes, AutoCompleteComboBox _productos, AutoCompleteComboBox _buques)
        {
            clientes = _clientes;
            productos = _productos;
            buques = _buques;

            var instanceContext = new InstanceContext(this);
            buquesClient = new BuquesClient(instanceContext);
            buquesClient.Subscribe();

            var instanceContext2 = new InstanceContext(this);
            clientesClient = new ClientesClient(instanceContext2);
            clientesClient.Subscribe();

            var instanceContext3 = new InstanceContext(this);
            productosClient = new ProductosClient(instanceContext3);
            productosClient.Subscribe();

            GetBuques = new ObservableCollection<Buques>(buquesClient.obtenerTodosBuque().AsEnumerable());
            GetClientes = new ObservableCollection<Clientes>(clientesClient.obtenerTodosClientes().AsEnumerable());
            GetProductos = new ObservableCollection<Productos>(productosClient.obtenerTodosProductos().AsEnumerable());

            clientes.DataContext = this;
            buques.DataContext = this;
            productos.DataContext = this;

            clientes.ItemsSource = GetClientes;
            buques.ItemsSource = GetBuques;
            productos.ItemsSource = GetProductos;
        }



        public void cambiosBuques(string BUQUE, string VIAJE)
        {
            if (GetBuques != null)
            {
                var exportacionIndex =GetBuques.IndexOf(GetBuques.FirstOrDefault(c => c.BUQUE == BUQUE));
                if (exportacionIndex >= 0)
                {
                    GetBuques[exportacionIndex] = new Buques { BUQUE = BUQUE, VIAJE=VIAJE};
                    buques.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                    {
                        buques.Items.Refresh();
                    }));
                }
                else
                {
                    GetBuques.Add(new Buques {BUQUE = BUQUE, VIAJE = VIAJE});
                }
            }
        }

        public void cambiosCliente(string CLIENTE)
        {
            throw new NotImplementedException();
        }

        public void cambiosProductos(string PRODUCTO)
        {
            throw new NotImplementedException();
        }

      
    }
}
