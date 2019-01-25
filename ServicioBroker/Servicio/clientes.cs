using ServicioBroker.Cambios;
using ServicioBroker.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.ServiceModel;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.EventArgs;


namespace ServicioBroker.Servicio
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    public class clientes : IClientes, IDisposable
    {

        #region Instance variables

        private readonly List<IclienteCallback> _callbackList = new List<IclienteCallback>();
        private  string _connectionString;

        private  SqlTableDependency<Clientes> _sqlTableDependency;
        #endregion

        #region Constructors

        public clientes()
        {
            iniciar();  
        }

        public void iniciar()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            _sqlTableDependency = new SqlTableDependency<Clientes>(_connectionString, "clientes");

            _sqlTableDependency.OnChanged += TableDependency_Changed;
            _sqlTableDependency.OnError += _sqlTableDependency_OnError;
            _sqlTableDependency.OnStatusChanged += _sqlTableDependency_OnStatusChanged;
            _sqlTableDependency.Start(watchDogTimeOut: 28800);

            while (!(_sqlTableDependency.Status == TableDependency.SqlClient.Base.Enums.TableDependencyStatus.WaitingForNotification)) { }

            Console.WriteLine(@"ESPERANDO NOTIFICACIONES CLIENTES");
        }

        private void _sqlTableDependency_OnStatusChanged(object sender, StatusChangedEventArgs e)
        {
            Console.WriteLine(e.Status);
            if (e.Status == TableDependency.SqlClient.Base.Enums.TableDependencyStatus.StopDueToError)
            {
                Unsubscribe();
                Dispose();
            }
        }

        private void _sqlTableDependency_OnError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine(e.Error);
            Unsubscribe();
            Dispose();
        }

        #endregion

        #region SqlTableDependency3
        private void TableDependency_Changed(object sender,RecordChangedEventArgs<Clientes> e)
        {
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine($"DML: {e.ChangeType}");
            Console.WriteLine($"TABLA : CLIENTES");
            this.cambiosCliente(e.Entity.CLIENTE,e.ChangeType.ToString());
            Unsubscribe();
            Subscribe();
        }

        public IList<Clientes> obtenerTodosClientes()
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "SELECT * FROM [clientes]";
                    var contenedores = new List<Clientes>();
                    using (var sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        if (sqlDataReader.HasRows)
                            while (sqlDataReader.Read())
                            {
                                contenedores.Add(new Clientes
                                {
                                    CLIENTE = sqlDataReader.SafeGetString("CLIENTE")
                                });
                            }
                    }
                    return contenedores;
                }
            }
        }
        #endregion

        #region Publish-Subscribe design pattern
        public void Subscribe()
        {
            var registeredUser = OperationContext.Current.GetCallbackChannel<IclienteCallback>();
            if (!_callbackList.Contains(registeredUser))
            {
                _callbackList.Add(registeredUser);
            }
        }

        public void Unsubscribe()
        {
            var registeredUser = OperationContext.Current.GetCallbackChannel<IclienteCallback>();
            if (_callbackList.Contains(registeredUser))
            {
                _callbackList.Remove(registeredUser);
            }
        }

        public void cambiosCliente(string CLIENTE, string tipo_Cambio)
        {
            _callbackList.ForEach(delegate (IclienteCallback callback)
            {
                callback.cambiosCliente(CLIENTE,tipo_Cambio);
            });
        }
        #endregion

        #region IDisposable

        public void Dispose()
        {
            _sqlTableDependency.Stop();
        }
        #endregion

    }
}
