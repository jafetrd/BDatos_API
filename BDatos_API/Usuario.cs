using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDatos_API
{
    public static class Usuario
    {
        /*Datos del Usuario*/
        public static int ID_USUARIO { get; set; }
        public static string USUARIO { get; set; }
        public static string CONTRASEÑA { get; set; }
        public static string TIPO_USUARIO { get; set; }

        public static void limpiar()
        {
            ID_USUARIO = 0;
            USUARIO = null;
            CONTRASEÑA = null;
            TIPO_USUARIO = null;
        }
    }

    public static class Usuario2
    {
        /*Datos del Usuario*/
        public static int ID_USUARIO { get; set; }
        public static string USUARIO { get; set; }
        public static string CONTRASEÑA { get; set; }
        public static string TIPO_USUARIO { get; set; }

        public static void limpiar()
        {
            ID_USUARIO = 0;
            USUARIO = null;
            CONTRASEÑA = null;
            TIPO_USUARIO = null;
        }
    }

   

}
