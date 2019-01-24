using ServicioBroker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

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
        void cambiosBuques(string BUQUE, string VIAJE);

    }
}
