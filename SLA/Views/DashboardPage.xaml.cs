namespace SLA.Views;

public partial class DashboardPage : ContentPage
{
	public DashboardPage()
	{
		InitializeComponent();
        DisplayAlertAsync("DEBUG", "Dashboard cargó", "OK");
    }
}