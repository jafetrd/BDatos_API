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
        public static MySqlConnection ObtenerConexion()
        {
            MySqlConnection conectar = new MySqlConnection("server=localhost; database=bd_api; Uid=root; pwd=0000000000;");
            //MySqlConnection conectar = new MySqlConnection("server=127.0.0.1; database=egresadosie; Uid=root; pwd=;");
            conectar.Open();
            return conectar;
        }//fin metodo estatico ObtenerConexion

        public static MySqlDataReader Consultas(string SQL)
        {
            /*Realizamos la consulta a la BD para vervicar los el usuario y su contraseña*/
            MySqlCommand comando = new MySqlCommand(SQL, ObtenerConexion());

            /*Ejecutamos la sentencia SQL descrita arriba*/
            MySqlDataReader reader = comando.ExecuteReader();

            /*Retornamos lo que nos devolvio la consulta*/
            return reader;
        }//fin metodo estatico Consultas

        public static int Inyectar(string SQL)
        {
            int respuesta = 0;
            /*Realizamos la inserción a la BD para guardar lo que indique el SQL*/
            MySqlCommand comando = new MySqlCommand(SQL, ObtenerConexion());

            /*Ejecutamos la sentencia SQL descrita arriba*/
            respuesta = comando.ExecuteNonQuery();

            /*Retornamos un valor cualquiera*/
            return respuesta;
        }
    }//fin de la clase
}//fin del namespace
