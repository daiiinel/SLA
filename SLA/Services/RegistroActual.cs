using System.Collections.ObjectModel;
using SLA.Models;

namespace SLA.Services;

public static class RegistroActual
{
    public static string TipoMovimiento { get; set; } = string.Empty;
    public static string Unidad { get; set; } = string.Empty;
    public static DateTime Fecha { get; set; }
    public static string Operador { get; set; } = string.Empty;
    public static string? Observaciones { get; set; }

    public static ObservableCollection<ItemRegistro> Items { get; } = new();
    //proximamente..
    // public List<DetalleArma> Armas { get; set; }

    public static void Limpiar()
    {
        TipoMovimiento = string.Empty;
        Unidad = string.Empty;
        Operador = string.Empty;
        Observaciones = null;
        Items.Clear();
    }

}
