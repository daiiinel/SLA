using CommunityToolkit.Maui.Views;
using CommunityToolkit.Maui.ImageSources;

namespace SLA.Views;

public partial class ConfigurarFirmaPage : ContentPage
{
    public ConfigurarFirmaPage()
    {
        InitializeComponent();
    }

    private async void OnGuardarFirmaClicked(object sender, EventArgs e)
    {
        // conversion de dibujo a stream
        var stream = await PadFirmaOperador.GetImageStream(300, 150);

        if (stream != null)
        {
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            byte[] imageBytes = memoryStream.ToArray();

            //a base64
            string base64 = Convert.ToBase64String(imageBytes);

            // preferences de MAUI pal guardado permanente
            Preferences.Set("FirmaOperadorBase64", base64);

            await DisplayAlertAsync("Éxito", "Su firma ha sido registrada correctamente", "OK");
            await Navigation.PopAsync();
        }
        else
            await DisplayAlertAsync("SLA", "Por favor, dibuje su firma antes de guardar..", "OK");
    }

    private void OnLimpiarClicked(object sender, EventArgs e)
    {
        PadFirmaOperador.Lines.Clear();
    }
}