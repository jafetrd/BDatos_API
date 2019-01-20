using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static BDatos_API.nombresPatioFerrocarril;
using static BDatos_API.Maquina_estados;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;

namespace BDatos_API.VISTAS
{

    public class modeloPatioFerrocarril : INotifyPropertyChanged
    {
        DataTable data;
        Metodos_bd metodos_Bd;
        public modeloPatioFerrocarril()
        {
            data = new DataTable();
            metodos_Bd = new Metodos_bd();
            comboBuqueDatos = new List<comboBuque>();
            comboClientes = new List<comboCliente>();
            comboProductos = new List<comboProducto>();
            cargarAutocompletado();
        }

        public void cargarAutocompletado()
        {
            comboBuqueDatos.Clear();
            comboClientes.Clear();
            comboProductos.Clear();
            data = metodos_Bd.REGRESAR_TODO(tablaBuque.NOMBRE_TABLA, TODO);
            foreach (DataRow row in data.Rows)
            {
                comboBuqueDatos.Add(new comboBuque() { BUQUE_2 = row[1].ToString(), VIAJE = row[2].ToString() });
            }

            data = null;

            data = metodos_Bd.REGRESAR_TODO(tablaCliente.NOMBRE_TABLA, TODO);
            foreach (DataRow row in data.Rows)
            {
                comboClientes.Add(new comboCliente { CLIENTE_2 = row[0].ToString() });
            }

            data = null;

            data = metodos_Bd.REGRESAR_TODO(tablaProducto.NOMBRE_TABLA, TODO);
            foreach (DataRow row in data.Rows)
            {
                comboProductos.Add(new comboProducto { PRODUCTO_2 = row[0].ToString() });
            }
        }

        public void limpiarPropiedades()
        {
            foreach (PropertyInfo prop in GetType().GetProperties())
            {
                if (prop.PropertyType == typeof(string))
                    prop.SetValue(this, string.Empty);
            }

        }

        public void conFormatoSQL()
        {
            _BUQUE = BUQUE;
            _VIAJE = VIAJE;
            char[] a = FECHA_ENTRADA.ToCharArray();
            _FECHA_ENTRADA = a[6].ToString() + a[7].ToString() + a[8].ToString() + a[9].ToString() +
                        "-" + a[3].ToString() + a[4].ToString() + "-" + a[0].ToString() + a[1].ToString();

            switch (REGIMEN)
            {
                case IMPORTACION: _REGIMEN = IMPO_SQL; break;
                case EXPORTACION: _REGIMEN = EXPO_SQL; break;
            }
            _ESTADO = Maquina_estados._ESTADO.ToString();
            switch (Maquina_estados._ESTADO)
            {
                case Entrada:
                    _SESION_ENTRADA = InicioSesion.USUARIOdato;
                    break;
                case Salida:
                    _SESION_ENTRADA = InicioSesion.USUARIOdato;
                    break;
            }
            _PRODUCTO = PRODUCTO;
            _CLIENTE = CLIENTE;
        }

        public string _ID;
        public string _BUQUE;
        public string _VIAJE;
        public string _REGIMEN;
        public string _FECHA_ENTRADA;

        public string _PRESENTACION;
        public string _INICIALES;
        public string _NUMERO;
        public string _PESO;
        public string _UNIDADES;
        public string _PRODUCTO;
        public string _CLIENTE;
        public string _PEDIMENTO;
        public string _VALOR_COMERCIAL;
        public string _FECHA_AUTORIZACION;
        public string _FECHA_SALIDA;
        public string _SESION_ENTRADA;
        public string _SESION_AUTORIZACION;
        public string _SESION_SALIDA;
        public string _ESTADO;
        public string _AGENTE;

        private string D1;
        private string D2;
        private string D3;
        private string D4;
        private string D5;
        private string D6;
        private string D7;
        private string D8;
        private string D9;
        private string D10;
        private string D11;
        private string D12;
        private string D13;
        private string D14;

