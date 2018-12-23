using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDatos_API
{
    public static class TablaUsuario
    {
        public static string ERROR_CONEXION_BD
        {
            get
            {
                return "No se conecto servidor. Contactar administrador\nDatos registrados: \nServidor: " + ConectorDB.server + "\nBase de datos: " + ConectorDB.database;
            }
        }

        public static string TABLA_USUARIO { get { return "tabla_usuario"; } }
        public static string ID_USUARIO { get { return "id_usuario"; } }
        public static string CONTRASEÑA { get { return "contraseña_Usuario"; } }
        public static string NOMBRE { get { return "nombre_Usuario"; } }
        public static string TIPO_USUARIO { get { return "tipo_Usuario"; } }
        public static string TODO { get { return "*"; } }
        public static int numeroColumnas { get { return 4; } }

        public const int CERRAR = 1;
        public const int ACTUALIZAR = 2;
        public const int BORRAR = 3;
        public const int SELECCIONAR = 4;
        public const int CREAR = 5;
        public const int ERROR_BD = 6;
        public const int LIMPIAR = 7;
        public const int APUNTAR = 8;

        public static string BTN_GUARDAR { get { return "GUARDAR"; } }
        public static string BTN_EDITAR { get { return "EDITAR"; } }
        public static string BTN_BORRAR { get { return "BORRAR"; } }
        public static string BTN_LIMPIAR { get { return "LIMPIAR"; } }

        public static string ID_USUARIOdato { get; set; }
        public static string USUARIOdato { get; set; }
        public static string CONTRASEÑAdato { get; set; }
        public static string TIPO_USUARIOdato { get; set; }

        public static string ID_USUARIOcopia { get; set; }
        public static string USUARIOcopia { get; set; }
        public static string CONTRASEÑAcopia { get; set; }
        public static string TIPO_USUARIOcopia { get; set; }

    }


}
