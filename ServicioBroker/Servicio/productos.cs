using ServicioBroker.Cambios;
using ServicioBroker.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.ServiceModel;
using System.Threading.Tasks;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.EventArgs;

namespace ServicioBroker.Servicio
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class productos : IProductos, IDisposable
    {
        #region Instance variables

        private readonly List<IproductosCallBack> _callbackList = new List<IproductosCallBack>();
        private readonly string _connectionString;
        private SqlTableDependency<Productos> _sqlTableDependency;
        #endregion

        #region Constructors

        public productos()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            _sqlTableDependency = new SqlTableDependency<Productos>(_connectionString, tableName: "productos");

            _sqlTableDependency.OnChanged += TableDependency_Changed;
            _sqlTableDependency.OnError += (sender, args) => Console.WriteLine($"error: {args.Message}");
            _sqlTableDependency.OnStatusChanged += _sqlTableDependency_OnStatusChanged;
            _sqlTableDependency.Start();

            while (!(_sqlTableDependency.Status == TableDependency.SqlClient.Base.Enums.TableDependencyStatus.WaitingForNotification)) { }
            Console.WriteLine(@"ESPERANDO NOTIFICACIONES productos");
        }

        #endregion

        #region SqlTableDependency
        private void _sqlTableDependency_OnStatusChanged(object sender, StatusChangedEventArgs e)
        {
            if (e.Status == TableDependency.SqlClient.Base.Enums.TableDependencyStatus.StopDueToCancellation)
            {
                _sqlTableDependency = null;
                _sqlTableDependency = new SqlTableDependency<Productos>(_connectionString, tableName: "productos");
                _sqlTableDependency.Start();
            }
        }

        private void TableDependency_Changed(
            object sender,
            RecordChangedEventArgs<Productos> e)
        {
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine($"DML: {e.ChangeType}");
            Console.WriteLine($"TABLA : PRODUCTOS");
            this.cambiosProductos(e.Entity.PRODUCTO);
        }
       
        public IList<Productos> obtenerTodosProductos()
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "SELECT * FROM [productos]";

                    return GetBuques(sqlCommand);
                }
            }
        }

        private IList<Productos> GetBuques(SqlCommand sqlCommand)
        {
            var contenedores = new List<Productos>();
            using (var sqlDataReader = sqlCommand.ExecuteReader())
            {
                if (sqlDataReader.HasRows)
                    while (sqlDataReader.Read())
                    {
                        contenedores.Add(new Productos
                        { 
                            PRODUCTO = sqlDataReader.SafeGetString("PRODUCTO")
                        });
                    }
            }
            return contenedores;
        }
        #endregion

        #region Publish-Subscribe design pattern


        public void Subscribe()
        {
            var registeredUser = OperationContext.Current.GetCallbackChannel<IproductosCallBack>();
            if (!_callbackList.Contains(registeredUser))
            {
                _callbackList.Add(registeredUser);
            }
        }

        public void Unsubscribe()
        {
            var registeredUser = OperationContext.Current.GetCallbackChannel<IproductosCallBack>();
            if (_callbackList.Contains(registeredUser))
            {
                _callbackList.Remove(registeredUser);
            }
        }

        public void cambiosProductos(string PRODUCTO)
        {
            _callbackList.ForEach(delegate (IproductosCallBack callback)
            {
                callback.cambiosProductos(PRODUCTO);
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
