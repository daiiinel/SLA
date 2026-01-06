namespace SLA.Services;

public static class SessionService
{
    public static string UsuarioActual { get; private set; } = String.Empty;
    public static Roles? RolActual { get; private set; }
    public static bool EstaLogueado => !string.IsNullOrEmpty(UsuarioActual) && RolActual.HasValue;

    public static void IniciarSesion(string usuario, Roles? rol)
    {
        UsuarioActual = usuario;
        RolActual = rol;
    }

    public static void CerrarSesion()
    {
        UsuarioActual = String.Empty;
        RolActual = Roles.Operador;
    }
}
