using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SLA.Models;
using SLA.Services;
using SLA.Views;
using System.Collections.ObjectModel;

namespace SLA.ViewModels;

public partial class ArchivoAuditoriaViewModel : ObservableObject
{
    public ObservableCollection<Registro> RegistrosFinalizados { get; } = new();

    private List<Registro> _todosLosRegistros = new();

    public List<string> TiposMovimiento { get; } = new() { "Todos", "Alta", "Baja", "Prestamo", "Traslado" };

    [RelayCommand]
    public async Task CargarArchivoAsync()
    {
        var todos = await RegistroStorageService.ObtenerTodosAsync();

        _todosLosRegistros = todos
            .Where(r => r.Estado == EstadoRegistro.Auditado || r.Estado == EstadoRegistro.Rechazado)
            .OrderByDescending(r => r.Fecha)
            .ToList();

        AplicarFiltros("Todos", DateTime.MinValue, DateTime.MaxValue);
    }

    public void AplicarFiltros(string? tipo, DateTime desde, DateTime hasta)
    {
        RegistrosFinalizados.Clear();

        var filtrados = _todosLosRegistros.AsEnumerable();

        // filtro por tipo
        if (!string.IsNullOrEmpty(tipo) && tipo != "Todos")
            filtrados = filtrados.Where(r => r.TipoMovimiento != null && r.TipoMovimiento.Equals(tipo, StringComparison.OrdinalIgnoreCase));

        // filtro por rango de fechas -> usando .Date para comparar solo el día
        filtrados = filtrados.Where(r => r.Fecha.Date >= desde.Date && r.Fecha.Date <= hasta.Date);

        foreach (var r in filtrados)
            RegistrosFinalizados.Add(r);
    }

    [RelayCommand]
    private async Task VerDetalle(Registro registro)
    {
        if (registro == null) 
            return;

        var parametros = new Dictionary<string, object> { { "Registro", registro } };
        await Shell.Current.GoToAsync(nameof(DetalleRegistroPage), parametros);
    }
}