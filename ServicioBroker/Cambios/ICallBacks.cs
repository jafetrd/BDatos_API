﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicioBroker.Cambios
{
    interface IContenedorCallback
    {
        [OperationContract]
        void cambiosImpo(string ID, string BUQUE, string CONTENEDOR, string VIAJE, string FECHA_ENTRADA, string ESTADO, string ALMACEN);

        [OperationContract]
        void cambiosExpo(string ID, string BUQUE, string CONTENEDOR, string VIAJE, string FECHA_ENTRADA, string ESTADO, string ALMACEN);
    }
    interface IclienteCallback
    {
        [OperationContract]
        void cambiosCliente(int ID, string CLIENTE);
    }

    interface IproductosCallBack
    {

        [OperationContract]
        void cambiosProductos(int ID, string PRODUCTO);
    }

    interface IbuquesCallBack
    {

        [OperationContract]
        void cambiosBuques(string BUQUE, string VIAJE);
    }
}
