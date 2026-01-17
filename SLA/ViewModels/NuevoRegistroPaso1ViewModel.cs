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

    private string operador = String.Empty;
    public string Operador { get => operador; set => SetProperty(ref operador, value); }

    private string? observaciones;
    public string? Observaciones { get => observaciones; set => SetProperty(ref observaciones, value); }

    //Listas
    public ObservableCollection<string> TiposMovimiento { get; } = new() {"Alta", "Baja", "Traslado", "Prestamo"};

    public NuevoRegistroPaso1ViewModel()
    {
        Operador = SessionService.UsuarioActual; //pa q se ejecute cuando entre a la pantalla, no en el viewmodel
    }


    //Comands
    [RelayCommand]
    private async Task Continuar()
    {
        //Mensajes mas claros (se supone q el principal user sera un soldadito)
        if (string.IsNullOrWhiteSpace(TipoSeleccionado))
        {
            await Shell.Current.DisplayAlertAsync("Falta información","Debe seleccionar el tipo de movimiento..", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(Unidad))
        {
            await Shell.Current.DisplayAlertAsync("Falta información","Debe ingresar la unidad o dependencia..","OK");
            return;
        }

        RegistroActualService.CrearNuevo(operador);

        var r = RegistroActualService.RegistroActual!;
        r.TipoMovimiento = TipoSeleccionado;
        r.Unidad = Unidad;
        r.Fecha = Fecha;
        r.Observaciones = Observaciones;

        await Shell.Current.GoToAsync(nameof(Views.NuevoRegistroPaso2Page));

        /*
        // Guardar estado temporal del registro
        RegistroActual.TipoMovimiento = TipoSeleccionado!;
        RegistroActual.Unidad = Unidad!;
        RegistroActual.Fecha = Fecha;
        RegistroActual.Operador = Operador;
        RegistroActual.Observaciones = Observaciones;

        // Nos vamosh al paso 2
        await Shell.Current.GoToAsync(nameof(Views.NuevoRegistroPaso2Page));*/
    }

    [RelayCommand]
    private async Task Cancelar()
    {
        bool salir = await Shell.Current.DisplayAlertAsync("Cancelar", "Se perderán los datos cargados", "Sí", "No");

        if (!salir)
            return;

        RegistroActualService.Limpiar();
        await Shell.Current.GoToAsync("//DashboardPage");
    }
}