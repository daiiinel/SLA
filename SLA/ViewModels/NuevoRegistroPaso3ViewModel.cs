using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Maui.Core.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SLA.Models;
using SLA.Services;
using SLA.ViewModels;
using SLA.Views;
using System.Collections.ObjectModel;

namespace SLA.ViewModels;

public partial class NuevoRegistroPaso3ViewModel : ObservableObject
{
    private Registro RegistroActual => RegistroActualService.RegistroActual ?? throw new InvalidOperationException("No hay registro activo");

    //con el new step  2, no es necesario crear otra collect, sino q solo mostramos lo q ya  existe (la cabecitah explotó)
    public ObservableCollection<ItemRegistro> Items => RegistroActual.Items;

    private readonly IPrintService _printService;

    public string Operador =>RegistroActual.Operador;

    // Usando CommunityToolkit.Mvvm
    [ObservableProperty]
    bool isBusy;

    //nuevos campos
    public string NombreReceptor => RegistroActual.NombreCompletoReceptor ?? "Sin identificar";

    public string GradoUnidadReceptor => RegistroActual.GradoUnidadReceptor ?? "---";

    public string DniReceptor => RegistroActual.BusquedaDNI ?? "---";

    public string ResumenMision =>
        $"{RegistroActual.TipoMovimiento} en {RegistroActual.Unidad}";

    public string ResumenGeneral =>
        $"Fecha: {RegistroActual.Fecha:dd/MM/yyyy}\n" +
        $"Observaciones: {RegistroActual.Observaciones ?? "-"}";

    //componentes pa la firma digital
    [ObservableProperty]
    private ObservableCollection<IDrawingLine> firmaLines = new();

    public NuevoRegistroPaso3ViewModel(IPrintService printService)
    {
        // lo recibimos en el constr y nuestro maui lo inyecta solito¿
        _printService = printService;
    }

    [RelayCommand]
    private async Task Confirmar(string firmaBase64)
    {
        if (IsBusy)
            return; // Seguridad: no permite doble envío

        // si no hay líneas, no hay firma.
        if (string.IsNullOrWhiteSpace(firmaBase64))
        {
            await Shell.Current.DisplayAlertAsync("SLA", "El receptor debe firmar..", "OK");
            return;
        }


        IsBusy = true;

        try
        {
            // estado final del envio
            RegistroActual.Estado = EstadoRegistro.Enviado;
            RegistroActual.Fecha = DateTime.Now;

            //RegistroService.GuardarRegistro(RegistroActual);
            // guardo un solo serv (guardaba en 2 por boludita)
            await RegistroStorageService.GuardarAsync(RegistroActual);

            // Simulamos la generación de PDF y envío (2 segundos)
            await Task.Delay(1000);

            await Shell.Current.DisplayAlertAsync("OK", "Registro guardado correctamente", "Aceptar");


            // preguntar si quiere ver el pdf
            bool verPdf = await Shell.Current.DisplayAlertAsync("Éxito", "Guardado, ¿desea generar el comprobante PDF?", "Si", "No");

            if (verPdf)
            {
                string html = await PDFService.GenerarHtmlResumen(RegistroActual, firmaBase64);
                _printService.PrintHtml(html);
            }

            RegistroActualService.Limpiar();
            await Shell.Current.GoToAsync("//DashboardPage");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
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

    //opcion de volver proximamente (en vez de cancelar y empezae de cero )
    [RelayCommand]
    async Task Volver()
    {
        // saca la pag actual del stack y vuelve a la anterior
        try
        {
            await Shell.Current.GoToAsync(nameof(NuevoRegistroPaso2Page));
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    //firm digital
    [RelayCommand]
    private void LimpiarFirma()
    {
        FirmaLines.Clear();
    }
}
