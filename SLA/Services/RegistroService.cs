using System.Text.Json;
using SLA.Models;

namespace SLA.Services;

public static class RegistroService
{
    private static readonly string Ruta =
        Path.Combine(FileSystem.AppDataDirectory, "registros_2404.json");

    public static void GuardarRegistro()
    {
        var registros = ObtenerRegistros();

        registros.Add(new Registro
        {
            Fecha = RegistroActual.Fecha,
            TipoMovimiento = RegistroActual.TipoMovimiento,
            Unidad = RegistroActual.Unidad,
            Operador = RegistroActual.Operador,
            Observaciones = RegistroActual.Observaciones,
            Items = RegistroActual.Items.ToList()
        });

        var json = JsonSerializer.Serialize(registros, new JsonSerializerOptions { WriteIndented = true });

        File.WriteAllText(Ruta, json);
    }

    private static List<Registro> ObtenerRegistros()
    {
        if (!File.Exists(Ruta))
            return new();

        var json = File.ReadAllText(Ruta);
        return JsonSerializer.Deserialize<List<Registro>>(json) ?? new();
    }
}
