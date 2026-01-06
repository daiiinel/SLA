using SLA.Models;
using SLA.Services;

namespace SLA.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        var usuario = UsuarioEntry.Text?.Trim();
        var password = PasswordEntry.Text;

        if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(password))
        {
            await DisplayAlertAsync("Error", "Complete todos los campos", "OK");
            return;
        }

        // LOGIN SIMULADO
        Roles? rol = null;

        if (usuario == "admin" && password == "123")
            rol = Roles.Operador;
        else if (usuario == "jefe" && password == "123")
            rol = Roles.Supervisor;
        else if (usuario == "auditor" && password == "123")
            rol = Roles.Auditor;

        if (rol == null)
        {
            await DisplayAlertAsync("Error", "Usuario o contraseña incorrectos", "OK");
            return;
        }

        //sesión activa
        SessionService.IniciarSesion(usuario, rol);

        //navegación
        await Shell.Current.GoToAsync("//DashboardPage");
    }
}
