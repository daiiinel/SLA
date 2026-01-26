using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SLA.Models;
using SLA.Services;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace SLA.ViewModels
{
    public partial class RevisionRegistroViewModel : ObservableObject
    {
        public ObservableCollection<Registro> RegistrosEnviados { get; } = new();

        private Registro? registroSeleccionado;
        public Registro? RegistroSeleccionado
        {
            get => registroSeleccionado;
            set
            {
                if (SetProperty(ref registroSeleccionado, value))
                    OnRegistroSeleccionadoChanged(value);
            }
        }
        private string? observacion;
        public string? Observacion { get => observacion; set => SetProperty(ref observacion, value); }

        public RevisionRegistroViewModel()
        {
            // Carga inicial automática
            _ = CargarRegistrosAsync();
        }

        private void OnRegistroSeleccionadoChanged(Registro? value)
        {
            if (value != null)
            {
                // Guardamos en el servicio "Puente" para que la página de detalle tenga info
                RegistroActualService.RegistroActual = value;

                // Opcional: Navegar automáticamente al detalle
                // Shell.Current.GoToAsync(nameof(DetalleRegistroPage));
            }
        }


        [RelayCommand]
        public async Task CargarRegistrosAsync()
        {
            RegistrosEnviados.Clear();

            var registros = await RegistroStorageService.ObtenerTodosAsync();
            var enviados = registros.Where(r => r.Estado == EstadoRegistro.Entregado).OrderByDescending(r => r.Fecha);

            foreach (var r in enviados)
                RegistrosEnviados.Add(r);
        }

        [RelayCommand]
        private async Task AprobarAsync()
        {
            if (RegistroSeleccionado == null)
                return;

            RegistroSeleccionado.Estado = EstadoRegistro.Auditado;

            await RegistroStorageService.ActualizarAsync(RegistroSeleccionado);

            RegistrosEnviados.Remove(RegistroSeleccionado);
            RegistroSeleccionado = null;
        }

        [RelayCommand]
        private async Task RechazarAsync()
        {
            if (RegistroSeleccionado == null)
                return;

            if (string.IsNullOrWhiteSpace(Observacion))
            {
                await Shell.Current.DisplayAlertAsync( "Observación requerida", "Debes indicar el motivo del rechazo", "OK");
                return;
            }

            bool confirmar = await Shell.Current.DisplayAlertAsync( "Rechazar registro", "¿Estás seguro de rechazar este registro?",  "Sí","Cancelar");

            if (!confirmar)
                return;

            RegistroSeleccionado.Estado = EstadoRegistro.Rechazado;
            RegistroSeleccionado.Observaciones = Observacion;

            await RegistroStorageService.ActualizarAsync(RegistroSeleccionado);

            RegistrosEnviados.Remove(RegistroSeleccionado);
            RegistroSeleccionado = null;
            Observacion = string.Empty;
        }
    }
}
