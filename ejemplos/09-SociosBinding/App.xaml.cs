using System.Globalization;
using System.Windows;
using System.Windows.Markup;

namespace SociosBinding;

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
}
