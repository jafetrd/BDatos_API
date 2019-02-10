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
using System.Windows.Data;
using DotNetKit.Windows.Controls;
using System.Collections.Generic;
using System.Windows;
using System.Data;
using System.Threading.Tasks;
using BDatos_API.ContenedoresSimple;

namespace BDatos_API.VISTAS
{
    /// <summary>
    /// Lógica de interacción para Principal.xaml
    /// </summary>
    public partial class Principal : Page, IContenedorCallback, IContenedorSimpleCallback
    {

        #region modelos y variables
        private class filtroBuque
        {
            public string BUQUE { get; set; }
            public int RESULTADOS { get; set; }
        }

        private class filtroViaje
        {
            public string VIAJE { get; set; }
            public int RESULTADOS { get; set; }
        }

        private class filtroEntrada
        {
            public string FECHA_ENTRADA { get; set; }
            public int RESULTADOS { get; set; }
        }

        private class filtroContenedor
        {
            public string CONTENEDOR { get; set; }
            public int RESULTADOS { get; set; }
        }

        private class filtroAlmacen
        {
            public string ALMACEN { get; set; }
            public int RESULTADOS { get; set; }
        }

        private class filtroDias
        {
            public string DIAS { get; set; }
            public int RESULTADOS { get; set; }
        }

        private readonly ToastViewModel _vm;
        private ObservableCollection<Contenedor> _Importaciones;
        private ObservableCollection<Contenedor> _Exportaciones;

        private ObservableCollection<ContenedorSimple> _impo;
        private ObservableCollection<ContenedorSimple> _expo;

        //private List<filtroBuque> buqueFiltro;
        private List<filtroAlmacen> almacenFiltro;
        private List<filtroDias> diasFiltro;
        private List<filtroEntrada> entradaFiltro;
        private List<filtroContenedor> contenedorFiltro;
        private List<filtroViaje> viajeFiltro;

        private List<filtroContenedor> contenedorFiltro2;
        private List<filtroAlmacen> almacenFiltro2;
        private List<filtroDias> diasFiltro2;
        private List<filtroEntrada> entradaFiltro2;

        private ObservableCollection<ContenedorSimple> datosVisibles1;
        private ObservableCollection<ContenedorSimple> datosVisibles1copia;
        private ObservableCollection<Contenedor> datosVisibles2copia;

        public ContenedorClient _contenedorClient;
        public ContenedorSimpleClient _contenedorSimpleClient;

        private int num = 10;
        private int cont = 0;
        private int cont2 = 0;
        private bool desdeBusqueda = false;
        private bool desdeBusqueda2 = false;
        private DataTemplate dataTemplate = null;
        private MessageOptions options=null;

        private IEnumerable<ContenedorSimple> filtro = null;
        private IEnumerable<Contenedor> filtro2 = null;
        //private int cantidadFiltrado = 0;
        //private int cantidadFiltrado2 = 0;

        private FrameworkElementFactory factory;
        private FrameworkElementFactory text;
        ListCollectionView collection;
        ListCollectionView collection2;
        ActualizarAutoCompletado autoCompletado;

        #endregion

        public Principal()
        {
            InitializeComponent();
            //Timeline.DesiredFrameRateProperty.OverrideMetadata(typeof(Timeline), new FrameworkPropertyMetadata { DefaultValue = 5 });
            VIAJE_BUSQUEDA.IsTextSearchCaseSensitive = false;

            var instanceContext = new InstanceContext(this);
            _contenedorClient = new ContenedorClient(instanceContext);
            _contenedorClient.Subscribe();
            _contenedorSimpleClient = new ContenedorSimpleClient(instanceContext);
            _contenedorSimpleClient.Subscribe();

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

            //filtro = _Importaciones;
            //filtro2 = _Exportaciones;

            datosVisibles1 = new ObservableCollection<ContenedorSimple>(_contenedorSimpleClient.obtenerTodasImportaciones());
            datosVisibles1copia = new ObservableCollection<ContenedorSimple>(_contenedorSimpleClient.obtenerTodasImportaciones());

            datosVisibles2copia = new ObservableCollection<Contenedor>(_Exportaciones);
            autoCompletado = new ActualizarAutoCompletado(); //buques en combobox

            construirFiltros();
            cargarFiltros();
            cargarFiltros2();
            colapsarColumnas();
            agrupacionDataGrid();
            paginacionImpo(num, 1);
            paginacionExpo(num, 1);

            //cambioRadioButton(checkViaje, null);
        }


