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
    public ObservableCollection<ItemRegistro> Items { get; } = RegistroActual.Items;

    [RelayCommand]
    private async Task AgregarItem()
    {
        if (string.IsNullOrWhiteSpace(TipoItem) || string.IsNullOrWhiteSpace(Descripcion) || !int.TryParse(Cantidad, out int cant) || cant <= 0)
        {
            await Shell.Current.DisplayAlertAsync( "Error", "Datos inválidos", "OK");
            return;
        }

        Items.Add(new ItemRegistro{ Tipo = TipoItem!, Descripcion = Descripcion!, Cantidad = cant});

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
        //pasho 3
        await Shell.Current.GoToAsync(nameof(Views.NuevoRegistroPaso3Page));
    }
}
