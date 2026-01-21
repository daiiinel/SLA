using SLA.Models;
using SLA.Services;

namespace SLA.Views;

[QueryProperty(nameof(Registro), "Registro")]
public partial class DetalleRegistroPage : ContentPage
{
    private readonly IPrintService _printService;
    public Registro? _registro;

    // prop q recibe el obj desde la navegación
    public Registro RegistroSeleccionado
    {
        get => _registro!;
        set
        {
            _registro = value;
            MainThread.BeginInvokeOnMainThread(async () => {
                await Task.Delay(50);
                BindingContext = null;
                BindingContext = _registro;
            });
        }
    }
    public DetalleRegistroPage(IPrintService printService)
    {
        InitializeComponent();
        _printService = printService;

        // al asignar el BindingContext acá todos los {Binding prop} 
        // en el xaml empezarán a buscar dentro de este obj
        BindingContext = RegistroActualService.RegistroActual;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext == null)
            BindingContext = RegistroActualService.RegistroActual;
    }
    // recibir datos en MAUI Shell
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("Registro"))
        {
            _registro = query["Registro"] as Registro;
            BindingContext = _registro;

            // forzamos actualización de la ui
            OnPropertyChanged(nameof(BindingContext));
        }
    }

    private async void OnExportarPdfClicked(object sender, EventArgs e)
    {
        if (_registro == null) 
            return;

        try
        {
            string html = await PDFService.GenerarHtmlResumen(_registro, _registro.FirmaBase64 ?? "");

            _printService.PrintHtml(html);
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", $"No se pudo generar el reporte: {ex.Message}", "OK");
        }
    }
}
