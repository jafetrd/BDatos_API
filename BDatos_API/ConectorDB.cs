using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Alumnos
{
    class ConectorDB
    {
        public static SqlConnection ObtenerConexion()
        {
            SqlConnection conectar = new SqlConnection("server=127.0.0.1; database=egresadosie; Uid=root; pwd=0000000000;");
            //MySqlConnection conectar = new MySqlConnection("server=127.0.0.1; database=egresadosie; Uid=root; pwd=;");
            conectar.Open();
            return conectar;
        }//fin metodo estatico ObtenerConexion

        public static SqlDataReader Consultas(string SQL)
        {
            /*Realizamos la consulta a la BD para vervicar los el usuario y su contraseña*/
            SqlCommand comando = new SqlCommand(SQL, ObtenerConexion());

            /*Ejecutamos la sentencia SQL descrita arriba*/
            SqlDataReader reader = comando.ExecuteReader();

            /*Retornamos lo que nos devolvio la consulta*/
            return reader;
        }//fin metodo estatico Consultas

        public static int Inyectar(string SQL)
        {
            int respuesta = 0;
            /*Realizamos la inserción a la BD para guardar lo que indique el SQL*/
            SqlCommand comando = new SqlCommand(SQL, ObtenerConexion());

            /*Ejecutamos la sentencia SQL descrita arriba*/
            respuesta = comando.ExecuteNonQuery();

            /*Retornamos un valor cualquiera*/
            return respuesta;
        }
    }//fin de la clase
}//fin del namespace
