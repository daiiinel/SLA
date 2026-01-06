using SLA.Services;
using SLA.Models;
using System.Collections.ObjectModel;
using SLA.Views;


namespace SLA
{
    public partial class MainPage : ContentPage
    {
        private readonly InicioSesionService _service = new();
        private ObservableCollection<InicioSesion> _historial = new();

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
            try
            {
                _service.Registrar(SesionActual.Usuario, tipo);
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

            if (!SesionActual.EstaAutenticado)
            {
                await Shell.Current.GoToAsync("Login");
                return;
            }
            if (SesionActual.Rol != "ADMIN")
            {
                HistorialButton.IsVisible = false;
            }

            BienvenidaLabel.Text = $"Hola, {SesionActual.Usuario}";
        }
        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            SesionActual.Usuario = string.Empty;
            SesionActual.Rol = string.Empty;

            // Reinicia la navegación y vuelve al Login
            await Shell.Current.GoToAsync("//Login");
        }


    }
}
