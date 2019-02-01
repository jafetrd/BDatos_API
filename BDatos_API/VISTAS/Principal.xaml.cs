using System;
using System.Linq;
using System.ServiceModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using BDatos_API.servicioContenedores;
using System.Collections.ObjectModel;
using ToastNotifications.Core;
using System.Timers;

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
        private ObservableCollection<Contenedor> _Importaciones;
        private ObservableCollection<Contenedor> _Exportaciones;

        private ObservableCollection<Contenedor> _impo;
        private ObservableCollection<Contenedor> _expo;

        public ContenedorClient _contenedorClient;
        Metodos_bd metodos_Bd;
        private int num = 15;
        private int cont = 0;
        private int cont2 = 0;

        MessageOptions options=null;
        public Principal()
        {
            InitializeComponent();

            //if (modelo == null) modelo = new modeloPrincipal();
            //this.DataContext = modelo;

            //metodos_Bd = new Metodos_bd();
            var instanceContext = new InstanceContext(this);
            _contenedorClient = new ContenedorClient(instanceContext);
            _contenedorClient.Subscribe();

            Timer timer = new Timer();
            timer.Interval = 180000;
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Start();

            _vm = new ToastViewModel();
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

            _Exportaciones = new ObservableCollection<Contenedor>(_contenedorClient.obtenerTodasExportaciones().AsEnumerable());
            _Importaciones = new ObservableCollection<Contenedor>(_contenedorClient.obtenerTodasImportaciones().AsEnumerable());

            _impo = _Importaciones;
            _expo = _Exportaciones;
            paginacionImpo(num, 1);
            paginacionExpo(num, 1);
            metodos_Bd = new Metodos_bd(); 
        }


        private void Principal_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _contenedorClient?.Unsubscribe();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                _contenedorClient.Unsubscribe();
                _contenedorClient?.Subscribe();
            }
            catch{ }
        }

        private void paginacionImpo(int num, int paginaActual)
        {
            cont = _Importaciones.Count;
            int tamanoPag = 0;
            if (cont % num == 0)
            {
                tamanoPag = cont / num;
            }
            else
            {
                tamanoPag = cont / num + 1;
            }
            
            tbkTotal.Text = tamanoPag.ToString();
            tbkCurrentsize.Text = paginaActual.ToString();

            _impo = new ObservableCollection<Contenedor>(_Importaciones.Take(num * paginaActual).Skip(num * (paginaActual - 1)));
            tabla_importaciones.ItemsSource = _impo;
        }

        private void paginacionExpo(int num, int paginaActual)
        {
            cont2 = _Exportaciones.Count;
            int tamanoPag = 0;
            if (cont2 % num == 0)
            {
                tamanoPag = cont2 / num;
            }
            else
            {
                tamanoPag = cont2 / num + 1;
            }

            tbkTotal2.Text = tamanoPag.ToString();
            tbkCurrentsize2.Text = paginaActual.ToString();

            _expo = new ObservableCollection<Contenedor>(_Exportaciones.Take(num * paginaActual).Skip(num * (paginaActual - 1)));
            tabla_exportaciones.ItemsSource = _expo;

        }

        public void cambiosExpo(string ID, string BUQUE, string CONTENEDOR, string VIAJE, string FECHA_ENTRADA, string ESTADO,string ALMACEN, string tipo_Cambio,string DIAS)
        {
            if (_Exportaciones != null)
            {
                var exportacionIndex = _Exportaciones.IndexOf(_Exportaciones.FirstOrDefault(c => c.BUQUE == BUQUE & c.CONTENEDOR == CONTENEDOR));
                if (exportacionIndex >= 0)
                {
                    _Exportaciones[exportacionIndex] = new Contenedor { CONTENEDOR = CONTENEDOR, BUQUE = BUQUE, VIAJE = VIAJE, FECHA_ENTRADA = FECHA_ENTRADA, ESTADO = ESTADO, ALMACEN=ALMACEN,DIAS=DIAS};

                    this.tabla_exportaciones.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                    {
                        this.tabla_exportaciones.Items.Refresh();
                    }));
                }else
                {
                    _vm.ShowSuccess("NUEVA ENTRADA", options);
                    _Exportaciones.Add(new Contenedor { CONTENEDOR = CONTENEDOR, BUQUE = BUQUE, VIAJE = VIAJE, FECHA_ENTRADA = FECHA_ENTRADA, ESTADO = ESTADO, ALMACEN = ALMACEN,DIAS=DIAS });
                }
            }
        }

        public void cambiosImpo(string ID, string BUQUE, string CONTENEDOR, string VIAJE, string FECHA_ENTRADA, string ESTADO,string ALMACEN, string tipo_Cambio,string DIAS)
        {
            if (_Importaciones != null)
            {
                var importacionIndex = _Importaciones.IndexOf(_Importaciones.FirstOrDefault(c => c.BUQUE == BUQUE & c.CONTENEDOR==CONTENEDOR)
                    );
                if (importacionIndex >= 0)
                {
                    _Importaciones[importacionIndex] = new Contenedor { CONTENEDOR = CONTENEDOR, BUQUE = BUQUE, VIAJE = VIAJE, FECHA_ENTRADA = FECHA_ENTRADA, ESTADO = ESTADO,ALMACEN=ALMACEN,DIAS=DIAS};
                    this.tabla_exportaciones.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                    {
                        this.tabla_exportaciones.Items.Refresh();
                    }));
                }
                else
                {
                    _vm.ShowSuccess("NUEVA ENTRADA",options);
                    _Importaciones.Add(new Contenedor { CONTENEDOR = CONTENEDOR, BUQUE = BUQUE, VIAJE = VIAJE, FECHA_ENTRADA = FECHA_ENTRADA, ESTADO = ESTADO,ALMACEN=ALMACEN,DIAS=DIAS });           
                }   
            }
        }

        private void Tabla_importaciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid x = (DataGrid)this.FindName(tabla_importaciones.Name);

            if ((x.SelectedItem as Contenedor).ALMACEN== "P. CONTENEDOR")
            {
                Console.WriteLine("contenedor");
            }

            if ((x.SelectedItem as Contenedor).ALMACEN == "P. FERROCARRIL")
            {
                Console.WriteLine("ferrocarril");
            }

        }

        private void DataGridCell_KeyDown(object sender, KeyEventArgs e)
        {
            DataGrid x = (DataGrid)this.FindName(tabla_importaciones.Name);
            Contenedor a;
            for (int i = x.SelectedItems.Count - 1; i >= 0; i--)
            {
                a = x.SelectedItem as Contenedor;

            }
        }

        private void Btn_ultima_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            int paginaActual = int.Parse(tbkCurrentsize.Text);
            if (paginaActual > 1)
            {
                paginacionImpo(num, paginaActual-1);
            }
        }

        private void Btn_siguiente_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            int total = int.Parse(tbkTotal.Text);
            int paginaActual = int.Parse(tbkCurrentsize.Text);
            if (paginaActual < total)
            {
                paginacionImpo(num, paginaActual + 1);
            }
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void Btn_ultima2_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            int paginaActual = int.Parse(tbkCurrentsize2.Text);
            if (paginaActual > 1)
            {
                paginacionExpo(num, paginaActual - 1);
            }
        }

        private void Btn_siguiente2_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            int total = int.Parse(tbkTotal2.Text);
            int paginaActual = int.Parse(tbkCurrentsize2.Text);
            if (paginaActual < total)
            {
                paginacionExpo(num, paginaActual + 1);
            }
        }
    }
}

