
using System.Windows.Controls;

namespace BDatos_API
{
    public class TUsuario
    {
        public static string ERROR_CONEXION_BD
        {
            get
            {
                return "No se conecto servidor. Contactar administrador\nDatos registrados: \nServidor: " + ConectorDB.server + "\nBase de datos: " + ConectorDB.database;
            }
        }


        public const string NOMBRE_TABLA = "tabla_usuario";
        public const string ID_USUARIO = "id_usuario"; 
        public const string CONTRASEÑA = "contraseña_Usuario";
        public const string NOMBRE = "nombre_Usuario"; 
        public const string TIPO_USUARIO = "tipo_Usuario";
        public const string TODO = "*";
        public const string RUTA_ENLACE_DATAGRID = "cargarUsuarios";
        public const int CANTIDAD_COLUMNAS = 4;

        public const string TITULO_MENSAJE = "Administrador de usuarios";
        public const string TIPO_ADMINISTRADOR = "Administrador";
        public const string COMUN = "Común";
        public const string REGISTRO = "Registro";

        public const int CERRAR = 1;
        public const int ACTUALIZAR = 2;
        public const int ELIMINAR = 3;
        public const int BUSCAR = 4;
        public const int GUARDAR = 5;
        public const int ERROR_BD = 6;
        public const int LIMPIAR = 7;
        public const int APUNTAR = 8;
        public const int ULTIMO_USUARIO=9;
      
        public const string BTN_GUARDAR = "GUARDAR"; 
        public const string BTN_EDITAR = "EDITAR"; 
        public const string BTN_BORRAR = "BORRAR"; 
        public const string BTN_LIMPIAR = "LIMPIAR"; 
      
        public static string ID_USUARIO_D { get; set; }
        public static string NOMBRE_D { get; set; }
        public static string CONTRASEÑA_D { get; set; }
        public static string TIPO_USUARIO_D { get; set; }
       
        public static string ID_USUARIO_C { get; set; }
        public static string NOMBRE_C { get; set; }
        public static string CONTRASEÑA_C { get; set; }
        public static string TIPO_USUARIO_C { get; set; }
      
        public static void LimpiarDatos()
        {
            ID_USUARIO_D = null;
            NOMBRE_D = null;
            CONTRASEÑA_D = null;
            TIPO_USUARIO_D = null;
        }

        public static void LimpiarCopias()
        {
            ID_USUARIO_C = null;
            NOMBRE_C = null;
            CONTRASEÑA_C = null;
            TIPO_USUARIO_C = null;
        }
    }

    public static class InicioSesion
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
        public static string TITULO_MENSAJE { get { return "Inicio de sesión"; } }
        public static string TIPO_ADMINISTRADOR { get { return "Administrador"; } }
        public static int numeroColumnas { get { return 4; } }

        public const int ABRIR = 1;
        public const int CUENTA_NUEVA = 2;
     


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
