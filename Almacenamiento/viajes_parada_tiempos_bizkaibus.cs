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
    
    public partial class viajes_parada_tiempos_bizkaibus
    {
        public int id { get; set; }
        public int paradas_bizkaibus_id { get; set; }
        public int viajes_bizkaibus_id { get; set; }
        public string tiempoLlegada { get; set; }
        public string tiempoSalida { get; set; }
    
        public virtual paradas_bizkaibus paradas_bizkaibus { get; set; }
        public virtual viajes_bizkaibus viajes_bizkaibus { get; set; }
    }
}