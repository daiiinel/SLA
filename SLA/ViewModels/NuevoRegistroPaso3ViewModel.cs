using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SLA.Models;
using SLA.Services;
using System.Collections.ObjectModel;

namespace SLA.ViewModels;

public partial class NuevoRegistroPaso3ViewModel : ObservableObject
{
    private Registro RegistroActual => RegistroActualService.RegistroActual ?? throw new InvalidOperationException("No hay registro activo");

    //con el new step  2, no es necesario crear otra collect, sino q solo mostramos lo q ya  existe (la cabecitah explotó)
    public ObservableCollection<ItemRegistro> Items => RegistroActual.Items; 

    public string Operador =>RegistroActual.Operador;

    public string ResumenGeneral =>
        $"Tipo: {RegistroActual.TipoMovimiento}\n" +
        $"Unidad: {RegistroActual.Unidad}\n" +
        $"Fecha: {RegistroActual.Fecha:dd/MM/yyyy}\n" +
        $"Observaciones: {RegistroActual.Observaciones ?? "-"}";

    /*public NuevoRegistroPaso3ViewModel()
    {
        // Convertimos la List del modelo en obscollection para la ui
        Items = new ObservableCollection<ItemRegistro>(RegistroActual.Items);
    }*/

    [RelayCommand]
    private async Task Confirmar()
    {
        try
        {
           // RegistroActual.Items = Items.ToList(); //ahora no necesario

            RegistroService.GuardarRegistro(RegistroActual);

            await Shell.Current.DisplayAlertAsync( "OK", "Registro guardado correctamente", "Aceptar");

            RegistroActualService.Limpiar();

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

        RegistroActualService.Limpiar();
        await Shell.Current.GoToAsync("//DashboardPage");
    }
}
