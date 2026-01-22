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
        public string Rol{ get => rol; set => SetProperty(ref rol, value); } //eliminable?¿ -> nio

        private Roles? _rolActual;
        public Roles? RolActual
        {
            get => _rolActual;
            set
            {
                if (SetProperty(ref _rolActual, value))
                {
                    OnPropertyChanged(nameof(IsOperador));
                    OnPropertyChanged(nameof(IsJefe));
                    OnPropertyChanged(nameof(IsAuditor));
                }
            }
        }

        // Flags por rol -> independientes del sservice y dependientes del vmodel

        public bool IsOperador => RolActual == Roles.Operador;
        public bool IsJefe => RolActual == Roles.Supervisor;
        public bool IsAuditor => RolActual == Roles.Auditor;

        // Constructor
        public DashboardViewModel()
        {
            Usuario = SessionService.UsuarioActual?? "Desconocido";
            RolActual = SessionService.RolActual;
            Rol = RolActual?.ToString()??"";
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

        [RelayCommand]
        async Task RevisarRegistros()
        {
            try
            {
                await Shell.Current.GoToAsync(nameof(RevisionRegistrosPage));
            }
            catch (Exception ex)
            {
                var page = Application.Current?.Windows.FirstOrDefault()?.Page;
                if (page != null)
                    await page.DisplayAlertAsync( "Error", ex.Message, "OK");
            }
        }
    }
}
