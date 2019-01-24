using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Controls.Primitives;
namespace BDatos_API
{
    class Metodos_bd
    {
        /// <summary>
        /// Metodo para rellenar una tabla 
        /// </summary>
        /// <param name="DataGrid"> Nombre de la tabla principal</param>
        /// <param name="NombreTabla"> Nombre de la tabla SQL </param>
        /// <param name="RutaEnlaceDataGrid"> nombre de la ruta de enlace con DataGrid </param>
        /// <param name="Columnas"> Columnas de la tabla SQL </param>
        public DataSet LLENAR_DATAGRID(MultiSelector DataGrid,string NombreTabla,string RutaEnlaceDataGrid, params string[] Columnas)
        {
            string campos_local = String.Join(",", Columnas);
            ConectorDB.AbrirConexion();
            string SQL = "SELECT "+ campos_local +" FROM "+NombreTabla;
            SqlCommand mySqlCommand = new SqlCommand(SQL, ConectorDB.conectar);
            SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter(mySqlCommand);
            DataSet dataSet = new DataSet();
            mySqlDataAdapter.Fill(dataSet,RutaEnlaceDataGrid);
            //DataGrid.DataContext = dataSet;
            ConectorDB.CerrarConexion();
            return dataSet;
        }

        /// <summary>
        /// Obtiene el ultimo ID de la tabla
        /// </summary>
        /// <param name="NombreTabla"> Nombre de la tabla SQL </param>
        /// <param name="NombreColumna"> Sobre que columna se ordenara la busqueda (casi siempre ID del registro) </param>
        /// <param name="CantidadColumnas">Numero de columnas de la tabla</param>
        /// <returns></returns>
        public int ULTIMO_REGISTRO(string NombreTabla, string NombreColumna)
        {
            string SQL = "SELECT MAX(" + NombreColumna + ") FROM " + NombreTabla;
            SqlCommand sqlCommand = ConectorDB.conectar.CreateCommand();
            sqlCommand.CommandText = SQL;
            ConectorDB.AbrirConexion();
            int maxId = Convert.ToInt32(sqlCommand.ExecuteScalar());
            ConectorDB.CerrarConexion();
            return maxId;
        }

        /// <summary>
        /// Regresa las columnas que se le indiquen
        /// </summary>
        /// <param name="NombreTabla">Nombre de la tabla</param>
        /// <param name="Columnas">Nombre de las columnas a retornar</param>
        /// <returns></returns>
        public DataTable REGRESAR_TODO(string NombreTabla, params string[] Columnas)
        {
            string campos_local = String.Join(",", Columnas);
            ConectorDB.AbrirConexion();
            string SQL = "SELECT " + campos_local + " FROM " + NombreTabla;
            SqlCommand mySqlCommand = new SqlCommand(SQL, ConectorDB.conectar);
            SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter(mySqlCommand);
            DataSet dataSet = new DataSet();
            mySqlDataAdapter.Fill(dataSet);
            ConectorDB.CerrarConexion();
            return dataSet.Tables[0];

        }

