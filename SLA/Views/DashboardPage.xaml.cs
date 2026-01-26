using SLA.ViewModels;

namespace SLA.Views;

public partial class DashboardPage : ContentPage
{
	public DashboardPage(DashboardViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}