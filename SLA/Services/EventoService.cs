using System.Text.Json;
using SLA.Models;

namespace SLA.Services;

public class EventoService
{
    private readonly string _rutaArchivo = Path.Combine(FileSystem.AppDataDirectory, "eventos.json");

    public void RegistrarEvento(Evento evento)
    {
        var eventos = ObtenerEventos();
        eventos.Add(evento);

        var json = JsonSerializer.Serialize(eventos, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(_rutaArchivo, json);
    }

    public List<Evento> ObtenerEventos()
    {
        if (!File.Exists(_rutaArchivo))
            return new List<Evento>();

        var json = File.ReadAllText(_rutaArchivo);

        return JsonSerializer.Deserialize<List<Evento>>(json) ?? new List<Evento>();
    }
}
