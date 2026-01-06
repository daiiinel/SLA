using SLA.Views;

namespace SLA;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("MainPage", typeof(MainPage));
        Routing.RegisterRoute(nameof(HistorialPage), typeof(HistorialPage));
        Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));
    }
}
