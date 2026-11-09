namespace Biblioteca.Nucleo.Modelos;

/// <summary>
/// Un libro de la biblioteca.
/// </summary>
public class Libro
{
    /// <summary>Identificador; 0 si todavía no se ha guardado.</summary>
    public int Id { get; set; }

    /// <summary>Título del libro.</summary>
    public string Titulo { get; set; } = "";

    /// <summary>Autor o autora.</summary>
    public string Autor { get; set; } = "";

    /// <summary>Año de publicación.</summary>
    public int Anio { get; set; }

    /// <summary>Uno de los géneros de <see cref="Generos.Todos"/>.</summary>
    public string Genero { get; set; } = Generos.Todos[0];

    /// <summary>Quién lo tiene prestado, o <c>null</c> si está en la biblioteca.</summary>
    public string? Lector { get; set; }

    /// <summary>Si está prestado.</summary>
    public bool Prestado => Lector is not null;
}