        private async void cargarFiltros()
        {
            //Task t1 = evitarRepeticion();
            if (datosVisibles1copia.Count > 0)
            {
                Task t4 = evitarRepeticion4();
                Task t5 = evitarRepeticion5();
                Task t6 = evitarRepeticion6();
                await Task.WhenAll(t4, t5, t6);
            }
            
            checkViaje.Checked += cambioRadioButton;
            checkBuque.Checked += cambioRadioButton;
            checkContenedor.Checked += cambioRadioButton;
            checkEntrada.Checked += cambioRadioButton;
            checkAlmacen.Checked += cambioRadioButton;
            checkDias.Checked += cambioRadioButton;
        }

        private async void cargarFiltros2()
        {
            if (datosVisibles2copia.Count > 0)
            {
                Task t7 = evitarRepeticion7();
                Task t8 = evitarRepeticion8();
                await Task.WhenAll(t7, t8);
            }
            checkAlmacen2.Checked += cambioRadioButton;
            checkContenedor2.Checked += cambioRadioButton;
            checkDias2.Checked += cambioRadioButton;
            checkEntrada2.Checked += cambioRadioButton;
        }

        private void unsubscribeChecks()
        {
            checkViaje.Checked -= cambioRadioButton;
            checkBuque.Checked -= cambioRadioButton;
            checkContenedor.Checked -= cambioRadioButton;
            checkEntrada.Checked -= cambioRadioButton;
            checkAlmacen.Checked -= cambioRadioButton;
            checkDias.Checked -= cambioRadioButton;

            checkAlmacen2.Checked -= cambioRadioButton;
            checkContenedor2.Checked -= cambioRadioButton;
            checkDias2.Checked -= cambioRadioButton;
            checkEntrada2.Checked -= cambioRadioButton;
        }

        private void construirFiltros()
        {
            //datos el combobox de importacion 
            almacenFiltro = new List<filtroAlmacen>();
            almacenFiltro.Add(new filtroAlmacen { ALMACEN = nombresPatioContenedor.PCONTENEDOR });
            almacenFiltro.Add(new filtroAlmacen { ALMACEN = nombresPatioFerrocarril.PFERROCARRIL });

            diasFiltro = new List<filtroDias>();
            entradaFiltro = new List<filtroEntrada>();
            contenedorFiltro = new List<filtroContenedor>();
            viajeFiltro = new List<filtroViaje>();

            //datos del combobox de exportacion 
            contenedorFiltro2 = new List<filtroContenedor>();
            //almacenFiltro2 = new List<filtroAlmacen>();

            almacenFiltro.Add(new filtroAlmacen { ALMACEN = nombresBodegaC.BODEGA_1 });
            almacenFiltro.Add(new filtroAlmacen { ALMACEN = nombresBodegaC.BODEGA_2 });
            almacenFiltro.Add(new filtroAlmacen { ALMACEN = nombresBodegaC.BODEGA_3 });
            almacenFiltro.Add(new filtroAlmacen { ALMACEN = nombresBodegaC.BODEGA_1 });
            almacenFiltro.Add(new filtroAlmacen { ALMACEN = nombresBodegaC.BODEGA_2 });
            almacenFiltro.Add(new filtroAlmacen { ALMACEN = nombresBodegaC.BODEGA_3 });
            almacenFiltro.Add(new filtroAlmacen { ALMACEN = nombresBodegaC.BODEGA_4 });
            almacenFiltro.Add(new filtroAlmacen { ALMACEN = nombresBodegaC.BODEGA_5 });
            almacenFiltro.Add(new filtroAlmacen { ALMACEN = nombresBodegaC.BODEGA_6 });

            diasFiltro2 = new List<filtroDias>();
            entradaFiltro2 = new List<filtroEntrada>();
        }

