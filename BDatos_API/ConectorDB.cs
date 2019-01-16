using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace BDatos_API
{
    class ConectorDB
    {
        public static SqlConnection conectar;
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
            //server = "localhost";
            //database = "bd_api";
            //Uid = "root";
            //pwd = "0000000000";
            //datos = "server=" + server + "; database=" + database + "; Uid=" + Uid + "; pwd=" + pwd + ";";
            datos = "Data Source=DESKTOP-481IKC1;" + "Initial Catalog=bd_api;" +"Integrated Security=SSPI; ";
            conectar = new SqlConnection(datos);
        }

       public static void AbrirConexion()
        {
            try
            {
                conectar.Open();
            }
            catch (SqlException)
            {

            }
        }

        public static bool ObtenerConexion()
        {
            {
                try
                {
                    conectar.Open();
                    return true;
                }
                catch (SqlException)
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
            catch (SqlException)
            {
              
            }
        }

        public static SqlDataReader Consultas(string SQL)
        {
            /*Realizamos la consulta a la BD para vervicar los el usuario y su contraseña*/
            SqlCommand comando = new SqlCommand(SQL, conectar);

            /*Ejecutamos la sentencia SQL descrita arriba*/
            SqlDataReader reader = comando.ExecuteReader();

            /*Retornamos lo que nos devolvio la consulta*/
            return reader;
        }//fin metodo estatico Consultas

        public static int Inyectar(string SQL)
        {
            int respuesta = 0;
            if (SQL != null)
            {
                /*Realizamos la inserción a la BD para guardar lo que indique el SQL*/
                SqlCommand comando = new SqlCommand(SQL, conectar);
                /*Ejecutamos la sentencia SQL descrita arriba*/
                respuesta = comando.ExecuteNonQuery();
                /*Retornamos un valor cualquiera*/
            }
            return respuesta;
        }

      
    }//fin de la clase
}//fin del namespace
