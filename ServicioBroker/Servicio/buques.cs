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
    public class buques : IBuques, IDisposable
    {
        #region Instance variables

        public static List<IbuquesCallBack> _callbackList = new List<IbuquesCallBack>();
        private string _connectionString;
        private SqlTableDependency<Buques> _sqlTableDependency;
        #endregion

        #region Constructors

        public buques()
        {
            iniciar();
        }

        private void iniciar()
        {
           
            _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            _sqlTableDependency = new SqlTableDependency<Buques>(_connectionString, "buques");

            _sqlTableDependency.OnChanged += TableDependency_Changed;
            _sqlTableDependency.OnError += _sqlTableDependency_OnError;
            _sqlTableDependency.OnStatusChanged += _sqlTableDependency_OnStatusChanged;
            _sqlTableDependency.Start(watchDogTimeOut: 28800);

            while (!(_sqlTableDependency.Status == TableDependency.SqlClient.Base.Enums.TableDependencyStatus.WaitingForNotification)) { }

            Console.WriteLine(@"ESPERANDO NOTIFICACIONES BUQUES");
        }

        private void _sqlTableDependency_OnStatusChanged(object sender, StatusChangedEventArgs e)
        {
            Console.WriteLine(e.Status+" buques");
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

        #region SqlTableDependency
        private void TableDependency_Changed(object sender, RecordChangedEventArgs<Buques> e)
        {
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine($"DML: {e.ChangeType}");
            Console.WriteLine($"TABLA : BUQUES");
            this.cambiosBuques(e.Entity.BUQUE, e.Entity.VIAJE, e.ChangeType.ToString());
            Unsubscribe();
            Subscribe();
        }

        public IList<Buques> obtenerTodosBuque()
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "SELECT * FROM [buques]";
                    var contenedores = new List<Buques>();
                    using (var sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        if (sqlDataReader.HasRows)
                            while (sqlDataReader.Read())
                            {
                                contenedores.Add(new Buques
                                {
                                    BUQUE = sqlDataReader.SafeGetString("BUQUE"),
                                    VIAJE = sqlDataReader.SafeGetString("VIAJE")
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
            var registeredUser = OperationContext.Current.GetCallbackChannel<IbuquesCallBack>();
            if (!_callbackList.Contains(registeredUser))
            {
                _callbackList.Add(registeredUser);
            }
        }

        public void Unsubscribe()
        {
            var registeredUser = OperationContext.Current.GetCallbackChannel<IbuquesCallBack>();
            if (_callbackList.Contains(registeredUser))
            {
                _callbackList.Remove(registeredUser);
            }
        }


        public void cambiosBuques(string BUQUE, string VIAJE, string tipo_Cambio)
        {
            _callbackList.ForEach(delegate (IbuquesCallBack callback)
            {
                callback.cambiosBuques(BUQUE, VIAJE,tipo_Cambio);
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
