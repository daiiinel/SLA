using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SLA.Models;
using SLA.Services;
using SLA.Views;
using System.Collections.ObjectModel;


namespace SLA.ViewModels;

public partial class NuevoRegistroPaso2ViewModel : ObservableObject
{
    private string? tipoItem;
    public string? TipoItem { get => tipoItem; set => SetProperty(ref tipoItem, value); }

    private string? descripcion;
    public string? Descripcion { get => descripcion; set => SetProperty(ref descripcion, value); }

    private string? cantidad;
    public string? Cantidad {get => cantidad; set => SetProperty(ref cantidad, value);}


    public ObservableCollection<string> TiposItem { get; } = new() { "Arma", "Munición", "Material" };
    public ObservableCollection<ItemRegistro> Items => RegistroActualService.RegistroActual?.Items ?? new ObservableCollection<ItemRegistro>();

    public NuevoRegistroPaso2ViewModel()
    {
        var registro = RegistroActualService.RegistroActual ?? throw new InvalidOperationException("No hay registro activo");
    }

    [RelayCommand]
    private async Task AgregarItem()
    {
        //validaciones mas claras (siempre pensanding en el user dea)
        if (string.IsNullOrWhiteSpace(TipoItem))
        {
            await Shell.Current.DisplayAlertAsync( "Falta información", "Seleccione el tipo de item..", "OK");
            return;
        }
        if(string.IsNullOrWhiteSpace(Descripcion))
        {
            await Shell.Current.DisplayAlertAsync("Falta información","Ingrese una descripción","OK");
            return;
        }
        if(!int.TryParse(Cantidad, out int cant) || cant <= 0)
        {
            await Shell.Current.DisplayAlertAsync("Cantidad inválida","Ingrese una cantidad válida.. (mayor a 0)","OK");
            return;
        }
        var nuevoItem = new ItemRegistro
        {
            Tipo = TipoItem,
            Cantidad = cant,
            Descripcion = Descripcion
        };

        if(RegistroActualService.RegistroActual!=null)
        {
            //Items.Add(nuevoItem);
            RegistroActualService.RegistroActual?.Items.Add(nuevoItem); //

            //opcional -> norifica si la ui se cambia
            OnPropertyChanged(nameof(Items));
        }
        else
            await Shell.Current.DisplayAlertAsync("Error", "No hay un registro activo iniciado.", "OK");

        // limpiar
        TipoItem = null;
        Descripcion = Cantidad = string.Empty;
    }

    [RelayCommand]
    private async Task Continuar()
    {
        if (Items.Count == 0)
        {
            await Shell.Current.DisplayAlertAsync( "Error", "Debe agregar al menos un ítem", "OK");
            return;
        }
        try
        {
            //pasho 3
            await Shell.Current.GoToAsync(nameof(NuevoRegistroPaso3Page));
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    [RelayCommand]
    private void EliminarItem(ItemRegistro item)
    {
        if (Items.Contains(item))
            Items.Remove(item);
    }
    [RelayCommand]
    async Task Volver()
    {
        // saca la pag actual del stack y vuelve a la anterior
        try
        {
            await Shell.Current.GoToAsync("..", animate:true);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
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