        private void colapsarColumnas()
        {
            tabla_importaciones.Columns[0].Visibility = Visibility.Collapsed;
            tabla_importaciones.Columns[3].Visibility = Visibility.Collapsed;
            tabla_importaciones.Columns[5].Visibility = Visibility.Collapsed;
        }

        private void mostrarColumnas()
        {
            tabla_importaciones.Columns[0].Visibility = Visibility.Visible;
            tabla_importaciones.Columns[3].Visibility = Visibility.Visible;
            tabla_importaciones.Columns[5].Visibility = Visibility.Visible;
        }

        private void agrupacionDataGrid()
        {
            if (tabla_importaciones.Columns[0].Visibility == Visibility.Collapsed)
            {
                grupoContenedor.Visibility = Visibility.Collapsed;
            }
            else
            {
                grupoContenedor.Visibility = Visibility.Visible;
            }

            if (tabla_importaciones.Columns[3].Visibility == Visibility.Collapsed)
            {
                grupoFechaEntrada.Visibility = Visibility.Collapsed;
            }
            else
            {
                grupoFechaEntrada.Visibility = Visibility.Visible;
            }

            if (tabla_importaciones.Columns[5].Visibility == Visibility.Collapsed)
            {
                grupoDias.Visibility = Visibility.Collapsed;
            }
            else
            {
                grupoDias.Visibility = Visibility.Visible;
            }
        }

        private void cambioRadioButton(object sender, RoutedEventArgs e)
        {
            dataTemplate = null;
            dataTemplate = new DataTemplate();
            if(factory!=null) factory=null;
            if(text!=null) text=null;

            text = new FrameworkElementFactory(typeof(TextBlock));
            factory = new FrameworkElementFactory(typeof(StackPanel));

            dataTemplate.DataType = typeof(AutoCompleteComboBox);
            if (areaTab.SelectedIndex == 0)
            {
                templateImpo();
            }
            if (areaTab.SelectedIndex == 1)
            {
                templateExpo();
            }
            factory.AppendChild(text);
            dataTemplate.VisualTree = factory;
            if (areaTab.SelectedIndex == 0)
                VIAJE_BUSQUEDA.ItemTemplate = dataTemplate;
            if (areaTab.SelectedIndex == 1)
                VIAJE_BUSQUEDA2.ItemTemplate = dataTemplate;
            if (filtro == null)
            {
                return;
            }

            //datosVisibles1 = filtro;
        }

        private void templateExpo()
        {
            if (checkContenedor2.IsChecked == true)
            {
                VIAJE_BUSQUEDA2.ItemsSource = _Exportaciones;
                VIAJE_BUSQUEDA2.SetValue(TextSearch.TextPathProperty, "CONTENEDOR");
                text.SetBinding(TextBlock.TextProperty, new Binding("CONTENEDOR"));
            }
            if (checkEntrada2.IsChecked == true)
            {
                VIAJE_BUSQUEDA2.ItemsSource = entradaFiltro2;
                VIAJE_BUSQUEDA2.SetValue(TextSearch.TextPathProperty, "FECHA_ENTRADA");
                text.SetBinding(TextBlock.TextProperty, new Binding("FECHA_ENTRADA"));
            }
            if (checkAlmacen2.IsChecked == true)
            {
                VIAJE_BUSQUEDA2.ItemsSource = almacenFiltro;
                VIAJE_BUSQUEDA2.SetValue(TextSearch.TextPathProperty, "ALMACEN");
                text.SetBinding(TextBlock.TextProperty, new Binding("ALMACEN"));
            }
            if (checkDias2.IsChecked == true)
            {
                VIAJE_BUSQUEDA2.ItemsSource = diasFiltro2;
                VIAJE_BUSQUEDA2.SetValue(TextSearch.TextPathProperty, "DIAS");
                text.SetBinding(TextBlock.TextProperty, new Binding("DIAS"));
            }
        }

