namespace SLA.Models;

//eventos -->proximamente ah
public class Evento : ContentPage
{
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Usuario { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; } = DateTime.Now;
        public string Origen { get; set; } = string.Empty;

}