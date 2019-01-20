using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows;
using BDatos_API.ServiceReference1;
using System.Diagnostics;
using System.Collections.ObjectModel;
using ToastNotifications.Core;

namespace BDatos_API.VISTAS
{
    /// <summary>
    /// Lógica de interacción para Principal.xaml
    /// </summary>
    public partial class Principal : Page,IContenedorCallback
    {
        //modeloPrincipal modelo;
        //Metodos_bd metodos_Bd;
        private readonly ToastViewModel _vm;
        private readonly ObservableCollection<Contenedor> _Importaciones;
        private readonly ObservableCollection<Contenedor> _Exportaciones;
        private readonly ContenedorClient _contenedorClient;

        MessageOptions options=null;
        public Principal()
        {
            InitializeComponent();

            //if (modelo == null) modelo = new modeloPrincipal();
            //this.DataContext = modelo;

            //metodos_Bd = new Metodos_bd();
            _vm = new ToastViewModel();
            var instanceContext = new InstanceContext(this);
            _contenedorClient = new ContenedorClient(instanceContext);
            _contenedorClient.Subscribe();

          
            _Importaciones = new ObservableCollection<Contenedor>(_contenedorClient.obtenerTodasImportaciones().AsEnumerable<Contenedor>());
            this.tabla_importaciones.ItemsSource = _Importaciones;


            _Exportaciones = new ObservableCollection<Contenedor>(_contenedorClient.obtenerTodasExportaciones().AsEnumerable<Contenedor>());
            this.tabla_exportaciones.ItemsSource = _Exportaciones;

            options = new MessageOptions
            {
                FontSize = 30, // set notification font size
                ShowCloseButton = true, // set the option to show or hide notification close button
                FreezeOnMouseEnter = false, // set the option to prevent notification dissapear automatically if user move cursor on it
                NotificationClickAction = n => // set the callback for notification click event
                {
                    n.Close(); // call Close method to remove notification
                }
            };
        }

        public void cambiosExpo(string ID, string BUQUE, string CONTENEDOR, string VIAJE, string FECHA_ENTRADA, string ESTADO,string ALMACEN)
        {
            if (_Exportaciones != null)
            {
                var exportacionIndex = _Exportaciones.IndexOf(_Exportaciones.FirstOrDefault(c => c.ID == ID));
                if (exportacionIndex >= 0)
                {
                    _Exportaciones[exportacionIndex] = new Contenedor { CONTENEDOR = CONTENEDOR, BUQUE = BUQUE, VIAJE = VIAJE, FECHA_ENTRADA = FECHA_ENTRADA, ESTADO = ESTADO, ALMACEN=ALMACEN};

                    this.tabla_exportaciones.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                    {
                        this.tabla_exportaciones.Items.Refresh();
                    }));
                }else
                {
                    _vm.ShowSuccess("NUEVA ENTRADA", options);
                    _Exportaciones.Add(new Contenedor { CONTENEDOR = CONTENEDOR, BUQUE = BUQUE, VIAJE = VIAJE, FECHA_ENTRADA = FECHA_ENTRADA, ESTADO = ESTADO, ALMACEN = ALMACEN });
                }
            }
        }

        public void cambiosImpo(string ID, string BUQUE, string CONTENEDOR, string VIAJE, string FECHA_ENTRADA, string ESTADO,string ALMACEN)
        {
            Debug.WriteLine(ID + " "+BUQUE+" "+CONTENEDOR+" "+VIAJE+" "+FECHA_ENTRADA+" "+ESTADO);
            if (_Importaciones != null)
            {
                var importacionIndex = _Importaciones.IndexOf(_Importaciones.FirstOrDefault(c => c.BUQUE == BUQUE & c.CONTENEDOR==CONTENEDOR)
                    );
                if (importacionIndex >= 0)
                {
                    _Importaciones[importacionIndex] = new Contenedor { CONTENEDOR = CONTENEDOR, BUQUE = BUQUE, VIAJE = VIAJE, FECHA_ENTRADA = FECHA_ENTRADA, ESTADO = ESTADO,ALMACEN=ALMACEN };
                    this.tabla_exportaciones.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                    {
                        this.tabla_exportaciones.Items.Refresh();
                    }));
                }
                else
                {
                    _vm.ShowSuccess("NUEVA ENTRADA",options);
                    _Importaciones.Add(new Contenedor { CONTENEDOR = CONTENEDOR, BUQUE = BUQUE, VIAJE = VIAJE, FECHA_ENTRADA = FECHA_ENTRADA, ESTADO = ESTADO,ALMACEN=ALMACEN });           
                }
            }
        }

        private void DataGridCell_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Tabla_importaciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

