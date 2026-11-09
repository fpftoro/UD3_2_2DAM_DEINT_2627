using CommunityToolkit.Mvvm.ComponentModel;

namespace MvvmToolkit;

/// <summary>
/// Un socio del club. El generador del Toolkit escribe el cuerpo de las propiedades y PropertyChanged.
/// </summary>
public partial class Socio : ObservableObject
{
    /// <summary>
    /// Crea un socio.
    /// </summary>
    /// <param name="nombre">Nombre completo.</param>
    /// <param name="categoria">Categoría: Infantil, Adulto o Senior.</param>
    /// <param name="antiguedad">Años como socio.</param>
    public Socio(string nombre, string categoria, int antiguedad)
    {
        Nombre = nombre;
        Categoria = categoria;
        Antiguedad = antiguedad;
    }

    /// <summary>Nombre completo.</summary>
    [ObservableProperty]
    public partial string Nombre { get; set; }

    /// <summary>Categoría del socio. Al cambiar, también avisa de <see cref="Cuota"/>.</summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Cuota))]
    public partial string Categoria { get; set; }

    /// <summary>Años como socio. Al cambiar, también avisa de <see cref="Cuota"/>.</summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Cuota))]
    public partial int Antiguedad { get; set; }

    /// <summary>Cuota mensual: 15, 35 o 25 € según la categoría, con un 10 % menos desde 10 años.</summary>
    public decimal Cuota
    {
        get
        {
            decimal cuota = Categoria switch
            {
                "Infantil" => 15m,
                "Senior" => 25m,
                _ => 35m
            };

            return Antiguedad >= 10 ? cuota * 0.9m : cuota;
        }
    }
}
