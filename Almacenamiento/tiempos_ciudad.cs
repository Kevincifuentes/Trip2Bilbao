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
    
    public partial class tiempos_ciudad
    {
        public tiempos_ciudad()
        {
            this.tiempos_dia_ciudad = new HashSet<tiempos_dia_ciudad>();
        }
    
        public int id { get; set; }
        public System.DateTime horaDeActualizacion { get; set; }
        public System.DateTime dia { get; set; }
        public string descripcionES { get; set; }
        public string descripcionEU { get; set; }
        public int tiempos_dia_ciudadId { get; set; }
    
        public virtual ICollection<tiempos_dia_ciudad> tiempos_dia_ciudad { get; set; }
    }
}
