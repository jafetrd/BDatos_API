using System;
using System.Linq;
using MahApps.Metro.IconPacks;

namespace BDatos_API.MODELO_VISTAS
{
    internal class ShellViewModel : modeloVistaBase
    {
        public ShellViewModel()
        {
           // Build the menus
            this.Menu.Add(new ElementoMenu() { Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.ShipSolid}, Text = "Patio de contenedores", NavigationDestination = new Uri("VISTAS/Patio_Contenedor.xaml", UriKind.RelativeOrAbsolute) });
            this.Menu.Add(new ElementoMenu() { Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.TrainSolid }, Text = "Patio de ferrocarriles", NavigationDestination = new Uri("VISTAS/Patio_Ferrocarril.xaml", UriKind.RelativeOrAbsolute) });
            this.Menu.Add(new ElementoMenu() { Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.WarehouseSolid}, Text = "Bodega 1", NavigationDestination = new Uri("VISTAS/Bodega.xaml", UriKind.RelativeOrAbsolute) });
            this.Menu.Add(new ElementoMenu() { Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.WarehouseSolid }, Text = "Bodega 2", NavigationDestination = new Uri("VISTAS/Bodega.xaml", UriKind.RelativeOrAbsolute) });

            this.OptionsMenu.Add(new ElementoMenu() { Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.CogsSolid }, Text = "Settings", NavigationDestination = new Uri("Views/SettingsPage.xaml", UriKind.RelativeOrAbsolute) });
            this.OptionsMenu.Add(new ElementoMenu() { Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.InfoCircleSolid }, Text = "About", NavigationDestination = new Uri("Views/AboutPage.xaml", UriKind.RelativeOrAbsolute) });
        }

        public object GetItem(object uri)
        {
            return null == uri ? null : this.Menu.FirstOrDefault(m => m.NavigationDestination.Equals(uri));
        }

        public object GetOptionsItem(object uri)
        {
            return null == uri ? null : this.OptionsMenu.FirstOrDefault(m => m.NavigationDestination.Equals(uri));
        }
    }
}