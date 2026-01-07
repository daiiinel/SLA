using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SLA.Services;
using SLA.Views;
using System.Windows.Input;


namespace SLA.ViewModels
{
    public partial class DashboardViewModel : ObservableObject
    {
        // Datos del usuario
        private string usuario = string.Empty;
        public string Usuario { get => usuario; set => SetProperty(ref usuario, value); }

        private string rol = string.Empty;
        public string Rol{ get => rol; set => SetProperty(ref rol, value); }

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
        private async Task NuevoRegistro()
        {
            try
            {
                await Shell.Current.GoToAsync(nameof(Views.NuevoRegistroPaso1Page));
            }
            catch(Exception ex)
            {
                var window = Application.Current?.Windows.FirstOrDefault();
                var page = window?.Page;
                if(page!=null)
                    await page.DisplayAlertAsync("Error", "No se pudo cerrar sesión correctamente.\n" + ex.Message, "OK");
            }
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
                var window = Application.Current?.Windows.FirstOrDefault();
                var page = window?.Page;

                if (page != null)
                    await page.DisplayAlertAsync("Error", "No se pudo cerrar sesión correctamente.\n" + ex.Message, "OK");
            }
            
        }

        [RelayCommand]
        async Task VerHistorial()
        {
            await Shell.Current.GoToAsync(nameof(HistorialPage));
        }

    }
}
