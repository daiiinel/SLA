using SLA.Services;
using SLA.Models;
using SLA.Views;


namespace SLA.Views
{
    public partial class MainPage : ContentPage
    {
        private readonly InicioSesionService _service = new();

        public MainPage()
        {
            InitializeComponent();
        }
        private async void OnInicioClicked(object sender, EventArgs e)
        {
            await RegistrarConTipo("Inicio");
        }

        private async void OnCierreClicked(object sender, EventArgs e)
        {
            await RegistrarConTipo("Cierre");
        }
        private async void OnVerHistorialClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(HistorialPage));
        }
        private async Task RegistrarConTipo(string tipo)
        {
            var usuario = SessionService.UsuarioActual;

            if(string.IsNullOrEmpty(usuario))
            {
                await DisplayAlertAsync("Sesion invalida", "usuario vacio","OK");

                await Shell.Current.GoToAsync("//LoginPage");
                return;
            }

            try
            {
                _service.Registrar(usuario, tipo);
                await DisplayAlertAsync("OK", $"{tipo} registrado correctamente", "Bien");
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Error", ex.Message, "OK");
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (!SessionService.EstaLogueado)
            {
                await Shell.Current.GoToAsync("//LoginPage");
                return;
            }
            if (SessionService.RolActual != Roles.Supervisor)
            {
                HistorialButton.IsVisible = false;
            }

            BienvenidaLabel.Text = $"Hola, {SessionService.UsuarioActual}";
        }
        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            SessionService.CerrarSesion();
            

            // Reinicia la navegación y vuelve al Login
            await Shell.Current.GoToAsync("//LoginPage");
        }


    }
}
