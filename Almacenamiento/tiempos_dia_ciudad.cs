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
    
    public partial class tiempos_dia_ciudad
    {
        public int id { get; set; }
        public string nombreCiudad { get; set; }
        public string descripcionES { get; set; }
        public string descripcionEU { get; set; }
        public int maxima { get; set; }
        public int minima { get; set; }
        public int tiempos_ciudadId { get; set; }
    
        public virtual tiempos_ciudad tiempos_ciudad { get; set; }
    }
}
