﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ModeloContainer : DbContext
    {
        public ModeloContainer()
            : base("name=ModeloContainer")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<entradas> entradasSet { get; set; }
        public virtual DbSet<tarifas> tarifasSet { get; set; }
        public virtual DbSet<parkings> parkingsSet { get; set; }
        public virtual DbSet<incidencias> incidenciasSet { get; set; }
        public virtual DbSet<paradas_tranvia> paradas_tranviaSet { get; set; }
        public virtual DbSet<hospitales> hospitalesSet { get; set; }
        public virtual DbSet<puntos_bici> puntos_biciSet { get; set; }
        public virtual DbSet<centros_de_salud> centros_de_saludSet { get; set; }
        public virtual DbSet<tiempos_dia_ciudad> tiempos_dia_ciudadSet { get; set; }
        public virtual DbSet<tiempos_ciudad> tiempos_ciudadSet { get; set; }
        public virtual DbSet<tiempos_comarca> tiempos_comarcaSet { get; set; }
        public virtual DbSet<tiempos_dia_comarca> tiempos_dia_comarcaSet { get; set; }
        public virtual DbSet<farmacias> farmaciasSet { get; set; }
        public virtual DbSet<tiempos_dia> tiempos_diaSet { get; set; }
        public virtual DbSet<tiempos_prediccion> tiempos_prediccionSet { get; set; }
        public virtual DbSet<lineas_metro> lineas_metroSet { get; set; }
        public virtual DbSet<viajes_metro> viajes_metroSet { get; set; }
        public virtual DbSet<paradas_metro> paradas_metroSet { get; set; }
        public virtual DbSet<lineas_bizkaibus> lineas_bizkaibusSet { get; set; }
        public virtual DbSet<viajes_bizkaibus> viajes_bizkaibusSet { get; set; }
        public virtual DbSet<paradas_bizkaibus> paradas_bizkaibusSet { get; set; }
        public virtual DbSet<paradas_bilbobus> paradas_bilbobusSet { get; set; }
        public virtual DbSet<tiempos_linea_parada> tiempos_linea_paradaSet { get; set; }
        public virtual DbSet<lineas_bilbobus> lineas_bilbobusSet { get; set; }
        public virtual DbSet<viajes_bilbobus> viajes_bilbobusSet { get; set; }
        public virtual DbSet<lineas_euskotren> lineas_euskotrenSet { get; set; }
        public virtual DbSet<viajes_euskotren> viajes_euskotrenSet { get; set; }
        public virtual DbSet<paradas_euskotren> paradas_euskotrenSet { get; set; }
        public virtual DbSet<estados_parking> estados_parkingSet { get; set; }
        public virtual DbSet<estados_puntobici> estados_puntobiciSet { get; set; }
        public virtual DbSet<parkingDeusto> parkingDeustoSet { get; set; }
        public virtual DbSet<viajes_parada_tiempos_euskotren> viajes_parada_tiempos_euskotrenSet { get; set; }
        public virtual DbSet<viajes_parada_tiempos_metro> viajes_parada_tiempos_metroSet { get; set; }
        public virtual DbSet<viajes_parada_tiempos_bizkaibus> viajes_parada_tiempos_bizkaibusSet { get; set; }
        public virtual DbSet<viajes_parada_tiempos_bilbobus> viajes_parada_tiempos_bilbobusSet { get; set; }
    }
}