        private string D16;
        private string D17;

        private string D19;
        private string D20;
        private string D21;

        public string ID
        {
            get { return this.D1; }
            set
            {
                this.D1 = value;
                OnPropertyChanged("ID");
            }
        }
        public string BUQUE
        {
            get { return this.D2; }
            set
            {
                this.D2 = value;
                OnPropertyChanged("BUQUE");
            }
        }
        public string VIAJE
        {
            get { return this.D3; }
            set
            {
                this.D3 = value;
                OnPropertyChanged("VIAJE");
            }
        }
        public string REGIMEN
        {
            get { return this.D4; }
            set
            {
                this.D4 = value;
                OnPropertyChanged("REGIMEN");
            }
        }

        public string FECHA_ENTRADA
        {
            get { return this.D5; }
            set
            {
                this.D5 = value;
                OnPropertyChanged("FECHA_ENTRADA");
            }
        }

        public string PRESENTACION
        {
            get { return this.D6; }
            set
            {
                this.D6 = value;
                OnPropertyChanged("PRESENTACION");
            }
        }
        public string INICIALES
        {
            get { return this.D7; }
            set
            {
                this.D7 = value;
                OnPropertyChanged("INICIALES");
            }
        }
        public string NUMERO
        {
            get { return this.D8; }
            set
            {
                this.D8 = value;
                OnPropertyChanged("NUMERO");
            }
        }
        public string PESO
        {
            get { return this.D9; }
            set
            {
                this.D9 = value;
                OnPropertyChanged("PESO");
            }
        }
        public string UNIDADES
        {
            get { return this.D10; }
            set
            {
                this.D10 = value;
                OnPropertyChanged("UNIDADES");
            }
        }
        public string PRODUCTO
        {
            get { return this.D11; }
            set
            {
                this.D11 = value;
                OnPropertyChanged("PRODUCTO");
            }
        }
        public string CLIENTE
        {
            get { return this.D12; }
            set
            {
                this.D12 = value;
                OnPropertyChanged("CLIENTE");
            }
        }
        public string PEDIMENTO
        {
            get { return this.D13; }
            set
            {
                this.D13 = value;
                OnPropertyChanged("PEDIMENTO");
            }
        }
        public string VALOR_COMERCIAL
        {
            get { return this.D14; }
            set
            {
                this.D14 = value;
                OnPropertyChanged("VALOR_COMERCIAL");
            }
        }

        public string FECHA_SALIDA
        {
            get { return this.D16; }
            set
            {
                this.D16 = value;
                OnPropertyChanged("FECHA_SALIDA");
            }
        }
        public string SESION_ENTRADA
        {
            get { return this.D17; }
            set
            {
                this.D17 = value;
                OnPropertyChanged("SESION_ENTRADA");
            }
        }


        public string SESION_SALIDA
        {
            get { return this.D19; }
            set
            {
                this.D19 = value;
                OnPropertyChanged("SESION_SALIDA");
            }
        }
        public string ESTADO
        {
            get { return this.D20; }
            set
            {
                this.D20 = value;
                OnPropertyChanged("ESTADO");
            }
        }

        public string AGENTE
        {
            get { return this.D21; }
            set
            {
                this.D21 = value;
                OnPropertyChanged("AGENTE");
            }
        }

        //modelo para el BUQUE
        public class comboBuque
        {
            public string BUQUE_2 { get; set; }
            public string VIAJE { get; set; }
        }
        
        public List<comboBuque> comboBuqueDatos{ get;set;}

        public comboBuque buqueSeleccionado { get; set; }

        public class comboCliente
        {
            public string CLIENTE_2 { get; set; }
        }

        public List<comboCliente> comboClientes { get; set;  }

        public comboCliente clienteSeleccionado { get; set; }

        public class comboProducto
        {
            public string PRODUCTO_2 { get; set; }
        }

        public List<comboProducto> comboProductos { get; set; }

        public comboProducto productoSeleccionado { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
