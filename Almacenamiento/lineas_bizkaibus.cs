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
    
    public partial class lineas_bizkaibus
    {
        public lineas_bizkaibus()
        {
            this.viajes_bizkaibus = new HashSet<viajes_bizkaibus>();
        }
    
        public int id { get; set; }
        public string abreviatura { get; set; }
        public string nombre { get; set; }
        public int tipoTransporte { get; set; }
    
        public virtual ICollection<viajes_bizkaibus> viajes_bizkaibus { get; set; }
    }
}
