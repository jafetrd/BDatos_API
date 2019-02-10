using ServicioBroker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicioBroker.Cambios
{

    [ServiceContract(CallbackContract = typeof(IContenedorSimpleCallBack))]
    public interface IContenedorSimple
    {
        [OperationContract]
        void Subscribe();

        [OperationContract]
        void Unsubscribe();

        [OperationContract]
        IList<ContenedorSimple> obtenerTodasImportaciones();

        [OperationContract(Name = "IContenedorImpoSimple")]
        void cambioImportacionesSimple(string BUQUE,string VIAJE, string ALMACEN,string regimen);

        [OperationContract]
        IList<ContenedorSimple> obtenerTodasExportaciones();

        [OperationContract(Name = "IContenedorExpoSimple")]
        void cambioExportacionesSimple(string BUQUE, string VIAJE,string ALMACEN,string regimen);
    }
}
