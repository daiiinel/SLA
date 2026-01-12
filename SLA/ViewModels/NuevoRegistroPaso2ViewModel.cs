using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SLA.Models;
using SLA.Services;
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
    public ObservableCollection<ItemRegistro> Items { get; }

    public NuevoRegistroPaso2ViewModel()
    {
        var registro = RegistroActualService.RegistroActual ?? throw new InvalidOperationException("No hay registro activo");

        // igualamos referencias..
        // ahora Items en el vm es la misma lista que en el reg.cs
        Items = registro.Items;
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

        Items.Add(new ItemRegistro{ Tipo = TipoItem, Descripcion = Descripcion, Cantidad = cant});

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
            await Shell.Current.GoToAsync(nameof(Views.NuevoRegistroPaso3Page));
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
}
