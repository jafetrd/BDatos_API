using ServicioBroker.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace ServicioBroker.Cambios
{
    [ServiceContract(CallbackContract = typeof(IproductosCallBack))]
    public interface IProductos
    {
        [OperationContract]
        void Subscribe();

        [OperationContract]
        void Unsubscribe();

        [OperationContract]
        IList<Productos>obtenerTodosProductos();

        [OperationContract(Name ="IProductos")]
        void cambiosProductos(string PRODUCTO);
    }
}
