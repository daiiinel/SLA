using SLA.Services;
using SLA.Models;
using System.Collections.ObjectModel;

namespace SLA.Views
{
    public partial class HistorialPage : ContentPage
    {
       // private readonly InicioSesionService _service = new();
        private ObservableCollection<Registro> _registros = new();
        private List<Registro> _todosLosRegistros = new();

        public HistorialPage()
        {
            InitializeComponent();
            HistorialView.ItemsSource = _registros;

            //llenamos el picker
            TipoFiltroPicker.ItemsSource = new List<string> { "Todos", "Alta", "Baja", "Traslado", "Prestamo" };

            TipoFiltroPicker.SelectedIndex = 0;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _ = CargarHistorialAsync(); //_= disparador las tasks¿
        }

        private async Task CargarHistorialAsync()
        {
            try
            {
                _registros.Clear();
                _todosLosRegistros = await RegistroStorageService.ObtenerTodosAsync();

                AplicarFiltros();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error",  $"No se pudo cargar el historial\n {ex.Message}", "Aceptar");

                // opcional: loguear el error para auditoría
                System.Diagnostics.Debug.WriteLine($"Error en Historial: {ex}");
            }
        }

        private void AplicarFiltros()
        {
            _registros.Clear();

            var filtrados = _todosLosRegistros.AsEnumerable();

            // filtro por tipo
            var tipo = TipoFiltroPicker.SelectedItem?.ToString();
            if (!string.IsNullOrWhiteSpace(tipo) && tipo != "Todos")
                filtrados = filtrados.Where(r => r.TipoMovimiento == tipo);

            // filtro por fecha
            var desde = FechaDesdePicker.Date;
            var hasta = FechaHastaPicker.Date;

            filtrados = filtrados.Where(r => r.Fecha.Date >= desde && r.Fecha.Date <= hasta);

            foreach (var r in filtrados)
                _registros.Add(r);
        }

        private void OnFiltroChanged(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        private async void OnRegistroSeleccionado(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is not Registro seleccionado)
                return;

            RegistroActualService.RegistroActual = seleccionado;

            // navegamos (sin pasar parámetros para no ensuciar la url ni fallar en la serialización)
            await Shell.Current.GoToAsync(nameof(DetalleRegistroPage));

            ((CollectionView)sender).SelectedItem = null;
        }
    }
}
