using System;
using System.Collections.Generic;
using System.Text;

namespace SLA.Models
{
    public class InicioSesion
    {
        public int Id { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
        public string Tipo { get; set; } = string.Empty; // inicio-cierre
        public string? Observacion { get; set; }
    }
}
