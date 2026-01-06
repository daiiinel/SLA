using SLA.Views;

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

        // LOGIN SIMULADO (por ahora)
        if (usuario == "admin" && password == "123")
        {
            SesionActual.Usuario = usuario;
            SesionActual.Rol = "ADMIN";

            try
            {
                await Shell.Current.GoToAsync("MainPage");
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("ERROR REAL", ex.ToString(), "OK");
            }
        }
        else
        {
            await DisplayAlertAsync("Error", "Usuario o contraseña incorrectos", "OK");
        }
    }
}