        private void templateImpo()
        {
           
            if (checkBuque.IsChecked == true)
            {
                VIAJE_BUSQUEDA.ItemsSource = autoCompletado.GetBuques;
                VIAJE_BUSQUEDA.SetValue(TextSearch.TextPathProperty, "BUQUE");
                text.SetBinding(TextBlock.TextProperty, new Binding("BUQUE"));
            }
            if (checkViaje.IsChecked == true)
            {
                VIAJE_BUSQUEDA.ItemsSource = viajeFiltro;
                VIAJE_BUSQUEDA.SetValue(TextSearch.TextPathProperty, "VIAJE");
                text.SetBinding(TextBlock.TextProperty, new Binding("VIAJE"));
            }
            if (checkContenedor.IsChecked == true)
            {
                VIAJE_BUSQUEDA.ItemsSource = _Importaciones;
                VIAJE_BUSQUEDA.SetValue(TextSearch.TextPathProperty, "CONTENEDOR");
                text.SetBinding(TextBlock.TextProperty, new Binding("CONTENEDOR"));
            }
            if (checkEntrada.IsChecked == true)
            {
                VIAJE_BUSQUEDA.ItemsSource = entradaFiltro;
                VIAJE_BUSQUEDA.SetValue(TextSearch.TextPathProperty, "FECHA_ENTRADA");
                text.SetBinding(TextBlock.TextProperty, new Binding("FECHA_ENTRADA"));
            }
            if (checkAlmacen.IsChecked == true)
            {
                VIAJE_BUSQUEDA.ItemsSource = almacenFiltro;
                VIAJE_BUSQUEDA.SetValue(TextSearch.TextPathProperty, "ALMACEN");
                text.SetBinding(TextBlock.TextProperty, new Binding("ALMACEN"));
            }
            if (checkDias.IsChecked == true)
            {
                VIAJE_BUSQUEDA.ItemsSource = diasFiltro;
                VIAJE_BUSQUEDA.SetValue(TextSearch.TextPathProperty, "DIAS");
                text.SetBinding(TextBlock.TextProperty, new Binding("DIAS"));
            }
           
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
                _contenedorSimpleClient.Unsubscribe();
                _contenedorSimpleClient.Subscribe();
            }
            catch(Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
        }

        private void paginacionImpo(int num, int paginaActual)
        {
            cont = datosVisibles1.Count;
            int a = cont;
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
            if(_impo!=null) _impo.Clear();
            if (desdeBusqueda == false)
            {
                _impo = new ObservableCollection<ContenedorSimple>(datosVisibles1.Take(num * paginaActual).Skip(num * (paginaActual - 1)));
                tabla_importaciones.ItemsSource = datosVisibles1copia;
            }
            if (desdeBusqueda == true)
            {
                datosVisibles1 = new ObservableCollection<ContenedorSimple>(filtro.Take(num * paginaActual).Skip(num * (paginaActual - 1)));
                tabla_importaciones.ItemsSource = datosVisibles1;
            }
            //collection = new ListCollectionView(_impo);
            //CheckBox_Checked(grupoAlmacen, null);
            //CheckBox_Checked(grupoBuque, null);
            //tabla_importaciones.ItemsSource = _impo;
        }

        private void paginacionExpo(int num, int paginaActual)
        {
            //cont2 = _Exportaciones.Count;
            //int tamanoPag = 0;
            //if (cont2 % num == 0)
            //{
            //    tamanoPag = cont2 / num;
            //}
            //else
            //{
            //    tamanoPag = cont2 / num + 1;
            //}

            //tbkTotal2.Text = tamanoPag.ToString();
            //tbkCurrentsize2.Text = paginaActual.ToString();

            //if (_expo != null) _expo.Clear();
            //if (desdeBusqueda2 == false)
            //{
            //    _expo = new ObservableCollection<ContenedorSimple>(_Exportaciones.Take(num * paginaActual).Skip(num * (paginaActual - 1)));
            //    tabla_exportaciones.ItemsSource = _expo;
            //}
            //if (desdeBusqueda2 == true)
            //{
            //    filtro2 = new ObservableCollection<ContenedorSimple>(filtro2.Take(num * paginaActual).Skip(num * (paginaActual - 1)));
            //    tabla_exportaciones.ItemsSource = filtro2;
            //}
        
            //collection2 = new ListCollectionView(_expo);
            //CheckBox_Checked(grupoAlmacen2, null);
            //tabla_exportaciones.ItemsSource = _expo;

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
                unsubscribeChecks();
                cargarFiltros();
            }
        }

