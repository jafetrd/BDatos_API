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
using System.Diagnostics;
using System.Globalization;

namespace BDatos_API.VISTAS
{
    /// <summary>
    /// Lógica de interacción para Principal.xaml
    /// </summary>
    public partial class Principal : Page, IContenedorCallback
    {
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

        private ObservableCollection<Contenedor> _impo;
        private ObservableCollection<Contenedor> _expo;

        private List<filtroBuque> buqueFiltro;
        private List<filtroAlmacen> almacenFiltro;
        private List<filtroDias> diasFiltro;
        private List<filtroEntrada> entradaFiltro;
        private List<filtroContenedor> contenedorFiltro;
        private List<filtroViaje> viajeFiltro;

        private List<filtroContenedor> contenedorFiltro2;
        private List<filtroAlmacen> almacenFiltro2;
        private List<filtroDias> diasFiltro2;
        private List<filtroEntrada> entradaFiltro2;

        public ContenedorClient _contenedorClient;
        private int num = 15;
        private int cont = 0;
        private int cont2 = 0;
        private DataTemplate dataTemplate = null;
        private MessageOptions options=null;

        private IEnumerable<Contenedor> filtro = null;
        private IEnumerable<Contenedor> filtro2 = null;
        private int cantidadFiltrado = 0;
        private int cantidadFiltrado2 = 0;

        private FrameworkElementFactory factory;
        private FrameworkElementFactory text; 

        public Principal()
        {
            InitializeComponent();

            VIAJE_BUSQUEDA.IsTextSearchCaseSensitive = false;

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

            filtro = _Importaciones;
            filtro2 = _Exportaciones;

            paginacionImpo(num, 1);
            paginacionExpo(num, 1);
            //datos el combobox de importacion 
            buqueFiltro = new List<filtroBuque>();
            almacenFiltro = new List<filtroAlmacen>();
            almacenFiltro.Add(new filtroAlmacen { ALMACEN = nombresPatioContenedor.PCONTENEDOR });
            almacenFiltro.Add(new filtroAlmacen { ALMACEN = nombresPatioFerrocarril.PFERROCARRIL});

            diasFiltro = new List<filtroDias>();
            entradaFiltro = new List<filtroEntrada>();
            contenedorFiltro = new List<filtroContenedor>();
            viajeFiltro = new List<filtroViaje>();

            //datos del combobox de exportacion 
            contenedorFiltro2 = new List<filtroContenedor>();
            almacenFiltro2 = new List<filtroAlmacen>();

            almacenFiltro2.Add(new filtroAlmacen { ALMACEN = nombresBodegaC.BODEGA_1});
            almacenFiltro2.Add(new filtroAlmacen { ALMACEN = nombresBodegaC.BODEGA_2});
            almacenFiltro2.Add(new filtroAlmacen { ALMACEN = nombresBodegaC.BODEGA_3});
            almacenFiltro2.Add(new filtroAlmacen { ALMACEN = nombresBodegaC.BODEGA_1});
            almacenFiltro2.Add(new filtroAlmacen { ALMACEN = nombresBodegaC.BODEGA_2});
            almacenFiltro2.Add(new filtroAlmacen { ALMACEN = nombresBodegaC.BODEGA_3});
            almacenFiltro2.Add(new filtroAlmacen { ALMACEN = nombresBodegaC.BODEGA_4});
            almacenFiltro2.Add(new filtroAlmacen { ALMACEN = nombresBodegaC.BODEGA_5});
            almacenFiltro2.Add(new filtroAlmacen { ALMACEN = nombresBodegaC.BODEGA_6});

            diasFiltro2 = new List<filtroDias>();
            entradaFiltro2 = new List<filtroEntrada>();

            cargarFiltros();
            
        }

        private async void cargarFiltros()
        {
            Task t1 = evitarRepeticion();
            Task t4 = evitarRepeticion4();
            Task t5 = evitarRepeticion5();
            Task t6 = evitarRepeticion6();
            Task t7 = evitarRepeticion7();
            Task t8 = evitarRepeticion8();
            await Task.WhenAll(t1, t4, t5, t6,t7,t8);

            checkViaje.Click += cambioRadioButton;
            checkBuque.Click += cambioRadioButton;
            checkContenedor.Click += cambioRadioButton;
            checkEntrada.Click += cambioRadioButton;
            checkAlmacen.Click += cambioRadioButton;
            checkDias.Click += cambioRadioButton;

            checkAlmacen2.Click += cambioRadioButton;
            checkContenedor2.Click += cambioRadioButton;
            checkDias2.Click += cambioRadioButton;
            checkEntrada2.Click += cambioRadioButton;
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
            
            //text.SetBinding(TextBlock.TextProperty, new Binding("RESULTADOS"));
        }

