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
    
    public partial class lineas_metro
    {
        public lineas_metro()
        {
            this.viajes_metro = new HashSet<viajes_metro>();
        }
    
        public int id { get; set; }
        public string abreviatura { get; set; }
        public string nombre { get; set; }
        public int tipo { get; set; }
        public string idMetro { get; set; }
    
        public virtual ICollection<viajes_metro> viajes_metro { get; set; }
    }
}