        public void cambiosImpo(string ID, string BUQUE, string CONTENEDOR, string VIAJE, string FECHA_ENTRADA, string ESTADO,string ALMACEN, string tipo_Cambio,string DIAS)
        {
            if (_Importaciones != null)
            {
                var importacionIndex = _Importaciones.IndexOf(_Importaciones.FirstOrDefault(c => c.BUQUE == BUQUE & c.CONTENEDOR==CONTENEDOR));
                if (importacionIndex >= 0)
                {
                    _Importaciones[importacionIndex] = new Contenedor { CONTENEDOR = CONTENEDOR, BUQUE = BUQUE, VIAJE = VIAJE, FECHA_ENTRADA = FECHA_ENTRADA, ESTADO = ESTADO,ALMACEN=ALMACEN,DIAS=DIAS};
                    //this.tabla_importaciones.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                    //{
                    //    this.tabla_importaciones.Items.Refresh();
                    //}));
                }
                else
                {
                    _vm.ShowSuccess("NUEVA ENTRADA",options);
                    _Importaciones.Add(new Contenedor { CONTENEDOR = CONTENEDOR, BUQUE = BUQUE, VIAJE = VIAJE, FECHA_ENTRADA = FECHA_ENTRADA, ESTADO = ESTADO,ALMACEN=ALMACEN,DIAS=DIAS });           
                }
                unsubscribeChecks();
                cargarFiltros();
            }
        }

