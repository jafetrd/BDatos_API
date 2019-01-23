
using MahApps.Metro;
using MahApps.Metro.Controls.Dialogs;
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

        public const string NOMBRE_TABLA = "dbo.tabla_usuario";
        public const string ID_USUARIO = "id_usuario";
        public const string CONTRASEÑA = "contraseña_Usuario";
        public const string NOMBRE = "nombre_Usuario";
        public const string TIPO_USUARIO = "tipo_Usuario";
        public static string TODO ="*";
        public static string TITULO_MENSAJE = "Inicio de sesión";
        public static string TIPO_ADMINISTRADOR = "Administrador";
        public static int CANTIDAD_COLUMNAS = 4;

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

    public static class tablaBuque
    {
        public const string NOMBRE_TABLA = "buques";

        public const string ID_ = "ID";
        public const string BUQUE_ = "BUQUE";
        public const string VIAJE_ = "VIAJE";
    }

    public static class tablaCliente
    {
        public const string NOMBRE_TABLA = "clientes";

        public const string ID_ = "ID";
        public const string CLIENTE_ = "CLIENTE";
        public const int CANTIDAD_COLUMNAS_ = 2;
    }

    public static class tablaProducto
    {
        public const string NOMBRE_TABLA = "productos";

        public const string ID_ = "ID";
        public const string PRODUCTO_ = "PRODUCTO";
        public const int CANTIDAD_COLUMNAS_ = 2;
    }

    public static class nombresPrincipal
    {
        public const string TABLA_FERROCARRIL = "tabla_patio_ferrocarril";
        public const string TABLA_CONTENEDOR = "tabla_patio_contenedor";
        public const string TODO = "*";
        public const string ID_ = "ID";
        public const string BUQUE_ = "BUQUE";
        public const string VIAJE_ = "VIAJE";
        public const string REGIMEN_ = "REGIMEN";
        public const string FECHA_ENTRADA_ = "FECHA_ENTRADA";

        public const string PRESENTACION_ = "PRESENTACION";
        public const string INICIALES_ = "INICIALES";
        public const string NUMERO_ = "NUMERO";
        public const string CONTENEDOR = "Contenedores";
        public const string FERROCARRIL = "Ferrocarril";
        public const string SESION_ENTRADA_ = "SESION_ENTRADA";
        public const string SESION_SALIDA_ = "SESION_SALIDA";
    }

    public static class nombresPatioFerrocarril
    {
        public const string NOMBRE_TABLA = "tabla_patio_ferrocarril";
        public const string ID_ = "ID";
        public const string BUQUE_ = "BUQUE";
        public const string VIAJE_ = "VIAJE";
        public const string REGIMEN_ = "REGIMEN";
        public const string FECHA_ENTRADA_ = "FECHA_ENTRADA";

        public const string PRESENTACION_ = "PRESENTACION";
        public const string INICIALES_ = "INICIALES";
        public const string NUMERO_ = "NUMERO";
        public const string PESO_ = "PESO";
        public const string UNIDADES_ = "UNIDADES";
        public const string PRODUCTO_ = "PRODUCTO";
        public const string CLIENTE_ = "CLIENTE";
        public const string PEDIMENTO_ = "PEDIMENTO";
        public const string VALOR_COMERCIAL_ = "VALOR_COMERCIAL";
     
        public const string FECHA_SALIDA_ = "FECHA_SALIDA";
        public const string SESION_ENTRADA_ = "SESION_ENTRADA";
        public const string SESION_AUTORIZACION_ = "SESION_AUTORIZACION";
        public const string SESION_SALIDA_ = "SESION_SALIDA";
        public const string ESTADO_ = "ESTADO";

        public const string ALMACEN_ = "ALMACEN";

        public const string NOMBRE_TABLA_2 = "dform_patioferrocarril";
        public const string TABLA2_ID = "ID";
        public const string TABLA2_PRESENTACIONES = "Presentaciones";
        public static int CANTIDAD_COLUMNAS_2 = 2;
        public static string TODO = "*";
        public static string TITULO_MENSAJE = "Patio de ferrocarriles";



        public const int NINGUNO = 0;
        public const int GUARDAR = 1;
        public const int ACTUALIZAR = 2;
        public const int ELIMINAR = 3;
        public const int LIMPIAR = 4;

        public const string IMPORTACION = "IMPORTACION";
        public const string EXPORTACION = "EXPORTACION";
        public const string IMPO_SQL = "IMPO";
        public const string EXPO_SQL = "EXPO";
    }

    public static class nombresPatioContenedor
    {
        public const string NOMBRE_TABLA = "tabla_patio_contenedor";
        public const string ID_ = "ID";
        public const string BUQUE_ = "BUQUE";
        public const string VIAJE_ = "VIAJE";
        public const string REGIMEN_ = "REGIMEN";
        public const string FECHA_ENTRADA_ = "FECHA_ENTRADA";

        public const string PRESENTACION_ = "PRESENTACION";
        public const string INICIALES_ = "INICIALES";
        public const string NUMERO_ = "NUMERO";
        public const string PESO_ = "PESO";
        public const string UNIDADES_ = "UNIDADES";
        public const string PRODUCTO_ = "PRODUCTO";
        public const string CLIENTE_ = "CLIENTE";
        public const string PEDIMENTO_ = "PEDIMENTO";
        public const string VALOR_COMERCIAL_ = "VALOR_COMERCIAL";

        public const string FECHA_SALIDA_ = "FECHA_SALIDA";
        public const string SESION_ENTRADA_ = "SESION_ENTRADA";
        public const string SESION_AUTORIZACION_ = "SESION_AUTORIZACION";
        public const string SESION_SALIDA_ = "SESION_SALIDA";
        public const string ESTADO_ = "ESTADO";

        public const string ALMACEN_ = "ALMACEN";

        public const string NOMBRE_TABLA_2 = "dform_patiocontenedor";
        public const string TABLA2_ID = "ID";
        public const string TABLA2_PRESENTACIONES = "Presentaciones";
        public static int CANTIDAD_COLUMNAS_2 = 2;
        public static string TODO = "*";
        public static string TITULO_MENSAJE = "Patio de contenedores";

        public const int NINGUNO = 0;
        public const int GUARDAR = 1;
        public const int ACTUALIZAR = 2;
        public const int ELIMINAR = 3;
        public const int LIMPIAR = 4;

        public const string IMPORTACION = "IMPORTACION";
        public const string EXPORTACION = "EXPORTACION";
        public const string IMPO_SQL = "IMPO";
        public const string EXPO_SQL = "EXPO";
    }

    public static class nombresBodegaC
    {
        public const string NOMBRE_TABLA_2 = "dform_bodegac";
        public const string TABLA2_ID = "ID";
        public const string TABLA2_PRESENTACIONES = "Presentaciones";
        public static int CANTIDAD_COLUMNAS_2 = 2;
        public static string TODO = "*";
        public const string BODEGA_C = "Bodega C";
        public const string BODEGA_2 = "Bodega 2";
        public const string TITULO_MENSAJE_C = "Bodega C";
        public const string TITULO_MENSAJE_2 = "Bodega 2";
    }

    public static class nombresVentanas
    {
        public const string BodegaC = "Bodega C";
        public const string Bodega2 = "Bodega 2";
        public const string PatioContenedores = "Patio de contenedores";
        public const string PatioFerrocarriles = "Patio de ferrocarriles";
        public const string BusquedayReportes = "Busqueda y reportes";
        public const string Principal = "Principal";
        public const string Configuracion = "Configuracion";
    }
    public class configMetroDialog
    {
        public MetroDialogSettings mensajeBorrar = new MetroDialogSettings
        {
            AffirmativeButtonText = "Si",
            NegativeButtonText = "No",
            ColorScheme = MetroDialogColorScheme.Inverted
        };

        public MetroDialogSettings mensajeNormal = new MetroDialogSettings
        {
            AffirmativeButtonText = "Si",
            NegativeButtonText = "No",
            ColorScheme = MetroDialogColorScheme.Theme
        };

        public MetroDialogSettings mensajeAcentuado = new MetroDialogSettings
        {
            AffirmativeButtonText = "Si",
            NegativeButtonText = "No",
            ColorScheme = MetroDialogColorScheme.Accented
        };

        public MetroDialogSettings PREVIEW = new MetroDialogSettings
        {
            AffirmativeButtonText = "Si",
            NegativeButtonText = "No",
            ColorScheme = MetroDialogColorScheme.Theme,
            DialogMessageFontSize = 30,
            AnimateShow = true,
            DialogTitleFontSize = 40
        };
    }

}
