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
    
    public partial class viajes_bizkaibus
    {
        public viajes_bizkaibus()
        {
            this.paradas_bizkaibus = new HashSet<paradas_bizkaibus>();
        }
    
        public int id { get; set; }
        public int lineas_bizkaibusId { get; set; }
    
        public virtual lineas_bizkaibus lineas_bizkaibus { get; set; }
        public virtual ICollection<paradas_bizkaibus> paradas_bizkaibus { get; set; }
    }
}
