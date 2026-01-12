using SLA.ViewModels;

namespace SLA.Views;

public partial class RevisionRegistrosPage : ContentPage
{
	public RevisionRegistrosPage()
	{
		InitializeComponent();
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is RevisionRegistroViewModel vm)
            await vm.CargarRegistrosAsync();
    }

}