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
    public class tabla_contenedor : IContenedor, IDisposable
    {
        #region Instance variables

        private readonly List<IContenedorCallback> _callbackList = new List<IContenedorCallback>();
        private string _connectionString;
        private SqlTableDependency<Contenedor> _sqlTableDependency;
        private SqlTableDependency<Contenedor> _sqlTableDependency2;
        #endregion

        #region Constructors

        public tabla_contenedor()
        {
            iniciar1();
            iniciar2();
        }

        private void iniciar1()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            Expression<Func<Contenedor, bool>> expression = p => p.REGIMEN == "EXPO";
            ITableDependencyFilter CONDICION = new SqlTableDependencyFilter<Contenedor>(expression);
            _sqlTableDependency = new SqlTableDependency<Contenedor>(_connectionString, tableName: "temporal", filter: CONDICION);
            _sqlTableDependency.OnChanged += TableDependency_Changed;
            _sqlTableDependency.OnError += _sqlTableDependency_OnError;
            _sqlTableDependency.OnStatusChanged += _sqlTableDependency_OnStatusChanged;
            _sqlTableDependency.Start(watchDogTimeOut: 28800);
            while (!(_sqlTableDependency.Status == TableDependency.SqlClient.Base.Enums.TableDependencyStatus.WaitingForNotification)) { }
            Console.WriteLine(@"ESPERANDO NOTIFICACIONES CONTENEDORES 1");
        }


        private void iniciar2()
        {
            Expression<Func<Contenedor, bool>> expression = p => p.REGIMEN == "IMPO";
            ITableDependencyFilter CONDICION = new SqlTableDependencyFilter<Contenedor>(expression);
            _sqlTableDependency2 = new SqlTableDependency<Contenedor>(_connectionString, tableName: "temporal", filter: CONDICION);
            _sqlTableDependency2.OnChanged += TableDependency2_Changed;
            _sqlTableDependency2.OnError += _sqlTableDependency2_OnError;
            _sqlTableDependency2.OnStatusChanged += _sqlTableDependency2_OnStatusChanged;
            _sqlTableDependency2.Start(watchDogTimeOut: 3600);
            while (!(_sqlTableDependency2.Status == TableDependency.SqlClient.Base.Enums.TableDependencyStatus.WaitingForNotification)) { }
            Console.WriteLine(@"ESPERANDO NOTIFICACIONES CONTENEDORES 2");
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

        private void _sqlTableDependency2_OnStatusChanged(object sender, StatusChangedEventArgs e)
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

        private void _sqlTableDependency2_OnError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine(e.Error);
            Unsubscribe();
            Dispose();
        }
        #endregion


        #region SqlTableDependency

        private void TableDependency_Changed(object sender,RecordChangedEventArgs<Contenedor> e)
        {
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine($"DML: {e.ChangeType}");
            Console.WriteLine($"TABLA : CONTENEDOR");
            this.cambioExportaciones(e.Entity.ID, e.Entity.BUQUE, e.Entity.INICIALES + e.Entity.NUMERO, e.Entity.VIAJE, e.Entity.FECHA_ENTRADA, e.Entity.ESTADO, e.Entity.ALMACEN,e.ChangeType.ToString(),e.Entity.DIAS);
            Unsubscribe();
            Subscribe();
        }

        private void TableDependency2_Changed(object sender, RecordChangedEventArgs<Contenedor> e)
        {
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine($"DML: {e.ChangeType}");
            Console.WriteLine($"TABLA : CONTENEDOR");
            this.cambioImportaciones(e.Entity.ID, e.Entity.BUQUE, e.Entity.INICIALES + e.Entity.NUMERO, e.Entity.VIAJE, e.Entity.FECHA_ENTRADA, e.Entity.ESTADO, e.Entity.ALMACEN,e.ChangeType.ToString(),e.Entity.DIAS);
            Unsubscribe();
            Subscribe();
        }

        public IList<Contenedor> obtenerTodasImportaciones()
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "SELECT * FROM [Temporal] WHERE REGIMEN = @REGIMEN";
                    sqlCommand.Parameters.AddWithValue("@REGIMEN", "IMPO");

                    return GetContenedors(sqlCommand);
                }
            }
        }

        public IList<Contenedor> obtenerTodasExportaciones()
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "SELECT * FROM [Temporal] WHERE REGIMEN = @REGIMEN";
                    sqlCommand.Parameters.AddWithValue("@REGIMEN", "EXPO");

                    return GetContenedors(sqlCommand);
                }
            }
        }

        private IList<Contenedor> GetContenedors(SqlCommand sqlCommand)
        {
            var contenedores = new List<Contenedor>();
            using (var sqlDataReader = sqlCommand.ExecuteReader())
            {
                if (sqlDataReader.HasRows)
                    while (sqlDataReader.Read())
                    {
                        contenedores.Add(new Contenedor
                        {
                            ID = sqlDataReader.SafeGetString("ID"),
                            BUQUE = sqlDataReader.SafeGetString("BUQUE"),
                            CONTENEDOR =sqlDataReader.SafeGetString("INICIALES") + sqlDataReader.SafeGetString("NUMERO"),
                            VIAJE = sqlDataReader.SafeGetString("VIAJE"),
                            FECHA_ENTRADA = sqlDataReader.SafeGetString("FECHA_ENTRADA"),
                            ESTADO = sqlDataReader.SafeGetString("ESTADO"),
                            ALMACEN = sqlDataReader.SafeGetString("ALMACEN"),
                            DIAS = dias(sqlDataReader.SafeGetString("FECHA_ENTRADA"))
                        });
                    }
            }
            return contenedores;
        }

        private string dias(string fecha_entrada)
        {
            DateTime date = DateTime.Parse(fecha_entrada);
            int dias = (DateTime.Today - date).Days;
            return dias.ToString();
        }
      
        public void Subscribe()
        {
            var registeredUser = OperationContext.Current.GetCallbackChannel<IContenedorCallback>();
            if (!_callbackList.Contains(registeredUser))
            {
                _callbackList.Add(registeredUser);
            }
        }

        public void Unsubscribe()
        {
            var registeredUser = OperationContext.Current.GetCallbackChannel<IContenedorCallback>();
            if (_callbackList.Contains(registeredUser))
            {
                _callbackList.Remove(registeredUser);
            }
        }


        public void cambioImportaciones(string ID, string BUQUE, string CONTENEDOR, string VIAJE, string FECHA_ENTRADA, string ESTADO, string ALMACEN, string tipo_Cambio,string DIAS)
        {
            _callbackList.ForEach(delegate (IContenedorCallback callback) {
                callback.cambiosImpo(ID, BUQUE, CONTENEDOR, VIAJE, FECHA_ENTRADA, ESTADO, ALMACEN, tipo_Cambio,DIAS);
            });
        }

        public void cambioExportaciones(string ID, string BUQUE, string CONTENEDOR, string VIAJE, string FECHA_ENTRADA, string ESTADO, string ALMACEN, string tipo_Cambio,string DIAS)
        {
            _callbackList.ForEach(delegate (IContenedorCallback callback)
            {
                callback.cambiosExpo(ID, BUQUE, CONTENEDOR, VIAJE, FECHA_ENTRADA, ESTADO, ALMACEN, tipo_Cambio,DIAS);
            });
        }
        #endregion

        public void Dispose()
        {
            _sqlTableDependency.Stop();
        }

        public void Dispose2()
        {
            _sqlTableDependency2.Stop();
        }

    }

    public static class extensionDataRead
    {
        public static string SafeGetString(this SqlDataReader reader, string Columna)
        {
            int columIndex = reader.GetOrdinal(Columna);
            if (!reader.IsDBNull(columIndex))
            {
                return reader.GetString(columIndex);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