        public void cambiosImpoSimple(string BUQUE, string VIAJE, string ALMACEN, string REGIMEN)
        {
            if (datosVisibles1copia != null)
            {
                var visibles = datosVisibles1copia.IndexOf(datosVisibles1copia.FirstOrDefault(c => c.VIAJE == VIAJE));
                if (visibles >= 0)
                {
                    datosVisibles1copia[visibles] = new ContenedorSimple { BUQUE = BUQUE, VIAJE = VIAJE, ALMACEN = ALMACEN };
                    this.tabla_importaciones.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                    {
                        this.tabla_importaciones.Items.Refresh();
                    }));
                }
                else
                {
                    _vm.ShowSuccess("NUEVA ENTRADA", options);
                    datosVisibles1copia.Add(new ContenedorSimple { BUQUE = BUQUE, VIAJE = VIAJE, ALMACEN = ALMACEN });
                }
                unsubscribeChecks();
                cargarFiltros();
            }
        }

        public void cambiosExpoSimple(string BUQUE, string VIAJE, string ALMACEN, string REGIMEN)
        {
           
        }

        private void Tabla_importaciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //DataGrid x = (DataGrid)this.FindName(tabla_importaciones.Name);
            //if ((x.SelectedItem as Contenedor).ALMACEN == "P. CONTENEDOR")
            //{
            //    Console.WriteLine("contenedor");
            //}

            //if ((x.SelectedItem as Contenedor).ALMACEN == "P. FERROCARRIL")
            //{
            //    Console.WriteLine("ferrocarril");
            //}
        }

        private void DataGridCell_KeyDown(object sender, KeyEventArgs e)
        {
            //DataGrid x = (DataGrid)this.FindName(tabla_importaciones.Name);
            //Contenedor a;
            //for (int i = x.SelectedItems.Count - 1; i >= 0; i--)
            //{
            //    a = x.SelectedItem as Contenedor;

            //}
        }

        private void Btn_ultima_Click(object sender, System.Windows.RoutedEventArgs e)
        {
                int paginaActual = int.Parse(tbkCurrentsize.Text);
                if (paginaActual > 1)
                {
                    paginacionImpo(num, paginaActual - 1);
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


        private void BusquedaImpo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //if (tabla_importaciones.Items.Count != datosVisibles1.Count)
                //{
                //    tabla_importaciones.ItemsSource = datosVisibles1;
                //}
                if(string.IsNullOrEmpty(VIAJE_BUSQUEDA.Text)) return;
            }
            if (e.Key == Key.Enter & string.IsNullOrEmpty(VIAJE_BUSQUEDA.Text) == false)
            {
                if (checkViaje.IsChecked == true)
                {
                    filtro = datosVisibles1.Where(busqueda => busqueda.VIAJE.Contains(VIAJE_BUSQUEDA.Text));
                    filtrado();
                }
                if (checkBuque.IsChecked == true)
                {
                    filtro = datosVisibles1.Where(busqueda => busqueda.BUQUE.Contains(VIAJE_BUSQUEDA.Text));
                    filtrado();
                }
                if (checkAlmacen.IsChecked == true)
                {
                    filtro = datosVisibles1.Where(busqueda => busqueda.ALMACEN.Contains(VIAJE_BUSQUEDA.Text));
                    filtrado();
                }
                //if (checkContenedor.IsChecked == true)
                //{
                //    filtro = datosVisibles1.Where(busqueda => busqueda.CONTENEDOR.Contains(VIAJE_BUSQUEDA.Text));
                //    filtrado();
                //}
                //if (checkDias.IsChecked == true)
                //{
                //    filtro = datosVisibles1.Where(busqueda => busqueda.DIAS.Contains(VIAJE_BUSQUEDA.Text));
                //    filtrado();
                //}
                //if (checkEntrada.IsChecked == true)
                //{
                //    filtro = datosVisibles1.Where(busqueda => busqueda.FECHA_ENTRADA.Contains(VIAJE_BUSQUEDA.Text));
                //    filtrado();
                //}
                
            }
        }

        private void BusquedaExpo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //if (tabla_exportaciones.Items.Count != _Exportaciones.Count)
                //{
                //    tabla_exportaciones.ItemsSource = _Exportaciones;
                //}
                if(string.IsNullOrEmpty(VIAJE_BUSQUEDA2.Text))return;
            }
           
            if (e.Key == Key.Enter & string.IsNullOrEmpty(VIAJE_BUSQUEDA2.Text) == false)
            {
                if (checkAlmacen2.IsChecked == true)
                {
                    filtro2 = _Exportaciones.Where(busqueda => busqueda.ALMACEN.Contains(VIAJE_BUSQUEDA2.Text));
                    filtrado();
                }
                if (checkContenedor2.IsChecked == true)
                {
                    filtro2 = _Exportaciones.Where(busqueda => busqueda.CONTENEDOR.Contains(VIAJE_BUSQUEDA2.Text));
                    filtrado();
                }
                if (checkDias2.IsChecked == true)
                {
                    filtro2 = _Exportaciones.Where(busqueda => busqueda.DIAS.Contains(VIAJE_BUSQUEDA2.Text));
                    filtrado();
                }
                if (checkEntrada2.IsChecked == true)
                {
                    filtro2 = _Exportaciones.Where(busqueda => busqueda.FECHA_ENTRADA.Contains(VIAJE_BUSQUEDA2.Text));
                    filtrado();
                }
            }
            
        }

        private void filtrado()
        {
            if (areaTab.SelectedIndex == 0)
            {
                if (filtro == null) return;
                //collection = new ListCollectionView(filtro.ToList());
                //checkbox_check(grupoAlmacen);

                desdeBusqueda = true;
                paginacionImpo(num, 1);
                //Resultados.Text = "Resultados: " + collection.Count;

            }
            if (areaTab.SelectedIndex == 1)
            {
                if (filtro2 == null) return;
                //collection2 = new ListCollectionView(filtro2.ToList());
                //checkbox_check(grupoAlmacen2);
                //tabla_exportaciones.ItemsSource = collection2;

                paginacionExpo(num, 1);
                desdeBusqueda2 = true;
                //Resultados2.Text = "Resultados: " + collection2.Count;
            }
        }


        private void VIAJE_BUSQUEDA_DropDownClosed(object sender, EventArgs e)
        {

            switch (areaTab.SelectedIndex)
            {
                case 0:
                    if (string.IsNullOrEmpty(VIAJE_BUSQUEDA.Text)) return;
                    BusquedaImpo_KeyUp(VIAJE_BUSQUEDA, new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Enter)
                    { RoutedEvent = Keyboard.KeyDownEvent });

                    break;
                case 1:
                    if (string.IsNullOrEmpty(VIAJE_BUSQUEDA2.Text)) return;
                    BusquedaExpo_KeyUp(VIAJE_BUSQUEDA2, new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Enter)
                    { RoutedEvent = Keyboard.KeyDownEvent });
                    break;
            }
        }

        private void Todo_Click(object sender, RoutedEventArgs e)
        {
            if (areaTab.SelectedIndex == 0)
            {
                desdeBusqueda = false;
                collection = null;
                datosVisibles1 = datosVisibles1copia;
                //filtro = null;
                //collection = new ListCollectionView(datosVisibles1);
                //CheckBox_Checked(grupoAlmacen, null);
                //grupoAlmacen.IsChecked = true;
                //CheckBox_Checked(grupoBuque, null);
                //grupoBuque.IsChecked = true;
                //tabla_importaciones
                paginacionImpo(num, 1);
            }
            if (areaTab.SelectedIndex == 1)
            {
                desdeBusqueda2 = false;
                collection2 = null;
                //collection2 = new ListCollectionView(_Exportaciones);
                //CheckBox_Checked(grupoAlmacen2, null);
                //tabla_exportaciones.ItemsSource = _Exportaciones;
                paginacionExpo(num, 1);
            }
        }

        private void Todo_MouseEnter(object sender, MouseEventArgs e)
        {
            if (areaTab.SelectedIndex == 0)
                Resultados.Text = "Mostrar todo";
            if (areaTab.SelectedIndex == 1)
                Resultados2.Text = "Mostrar todo";
        }

        private void Todo_MouseLeave(object sender, MouseEventArgs e)
        {

        }
       
        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {


        }

        #region filtrado para combobox con busqueda
        
        private async Task evitarRepeticion4()
        {
            diasFiltro.Add(new filtroDias { DIAS = _Importaciones[0].DIAS, RESULTADOS = 0 });
            bool salir = false;
            int limite = _Importaciones.Count;
            int b = 0;
            for (int a = 0; a < limite; a++)
            {
                string pivote = diasFiltro[a].DIAS;
                while (b < limite)
                {
                    if (_Importaciones[b].DIAS != pivote)
                    {
                        diasFiltro.Add(new filtroDias { DIAS = _Importaciones[b].DIAS, RESULTADOS = 0 });
                        break;
                    }
                    b++;
                }
                diasFiltro[a].RESULTADOS = b;
                if (b == limite)
                {
                    salir = true;
                    break;
                }
            }
            await Task.FromResult(salir == true);
        }

        private async Task evitarRepeticion5()
        {
            entradaFiltro.Add(new filtroEntrada { FECHA_ENTRADA = _Importaciones[0].FECHA_ENTRADA, RESULTADOS = 0 });
            bool salir = false;
            int limite = _Importaciones.Count;
            int b = 0;
            for (int a = 0; a < limite; a++)
            {
                string pivote = entradaFiltro[a].FECHA_ENTRADA;
                while (b < limite)
                {
                    if (_Importaciones[b].FECHA_ENTRADA != pivote)
                    {
                        entradaFiltro.Add(new filtroEntrada { FECHA_ENTRADA = _Importaciones[b].FECHA_ENTRADA, RESULTADOS = 0 });
                        break;
                    }
                    b++;
                }
                entradaFiltro[a].RESULTADOS = b;
                if (b == limite)
                {
                    salir = true;
                    break;
                }
            }
            await Task.FromResult(salir == true);
        }

        private async Task evitarRepeticion6()
        {
            viajeFiltro.Add(new filtroViaje { VIAJE = _Importaciones[0].VIAJE, RESULTADOS = 0 });
            bool salir = false;
            int limite = _Importaciones.Count;
            int b = 0;
            for (int a = 0; a < limite; a++)
            {
                string pivote = viajeFiltro[a].VIAJE;
                while (b < limite)
                {
                    if (_Importaciones[b].VIAJE != pivote)
                    {
                        viajeFiltro.Add(new filtroViaje { VIAJE = _Importaciones[b].VIAJE, RESULTADOS = 0 });
                        break;
                    }
                    b++;
                }
                viajeFiltro[a].RESULTADOS = b;
                if (b == limite)
                {
                    datosVisibles1copia = datosVisibles1;
                    salir = true;
                    break;
                }
            }
            await Task.FromResult(salir == true);
        }

        private async Task evitarRepeticion7()
        {
            entradaFiltro2.Add(new filtroEntrada { FECHA_ENTRADA = _Exportaciones[0].FECHA_ENTRADA, RESULTADOS = 0 });
            bool salir = false;
            int limite = _Exportaciones.Count;
            int b = 0;
            for (int a = 0; a < limite; a++)
            {
                string pivote = entradaFiltro2[a].FECHA_ENTRADA;
                while (b < limite)
                {
                    if (_Exportaciones[b].FECHA_ENTRADA != pivote)
                    {
                        entradaFiltro2.Add(new filtroEntrada { FECHA_ENTRADA = _Exportaciones[b].FECHA_ENTRADA, RESULTADOS = 0 });
                        break;
                    }
                    b++;
                }
                entradaFiltro2[a].RESULTADOS = b;
                if (b == limite)
                {
                    salir = true;
                    break;
                }
            }
            await Task.FromResult(salir == true);
        }

        private async Task evitarRepeticion8()
        {
            diasFiltro2.Add(new filtroDias { DIAS = _Exportaciones[0].DIAS, RESULTADOS = 0 });
            bool salir = false;
            int limite = _Exportaciones.Count;
            int b = 0;
            for (int a = 0; a < limite; a++)
            {
                string pivote = diasFiltro2[a].DIAS;
                while (b < limite)
                {
                    if (_Exportaciones[b].DIAS != pivote)
                    {
                        diasFiltro2.Add(new filtroDias { DIAS = _Exportaciones[b].DIAS, RESULTADOS = 0 });
                        break;
                    }
                    b++;
                }
                diasFiltro2[a].RESULTADOS = b;
                if (b == limite)
                {
                    salir = true;
                    break;
                }
            }
            await Task.FromResult(salir == true);
        }
        #endregion

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            //checkbox_check(sender);
            //if (areaTab.SelectedIndex == 0)
            //    tabla_importaciones.ItemsSource = collection;
            //if (areaTab.SelectedIndex == 1)
            //    tabla_exportaciones.ItemsSource = collection2;
        }

        private void checkbox_check(object sender)
        {
            //string name = (sender as CheckBox).Content.ToString();
            //if(areaTab.SelectedIndex==0)
            //collection.GroupDescriptions.Add(new PropertyGroupDescription(name));
            //if(areaTab.SelectedIndex==1)
            //collection2.GroupDescriptions.Add(new PropertyGroupDescription(name));
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            //checkbox_uncheck(sender);
            //if (areaTab.SelectedIndex == 0)
            //    tabla_importaciones.ItemsSource = collection;
            //if (areaTab.SelectedIndex == 1)
            //    tabla_importaciones.ItemsSource = collection2;
        }

        private void checkbox_uncheck(object sender)
        {
            //string name = (sender as CheckBox).Content.ToString();
            //if (areaTab.SelectedIndex == 0)
            //{
            //    var remover = collection.GroupDescriptions.OfType<PropertyGroupDescription>().FirstOrDefault(pgd => pgd.PropertyName == name);
            //    collection.GroupDescriptions.Remove(remover);
            //}
            //if (areaTab.SelectedIndex == 1)
            //{
            //    var remover2 = collection2.GroupDescriptions.OfType<PropertyGroupDescription>().FirstOrDefault(pgd => pgd.PropertyName == name);
            //    collection2.GroupDescriptions.Remove(remover2);
            //}
        }

    }
}

