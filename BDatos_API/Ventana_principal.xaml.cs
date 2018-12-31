using System;
using BDatos_API.MODELO_VISTAS;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using MahApps.Metro.Controls;
using ElementoMenu = BDatos_API.MODELO_VISTAS.ElementoMenu;
using System.Windows.Threading;

namespace BDatos_API
{
    /// <summary>
    /// Lógica de interacción para Ventana_principal.xaml
    /// </summary>
    public partial class Ventana_principal : MetroWindow
    {
        public Ventana_principal()
        {
            InitializeComponent();
            Navegacion.Frame = new Frame()
            {
                NavigationUIVisibility = NavigationUIVisibility.Hidden
            };
            Navegacion.Frame.Navigated += SplitViewFrame_OnNavigated;
        }

        private void SplitViewFrame_OnNavigated(object sender, NavigationEventArgs e)
        {
            this.HamburgerMenuControl.Content = e.Content;
            this.HamburgerMenuControl.SelectedItem = e.ExtraData ?? ((ShellViewModel)this.DataContext).GetItem(e.Uri);
            this.HamburgerMenuControl.SelectedOptionsItem = e.ExtraData ?? ((ShellViewModel)this.DataContext).GetOptionsItem(e.Uri);
            GoBackButton.Visibility = Navegacion.Frame.CanGoBack ? Visibility.Visible : Visibility.Collapsed;
        }

        private void GoBack_OnClick(object sender, RoutedEventArgs e)
        {
            if (Navegacion.Frame.CanGoBack == false)
                Navegacion.NavegarAtras();
            else
                Navegacion.Regresar_frame();
        }

        private void HamburgerMenuControl_OnItemInvoked(object sender, HamburgerMenuItemInvokedEventArgs e)
        {
            var menuItem = e.InvokedItem as ElementoMenu;
            if (menuItem != null && menuItem.IsNavigation)
            {
                Navegacion.NavegarA(menuItem.NavigationDestination, menuItem);
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
