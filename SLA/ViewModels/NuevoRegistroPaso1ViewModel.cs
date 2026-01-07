using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SLA.Services;
using System.Collections.ObjectModel;

namespace SLA.ViewModels;

public partial class NuevoRegistroPaso1ViewModel : ObservableObject
{
    //Campos del form
    private string? tipoSeleccionado;
    public string? TipoSeleccionado { get => tipoSeleccionado; set => SetProperty(ref tipoSeleccionado, value); }

    private string? unidad;
    public string? Unidad { get => unidad; set => SetProperty(ref unidad, value); }

    private DateTime fecha = DateTime.Today;
    public DateTime Fecha { get => fecha; set => SetProperty(ref fecha, value); }

    private string operador = SessionService.UsuarioActual;
    public string Operador { get => operador; set => SetProperty(ref operador, value); }

    private string? observaciones;
    public string? Observaciones { get => observaciones; set => SetProperty(ref observaciones, value); }

    //Listas
    public ObservableCollection<string> TiposMovimiento { get; } = new() {"Alta", "Baja", "Traslado"};

    //Comands
    [RelayCommand]
    private async Task Continuar()
    {
        if (string.IsNullOrWhiteSpace(TipoSeleccionado) || string.IsNullOrWhiteSpace(Unidad))
        {
            await Shell.Current.DisplayAlertAsync("Error","Complete los campos obligatorios", "OK");
            return;
        }

        // Guardar estado temporal del registro
        RegistroActual.TipoMovimiento = TipoSeleccionado!;
        RegistroActual.Unidad = Unidad!;
        RegistroActual.Fecha = Fecha;
        RegistroActual.Operador = Operador;
        RegistroActual.Observaciones = Observaciones;

        // Nos vamosh al paso 2
        await Shell.Current.GoToAsync(nameof(Views.NuevoRegistroPaso2Page));
    }
}