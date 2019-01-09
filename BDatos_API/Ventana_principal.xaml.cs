using System;
using BDatos_API.MODELO_VISTAS;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using MahApps.Metro.Controls;
using ElementoMenu = BDatos_API.MODELO_VISTAS.ElementoMenu;
using System.Windows.Threading;
using BDatos_API.VISTAS;
using static BDatos_API.nombresVentanas;
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
        Bodega2 bodega2;
        Patio_Ferrocarril ferrocarril;
        Patio_Contenedor contenedor;
        Principal principal;
        Busquedayreportes busquedayreportes;

        public static string nombreVentana;
        public Ventana_principal()
        {
            InitializeComponent();

            Navegacion.Frame = new Frame()
            {
                NavigationUIVisibility = NavigationUIVisibility.Hidden
            };
            Navegacion.Frame.Navigated += SplitViewFrame_OnNavigated;
            Usuario.Text = "Usuario: "+USUARIOdato + " Privilegios: "+TIPO_USUARIOdato+" ";


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
                fase = Entrada;
                nombreVentana = menuItem.Text;
                switch (nombreVentana)
                {
                    case nombresVentanas.PatioContenedores:
                        if (contenedor == null) contenedor = new Patio_Contenedor();
                        Navegacion.NavegarA(contenedor);
                        break;
                    case nombresVentanas.PatioFerrocarriles:
                        if (ferrocarril == null) ferrocarril = new Patio_Ferrocarril();
                        Navegacion.NavegarA(ferrocarril);
                        break;
                    case nombresVentanas.Bodega2:
                        if (bodega2 == null) bodega2 = new Bodega2();
                        Navegacion.NavegarA(bodega2);
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
                }
                
            }
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (DispatcherOperationCallback)delegate (object o)
            {
                Navegacion.NavegarAtras();
                return null;
            }, null);

            e.Cancel = true;
        }
    }
}
