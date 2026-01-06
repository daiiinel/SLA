using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SLA.Services;


namespace SLA.ViewModels
{
    public partial class DashboardViewModel : ObservableObject
    {
        // Datos del usuario

        [ObservableProperty]
        private string usuario;

        [ObservableProperty]
        private string rol;

        // Flags por rol

        public bool IsOperador => SessionService.RolActual == Roles.Operador;
        public bool IsJefe => SessionService.RolActual == Roles.Supervisor;
        public bool IsAuditor => SessionService.RolActual == Roles.Auditor;

        // Constructor
        public DashboardViewModel()
        {
            Usuario = SessionService.UsuarioActual?? "Desconocido";
            Rol = SessionService.RolActual?.ToString()??"";
        }

        [RelayCommand]
        private async Task CerrarSesion()
        {
            try
            {
                SessionService.CerrarSesion();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    var window = Application.Current?.Windows?.FirstOrDefault();
                    if (window == null)
                        throw new InvalidOperationException("No hay ventana activa");

                    window.Page = new AppShell();
                });
            }
            catch(Exception ex)
            {
                // por ahora mostramos algo..
                Application.Current?.MainPage?.DisplayAlertAsync("Error", "No se pudo cerrar sesión correctamente.\n" + ex.Message, "OK" );
            }
            
        }
    }
}
