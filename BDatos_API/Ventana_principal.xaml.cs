using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using MahApps.Metro.Controls;
using ElementoMenu = BDatos_API.MODELO_VISTAS.ElementoMenu;
using System.Windows.Threading;
using BDatos_API.VISTAS;
using static BDatos_API.InicioSesion;
using static BDatos_API.Maquina_estados;

namespace BDatos_API
{
    /// <summary>
    /// Lógica de interacción para Ventana_principal.xaml
    /// </summary>
    public partial class Ventana_principal : MetroWindow
    {
        

        Bodega bodega;
        public  Patio_Ferrocarril ferrocarril;
        public  Patio_Contenedor contenedor;
        Principal principal;
        Busquedayreportes busquedayreportes;
        configuracion config;

        public static string nombreVentana;
        public Ventana_principal()
        {
            InitializeComponent();
            if (Navegacion.Frame == null)
            Navegacion.Frame = new Frame()
            {
                NavigationUIVisibility = NavigationUIVisibility.Hidden
            };
            Navegacion.Frame.Navigated += SplitViewFrame_OnNavigated;
            Usuario.Text = "Usuario: "+USUARIOdato + " Privilegios: "+TIPO_USUARIOdato+" ";
            if (principal == null) principal = new Principal();

            principal._contenedorClient.Subscribe();
            Navegacion.NavegarA(principal);
        }

        private void SplitViewFrame_OnNavigated(object sender, NavigationEventArgs e)
        {

            this.HamburgerMenuControl.Content = e.Content;
            //this.HamburgerMenuControl.SelectedItem = e.ExtraData ?? ((ShellViewModel)this.DataContext).GetItem(e.Uri);
            //this.HamburgerMenuControl.SelectedOptionsItem = e.ExtraData ?? ((ShellViewModel)this.DataContext).GetOptionsItem(e.Uri);
           // GoBackButton.Visibility = Navegacion.Frame.CanGoBack ? Visibility.Visible : Visibility.Collapsed;
        }

        //private void GoBack_OnClick(object sender, RoutedEventArgs e)
        //{
        //    if (Navegacion.Frame.CanGoBack == false)
        //        Navegacion.NavegarAtras();
        //    else
        //        Navegacion.Regresar_frame();
        //}

        private void HamburgerMenuControl_OnItemInvoked(object sender, HamburgerMenuItemInvokedEventArgs e)
        {
            var menuItem = e.InvokedItem as ElementoMenu;
            if (menuItem != null && menuItem.IsNavigation)
            {
                _ESTADO = Entrada;
                nombreVentana = menuItem.Text;
                switch (nombreVentana)
                {
                    case nombresVentanas.PatioContenedores:
                        if (contenedor == null) contenedor = new Patio_Contenedor(Entrada,nombresPatioContenedor.IMPORTACION);
                        Navegacion.NavegarA(contenedor);
                        break;
                    case nombresVentanas.PatioFerrocarriles:
                        if (ferrocarril == null) ferrocarril = new Patio_Ferrocarril(Entrada,nombresPatioFerrocarril.IMPORTACION);
                        Navegacion.NavegarA(ferrocarril);
                        break;
                    case nombresVentanas.BodegaC:
                        if (bodega == null) bodega = new Bodega();
                        Navegacion.NavegarA(bodega);
                        break;
                    case nombresVentanas.Principal:
                        if (principal == null) principal = new Principal();
                        Navegacion.NavegarA(principal);
                        break;
                    case nombresVentanas.BusquedayReportes:
                        if (busquedayreportes == null) busquedayreportes = new Busquedayreportes();
                        Navegacion.NavegarA(busquedayreportes);
                        break;
                    case nombresVentanas.Configuracion:
                        if (config == null) config = new configuracion();
                        Navegacion.NavegarA(config);
                        break;
                }
                
            }
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            principal._contenedorClient.Unsubscribe();
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (DispatcherOperationCallback)delegate (object o)
            {
                Navegacion.NavegarAtras();
                return null;
            }, null);

            e.Cancel = true;
        }
    }
}