        private void templateImpo()
        {
            if (checkBuque.IsChecked == true)
            {
                VIAJE_BUSQUEDA.ItemsSource = buqueFiltro;
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
            //text.SetBinding(TextBlock.TextProperty, new Binding("RESULTADOS"));
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
                _expo = _Exportaciones;
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
                _impo = _Importaciones;
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
            if (string.IsNullOrEmpty(VIAJE_BUSQUEDA.Text) == true)
            {
                if (tabla_importaciones.Items.Count != _Importaciones.Count)
                {
                    tabla_importaciones.ItemsSource = _Importaciones;
                }
            }
            else
            {
                if (e.Key == Key.Enter & string.IsNullOrEmpty(VIAJE_BUSQUEDA.Text) == false)
                {
                    if (checkViaje.IsChecked == true)
                    {
                        filtro = _Importaciones.Where(busqueda => busqueda.VIAJE.Contains(VIAJE_BUSQUEDA.Text));
                    }
                    if (checkBuque.IsChecked == true)
                    {
                        filtro = _Importaciones.Where(busqueda => busqueda.BUQUE.Contains(VIAJE_BUSQUEDA.Text));
                    }
                    if (checkAlmacen.IsChecked == true)
                    {
                        filtro = _Importaciones.Where(busqueda => busqueda.ALMACEN.Contains(VIAJE_BUSQUEDA.Text));
                    }
                    if (checkContenedor.IsChecked == true)
                    {
                        filtro = _Importaciones.Where(busqueda => busqueda.CONTENEDOR.Contains(VIAJE_BUSQUEDA.Text));
                    }
                    if (checkDias.IsChecked == true)
                    {
                        filtro = _Importaciones.Where(busqueda => busqueda.DIAS.Contains(VIAJE_BUSQUEDA.Text));
                    }
                    if (checkEntrada.IsChecked == true)
                    {
                        filtro = _Importaciones.Where(busqueda => busqueda.FECHA_ENTRADA.Contains(VIAJE_BUSQUEDA.Text));
                    }
                    filtrado();
                }
            }
        }

        private void BusquedaExpo_KeyUp(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(VIAJE_BUSQUEDA2.Text) == true)
            {
                if (tabla_exportaciones.Items.Count != _Exportaciones.Count)
                {
                    tabla_exportaciones.ItemsSource = _Exportaciones;
                }
            }
            else
            {
                if (e.Key == Key.Enter & string.IsNullOrEmpty(VIAJE_BUSQUEDA2.Text) == false)
                {
                    if (checkAlmacen2.IsChecked == true)
                    {
                        filtro2 = _Exportaciones.Where(busqueda => busqueda.ALMACEN.Contains(VIAJE_BUSQUEDA2.Text));
                    }
                    if (checkContenedor2.IsChecked == true)
                    {
                        filtro2 = _Exportaciones.Where(busqueda => busqueda.CONTENEDOR.Contains(VIAJE_BUSQUEDA2.Text));
                    }
                    if (checkDias2.IsChecked == true)
                    {
                        filtro2 = _Exportaciones.Where(busqueda => busqueda.DIAS.Contains(VIAJE_BUSQUEDA2.Text));
                    }
                    if (checkEntrada2.IsChecked == true)
                    {
                        filtro2 = _Exportaciones.Where(busqueda => busqueda.FECHA_ENTRADA.Contains(VIAJE_BUSQUEDA2.Text));
                    }
                    filtrado();
                }
            }
        }

        private void filtrado()
        {
                if (areaTab.SelectedIndex == 0)
                {
                    cantidadFiltrado = filtro.Count<Contenedor>();
                    if (cantidadFiltrado > 0)
                    {
                        tabla_importaciones.ItemsSource = filtro;
                        Resultados.Text = "Resultados: " + cantidadFiltrado;
                }
                    else
                    {
                        Resultados.Text = "Resultados: " + 0;
                    }
                }
                if (areaTab.SelectedIndex == 1)
                {
                    cantidadFiltrado2 = filtro2.Count<Contenedor>();
                    if (cantidadFiltrado2 > 0)
                    {
                        tabla_exportaciones.ItemsSource = filtro2;
                        Resultados2.Text = "Resultados: " + cantidadFiltrado2;
                    }
                    else
                    {
                        Resultados2.Text = "Resultados: " + 0;
                    }
                }
        }

        private void VIAJE_BUSQUEDA_DropDownClosed(object sender, EventArgs e)
        {
            if (areaTab.SelectedIndex == 0)
                BusquedaImpo_KeyUp(null, new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Enter)
                { RoutedEvent = Keyboard.KeyDownEvent });
            if (areaTab.SelectedIndex == 1)
                BusquedaExpo_KeyUp(null, new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Enter)
                { RoutedEvent = Keyboard.KeyDownEvent });
        }

        private void Todo_Click(object sender, RoutedEventArgs e)
        {
            if(areaTab.SelectedIndex == 0)
                tabla_importaciones.ItemsSource = _Importaciones;
            if (areaTab.SelectedIndex == 1)
                tabla_exportaciones.ItemsSource = _Exportaciones;
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
            if (areaTab.SelectedIndex == 0)
                Resultados.Text = "Resultados: " + cantidadFiltrado;
            if (areaTab.SelectedIndex == 1)
                Resultados2.Text = "Resultados: " + cantidadFiltrado2;
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        #region filtrado para combobox con busqueda
        private async Task evitarRepeticion()
        {
            buqueFiltro.Add(new filtroBuque { BUQUE = _Importaciones[0].BUQUE,RESULTADOS=0 });
            bool salir = false;
            int limite = _Importaciones.Count;
            int b = 0;
            for (int a = 0; a < limite; a++)
            {
                string pivote = buqueFiltro[a].BUQUE;
                 while (b < limite)
                {
                    if (_Importaciones[b].BUQUE != pivote)
                    {
                        buqueFiltro.Add(new filtroBuque {BUQUE= _Importaciones[b].BUQUE,RESULTADOS=0 });
                        break;
                    }
                    b++;   
                }
                buqueFiltro[a].RESULTADOS = b;
                if (b == limite)
                {
                    salir = true;
                    break;
                }
            }
            await Task.FromResult(salir == true);
        }
        
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
    }
}

