using SLA.Services;
using SLA.Models;
using System.Collections.ObjectModel;

namespace SLA.Views
{
    public partial class HistorialPage : ContentPage
    {
        private readonly InicioSesionService _service = new();
        private ObservableCollection<InicioSesion> _registros = new();

        public HistorialPage()
        {
            InitializeComponent();
            HistorialView.ItemsSource = _registros;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CargarHistorial();
        }

        private void CargarHistorial()
        {
            _registros.Clear();

            var datos = _service.ObtenerRegistros() ?? new List<InicioSesion>();

            foreach (var r in datos)
                _registros.Add(r);
        }

    }
}
