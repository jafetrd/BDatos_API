using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace BDatos_API
{
    class Metodos_bd
    {
        /// <summary>
        /// Metodo para rellenar una tabla 
        /// </summary>
        /// <param name="grid"> Nombre de la tabla principal</param>
        /// <param name="campos"> Columnas de la tabla SQL "columna1,columna2" </param>
        /// <param name="tabla_SQL"> Nombre de la tabla SQL </param>
        /// <param name="tabla_DataGRID"> nombre de la ruta de enlace con DataGrid </param>
        public void popular_tabla(MultiSelector grid, string campos,string tabla_SQL,string tabla_DataGRID)
        {
            ConectorDB.AbrirConexion();
            string SQL = "SELECT "+ campos +" FROM "+tabla_SQL;
            MySqlCommand mySqlCommand = new MySqlCommand(SQL, ConectorDB.conectar);
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
            DataSet dataSet = new DataSet();
            mySqlDataAdapter.Fill(dataSet, tabla_DataGRID);
            grid.DataContext = dataSet;
            ConectorDB.CerrarConexion();
        }

        /// <summary>
        /// Metodo para obtener los datos basados en un criterio de seleccion
        /// </summary>
        /// <param name="tabla_SQL">Nombre de la tabla SQL</param>
        /// <param name="criterio_seleccion"> Columnas de la tabla SQL, ejemplo: columna1 = '{0}' AND columna2 = '{1}'</param>
        /// <param name="datos">Datos a guardar en fomato array </param>
        public void obtener_por_criterio(string tabla_SQL,string criterio_seleccion,object[] datos)
        {
            string SQL = string.Format("SELECT * FROM "+tabla_SQL+" WHERE "+criterio_seleccion, datos);
            MySqlDataReader reader = ConectorDB.Consultas(SQL);
            while (reader.Read())
            {
                Usuario2.ID_USUARIO = reader.GetInt32(0);
                Usuario2.USUARIO = reader.GetString(1);
                Usuario2.CONTRASEÑA = reader.GetString(2);
                Usuario2.TIPO_USUARIO = reader.GetInt16(3);
            }
        }
    }
}
