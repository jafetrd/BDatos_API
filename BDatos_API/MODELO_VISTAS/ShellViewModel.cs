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
            this.Menu.Add(new ElementoMenu() {
                Icon = new PackIconFontAwesome() {Kind = PackIconFontAwesomeKind.HomeSolid},
                Text = "Principal",
                NavigationDestination = new Uri("VISTAS/Principal.xaml", UriKind.RelativeOrAbsolute),
                Tooltip = "Pagina principal"
            });

            this.Menu.Add(new ElementoMenu() {
                Icon = new PackIconFontAwesome() {Kind = PackIconFontAwesomeKind.SearchSolid },
                Text = "Busqueda y reportes",
                NavigationDestination = new Uri("VISTAS/Busquedayreportes.xaml", UriKind.RelativeOrAbsolute),
                Tooltip = "Busqueda y reportes"
            });

            this.Menu.Add(new ElementoMenu() {
                Icon = new PackIconFontAwesome() {Kind = PackIconFontAwesomeKind.ShipSolid},
                Text = "Patio de contenedores",
                NavigationDestination = new Uri("VISTAS/Patio_Contenedor.xaml", UriKind.RelativeOrAbsolute),
                Tooltip = "Patio de contenedores"
            });

            this.Menu.Add(new ElementoMenu() {
                Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.TrainSolid },
                Text = "Patio de ferrocarriles",
                NavigationDestination = new Uri("VISTAS/Patio_Ferrocarril.xaml", UriKind.RelativeOrAbsolute),
                Tooltip = "Patio de ferrocarriles"
            });

            this.Menu.Add(new ElementoMenu() {
                Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.WarehouseSolid},
                Text = "Bodega C",
                NavigationDestination = new Uri("VISTAS/Bodega.xaml", UriKind.RelativeOrAbsolute),
                Tooltip = "Bodega C"
            });

            this.Menu.Add(new ElementoMenu() {
                Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.WarehouseSolid },
                Text = "Bodega 2",
                NavigationDestination = new Uri("VISTAS/Bodega2.xaml", UriKind.RelativeOrAbsolute),
                Tooltip = "Bodega 2"
            });

            this.OptionsMenu.Add(new ElementoMenu() { Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.CogsSolid }, Text = "Configuracion", NavigationDestination = new Uri("Views/SettingsPage.xaml", UriKind.RelativeOrAbsolute) });
            this.OptionsMenu.Add(new ElementoMenu() { Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.InfoCircleSolid }, Text = "Acerca de", NavigationDestination = new Uri("Views/AboutPage.xaml", UriKind.RelativeOrAbsolute) });
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