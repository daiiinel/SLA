namespace SLA.Views;

public partial class NuevoRegistroPaso3Page : ContentPage
{
	public NuevoRegistroPaso3Page(ViewModels.NuevoRegistroPaso3ViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}