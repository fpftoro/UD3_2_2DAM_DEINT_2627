using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using Club.Datos;
using Club.Nucleo.Datos;
using Club.Nucleo.ViewModels;
using Club.Wpf.Servicios;

namespace Club.Wpf;

/// <summary>
/// Lógica de interacción para App.xaml
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Hace que los <c>{Binding}</c> conviertan los números y las fechas con el idioma de Windows
    /// (por defecto, WPF usa en-US: 35.00 € y 7,5 leído como 75).
    /// </summary>
    public App()
    {
        FrameworkElement.LanguageProperty.OverrideMetadata(
            typeof(FrameworkElement),
            new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
    }

    /// <summary>
    /// Raíz de composición: elige SQLite como repositorio y monta la ventana.
    /// Es el único sitio de la aplicación que sabe que los datos están en SQLite.
    /// </summary>
    /// <param name="e">Argumentos del arranque.</param>
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // La base de datos se copia junto al ejecutable al compilar (ver Club.Wpf.csproj)
        var fichero = Path.Combine(AppContext.BaseDirectory, "Datos", "club.db");
        var cadena = $"Data Source={fichero}";
        var dialogos = new ServicioDialogosWpf();

        try
        {
            BaseDatosClub.Inicializar(cadena, conDatosDeEjemplo: true);
        }
        catch (DatosException ex)
        {
            dialogos.MostrarError($"{ex.Message} ({fichero})", "Club Deportivo");
            Shutdown(1);
            return;
        }

        IRepositorioClub repositorio = new RepositorioSqlite(cadena);
        var ventana = new MainWindow { DataContext = new ClubViewModel(repositorio, dialogos) };
        ventana.Show();
    }
}
