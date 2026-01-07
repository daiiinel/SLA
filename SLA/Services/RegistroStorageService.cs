using System.Text.Json;
using SLA.Models;

namespace SLA.Services;

public static class RegistroStorageService
{
    private static readonly string Ruta = Path.Combine(FileSystem.AppDataDirectory, "registros.json");

    public static async Task GuardarAsync(Registro registro)
    {
        List<Registro> registros = new();

        if (File.Exists(Ruta))
            registros = JsonSerializer.Deserialize<List<Registro>>(await File.ReadAllTextAsync(Ruta)) ?? new();

        registros.Add(registro);

        var nuevoJson = JsonSerializer.Serialize(registros, new JsonSerializerOptions { WriteIndented = true });

        await File.WriteAllTextAsync(Ruta, nuevoJson);
    }
}
