using SLA.Models;
using System.Text.Json;

namespace SLA.Services;

public class InicioSesionService
{
    private readonly string _rutaArchivo = Path.Combine(FileSystem.AppDataDirectory, "inicios_sesion.json");

    private readonly EventoService _eventoService = new();

    public void Registrar(string usuario, string origen)
    {
        var evento = new Evento
        {
            Usuario = usuario,
            Tipo = "Inicio de sesión",
            Origen = origen
        };

        _eventoService.RegistrarEvento(evento);
    }

    public List<InicioSesion> ObtenerRegistros()
    {
        if (!File.Exists(_rutaArchivo))
            return new List<InicioSesion>();

        var json = File.ReadAllText(_rutaArchivo);

        if (string.IsNullOrWhiteSpace(json))
            return new List<InicioSesion>();

        return JsonSerializer.Deserialize<List<InicioSesion>>(json) ?? new List<InicioSesion>();
    }
}
