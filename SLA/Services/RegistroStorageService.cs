using System.Text.Json;
using SLA.Models;

namespace SLA.Services;

public static class RegistroStorageService
{
    private static readonly string Ruta = Path.Combine(FileSystem.AppDataDirectory, "registros.json");

    private static readonly SemaphoreSlim _semaphore = new(1, 1);
    
    public static async Task GuardarAsync(Registro registro)
    {
        await _semaphore.WaitAsync(); // bloquea hasta q termine otra escritura

        try
        {
            List<Registro> registros = await ObtenerTodosAsync();

            var existe = registros.FirstOrDefault(r => r.Id == registro.Id);
            if (existe != null)
                registros.Remove(existe);

            registros.Add(registro);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive= true,
                IncludeFields = true //por si hay campos internos
            };

            var nuevoJson = JsonSerializer.Serialize(registros, options);

            await File.WriteAllTextAsync(Ruta, nuevoJson);
        }
        finally
        {
            _semaphore.Release(); // libera pal sig
        }
    }

    public static async Task<List<Registro>> ObtenerTodosAsync()
    {
        if (!File.Exists(Ruta))
            return new List<Registro>();

        return JsonSerializer.Deserialize<List<Registro>>(await File.ReadAllTextAsync(Ruta)) ?? new List<Registro>();
    }

    public static async Task ActualizarAsync(Registro registroActualizado)
    {
        if (!File.Exists(Ruta))
            return;

        var json = await File.ReadAllTextAsync(Ruta);
        var registros = JsonSerializer.Deserialize<List<Registro>>(json) ?? new();

        var index = registros.FindIndex(r => r.Id == registroActualizado.Id);
        if (index == -1)
            return;

        registros[index] = registroActualizado;

        var nuevoJson = JsonSerializer.Serialize(registros, new JsonSerializerOptions { WriteIndented = true });

        await File.WriteAllTextAsync(Ruta, nuevoJson);
    }
}
