using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.Win32;
using ServicioBroker.Servicio;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.ServiceModel;
using System.ServiceProcess;

namespace ServicioBroker
{
    public class ServicioInicial
    {
        public const string NombreServicio = "Servicio_API";
        private static Service1 service;

        private static ServiceHost host, host2, host3, host4;
        public static bool ESTADO_MAQUINA = true;
        //[STAThread]
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        public static void Main(string[] args)
        {
            host = null;
            host2 = null;
            host3 = null;
            host4 = null;
            SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;
            service = new Service1();
            //ServiceBase.Run(service);

            if (Environment.UserInteractive)
            {
                RunAsConsole(null);
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] { service };
                ServiceBase.Run(ServicesToRun);
            }
        }

        private static void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            switch (e.Mode)
            {
                case PowerModes.Resume:
                    Start(null);
                    Console.WriteLine("Reinicio despues de suspender");
                    break;
                case PowerModes.Suspend:
                    Stop();
                    Console.WriteLine("se suspendio la maquina");
                    break;
            }
        }

        public static void RunAsConsole(string[] args)
        {

            if (CheckDatabaseExists())
            {
                try
                {
                    Start(args);
                    Console.WriteLine("Presiona cualquier tecla para terminar el servicio");
                    Console.ReadLine();
                    Stop();
                }
                catch (System.TimeoutException timeProblem)
                {
                    Console.WriteLine(timeProblem.Message);
                    Console.ReadLine();
                }
                catch (CommunicationException commProblem)
                {
                    Console.WriteLine(commProblem.Message);
                    Console.ReadLine();
                }

            }
            else
            {
                Console.WriteLine("La base de datos no existe");
                ejecutarScript(GetAppFolder());

                string[] b = new string[] { "CONTENEDOR 20", "CONTENEDOR 40", "CONTENEDOR 20 CON REFRIGERANTE",
                                             "CONTENEDOR 40 CON REFRIGERANTE","ISOTANQUE","ESTANDAR 20",
                                             "ESTANDAR 40", "HARDTOP 20","HARDTOP 40","OPENTOP 20",
                                              "PLATAFORMA 20","PLATAFORMA 40"};
                for (int x = 0; x < b.Length; x++)
                {
                    agregarDatos(query: "INSERT INTO dbo.dform_patiocontenedor(PRESENTACIONES) VALUES(@PRESENTACIONES)",
                                 datos: b[x], parametro: "@PRESENTACIONES");
                }

                string[] a = new string[] { "FURGON", "TOLVA", "TANQUE" };
                for (int x = 0; x < a.Length; x++)
                {
                    agregarDatos(query: "INSERT INTO dbo.dform_patioferrocarril(Presentaciones) VALUES(@Presentaciones)",
                                 datos: a[x], parametro: "@Presentaciones");
                }

                string[] c = new string[] { "CARGA SUELTA","SACO,COSTAL,BOLSA","TAMBOR,BARRIL Y SIMILARES",
                "CUBETA,CUÑETA Y SIMILARES", "CAJAS","SUPER SACO","JAULA,REJA Y SIMILARES","ROLLO","BOBINA",
                "TARIMA,PALLETS Y SIMILARES","BARRA","TUBOS","LINGOTE","ATADO","VEHICULO","CILINDRO","MALETA, BAUL Y OTROS EQUIPAJES",
                "GRANEL SOLIDO","GRANEL LIQUIDO","OTROS EMPAQUES Y OTRAS UNIDADES SIN EMPAQUE"};
                for (int x = 0; x < c.Length; x++)
                {
                    agregarDatos(query: "INSERT INTO dbo.dform_bodegac(Presentaciones) VALUES(@Presentaciones)",
                                 datos: a[x], parametro: "@Presentaciones");
                }
                RunAsConsole(null);
            }
        }



        private static void Start(string[] args)
        {

            File.AppendAllText(GetAppFolder()+"\\MyService.txt", string.Format("{0} started{1}", DateTime.Now, Environment.NewLine));

            host = new ServiceHost(typeof(tabla_contenedor));
            host.Open();
            //Console.WriteLine($"Servicio 1 iniciado en {host.Description.Endpoints[1].Address}");
            while (!(host.State == CommunicationState.Opened)) { }
            host2 = new ServiceHost(typeof(buques));
            host2.Open();
            //Console.WriteLine($"Servicio 2 iniciado en {host2.Description.Endpoints[1].Address}");
            while (!(host2.State == CommunicationState.Opened)) { }
            host3 = new ServiceHost(typeof(clientes));
            host3.Open();
            //Console.WriteLine($"Servicio 3 iniciado en {host3.Description.Endpoints[1].Address}");
            while (!(host3.State == CommunicationState.Opened)) { }
            host4 = new ServiceHost(typeof(productos));
            host4.Open();
            //Console.WriteLine($"Servicio 4 iniciado en {host4.Description.Endpoints[1].Address}");
            while (!(host4.State == CommunicationState.Opened)) { }
        }

        public static void Stop()
        {
            host.Close();
            host2.Close();
            host3.Close();
            host4.Close();
            File.AppendAllText(GetAppFolder()+"\\MyService.txt", String.Format("{0} stopped{1}", DateTime.Now, Environment.NewLine));
        }

        private static bool CheckDatabaseExists()
        {
            string name = "bd_api";
            string _connectionString = ConfigurationManager.ConnectionStrings["connectionString2"].ConnectionString;
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand($"SELECT db_id('{name}')", connection))
                {
                    connection.Open();
                    return (command.ExecuteScalar() != DBNull.Value);
                }
            }
        }

        public static void ejecutarScript(string directorio)
        {
            string scriptDirectory = directorio;
            string _connectionString = ConfigurationManager.ConnectionStrings["connectionString2"].ConnectionString;
            DirectoryInfo di = new DirectoryInfo(scriptDirectory);
            FileInfo[] rgFiles = di.GetFiles("*.sql");
            foreach (FileInfo fi in rgFiles)
            {
                FileInfo fileInfo = new FileInfo(fi.FullName);
                string script = fileInfo.OpenText().ReadToEnd();
                SqlConnection connection = new SqlConnection(_connectionString);
                Server server = new Server(new ServerConnection(connection));
                server.ConnectionContext.ExecuteNonQuery(script);
                Console.WriteLine("Instalado " + fi.FullName);
            }
        }

        private static string GetAppFolder()
        {
            return new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
        }

        private static void agregarDatos(string datos, string query, string parametro)
        {
            string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue(parametro, datos);
                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    if (result < 0)
                        Console.WriteLine("Error al insertar datos");
                    else
                        Console.WriteLine("Datos agregados con exito");
                }
            }
        }
    }
}
