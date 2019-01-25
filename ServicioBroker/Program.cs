using Microsoft.Win32;
using System;
using System.ServiceProcess;

namespace ServicioBroker
{
    public class Program
    {
        private static Service1 service;
       [STAThread]
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        public static void Main()
        {
           
            service = new Service1();
           // ServiceBase.Run(service);

            if (Environment.UserInteractive)
            {
                service.RunAsConsole(null);
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] { service };
                ServiceBase.Run(ServicesToRun);
            }
            
        }

       
    }
}
