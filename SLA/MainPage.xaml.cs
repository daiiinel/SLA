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

            HistorialView.ItemsSource = _historial;
        }

        private async void OnRegistrarInicioClicked(object sender, EventArgs e)
        {
            var usuario = UsuarioEntry.Text?.Trim();

            if (string.IsNullOrEmpty(usuario))
            {
                await DisplayAlertAsync("Error", "Debe ingresar un usuario", "OK");
                return;
            }

            _service.Registrar(usuario, "Inicio manual");

            UsuarioEntry.Text = string.Empty;
            await DisplayAlertAsync("OK", "Inicio registrado correctamente", "Bien");
        }

        private async void OnInicioClicked(object sender, EventArgs e)
        {
            await RegistrarConTipo("Inicio");
        }

        private async void OnCierreClicked(object sender, EventArgs e)
        {
            await RegistrarConTipo("Cierre");
        }

        private async Task RegistrarConTipo(string tipo)
        {
            var usuario = UsuarioEntry?.Text?.Trim(); // <- protegemos contra null

            if (string.IsNullOrEmpty(usuario))
            {
                await DisplayAlertAsync("Error", "Debe ingresar un usuario...", "OK");
                return;
            }

            try
            {
                _service.Registrar(usuario, tipo);
                UsuarioEntry.Text = string.Empty;
                await DisplayAlertAsync("OK", $"{tipo} registrado correctamente", "Bien...");
            }
            catch (Exception ex)//la excepcion surge si o si (solucion en el prox commit dea)
            {
                await DisplayAlertAsync("Error inesperado", ex.Message, "OK");
            }
        }

        private async void OnVerHistorialClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(HistorialPage));
        }
    }
}
