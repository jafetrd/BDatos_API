using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
namespace BDatos_API
{
    class Metodos_bd
    {
        /// <summary>
        /// Metodo para rellenar una tabla 
        /// </summary>
        /// <param name="grid"> Nombre de la tabla principal</param>
        /// <param name="tabla_SQL"> Nombre de la tabla SQL </param>
        /// <param name="tabla_DataGRID"> nombre de la ruta de enlace con DataGrid </param>
        /// <param name="campos"> Columnas de la tabla SQL </param>
        public void popular_tabla(MultiSelector grid,string tabla_SQL,string tabla_DataGRID, params string[] campos)
        {
            string campos_local = String.Join(",", campos);
            ConectorDB.AbrirConexion();
            string SQL = "SELECT "+ campos_local +" FROM "+tabla_SQL;
            MySqlCommand mySqlCommand = new MySqlCommand(SQL, ConectorDB.conectar);
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
            DataSet dataSet = new DataSet();
            mySqlDataAdapter.Fill(dataSet, tabla_DataGRID);
            grid.DataContext = dataSet;
            ConectorDB.CerrarConexion();
        }

        /// <summary>
        /// Metodo para seleccionar (buscar) datos en la tabla SQL
        /// </summary>
        /// <param name="tabla_SQL">Nombre de la tabla SQL</param>
        /// <param name="selector">Nombre de la columna en la cual se buscara</param>
        /// <param name="dato">Dato que se buscara en la columna seleccionada</param>
        /// <param name="numColum">Cantidad de columnas de la tabla</param>
        /// <param name="columnas">Las columnas que se retornaran de la busqueda</param>
        /// <returns></returns>
        public ArrayList obtener_por_criterio(string tabla_SQL,string selector,string dato,int numColum, params string[] columnas)
        {
            int count = 0;
            string columna = String.Join(",", columnas);
            TemporalGetSet temporal = new TemporalGetSet();
            string parametroSelector = "@" + selector;
                string SQL = "SELECT "+columna+" FROM " + tabla_SQL + " WHERE " + selector + " = " + parametroSelector;
                MySqlCommand sqlCommand = ConectorDB.conectar.CreateCommand();
                sqlCommand.CommandText = SQL;
                sqlCommand.Parameters.AddWithValue(parametroSelector, dato);
                ConectorDB.AbrirConexion();
                MySqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read()) { count++; }
            if (count > 0)
            {
                for (int a = 0; a < numColum; a++)
                {
                    temporal.Lista.Add(reader.GetString(a));
                }
            }
                ConectorDB.CerrarConexion(); 
                return temporal.Lista;
        }
        /// <summary>
        /// Metodo para guardar datos en la base SQL
        /// </summary>
        /// <param name="tabla">Insertar el nombre de la tabla SQL</param>
        /// <param name="datos">Insertar el nombre de la columna y la variable de donde se obtiene la informacion (columna,campo)</param>

        public void guardarenSQL(string tabla, params (string columnas, string campos)[] datos)
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

            MySqlCommand sqlCommand = ConectorDB.conectar.CreateCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = SQL;

            for (int a = 0; a < datos.Length; a++)
            {
                sqlCommand.Parameters.AddWithValue(parametro[a], datos[a].campos);
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
        public void actualizar(string tabla,string selector,string dato, params (string columnas, string campos)[] datos)
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
            MySqlCommand sqlCommand = ConectorDB.conectar.CreateCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = SQL;
            for (int a = 0; a < datos.Length; a++)
            {
                sqlCommand.Parameters.AddWithValue(parametros[a], datos[a].campos);
            }
            sqlCommand.Parameters.AddWithValue(parametroSelector, dato);
            ConectorDB.AbrirConexion();
            int query = sqlCommand.ExecuteNonQuery();
            ConectorDB.CerrarConexion();
        }

        public void eliminar(string tabla,string selector,params string[] filas)
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
            
            MySqlCommand sqlCommand = ConectorDB.conectar.CreateCommand();
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
