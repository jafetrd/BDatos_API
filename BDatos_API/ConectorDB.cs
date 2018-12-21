using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BDatos_API
{
    class ConectorDB
    {
        public static MySqlConnection conectar;
        public static string server=null;
        public static string database = null;
        public static string Uid = null;
        public static string pwd = null;
        public static string datos = null;
        public ConectorDB(){
            Initialize();
        }

        public static void Initialize()
        {
            server = "localhost";
            database = "bd_api";
            Uid = "root";
            pwd = "0000000000";
            datos = "server=" + server + "; database=" + database + "; Uid=" + Uid + "; pwd=" + pwd + ";";
            conectar = new MySqlConnection(datos);
        }

        public static MySqlConnection ObtenerConexion2()
        {
            conectar.Open();
            return conectar;
        }

        public static bool ObtenerConexion()
        {
            {
                try
                {
                    conectar.Open();
                    return true;
                }
                catch (MySqlException)
                {
                    return false;
                }
            }
        }//fin metodo estatico ObtenerConexion

        public static void CerrarConexion()
        {
            try
            {
                conectar.Close();
            }
            catch (MySqlException)
            {
              
            }
        }

        public static MySqlDataReader Consultas(string SQL)
        {
            /*Realizamos la consulta a la BD para vervicar los el usuario y su contraseña*/
            MySqlCommand comando = new MySqlCommand(SQL, conectar);

            /*Ejecutamos la sentencia SQL descrita arriba*/
            MySqlDataReader reader = comando.ExecuteReader();

            /*Retornamos lo que nos devolvio la consulta*/
            return reader;
        }//fin metodo estatico Consultas

        public static int Inyectar(string SQL)
        {
            int respuesta = 0;
            if (SQL != null)
            {
                /*Realizamos la inserción a la BD para guardar lo que indique el SQL*/
                MySqlCommand comando = new MySqlCommand(SQL, conectar);
                /*Ejecutamos la sentencia SQL descrita arriba*/
                respuesta = comando.ExecuteNonQuery();
                /*Retornamos un valor cualquiera*/
            }
            return respuesta;
        }

      
    }//fin de la clase
}//fin del namespace
