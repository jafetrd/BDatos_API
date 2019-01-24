using System;
using System.ServiceProcess;

namespace ServicioBroker
{
    public class Program
    {
       [STAThread]
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        public static void Main()
        {
            Service1 service = new Service1();
            

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
