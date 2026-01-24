using SLA.Models;

namespace SLA.Services;

public static class RegistroActualService
{
    public static Registro? RegistroActual { get; set; } = new Registro();

    public static void CrearNuevo(string operador)
    {
        RegistroActual = new Registro{ Operador = operador, Fecha = DateTime.Now };
    }
    public static bool HayRegistroActivo()
    {
        return RegistroActual != null;
    }

    public static void Limpiar()
    {
        RegistroActual = new Registro();
    }
}
