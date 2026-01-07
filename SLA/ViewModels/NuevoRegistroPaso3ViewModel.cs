using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SLA.Models;
using SLA.Services;
using System.Collections.ObjectModel;

namespace SLA.ViewModels;

public partial class NuevoRegistroPaso3ViewModel : ObservableObject
{
    public ObservableCollection<ItemRegistro> Items =>
        RegistroActual.Items;

    public string Operador =>
        RegistroActual.Operador;

    public string ResumenGeneral =>
        $"Tipo: {RegistroActual.TipoMovimiento}\n" +
        $"Unidad: {RegistroActual.Unidad}\n" +
        $"Fecha: {RegistroActual.Fecha:dd/MM/yyyy}\n" +
        $"Observaciones: {RegistroActual.Observaciones ?? "-"}";

    [RelayCommand]
    private async Task Confirmar()
    {
        try
        {
            RegistroService.GuardarRegistro();

            await Shell.Current.DisplayAlertAsync( "OK", "Registro guardado correctamente", "Aceptar");

            RegistroActual.Limpiar();

            await Shell.Current.GoToAsync("//DashboardPage");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync( "Error", ex.Message,"OK");
        }
    }

    [RelayCommand]
    private async Task Cancelar()
    {
        bool salir = await Shell.Current.DisplayAlertAsync("Cancelar", "Se perderán los datos cargados", "Sí", "No");

        if (!salir)
            return;

        RegistroActual.Limpiar();
        await Shell.Current.GoToAsync("//DashboardPage");
    }
}
