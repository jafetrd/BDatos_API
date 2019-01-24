using ServicioBroker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

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
        void cambioImportaciones(string ID, string BUQUE, string CONTENEDOR, string VIAJE, string FECHA_ENTRADA, string ESTADO, string ALMACEN);

        [OperationContract]
        IList<Contenedor> obtenerTodasExportaciones();

        [OperationContract(Name = "IContenedorExpo")]
        void cambioExportaciones(string ID, string BUQUE, string CONTENEDOR, string VIAJE, string FECHA_ENTRADA, string ESTADO, string ALMACEN);
    }

}
