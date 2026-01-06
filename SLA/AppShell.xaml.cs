using SLA.Views;

namespace SLA
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(HistorialPage), typeof(HistorialPage));
        }
    }
}
