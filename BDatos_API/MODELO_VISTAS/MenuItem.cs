using System;
using System.Windows.Input;
using BDatos_API.MVVM;

namespace BDatos_API.MODELO_VISTAS
{
    internal class ElementoMenu : BindableBase
    {
        private object _icono;
        private string _texto;
        private bool _estaActivado = true;
        private DelegateCommand _comando;
        private Uri _Destino;
        private object _tooltip;

        public object Icon
        {
            get { return this._icono; }
            set { this.SetProperty(ref this._icono, value); }
        }

        public string Text
        {
            get { return this._texto; }
            set { this.SetProperty(ref this._texto, value); }
        }

        public bool IsEnabled
        {
            get { return this._estaActivado; }
            set { this.SetProperty(ref this._estaActivado, value); }
        }

        public ICommand Command
        {
            get { return this._comando; }
            set { this.SetProperty(ref this._comando, (DelegateCommand)value); }
        }

        public Uri NavigationDestination
        {
            get { return this._Destino; }
            set { this.SetProperty(ref this._Destino, value); }
        }

        public object Tooltip
        {
            get { return this._tooltip; }
            set { this.SetProperty(ref this._tooltip, value); }
        }

        public bool IsNavigation => this._Destino != null;
    }
}