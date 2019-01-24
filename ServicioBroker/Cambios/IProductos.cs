using ServicioBroker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

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
