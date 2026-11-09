using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace FichaSocio;

/// <summary>
/// La misma ventana que <c>MainWindow.xaml</c>, construida solo con código C#.
/// No se abre al arrancar: está para comparar las dos formas de describir una interfaz.
/// </summary>
public class VentanaEnCodigo : Window
{
    /// <summary>Crea la ventana y todos sus controles.</summary>
    public VentanaEnCodigo()
    {
        Title = "Ficha del socio";
        Height = 300;
        Width = 420;
        FontFamily = new FontFamily("Segoe UI");
        FontSize = 14;

        var cabecera = new TextBlock
        {
            Text = FichaClub.Nombre,
            FontSize = 22,
            FontWeight = FontWeights.Bold
        };

        var cuota = new TextBlock { Margin = new Thickness(0, 4, 0, 0) };
        cuota.Inlines.Add(new Run("Cuota mensual: "));
        cuota.Inlines.Add(new Run("35,00 €") { FontWeight = FontWeights.Bold });

        var editar = new Button
        {
            Content = "Editar",
            Margin = new Thickness(0, 12, 0, 0),
            Padding = new Thickness(16, 4, 16, 4),
            HorizontalAlignment = HorizontalAlignment.Left,
            Background = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 1),
                GradientStops =
                {
                    new GradientStop(Color.FromRgb(0xE3, 0xF2, 0xFD), 0),
                    new GradientStop(Color.FromRgb(0x90, 0xCA, 0xFA), 1)
                }
            }
        };

        var borde = new Border
        {
            Margin = new Thickness(0, 8, 0, 0),
            Padding = new Thickness(12),
            BorderBrush = Brushes.Gray,
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(4),
            Child = new StackPanel
            {
                Children =
                {
                    new TextBlock { Text = "Socio: Lucía Pérez" },
                    cuota,
                    editar
                }
            }
        };

        Content = new StackPanel
        {
            Margin = new Thickness(16),
            Children = { cabecera, borde }
        };
    }
}
