//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Almacenamiento
{
    using System;
    using System.Collections.Generic;
    
    public partial class tiempos_linea_parada
    {
        public int id { get; set; }
        public int tiempoEspera { get; set; }
        public int lineas_bilbobusId { get; set; }
        public int paradas_bilbobusId { get; set; }
        public System.DateTime fecha { get; set; }
    
        public virtual lineas_bilbobus lineas_bilbobus { get; set; }
        public virtual paradas_bilbobus paradas_bilbobus { get; set; }
    }
}
