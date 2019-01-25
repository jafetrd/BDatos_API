using ServicioBroker.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace ServicioBroker.Cambios
{
    [ServiceContract(CallbackContract = typeof(IContenedorCallback))]
    public interface IContenedor
    {
        [OperationContract]
        void Subscribe();

        [OperationContract]
        void Unsubscribe();

        [OperationContract]
        IList<Contenedor> obtenerTodasImportaciones();

        [OperationContract(Name ="IContenedorImpo")]
        void cambioImportaciones(string ID, string BUQUE, string CONTENEDOR, string VIAJE, string FECHA_ENTRADA, string ESTADO, string ALMACEN, string tipo_Cambio,string DIAS);

        [OperationContract]
        IList<Contenedor> obtenerTodasExportaciones();

        [OperationContract(Name = "IContenedorExpo")]
        void cambioExportaciones(string ID, string BUQUE, string CONTENEDOR, string VIAJE, string FECHA_ENTRADA, string ESTADO, string ALMACEN, string tipo_Cambio,string DIAS);
    }

}
