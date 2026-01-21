using CommunityToolkit.Maui.Views;
using SLA.ViewModels;


namespace SLA.Views;

public partial class NuevoRegistroPaso3Page : ContentPage
{
	public NuevoRegistroPaso3Page(ViewModels.NuevoRegistroPaso3ViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    private async Task<string?> ObtenerFirmaBase64Async()
    {
        if (FirmaDrawingView.Lines.Count == 0)
            return null;

        try
        {
            // Usamos 400x200 q es una proporción ideal para el 2404.
            await using var stream = await FirmaDrawingView.GetImageStream(400, 200,default);

            if (stream == null || stream == Stream.Null)
                return null;

            using var memory = new MemoryStream();
            await stream.CopyToAsync(memory);

            //  Base64 pa pasarlo al Vm
            return Convert.ToBase64String(memory.ToArray());
        }
        catch (Exception)
        {
            return null;
        }
    }

    private async void OnConfirmarClicked(object sender, EventArgs e)
    {
        var firmaBase64 = await ObtenerFirmaBase64Async();

        if (BindingContext is NuevoRegistroPaso3ViewModel vm)
            await vm.ConfirmarCommand.ExecuteAsync(firmaBase64);
    }

    private void OnLimpiarFirmaClicked(object sender, EventArgs e)
    {
        FirmaDrawingView.Lines.Clear();
        if (BindingContext is NuevoRegistroPaso3ViewModel vm)
            vm.LimpiarFirmaCommand.Execute(null);
    }
}