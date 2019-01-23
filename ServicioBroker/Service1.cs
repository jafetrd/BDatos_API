
using ServicioBroker.Servicio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ServicioBroker
{
    public partial class Service1 : ServiceBase
    {
        ServiceHost host, host2, host3, host4;
        private string _connectionString;
        public Service1()
        {
            InitializeComponent();
            host = null;
            host2 = null;
            host3 = null;
            host4 = null;
        }

        internal void RunAsConsole(string[] args)
        {
            if (verificarDataBase())
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

            }
        }

        protected override void OnStart(string[] args)
        {
            host = new ServiceHost(typeof(tabla_contenedor));
            host.Open();
            Console.WriteLine($"Servicio 1 iniciado en {host.Description.Endpoints[0].Address}");
            while (!(host.State == CommunicationState.Opened)) { }
            host2 = new ServiceHost(typeof(buques));
            host2.Open();
            Console.WriteLine($"Servicio 2 iniciado en {host2.Description.Endpoints[0].Address}");
            while (!(host2.State == CommunicationState.Opened)) { }
            host3 = new ServiceHost(typeof(clientes));
            host3.Open();
            Console.WriteLine($"Servicio 3 iniciado en {host3.Description.Endpoints[0].Address}");
            while (!(host3.State == CommunicationState.Opened)) { }
            host4 = new ServiceHost(typeof(productos));
            host4.Open();
            Console.WriteLine($"Servicio 4 iniciado en {host4.Description.Endpoints[0].Address}");
            while (!(host4.State == CommunicationState.Opened)) { }



            //BDatos_API.App mainWindow = new BDatos_API.App();
            //mainWindow.InitializeComponent();

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

        private bool verificarDataBase()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            string cmdText = "select count(*) from master.dbo.sysdatabases where name = @database";
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                using (var sqlCmd = new SqlCommand(cmdText, sqlConnection))
                { 
                    sqlCmd.Parameters.Add("@database",SqlDbType.NVarChar).Value = "bd_api";
                    sqlConnection.Open();
                    return Convert.ToInt32(sqlCmd.ExecuteScalar()) == 1;
                }
            }
        }


    }
}
