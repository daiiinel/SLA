using System;
using System.Collections.Generic;
using System.Text;

namespace SLA.Models;

public class ItemRegistro
{
    public string Tipo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int Cantidad { get; set; }

    public string Resumen => $"{Tipo} – Cantidad: {Cantidad}";
}
