using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace ServicioBroker
{
    [RunInstaller(true)]
    public class Instalador: Installer
    {
        public Instalador()
        {
            var spi = new ServiceProcessInstaller();
            var si = new ServiceInstaller();

            spi.Account = ServiceAccount.LocalSystem;
            spi.Username = null;
            spi.Password = null;

            si.DisplayName = ServicioInicial.NombreServicio;
            si.ServiceName = ServicioInicial.NombreServicio;
            si.StartType = ServiceStartMode.Automatic;

            Installers.Add(spi);
            Installers.Add(si);

        }
    }
}
