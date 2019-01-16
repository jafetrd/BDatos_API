using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BDatos_API.Maquina_estados;
using static BDatos_API.nombresPrincipal;

namespace BDatos_API.VISTAS
{
    class modeloPrincipal : INotifyPropertyChanged
    {

        public modeloPrincipal()
        {
            CARGARTABLA();

        }

        public void CARGARTABLA()
        {
            DataTable data = new DataTable();
            Metodos_bd metodos_Bd = new Metodos_bd();

            DatosImportacion = new List<datosImpo>();

           

        }

        public class datosImpo
        {
            public string _CONTENEDOR;
            public string ID { get; set; }
            public string INICIALES { get; set; }
            public string NUMERO { get; set; }
            public string CONTENEDOR
            {
                get { return INICIALES + NUMERO; }
                set { _CONTENEDOR = value; }
            }
            public string FECHA_ENTRADA { get; set; }
            public string BUQUE { get; set; }
            public string VIAJE { get; set; }
            public string PATIO { get; set; }
        }

        private List<datosImpo> _datosImportacion;

        public List<datosImpo> DatosImportacion
        {
            get { return this._datosImportacion; }
            set
            {
                this._datosImportacion = value;
                OnPropertyChanged("DatosImportacion");
            }
        }

        public datosImpo datosSeleccionados { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
