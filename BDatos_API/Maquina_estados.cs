using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDatos_API
{
    public static class Maquina_estados
    {
        public static int _ESTADO { get; set; }
        /// <summary>
        /// Fase 1
        /// </summary>
        public const int Entrada = 1;
        /// <summary>
        /// Fase 2
        /// </summary>
        public const int Autorizacion = 2;
        /// <summary>
        /// Fase 3
        /// </summary>
        public const int Salida = 3;

    }
}
