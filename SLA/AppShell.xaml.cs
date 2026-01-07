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
        Routing.RegisterRoute(nameof(NuevoRegistroPaso1Page), typeof(NuevoRegistroPaso1Page));
        Routing.RegisterRoute(nameof(NuevoRegistroPaso2Page), typeof(NuevoRegistroPaso2Page));
        Routing.RegisterRoute(nameof(NuevoRegistroPaso3Page), typeof(NuevoRegistroPaso3Page));
        Routing.RegisterRoute(nameof(HistorialPage), typeof(HistorialPage));
    }
}
