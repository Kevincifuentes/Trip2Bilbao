using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class Farmacia
    {

        public Farmacia()
        {
            
        }

        public Farmacia(string nombrePropietario, string direccionCompleta, string nombreFarmacia, int codigoPostal, string provincia, string direccionAbreviada, string ciudad, string urlInfo, Coordenadas localizacion, long telefono, long fax, string email, string web, string infoAdicional)
        {
            this.nombrePropietario = nombrePropietario;
            this.direccionCompleta = direccionCompleta;
            this.nombreFarmacia = nombreFarmacia;
            this.codigoPostal = codigoPostal;
            this.provincia = provincia;
            this.direccionAbreviada = direccionAbreviada;
            this.ciudad = ciudad;
            this.urlInfo = urlInfo;
            this.localizacion = localizacion;
            this.telefono = telefono;
            this.fax = fax;
            this.email = email;
            this.web = web;
            this.infoAdicional = infoAdicional;
        }

        public string nombrePropietario { get; set; }
        public string direccionCompleta { get; set; }
        public string nombreFarmacia { get; set; }
        public int codigoPostal { get; set; }
        public string provincia { get; set; }
        public string direccionAbreviada { get; set; }
        public string ciudad { get; set; }
        public string urlInfo { get; set; }
        public Coordenadas localizacion { get; set; }
        public long telefono { get; set; }
        public long fax { get; set; }
        public string email { get; set; }
        public string web { get; set; }
        public string infoAdicional { get; set; }


        public override string ToString()
        {
            string respuesta = nombreFarmacia + " : Propietario " + nombrePropietario + " Direccion completa: " +
                               direccionCompleta + " Localización: ";
            if (localizacion != null)
            {
                respuesta= respuesta +localizacion.longitud + " , " +
                               localizacion.latitud + "\n";
            }
            else
            {
                respuesta = respuesta + "null" + "\n";
            }
            respuesta = respuesta + "CP: " + codigoPostal +" Ciudad: "+ciudad+ " Provincia: " + provincia + " Dirección abreviada: " +
                        direccionAbreviada + " Telefono: " + telefono + " Web: " + web+"\n";
            respuesta = respuesta + "Url información: " + urlInfo + " Fax: " + fax + " Información adicional " +
                       infoAdicional+ "URL: "+urlInfo+ "\n";
            return respuesta; 
        }
    }
}
