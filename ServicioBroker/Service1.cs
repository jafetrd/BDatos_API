﻿using ServicioBroker.Servicio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ServicioBroker
{
    public partial class Service1 : ServiceBase
    {
        
        public Service1()
        {
            InitializeComponent();
        }

        internal void RunAsConsole(string[] args)
        {
            OnStart(args);
            OnStop();
        }

        protected override void OnStart(string[] args)
        {
            var host = new ServiceHost(typeof(tabla_contenedor));
            host.Open();
            Console.WriteLine($"Servicio 1 iniciado en {host.Description.Endpoints[0].Address}");
            while (!(host.State == CommunicationState.Opened)) { }
            var host2 = new ServiceHost(typeof(buques));
            host2.Open();
            Console.WriteLine($"Servicio 2 iniciado en {host2.Description.Endpoints[0].Address}");
            while (!(host2.State == CommunicationState.Opened)) { }
            var host3 = new ServiceHost(typeof(clientes));
            host3.Open();
            Console.WriteLine($"Servicio 3 iniciado en {host3.Description.Endpoints[0].Address}");
            while (!(host3.State == CommunicationState.Opened)) { }
            var host4 = new ServiceHost(typeof(productos));
            host4.Open();
            Console.WriteLine($"Servicio 4 iniciado en {host4.Description.Endpoints[0].Address}");
            while (!(host4.State == CommunicationState.Opened)) { }
            Console.WriteLine("Presiona cualquier tecla para terminar el servicio");
            Console.ReadLine();
            host.Close();
            host2.Close();
            host3.Close();
            host4.Close();
        }

        protected override void OnStop()
        {
          
        }
    }
}
