using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using BDatos_API.servicioBuques;
using BDatos_API.servicioClientes;
using BDatos_API.servicioProductos;
using TableDependency.SqlClient.Base.Enums;

namespace BDatos_API.VISTAS
{
    public class ActualizarAutoCompletado : IBuquesCallback, IClientesCallback, IProductosCallback,INotifyPropertyChanged
    {
        public ObservableCollection<Buques> GetBuques;
        public ObservableCollection<Clientes> GetClientes;
        public ObservableCollection<Productos> GetProductos;

        public BuquesClient buquesClient;
        public ClientesClient clientesClient;
        public ProductosClient productosClient;

        public event PropertyChangedEventHandler PropertyChanged;

        public Buques buqueSeleccionado { get; set; }
        public Productos productoSeleccionado { get; set; }
        public Clientes clienteSeleccionado { get; set; }

        public string BUQUE { get; set; }
        public string CLIENTE { get; set; }
        public string PRODUCTO { get; set; }

        public ActualizarAutoCompletado()
        {
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
        }

      

        public void cambiosBuques(string BUQUE, string VIAJE, string tipo_Cambio)
        {
            if (GetBuques != null)
            {
                var exportacionIndex = GetBuques.IndexOf(GetBuques.FirstOrDefault(c => c.BUQUE == BUQUE));
                if (exportacionIndex >= 0)
                {
                    if(tipo_Cambio==ChangeType.Update.ToString())
                    GetBuques[exportacionIndex] = new Buques { BUQUE = BUQUE, VIAJE = VIAJE };
                    if (tipo_Cambio == ChangeType.Delete.ToString())
                    GetBuques.RemoveAt(exportacionIndex);
                }
                else
                {
                    GetBuques.Add(new Buques { BUQUE = BUQUE, VIAJE = VIAJE });
                }
            }
        }

        public void cambiosCliente(string CLIENTE, string tipo_Cambio)
        {
            if (GetClientes != null)
            {
                var clienteIndex = GetClientes.IndexOf(GetClientes.FirstOrDefault(c => c.CLIENTE ==CLIENTE));
                if (clienteIndex >= 0)
                {
                    if (tipo_Cambio == ChangeType.Update.ToString())
                        GetClientes[clienteIndex] = new Clientes { CLIENTE=CLIENTE };
                    if (tipo_Cambio == ChangeType.Delete.ToString())
                        GetClientes.RemoveAt(clienteIndex);
                }
                else
                {
                    GetClientes.Add(new Clientes { CLIENTE = CLIENTE});
                }
            }
        }

        public void cambiosProductos(string PRODUCTO, string tipo_Cambio)
        {
            if (GetProductos != null)
            {
                var clienteIndex = GetProductos.IndexOf(GetProductos.FirstOrDefault(c => c.PRODUCTO == PRODUCTO));
                if (clienteIndex >= 0)
                {
                    if (tipo_Cambio == ChangeType.Update.ToString())
                        GetProductos[clienteIndex] = new Productos{ PRODUCTO = PRODUCTO };
                    if (tipo_Cambio == ChangeType.Delete.ToString())
                        GetProductos.RemoveAt(clienteIndex);
                }
                else
                {
                    GetProductos.Add(new Productos { PRODUCTO = PRODUCTO });
                }
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
