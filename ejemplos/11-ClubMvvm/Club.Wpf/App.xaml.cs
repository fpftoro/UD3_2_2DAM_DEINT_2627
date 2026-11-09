using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using Club.Nucleo.Modelos;
using Club.Nucleo.Servicios;
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
    /// Raíz de composición: el único sitio donde se crean los servicios y se entregan a los ViewModels.
    /// </summary>
    /// <param name="e">Argumentos del arranque.</param>
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        IServicioSocios servicio = new ServicioSociosMemoria(
        [
            new Socio { Nombre = "Lucía Pérez", Categoria = "Adulto", Antiguedad = 12 },
            new Socio { Nombre = "Mario Gil", Categoria = "Infantil", Antiguedad = 2 },
            new Socio { Nombre = "Carmen Ruiz", Categoria = "Senior", Antiguedad = 25 }
        ]);
        IServicioDialogos dialogos = new ServicioDialogosWpf();

        var ventana = new MainWindow { DataContext = new PrincipalViewModel(servicio, dialogos) };
        ventana.Show();
    }
}
