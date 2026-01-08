using System.Collections.ObjectModel;

namespace SLA.Models;

public class Registro
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Fecha { get; set; }
    public string TipoMovimiento { get; set; } = string.Empty;
    public string Unidad { get; set; } = string.Empty;
    public string Operador { get; set; } = string.Empty;
    public string? Observaciones { get; set; }
    public ObservableCollection<ItemRegistro> Items { get; set; } = new();
    public EstadoRegistro Estado { get; set; } = EstadoRegistro.Borrador;
}

/*
 tema con el public List<ItemRegistro> Items { get; set; } = new() en el step 2:
cuando el user agrega items, se visualizan en pantalla, pero en el reg real no se guardan
y cuando se llega al step 3 tenemos la lista vacía ..

por eso, como registro.cs es nuestra verdadera fuente de tod, la hacemos obscollect
asi no tenemos regs en memoria, copias ni reasignamos listas :D

step 1-> crea reg y datos
step 2-> agrega a ese reg los items 
step 3-> solo muestra el reg, confirma o cancela 
 */