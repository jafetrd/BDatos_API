using ServicioBroker.Cambios;
using ServicioBroker.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.ServiceModel;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Abstracts;
using TableDependency.SqlClient.Base.EventArgs;
using TableDependency.SqlClient.Where;

namespace ServicioBroker.Servicio
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class contenedoresSimple : IContenedorSimple, IDisposable
    {
        #region Instance variables

        public static List<IContenedorSimpleCallBack> _callbackList = new List<IContenedorSimpleCallBack>();
        private string _connectionString;
        public SqlTableDependency<ContenedorSimple> _sqlTableDependency;
        public SqlTableDependency<ContenedorSimple> _sqlTableDependency2;

        #endregion

        #region Constructors
        public contenedoresSimple()
        {
            iniciar1();
            iniciar2();
        }

        private void iniciar1()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            Expression<Func<ContenedorSimple, bool>> expression = p => p.REGIMEN == "IMPO";
            ITableDependencyFilter CONDICION = new SqlTableDependencyFilter<ContenedorSimple>(expression);
            _sqlTableDependency = new SqlTableDependency<ContenedorSimple>(_connectionString, tableName: "TEMPORAL2", filter: CONDICION);
            _sqlTableDependency.OnChanged += TableDependency_Changed;
            _sqlTableDependency.OnError += _sqlTableDependency_OnError;
            _sqlTableDependency.OnStatusChanged += _sqlTableDependency_OnStatusChanged;
            _sqlTableDependency.Start(watchDogTimeOut: 28800);
            while (!(_sqlTableDependency.Status == TableDependency.SqlClient.Base.Enums.TableDependencyStatus.WaitingForNotification)) { }
            Console.WriteLine(@"ESPERANDO NOTIFICACIONES CONTENEDORESsimple 1");
        }

        private void TableDependency_Changed(object sender, RecordChangedEventArgs<ContenedorSimple> e)
        {
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine($"DML: {e.ChangeType}");
            Console.WriteLine($"TABLA : CONTENEDORsimple impo");
            this.cambioImportacionesSimple(e.Entity.BUQUE, e.Entity.VIAJE, e.Entity.ALMACEN,e.Entity.REGIMEN);
            Unsubscribe();
            Subscribe();
        }

        private void iniciar2()
        {
            Expression<Func<ContenedorSimple, bool>> expression = p => p.REGIMEN == "EXPO";
            ITableDependencyFilter CONDICION = new SqlTableDependencyFilter<ContenedorSimple>(expression);
            _sqlTableDependency2 = new SqlTableDependency<ContenedorSimple>(_connectionString, tableName: "TEMPORAL2", filter: CONDICION);
            _sqlTableDependency2.OnChanged += TableDependency2_Changed;
            _sqlTableDependency2.OnError += _sqlTableDependency2_OnError;
            _sqlTableDependency2.OnStatusChanged += _sqlTableDependency2_OnStatusChanged;
            _sqlTableDependency2.Start(watchDogTimeOut: 28800);
            while (!(_sqlTableDependency2.Status == TableDependency.SqlClient.Base.Enums.TableDependencyStatus.WaitingForNotification)) { }
            Console.WriteLine(@"ESPERANDO NOTIFICACIONES CONTENEDORESsimple 2");
        }

        private void TableDependency2_Changed(object sender, RecordChangedEventArgs<ContenedorSimple> e)
        {
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine($"DML: {e.ChangeType}");
            Console.WriteLine($"TABLA : CONTENEDORsimple expo");
            this.cambioExportacionesSimple(e.Entity.BUQUE, e.Entity.VIAJE, e.Entity.ALMACEN,e.Entity.REGIMEN);
            Unsubscribe();
            Subscribe();
        }

        public void _sqlTableDependency_OnStatusChanged(object sender, StatusChangedEventArgs e)
        {
            Console.WriteLine(e.Status+" contenedoresSimple1");
            if (e.Status == TableDependency.SqlClient.Base.Enums.TableDependencyStatus.StopDueToError)
            {
                Unsubscribe();
                Dispose();
            }

        }

        private void _sqlTableDependency2_OnStatusChanged(object sender, StatusChangedEventArgs e)
        {
            Console.WriteLine(e.Status+" contenedoresSimple2");
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

        private void _sqlTableDependency2_OnError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine(e.Error);
            Unsubscribe();
            Dispose();
        }
#endregion

        public void cambioExportacionesSimple(string BUQUE, string VIAJE, string ALMACEN,string regimen)
        {
            _callbackList.ForEach(delegate (IContenedorSimpleCallBack callback)
            {
                callback.cambiosExpoSimple(BUQUE, VIAJE, ALMACEN, regimen);
            });
        }

        public void cambioImportacionesSimple(string BUQUE, string VIAJE, string ALMACEN,string regimen)
        {
            _callbackList.ForEach(delegate (IContenedorSimpleCallBack callback) {
                callback.cambiosImpoSimple(BUQUE, VIAJE, ALMACEN, regimen);
            });
        }

        public void Dispose()
        {
            _sqlTableDependency.Stop();
            _sqlTableDependency2.Stop();
        }

        public IList<ContenedorSimple> obtenerTodasExportaciones()
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "SELECT * FROM [TEMPORAL2] WHERE REGIMEN = @REGIMEN";
                    sqlCommand.Parameters.AddWithValue("@REGIMEN", "EXPO");

                    return GetContenedors(sqlCommand);
                }
            }
        }

        public IList<ContenedorSimple> obtenerTodasImportaciones()
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "SELECT * FROM [TEMPORAL2] WHERE REGIMEN = @REGIMEN";
                    sqlCommand.Parameters.AddWithValue("@REGIMEN", "IMPO");

                    return GetContenedors(sqlCommand);
                }
            }
        }

        private IList<ContenedorSimple> GetContenedors(SqlCommand sqlCommand)
        {
            var contenedores = new List<ContenedorSimple>();
            using (var sqlDataReader = sqlCommand.ExecuteReader())
            {
                if (sqlDataReader.HasRows)
                    while (sqlDataReader.Read())
                    {
                        contenedores.Add(new ContenedorSimple
                        {
                            BUQUE = sqlDataReader.SafeGetString("BUQUE"),
                            VIAJE = sqlDataReader.SafeGetString("VIAJE"),
                            ALMACEN = sqlDataReader.SafeGetString("ALMACEN"),
                            REGIMEN = sqlDataReader.SafeGetString("REGIMEN")
                        });
                    }
            }
            return contenedores;
        }

        public void Subscribe()
        {
            var registeredUser = OperationContext.Current.GetCallbackChannel<IContenedorSimpleCallBack>();
            if (!_callbackList.Contains(registeredUser))
            {
                _callbackList.Add(registeredUser);
            }

        }

        public void Unsubscribe()
        {
            var registeredUser = OperationContext.Current.GetCallbackChannel<IContenedorSimpleCallBack>();
            if (_callbackList.Contains(registeredUser))
            {
                _callbackList.Remove(registeredUser);
            }
        }
    }
}
