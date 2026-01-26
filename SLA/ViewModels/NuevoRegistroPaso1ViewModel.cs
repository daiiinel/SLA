using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SLA.Services;
using System.Collections.ObjectModel;
using SLA.Views;

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

    //nuevos campos
    private string busquedaDNI = String.Empty;
    public string BusquedaDNI { get => busquedaDNI; set => SetProperty(ref busquedaDNI, value); }

    private string nombreCompletoReceptor = "Nombre: ---";
    public string NombreCompletoReceptor { get => nombreCompletoReceptor; set=>SetProperty(ref nombreCompletoReceptor, value);}

    private string gradoUnidadReceptor = "Grado/Unidad: ---";
    public string GradoUnidadReceptor { get => gradoUnidadReceptor; set => SetProperty(ref gradoUnidadReceptor, value); }
   
    private bool receptorEncontrado = false;
    public bool ReceptorEncontrado
    {
        get => receptorEncontrado;
        set
        {
            if (SetProperty(ref receptorEncontrado, value))
            {
                ContinuarCommand.NotifyCanExecuteChanged(); // continuar debe reevaluarse
                OnPropertyChanged(nameof(MostrarAdvertenciaReceptor)); //notifcamos q se cambie el msj de error
            }
        }
    }

    public bool MostrarAdvertenciaReceptor => !ReceptorEncontrado;

    //Listas
    public ObservableCollection<string> TiposMovimiento { get; } = new() {"Alta", "Baja", "Traslado", "Prestamo"};

    public NuevoRegistroPaso1ViewModel()
    {
        Operador = SessionService.UsuarioActual; //pa q se ejecute cuando entre a la pantalla, no en el viewmodel
    }


    //Comands
    //solo se puede ejecutar si ReceptorEncontrado=true
    [RelayCommand(CanExecute =nameof(ReceptorEncontrado))]
    private async Task Continuar()
    {
        //Mensajes mas claros (se supone q el principal user sera un we q trabaje con soldaditos ah)
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

        // creamos si no hay un registro iniciado
        // si ya existe (pq volvimos del step2), mantenemos el objeto actual
        if (RegistroActualService.RegistroActual == null)
            RegistroActualService.CrearNuevo(operador);

        var r = RegistroActualService.RegistroActual!;
        r.TipoMovimiento = TipoSeleccionado;
        r.Unidad = Unidad;
        r.Fecha = Fecha;
        r.Observaciones = Observaciones;
        r.BusquedaDNI = BusquedaDNI;
        r.NombreCompletoReceptor = NombreCompletoReceptor;
        r.GradoUnidadReceptor = GradoUnidadReceptor;
        r.Operador = operador; //sirve pa cuando tengamos mas de un admin¿

        await Shell.Current.GoToAsync(nameof(NuevoRegistroPaso2Page));

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
    private static async Task Cancelar()
    {
        bool salir = await Shell.Current.DisplayAlertAsync("Cancelar", "Se perderán los datos cargados", "Sí", "No");

        if (!salir)
            return;

        RegistroActualService.Limpiar();
        await Shell.Current.GoToAsync("//DashboardPage");
    }

    [RelayCommand]
    private async Task BuscarReceptor()
    {
        if (string.IsNullOrWhiteSpace(BusquedaDNI))
        {
            await Shell.Current.DisplayAlertAsync("SLA INFO", "Ingrese un DNI o Legajo para buscar.", "OK");
            return;
        }

        // IsBusy = true; 

        await Task.Delay(1000); // simulo delay bbdd

        // prox --> var p = await _database.GetPersonaByDni(BusquedaDNI); //con mi bbdd
        if (BusquedaDNI == "111")
        {
            NombreCompletoReceptor = "AGUILUCHO, JUAN JESÚS";
            GradoUnidadReceptor = "SARGENTO / BATALLÓN COM 02";
            ReceptorEncontrado = true;
        }
        else if (BusquedaDNI == "222")
        {
            NombreCompletoReceptor = "LOBO, RODRIGO ANTONIO";
            GradoUnidadReceptor = "CABO PRIMERO / PFA";
            ReceptorEncontrado = true;
        }
        else
        {
            await Shell.Current.DisplayAlertAsync("SLA ERROR", "Personal no identificado en la base de datos..", "Reintentar");
            NombreCompletoReceptor = "Nombre: ---";
            GradoUnidadReceptor = "Grado/Unidad: ---";
            ReceptorEncontrado = false;
        }

        // IsBusy = false;
    }
}