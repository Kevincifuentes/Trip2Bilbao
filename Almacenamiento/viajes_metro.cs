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
    
    public partial class viajes_metro
    {
        public viajes_metro()
        {
            this.viajes_parada_tiempos_metro = new HashSet<viajes_parada_tiempos_metro>();
        }
    
        public int id { get; set; }
        public int lineas_metro_id { get; set; }
        public string tiempoInicio { get; set; }
        public string tiempoFin { get; set; }
    
        public virtual lineas_metro lineas_metro { get; set; }
        public virtual ICollection<viajes_parada_tiempos_metro> viajes_parada_tiempos_metro { get; set; }
    }
}
