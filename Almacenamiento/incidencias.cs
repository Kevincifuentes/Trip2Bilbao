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
    
    public partial class incidencias
    {
        public int id { get; set; }
        public string tipo { get; set; }
        public string descripcion { get; set; }
        public double latitud { get; set; }
        public double longitud { get; set; }
        public System.DateTime fechaInicio { get; set; }
        public System.DateTime fechaFin { get; set; }
    }
}