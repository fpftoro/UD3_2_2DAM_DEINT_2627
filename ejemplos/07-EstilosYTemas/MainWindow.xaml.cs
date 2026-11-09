using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using EstilosYTemas.Servicios;

namespace EstilosYTemas;

/// <summary>
/// Lógica de interacción para MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly ServicioConfiguracion _servicio = new();
    private readonly ConfiguracionUsuario _configuracion;

    public MainWindow()
    {
        InitializeComponent();

        _configuracion = _servicio.Cargar();
        AplicarConfiguracion();
    }

    // --- Configuración del usuario ---

    private void AplicarConfiguracion()
    {
        Width = _configuracion.Ancho;
        Height = _configuracion.Alto;

        // La posición solo se recupera si sigue dentro de la pantalla (puede haberse quitado un monitor)
        if (_configuracion.Izquierda is double x && _configuracion.Arriba is double y && EstaEnPantalla(x, y))
        {
            WindowStartupLocation = WindowStartupLocation.Manual;
            Left = x;
            Top = y;
        }
        else
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        if (_configuracion.Maximizada)
        {
            WindowState = WindowState.Maximized;
        }

        cmbTema.SelectedIndex = _configuracion.Tema switch
        {
            "Claro" => 1,
            "Oscuro" => 2,
            _ => 0
        };

        var radio = pnlColores.Children.OfType<RadioButton>()
            .FirstOrDefault(r => (string)r.Tag == _configuracion.ColorClub) ?? rbVerde;
        radio.IsChecked = true;
    }

    private static bool EstaEnPantalla(double x, double y) =>
        x >= SystemParameters.VirtualScreenLeft
        && y >= SystemParameters.VirtualScreenTop
        && x < SystemParameters.VirtualScreenLeft + SystemParameters.VirtualScreenWidth - 100
        && y < SystemParameters.VirtualScreenTop + SystemParameters.VirtualScreenHeight - 100;

    private void Window_Closing(object? sender, CancelEventArgs e)
    {
        // Maximizada, Width y Height valen el tamaño de la pantalla: se guarda el tamaño «normal»
        var normal = WindowState == WindowState.Normal ? new Rect(Left, Top, Width, Height) : RestoreBounds;

        _configuracion.Izquierda = normal.Left;
        _configuracion.Arriba = normal.Top;
        _configuracion.Ancho = normal.Width;
        _configuracion.Alto = normal.Height;
        _configuracion.Maximizada = WindowState == WindowState.Maximized;

        try
        {
            _servicio.Guardar(_configuracion);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            // Perder las preferencias no justifica impedir que se cierre la aplicación
        }
    }

    // --- Tema y color ---

    private void cmbTema_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var (modo, nombre) = cmbTema.SelectedIndex switch
        {
            1 => (ThemeMode.Light, "Claro"),
            2 => (ThemeMode.Dark, "Oscuro"),
            _ => (ThemeMode.System, "Sistema")
        };

        Application.Current.ThemeMode = modo;
        _configuracion.Tema = nombre;
    }

    private void Color_Checked(object sender, RoutedEventArgs e)
    {
        if (e.OriginalSource is not RadioButton { Tag: string codigo })
        {
            return;
        }

        // Se sustituye el recurso: todo lo que lo usa con DynamicResource se actualiza
        var color = (Color)ColorConverter.ConvertFromString(codigo);
        Application.Current.Resources["ColorClub"] = new SolidColorBrush(color);
        _configuracion.ColorClub = codigo;
    }
}
