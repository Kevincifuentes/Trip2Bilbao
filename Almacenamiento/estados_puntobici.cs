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
    
    public partial class estados_puntobici
    {
        public int id { get; set; }
        public int anclajeslibres { get; set; }
        public int bicisLibres { get; set; }
        public System.DateTime fecha { get; set; }
        public int puntos_bici_id { get; set; }
    
        public virtual puntos_bici puntos_bici { get; set; }
    }
}