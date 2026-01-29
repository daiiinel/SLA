using SLA.Models;
using SLA.ViewModels;

namespace SLA.Views;

public partial class ArchivoAuditoriaPage : ContentPage
{
    private bool _isInitializing = true;

    public ArchivoAuditoriaPage(ArchivoAuditoriaViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _isInitializing = true;

        TipoFiltroPicker.SelectedIndex = 0;
        FechaDesdePicker.Date = DateTime.Now.AddMonths(-1);
        FechaHastaPicker.Date = DateTime.Now;

        _isInitializing = false;

        if (BindingContext is ArchivoAuditoriaViewModel vm)
            await vm.CargarArchivoAsync();
    }

    private async void OnRegistroSeleccionado(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Registro registroSeleccionado)
        {
            if (BindingContext is ArchivoAuditoriaViewModel vm)
                await vm.VerDetalleCommand.ExecuteAsync(registroSeleccionado);

            if (sender is CollectionView collectionView)
                collectionView.SelectedItem = null;
        }
    }

    private void OnFiltroChanged(object sender, EventArgs e)
    {
        if (_isInitializing) 
            return;

        if (BindingContext is ArchivoAuditoriaViewModel vm)
        {
            string tipoSeleccionado = TipoFiltroPicker.SelectedItem?.ToString() ?? "Todos";

            DateTime desde = FechaDesdePicker.Date ?? DateTime.Now.AddMonths(-1);
            DateTime hasta = FechaHastaPicker.Date ?? DateTime.Now;

            vm.AplicarFiltros(tipoSeleccionado, desde, hasta);
        }
    }
}