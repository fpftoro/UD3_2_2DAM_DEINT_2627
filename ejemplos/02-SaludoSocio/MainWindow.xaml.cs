using System.Windows;

namespace SaludoSocio;

/// <summary>
/// Lógica de interacción para MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void btnSaludar_Click(object sender, RoutedEventArgs e)
    {
        var nombre = txtNombre.Text.Trim();

        txtSaludo.Text = nombre.Length == 0
            ? "Escribe primero un nombre."
            : $"¡Hola, {nombre}! Bienvenido al club.";
    }
}
