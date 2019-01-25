using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicioBroker.Cambios
{
    public interface IContenedorCallback
    {
        [OperationContract]
        void cambiosImpo(string ID, string BUQUE, string CONTENEDOR, string VIAJE, string FECHA_ENTRADA, string ESTADO, string ALMACEN, string tipo_Cambio,string DIAS);

        [OperationContract]
        void cambiosExpo(string ID, string BUQUE, string CONTENEDOR, string VIAJE, string FECHA_ENTRADA, string ESTADO, string ALMACEN, string tipo_Cambio,string DIAS);
    }
    public interface IclienteCallback
    {
        [OperationContract]
        void cambiosCliente(string CLIENTE, string tipo_Cambio);
    }

    public interface IproductosCallBack
    {

        [OperationContract]
        void cambiosProductos(string PRODUCTO,string tipo_Cambio);
    }

    public interface IbuquesCallBack
    {

        [OperationContract]
        void cambiosBuques(string BUQUE, string VIAJE,string tipo_Cambio);
    }
}
