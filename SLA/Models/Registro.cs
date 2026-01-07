namespace SLA.Models;

public class Registro
{
    public DateTime Fecha { get; set; }
    public string TipoMovimiento { get; set; } = string.Empty;
    public string Unidad { get; set; } = string.Empty;
    public string Operador { get; set; } = string.Empty;
    public string? Observaciones { get; set; }
    public List<ItemRegistro> Items { get; set; } = new();
}
