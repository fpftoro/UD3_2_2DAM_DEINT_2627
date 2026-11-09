using System.Windows;
using System.Windows.Controls;

namespace SociosComandos.Controles;

/// <summary>
/// Barra que muestra un valor sobre un máximo («Plazas ocupadas 3 de 10»).
/// Sus propiedades son de dependencia para poder ser destino de un binding.
/// </summary>
public partial class Medidor : UserControl
{
    /// <summary>Identifica la propiedad de dependencia <see cref="Valor"/>.</summary>
    public static readonly DependencyProperty ValorProperty =
        DependencyProperty.Register(nameof(Valor), typeof(double), typeof(Medidor), new PropertyMetadata(0.0));

    /// <summary>Identifica la propiedad de dependencia <see cref="Maximo"/>.</summary>
    public static readonly DependencyProperty MaximoProperty =
        DependencyProperty.Register(nameof(Maximo), typeof(double), typeof(Medidor), new PropertyMetadata(100.0));

    /// <summary>Identifica la propiedad de dependencia <see cref="Titulo"/>.</summary>
    public static readonly DependencyProperty TituloProperty =
        DependencyProperty.Register(nameof(Titulo), typeof(string), typeof(Medidor), new PropertyMetadata(""));

    public Medidor()
    {
        InitializeComponent();
    }

    /// <summary>Valor actual.</summary>
    public double Valor
    {
        get => (double)GetValue(ValorProperty);
        set => SetValue(ValorProperty, value);
    }

    /// <summary>Valor máximo.</summary>
    public double Maximo
    {
        get => (double)GetValue(MaximoProperty);
        set => SetValue(MaximoProperty, value);
    }

    /// <summary>Texto que precede a los números.</summary>
    public string Titulo
    {
        get => (string)GetValue(TituloProperty);
        set => SetValue(TituloProperty, value);
    }
}
