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
    
    public partial class viajes_parada_tiempos_euskotren
    {
        public int Id { get; set; }
        public int viajes_euskotren_id { get; set; }
        public int paradas_euskotren_id { get; set; }
        public string tiempoLlegada { get; set; }
        public string tiempoSalida { get; set; }
    
        public virtual viajes_euskotren viajes_euskotren { get; set; }
        public virtual paradas_euskotren paradas_euskotren { get; set; }
    }
}
