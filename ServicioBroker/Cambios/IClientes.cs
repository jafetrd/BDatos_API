using ServicioBroker.Models;
using System.Collections.Generic;
using System.ServiceModel;

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
