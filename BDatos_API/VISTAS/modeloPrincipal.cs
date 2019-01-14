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
            DataTable data = new DataTable();
            Metodos_bd metodos_Bd = new Metodos_bd();

            DatosImportacion = new List<datosImpo>();
            datosSeleccionados = new List<datosImpo>();

            if (DatosImportacion.Count == 0)
            {
                data = metodos_Bd.REGRESAR_TODO(TABLA_FERROCARRIL,ID_,INICIALES_,NUMERO_,BUQUE_,VIAJE_,FECHA_ENTRADA_);
                foreach (DataRow row in data.Rows)
                {
                    DatosImportacion.Add(new datosImpo
                    {
                        ID = row[0].ToString(),
                        INICIALES = row[1].ToString(),
                        NUMERO = row[2].ToString(),
                        BUQUE = row[3].ToString(),
                        VIAJE = row[4].ToString(),
                        FECHA_ENTRADA = row[5].ToString(),
                        PATIO = "Ferrocarril"
                    });
                }

                //data = metodos_Bd.REGRESAR_TODO(TABLA_CONTENEDOR, ID_, INICIALES_, NUMERO_, BUQUE_, VIAJE_, FECHA_ENTRADA_);
                //foreach (DataRow row in data.Rows)
                //{
                //    DatosImportacion.Add(new datosImpo
                //    {
                //        ID = row[0].ToString(),
                //        INICIALES = row[1].ToString(),
                //        NUMERO = row[2].ToString(),
                //        BUQUE = row[3].ToString(),
                //        VIAJE = row[4].ToString(),
                //        FECHA_ENTRADA = row[5].ToString(),
                //        PATIO = "Contenedores"
                //    });
                //}
         
               
                
            }

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

        public List<datosImpo> DatosImportacion { get; set; }

        public List<datosImpo> datosSeleccionados { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
