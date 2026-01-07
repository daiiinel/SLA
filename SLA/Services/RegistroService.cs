using System.Text.Json;
using SLA.Models;

namespace SLA.Services;

public static class RegistroService //no debe saber como se construyó el reg -> generalizamos un poquito washo :b
{
    private static readonly string Ruta = Path.Combine(FileSystem.AppDataDirectory, "registros_2404.json");

    //update: pasamos como paramentro el registro YA armado, ahora no es necesario crearlo en esta funcion
    public static void GuardarRegistro(Registro registro)
    {
        var registros = ObtenerRegistros();

        registros.Add(registro);

        var json = JsonSerializer.Serialize(registros, new JsonSerializerOptions { WriteIndented = true });

        File.WriteAllText(Ruta, json);
    }

    public static List<Registro> ObtenerRegistros()
    {
        if (!File.Exists(Ruta))
            return new();

        var json = File.ReadAllText(Ruta);
        return JsonSerializer.Deserialize<List<Registro>>(json) ?? new();
    }
}
