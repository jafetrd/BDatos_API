using System.Collections.ObjectModel;
using BDatos_API.MVVM;


namespace BDatos_API.MODELO_VISTAS
{
    internal class modeloVistaBase : BindableBase
    {
        private static readonly ObservableCollection<ElementoMenu> _Menu = new ObservableCollection<ElementoMenu>();
        private static readonly ObservableCollection<ElementoMenu> _OpcionesMenu = new ObservableCollection<ElementoMenu>();

        public modeloVistaBase()
        {
        }

        public ObservableCollection<ElementoMenu> Menu => _Menu;

        public ObservableCollection<ElementoMenu> OptionsMenu => _OpcionesMenu;
    }
}