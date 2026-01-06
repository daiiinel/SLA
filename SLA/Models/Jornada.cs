using System;
using System.Collections.Generic;
using System.Text;

namespace SLA.Models
{
    public class Jornada
    {
        public string Usuario { get; set; } = string.Empty;
        public DateTime Inicio { get; set; }
        public DateTime? Cierre { get; set; }

        public TimeSpan? Duracion => Cierre.HasValue ? Cierre - Inicio : null; //hermosa terna..
    }
}
