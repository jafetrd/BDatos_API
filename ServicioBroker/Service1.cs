
using ServicioBroker.Servicio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Timers;
using static System.Net.Mime.MediaTypeNames;
//Microsoft.SqlServer.Smo.dll
using Microsoft.SqlServer.Management.Smo;
//Microsoft.SqlServer.ConnectionInfo.dll
using Microsoft.SqlServer.Management.Common;
using System.Reflection;

namespace ServicioBroker
{
    public partial class Service1 : ServiceBase
    {
        ServiceHost host, host2, host3, host4;

        Timer timer;
        bool entrada = true;
        public Service1()
        {
            InitializeComponent();
            host = null;
            host2 = null;
            host3 = null;
            host4 = null;
            timer = new Timer(60000);
            timer.AutoReset = true;
            timer.Start();
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (entrada == false)
            {
                OnStop();
                OnStart(null);
                Console.WriteLine("Reinicio");
            }
            entrada = false;
        }

        internal void RunAsConsole(string[] args)
        {
            if (CheckDatabaseExists())
            {
                try
                {
                    OnStart(args);
                    Console.WriteLine("Presiona cualquier tecla para terminar el servicio");
                    Console.ReadLine();
                    OnStop();
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
                ejecutarScript(GetAppFolder());
                string[] b = new string[] { "CONTENEDOR 20", "CONTENEDOR 40", "CONTENEDOR 20 CON REFRIGERANTE",
                                             "CONTENEDOR 40 CON REFRIGERANTE","ISOTANQUE","ESTANDAR 20",
                                             "ESTANDAR 40", "HARDTOP 20","HARDTOP 40","OPENTOP 20",
                                              "PLATAFORMA 20","PLATAFORMA 40"};
                for (int x = 0; x < b.Length; x++) {
                    agregarDatos(query: "INSERT INTO dbo.dform_patiocontenedor(PRESENTACIONES) VALUES(@PRESENTACIONES)",
                                 datos: b[x], parametro: "@PRESENTACIONES");
                }
               
                string[] a = new string[] { "FURGON", "TOLVA", "TANQUE" };
                for (int x = 0; x < a.Length; x++)
                {
                    agregarDatos(query: "INSERT INTO dbo.dform_patioferrocarril(Presentaciones) VALUES(@Presentaciones)",
                                 datos: a[x], parametro: "@Presentaciones");
                }
                RunAsConsole(null);
            }
        }

        protected override void OnStart(string[] args)
        {
            host = new ServiceHost(typeof(tabla_contenedor));
            host.Open();
            Console.WriteLine($"Servicio 1 iniciado en {host.Description.Endpoints[1].Address}");
            while (!(host.State == CommunicationState.Opened)) { }
            host2 = new ServiceHost(typeof(buques));
            host2.Open();
            Console.WriteLine($"Servicio 2 iniciado en {host2.Description.Endpoints[1].Address}");
            while (!(host2.State == CommunicationState.Opened)) { }
            host3 = new ServiceHost(typeof(clientes));
            host3.Open();
            Console.WriteLine($"Servicio 3 iniciado en {host3.Description.Endpoints[1].Address}");
            while (!(host3.State == CommunicationState.Opened)) { }
            host4 = new ServiceHost(typeof(productos));
            host4.Open();
            Console.WriteLine($"Servicio 4 iniciado en {host4.Description.Endpoints[1].Address}");
            while (!(host4.State == CommunicationState.Opened)) { }

            //BDatos_API.App mainWindow = new BDatos_API.App();
            //mainWindow.InitializeComponent();
            //mainWindow.Run();

            //BDatos_API.MainWindow mainWindow2 = new BDatos_API.MainWindow();

            //mainWindow.Run();
        }

        protected override void OnStop()
        {
            host.Close();
            host2.Close();
            host3.Close();
            host4.Close();
        }

        public bool CheckDatabaseExists()
        {
           string name = "bd_api";
            string _connectionString = "Data Source=DESKTOP-481IKC1;Integrated Security=SSPI;";
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand($"SELECT db_id('{name}')", connection))
                    {
                        connection.Open();
                        return (command.ExecuteScalar() != DBNull.Value);
                    }
                }
        }

        public void ejecutarScript(string directorio)
        {
            string scriptDirectory = directorio;
            string _connectionString = "Data Source=DESKTOP-481IKC1;Integrated Security=SSPI;";
            DirectoryInfo di = new DirectoryInfo(scriptDirectory);
            FileInfo[] rgFiles = di.GetFiles("*.sql");
            foreach (FileInfo fi in rgFiles)
            {
                FileInfo fileInfo = new FileInfo(fi.FullName);
                string script = fileInfo.OpenText().ReadToEnd();
                SqlConnection connection = new SqlConnection(_connectionString);
                Server server = new Server(new ServerConnection(connection));
                server.ConnectionContext.ExecuteNonQuery(script);
            }
        }

        private static string GetAppFolder()
        {
            return new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
        }

       private void agregarDatos(string datos,string query,string parametro)
        {
            string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                //String query = "INSERT INTO dbo.dform_patiocontenedor (PRESENTACIONES) VALUES (@PRESENTACIONES)";
                
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue(parametro,datos);
                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    // Check Error
                    if (result < 0)
                        Console.WriteLine("Error inserting data into Database!");
                }
            }
        }

    }
}
