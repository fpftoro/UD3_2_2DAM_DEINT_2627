namespace Biblioteca.Nucleo.Modelos;

/// <summary>
/// Los géneros que puede tener un libro.
/// </summary>
public static class Generos
{
    /// <summary>Todos los géneros, en el orden en que se ofrecen.</summary>
    public static IReadOnlyList<string> Todos { get; } = ["Novela", "Poesía", "Ensayo", "Cómic", "Divulgación"];
}
