using SLA.ViewModels;

namespace SLA.Views;

public partial class NuevoRegistroPaso2Page : ContentPage
{
	public NuevoRegistroPaso2Page(NuevoRegistroPaso2ViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}