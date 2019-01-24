using ServicioBroker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicioBroker.Cambios
{
    [ServiceContract(CallbackContract = typeof(IclienteCallback))]
    public interface IClientes
    {
        [OperationContract]
        void Subscribe();

        [OperationContract]
        void Unsubscribe();

        [OperationContract]
        IList<Clientes> obtenerTodosClientes();
        
        [OperationContract(Name ="IClientes")]
        void cambiosCliente(string CLIENTE);
    }
}
