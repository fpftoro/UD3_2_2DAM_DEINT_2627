namespace LaboratorioBinding;

/// <summary>
/// Un extra que se puede añadir a una reserva de pista.
/// </summary>
public class Extra : ObjetoObservable
{
    private bool _seleccionado;

    /// <summary>
    /// Crea un extra.
    /// </summary>
    /// <param name="nombre">Nombre que se muestra.</param>
    /// <param name="precio">Precio en euros.</param>
    public Extra(string nombre, decimal precio)
    {
        Nombre = nombre;
        Precio = precio;
    }

    /// <summary>Nombre que se muestra.</summary>
    public string Nombre { get; }

    /// <summary>Precio en euros.</summary>
    public decimal Precio { get; }

    /// <summary>Si el usuario ha marcado el extra.</summary>
    public bool Seleccionado
    {
        get => _seleccionado;
        set => Asignar(ref _seleccionado, value);
    }
}
