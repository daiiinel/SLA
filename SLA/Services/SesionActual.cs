namespace SLA;

public static class SesionActual
{
    public static string Usuario { get; set; } = string.Empty;
    public static string Rol { get; set; } = string.Empty;

    public static bool EstaAutenticado => !string.IsNullOrEmpty(Usuario);
}
