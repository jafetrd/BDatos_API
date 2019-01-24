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
        private readonly string _connectionString;
        private SqlTableDependency<Contenedor> _sqlTableDependency;
        private SqlTableDependency<Contenedor> _sqlTableDependency2;
        #endregion

        #region Constructors

        public tabla_contenedor()
        {
    
            _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            Expression<Func<Contenedor, bool>> expression = p => p.REGIMEN == "EXPO";
            ITableDependencyFilter CONDICION = new SqlTableDependencyFilter<Contenedor>(expression);
            _sqlTableDependency = new SqlTableDependency<Contenedor>(_connectionString,tableName:"temporal",filter:CONDICION);
            _sqlTableDependency.OnChanged += TableDependency_Changed;
            _sqlTableDependency.OnError += (sender, args) => Console.WriteLine($"error: {args.Message}");
            _sqlTableDependency.Start();
            while (!(_sqlTableDependency.Status == TableDependency.SqlClient.Base.Enums.TableDependencyStatus.WaitingForNotification)) { }

             expression = p => p.REGIMEN == "IMPO";
            CONDICION = new SqlTableDependencyFilter<Contenedor>(expression);
            _sqlTableDependency2 = new SqlTableDependency<Contenedor>(_connectionString, tableName: "temporal", filter: CONDICION);
            _sqlTableDependency2.OnChanged += TableDependency2_Changed;
            _sqlTableDependency2.OnError += (sender, args) => Console.WriteLine($"error: {args.Message}");
            _sqlTableDependency2.Start();
            while (!(_sqlTableDependency.Status == TableDependency.SqlClient.Base.Enums.TableDependencyStatus.WaitingForNotification)) { }
            Console.WriteLine(@"ESPERANDO NOTIFICACIONES contenedores");
        }
        #endregion


        #region SqlTableDependency

        private void TableDependency_Changed(object sender,RecordChangedEventArgs<Contenedor> e)
        {
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine($"DML: {e.ChangeType}");
            Console.WriteLine($"TABLA : CONTENEDOR");
            this.cambioExportaciones(e.Entity.ID, e.Entity.BUQUE, e.Entity.INICIALES + e.Entity.NUMERO, e.Entity.VIAJE, e.Entity.FECHA_ENTRADA, e.Entity.ESTADO, e.Entity.ALMACEN);            
        }

        private void TableDependency2_Changed(object sender, RecordChangedEventArgs<Contenedor> e)
        {
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine($"DML: {e.ChangeType}");
            Console.WriteLine($"TABLA : CONTENEDOR");
            this.cambioImportaciones(e.Entity.ID, e.Entity.BUQUE, e.Entity.INICIALES + e.Entity.NUMERO, e.Entity.VIAJE, e.Entity.FECHA_ENTRADA, e.Entity.ESTADO, e.Entity.ALMACEN);
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
            Console.WriteLine(dias.ToString());
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


        public void cambioImportaciones(string ID, string BUQUE, string CONTENEDOR, string VIAJE, string FECHA_ENTRADA, string ESTADO, string ALMACEN)
        {
            _callbackList.ForEach(delegate (IContenedorCallback callback) {
                callback.cambiosImpo(ID, BUQUE, CONTENEDOR, VIAJE, FECHA_ENTRADA, ESTADO, ALMACEN);
            });
        }

        public void cambioExportaciones(string ID, string BUQUE, string CONTENEDOR, string VIAJE, string FECHA_ENTRADA, string ESTADO, string ALMACEN)
        {
            _callbackList.ForEach(delegate (IContenedorCallback callback)
            {
                callback.cambiosExpo(ID, BUQUE, CONTENEDOR, VIAJE, FECHA_ENTRADA, ESTADO, ALMACEN);
            });
        }
        #endregion

        public void Dispose()
        {
            _sqlTableDependency.Stop();
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
