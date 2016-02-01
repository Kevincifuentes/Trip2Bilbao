using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using Clases;

namespace Almacenamiento
{
    
    public class Insertar
    {
        private ModeloContainer contexto;

        public Insertar()
        {
            contexto = new ModeloContainer();
        }

        public void centrodeSalud(CentroSalud c)
        {
            centros_de_salud nuevo = new centros_de_salud();
            nuevo.calle = c.calle;
            nuevo.ciudad = c.ciudad;
            nuevo.codigoPostal = c.codigoPostal;
            nuevo.codigo_centro = c.codigoCentro;
            nuevo.direcionCompleta = c.direccionCompleta;
            nuevo.horario = c.horario;
            nuevo.latitud = c.localizacion.latitud;
            nuevo.longitud = c.localizacion.longitud;
            nuevo.nombre = c.nombreCentro;
            nuevo.provincia = c.provincia;
            nuevo.region = c.region;
            nuevo.telefono = c.telefono;
            nuevo.urlAdicional = c.urlAdicional;
            nuevo.web = c.web;

            contexto.centros_de_saludSet.Add(nuevo);
            contexto.SaveChanges();
        }

        public void hospital(Hospital h)
        {
            hospitales nuevo = new hospitales();
            nuevo.calle = h.calle;
            nuevo.nombre = h.nombreHospital;
            nuevo.ciudad = h.ciudad;
            nuevo.codigoPostal = h.codigoPostal;
            nuevo.direccion = h.direccionCompleta;
            nuevo.idHospital = h.codigoHospital;
            nuevo.latitud = h.localizacion.latitud;
            nuevo.longitud = h.localizacion.longitud;
            nuevo.region = h.region;
            nuevo.telefono = h.telefono;
            nuevo.web = h.web;

            contexto.hospitalesSet.Add(nuevo);
            contexto.SaveChanges();
        }

        public void evento(Evento e)
        {
            incidencias nuevo = new incidencias();
            nuevo.id = e.id;
            nuevo.descripcion = e.descripcion;
            nuevo.fechaFin = e.fechaFin;
            nuevo.fechaInicio = e.fechaInicio;
            nuevo.latitud = e.localizacion.latitud;
            nuevo.longitud = e.localizacion.longitud;
            nuevo.tipo = "evento";

            contexto.incidenciasSet.Add(nuevo);
            contexto.SaveChanges();
        }

        public void incidencia(Incidencia i)
        {
            incidencias nuevo = new incidencias();
            nuevo.id = i.id;
            nuevo.descripcion = i.descripcion;
            nuevo.fechaFin = i.fechaFin;
            nuevo.fechaInicio = i.fechaInicio;
            nuevo.latitud = i.localizacion.latitud;
            nuevo.longitud = i.localizacion.longitud;
            nuevo.tipo = "incidencia";

            contexto.incidenciasSet.Add(nuevo);
            contexto.SaveChanges();
        }

        public void mantenimiento(Mantenimiento m)
        {
            incidencias nuevo = new incidencias();
            nuevo.id = m.id;
            nuevo.descripcion = m.descripcion;
            nuevo.fechaFin = m.fechaFin;
            nuevo.fechaInicio = m.fechaInicio;
            nuevo.latitud = m.localizacion.latitud;
            nuevo.longitud = m.localizacion.longitud;
            nuevo.tipo = "mantenimiento";

            contexto.incidenciasSet.Add(nuevo);
            contexto.SaveChanges();
        }

        public void obra(Obra o)
        {
            incidencias nuevo = new incidencias();
            nuevo.id = o.id;
            nuevo.descripcion = o.descripcion;
            nuevo.fechaFin = o.fechaFin;
            nuevo.fechaInicio = o.fechaInicio;
            nuevo.latitud = o.localizacion.latitud;
            nuevo.longitud = o.localizacion.longitud;
            nuevo.tipo = "obra";

            contexto.incidenciasSet.Add(nuevo);
            contexto.SaveChanges();
        }

    }
}
