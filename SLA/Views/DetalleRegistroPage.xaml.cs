using SLA.Models;
using SLA.Services;

namespace SLA.Views;

[QueryProperty(nameof(Registro), "Registro")]
public partial class DetalleRegistroPage : ContentPage, IQueryAttributable
{
    private readonly IPrintService _printService;
    public Registro? _registro;

    //props pal mood auditor
    public bool IsAuditorMode => SessionService.RolActual == Roles.Auditor && _registro?.Estado == EstadoRegistro.Entregado;

    private string _observacionAuditoria = string.Empty;
    public string ObservacionAuditoria
    {
        get => _observacionAuditoria;
        set { _observacionAuditoria = value; OnPropertyChanged(); }
    }
    
    public DetalleRegistroPage(IPrintService printService)
    {
        InitializeComponent();
        _printService = printService;

        // al asignar el BindingContext acá todos los {Binding prop} 
        // en el xaml empezarán a buscar dentro de este obj
        //BindingContext = RegistroActualService.RegistroActual;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        OnPropertyChanged(nameof(IsAuditorMode)); //forzamos act 
        //if (BindingContext == null)
          //  BindingContext = RegistroActualService.RegistroActual;
    }
    // recibir datos en MAUI Shell
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("Registro"))
        {
            _registro = query["Registro"] as Registro;
            this.BindingContext = _registro; //llena los campos del xaml

            if (!string.IsNullOrWhiteSpace(_registro?.FirmaBase64))
            {
                try
                {
                    // Limpiamos el string por si acaso trae el prefijo de html
                    string base64 = _registro.FirmaBase64;
                    if (base64.Contains(","))
                        base64 = base64.Split(',')[1];

                    byte[] imageBytes = Convert.FromBase64String(base64);
                    ImgFirmaReceptor.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error cargando firma: {ex.Message}");
                }
            }
            OnPropertyChanged(nameof(IsAuditorMode));
        }
    }

    private async void OnAprobarClicked(object sender, EventArgs e)
    {
        if (_registro == null) 
            return;

        bool confirmar = await DisplayAlertAsync("Confirmar", "Desea aprobar y validar este movimiento?", "Sí", "No");
        if (!confirmar) 
            return;

        try
        {
            // Actualizamos estado
            _registro.Estado = EstadoRegistro.Auditado;

            // Si el auditor escribe algo -> lo sumamos a las obs y actualizamos
            if (!string.IsNullOrWhiteSpace(ObservacionAuditoria))
                _registro.Observaciones += $"\n[AUDITORÍA]: {ObservacionAuditoria}";

            await RegistroStorageService.ActualizarAsync(_registro);

            await DisplayAlertAsync("Éxito", "El registro ha sido auditado y aprobado", "OK");

            // Volvemos a la lista de revisión
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", $"No se pudo actualizar el registro: {ex.Message}", "OK");
        }
    }

    private async void OnRechazarClicked(object sender, EventArgs e)
    {
        if (_registro == null) 
            return;

        if (string.IsNullOrWhiteSpace(ObservacionAuditoria))
        {
            await DisplayAlertAsync("Atención", "Debe indicar el motivo del rechazo en el campo de observaciones.", "OK");
            return;
        }

        try
        {
            _registro.Estado = EstadoRegistro.Rechazado;
            _registro.Observaciones += $"\n[RECHAZADO]: {ObservacionAuditoria}";

            await RegistroStorageService.ActualizarAsync(_registro);

            await DisplayAlertAsync("Registro Rechazado", "El registro ha sido devuelto al operador.", "OK");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    private async void OnExportarPdfClicked(object sender, EventArgs e)
    {
        if (_registro == null)
        {
            await DisplayAlertAsync("Error", "Ocurrió un problema con la reimpresión...", "OK");
            return;
        }
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