        /// <summary>
        /// Metodo para seleccionar (buscar) datos en la tabla SQL
        /// </summary>
        /// <param name="NombreTabla">Nombre de la tabla SQL</param>
        /// <param name="NombreColumna">Nombre de la columna en la cual se buscara</param>
        /// <param name="Dato_Buscar">Dato que se buscara en la columna seleccionada</param>
        /// <param name="CantidadColumnas">Cantidad de columnas de la tabla</param>
        /// <param name="ColumnasRetorno">Las columnas que se retornaran de la busqueda</param>
        /// <returns></returns>
        public ArrayList BUSCAR(string NombreTabla,string NombreColumna,string Dato_Buscar,int CantidadColumnas, params string[] ColumnasRetorno)
        {
            object[] local;
            string columna = String.Join(",", ColumnasRetorno);
            TemporalGetSet temporal = new TemporalGetSet();
            string parametroSelector = "@" + NombreColumna;
            //string SQL = "SELECT "+columna+" FROM " + NombreTabla + " WHERE " + NombreColumna + " = " + parametroSelector;
            string SQL = "SELECT " + columna + " FROM " + NombreTabla + " WHERE " + NombreColumna + " IN (" + parametroSelector + ")";
                SqlCommand sqlCommand = ConectorDB.conectar.CreateCommand();
                sqlCommand.CommandText = SQL;
                sqlCommand.Parameters.AddWithValue(parametroSelector, Dato_Buscar);
                ConectorDB.AbrirConexion();
                SqlDataReader reader = sqlCommand.ExecuteReader();

            local = new object[reader.FieldCount];
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int a = reader.GetValues(local);
                }
                for (int a = 0; a < reader.FieldCount; a++)
                    temporal.Lista.Add(local[a]);
            }
            ConectorDB.CerrarConexion(); 
                return temporal.Lista;
        }


        /// <summary>
        /// Metodo para guardar datos en la base SQL
        /// </summary>
        /// <param name="tabla">Insertar el nombre de la tabla SQL</param>
        /// <param name="datos">Insertar el nombre de la columna y la variable de donde se obtiene la informacion (columna,campo)</param>

        public void GUARDAR(string tabla, params (string columnas, string campos)[] datos)
        {
            string nombreParametroColumna = null;
            string campos_local = null;
            int longitud = datos.Length - 1;
            string[] parametro = new string[datos.Length];
            //concatenacion de las columnas en formato SQL
            for (int a = 0; a < longitud; a++)
            {
                nombreParametroColumna += "@" + datos[a].columnas + ",";
                parametro[a] = "@" + datos[a].columnas;
            }
            nombreParametroColumna += "@" + datos[longitud].columnas;
            parametro[longitud] = "@" + datos[longitud].columnas;

            //concatenacion de los campos en formato SQL 
            for (int a = 0; a < longitud; a++)
            {
                campos_local += datos[a].columnas + ",";
            }
            campos_local += datos[longitud].columnas;

            string SQL = "Insert into " + tabla + " (" + campos_local + ") values (" + nombreParametroColumna + ")";

            SqlCommand sqlCommand = ConectorDB.conectar.CreateCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = SQL;

            for (int a = 0; a < datos.Length; a++)
            {
                sqlCommand.Parameters.AddWithValue(parametro[a], ((object)datos[a].campos)??DBNull.Value);
            }
            ConectorDB.AbrirConexion();
            int query = sqlCommand.ExecuteNonQuery();
            ConectorDB.CerrarConexion();
        }

        /// <summary>
        /// Metodo para actualizar la base SQL
        /// </summary>
        /// <param name="tabla">Agregar el nombre de la tabla</param>
        /// <param name="selector">Dato que especifica que fila se va a actualizar</param>
        /// <param name="dato"> Indica sobre que registro se hara la actualizacion </param>
        /// <param name="datos"> Insertar el nombre de la columna y la variable de donde se obtiene la informacion (columna,campo)</param>
        public void ACTUALIZAR(string tabla,string selector,string dato, params (string columnas, string campos)[] datos)
        {
            string aux=null;
            string[] parametros = new string[datos.Length];
            int longitud = datos.Length - 1;
            string parametroSelector = "@" + selector;

            for (int a = 0; a < longitud; a++)
            {
                aux += datos[a].columnas + "=@" + datos[a].columnas+",";
                parametros[a] = "@" + datos[a].columnas;
            }
            aux += datos[longitud].columnas + "=@" + datos[longitud].columnas;
            parametros[longitud] = "@" + datos[longitud].columnas;

            string SQL = "UPDATE " + tabla + " SET "+aux+" WHERE "+selector+ "=@" + selector;
            SqlCommand sqlCommand = ConectorDB.conectar.CreateCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = SQL;
            for (int a = 0; a < datos.Length; a++)
            {
                sqlCommand.Parameters.AddWithValue(parametros[a], ((object)datos[a].campos) ?? DBNull.Value);
            }
            sqlCommand.Parameters.AddWithValue(parametroSelector, dato);
            ConectorDB.AbrirConexion();
            int query = sqlCommand.ExecuteNonQuery();
            ConectorDB.CerrarConexion();
        }

        /// <summary>
        /// Metodo para eliminar datos de la Base SQL
        /// </summary>
        /// <param name="tabla">Agregar el nombre de la tabla</param>
        /// <param name="selector">Dato que especifica que fila se va a actualizar</param>
        /// <param name="filas">Indica sobre que registro se hara la actualizacion</param>
        public void ELIMINAR(string tabla,string selector,params string[] filas)
        {
            int longitud = filas.Length;
            string[] parametros = new string[filas.Length];
            string aux = null;
            for (int a = 0; a < longitud; a++)
            {
                parametros[a] = "@" + filas[a]; 
            }
            aux = string.Join(",", parametros);
            string SQL_1 = "DELETE from " + tabla + " WHERE " + selector + " IN ({0})";
            string SQL = string.Format(SQL_1, aux);
            
            SqlCommand sqlCommand = ConectorDB.conectar.CreateCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = SQL;
            for(int a = 0; a < longitud; a++)
            {
                sqlCommand.Parameters.AddWithValue(parametros[a], filas[a]);
            }
            ConectorDB.AbrirConexion();
            int query = sqlCommand.ExecuteNonQuery();
            ConectorDB.CerrarConexion();
        }
    }

    public class TemporalGetSet
    {
        private ArrayList _array = new ArrayList(); // use underscore to indicate private field
        public ArrayList Lista
        {
            get { return _array; } // do not implement setter as to avoid outside to overwrite the object's array instance.
        }
        /* Shortcut getter/setter to the array */
        public object this[int index]
        {
            get { return _array[index]; }
            set { _array[index] = value; }
        }
    }

}
