using ServicioBroker.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace ServicioBroker.Cambios
{
    [ServiceContract(CallbackContract = typeof(IbuquesCallBack))]
    public interface IBuques
    {
        [OperationContract]
        void Subscribe();

        [OperationContract]
        void Unsubscribe();

        [OperationContract]
        IList<Buques> obtenerTodosBuque();

        [OperationContract(Name ="IBuques")]
        void cambiosBuques(string BUQUE, string VIAJE, string tipo_Cambio);

    }
}
