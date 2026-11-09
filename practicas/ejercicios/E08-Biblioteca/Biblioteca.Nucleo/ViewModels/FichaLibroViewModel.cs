using System.Collections;
using System.ComponentModel;
using Biblioteca.Nucleo.Modelos;
using Biblioteca.Nucleo.Mvvm;

namespace Biblioteca.Nucleo.ViewModels;

/// <summary>
/// Ficha para crear o editar un libro, con validación: la vista marca los campos con error
/// gracias a <see cref="INotifyDataErrorInfo"/>.
/// </summary>
public class FichaLibroViewModel : ObjetoObservable, INotifyDataErrorInfo
{
    /// <summary>Año más antiguo que se admite (la imprenta de Gutenberg).</summary>
    public const int AnioMinimo = 1450;

    /// <summary>
    /// Crea la ficha de un libro existente o, con <c>null</c>, la de uno nuevo (vacía, con el primer género).
    /// Valida todos los campos al crearse.
    /// </summary>
    /// <param name="libro">El libro que se edita, o <c>null</c> para uno nuevo.</param>
    public FichaLibroViewModel(Libro? libro = null)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    /// <summary>Si es un libro que todavía no se ha guardado.</summary>
    public bool EsNuevo => throw new NotImplementedException();

    /// <summary>Encabezado de la ficha: «Nuevo libro» o «Editar libro».</summary>
    public string Encabezado => throw new NotImplementedException();

    /// <summary>Los géneros que se pueden elegir.</summary>
    public IReadOnlyList<string> Generos => throw new NotImplementedException();

    /// <summary>Título. Obligatorio.</summary>
    public string Titulo
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    /// <summary>Autor. Obligatorio.</summary>
    public string Autor
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    /// <summary>Año escrito como texto: un número entre <see cref="AnioMinimo"/> y el año actual.</summary>
    public string AnioTexto
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    /// <summary>Género del libro.</summary>
    public string Genero
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    /// <inheritdoc />
    public bool HasErrors => throw new NotImplementedException();

    /// <inheritdoc />
    public IEnumerable GetErrors(string? propertyName)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Crea el libro con los datos de la ficha (sin espacios en los extremos). Conserva el identificador
    /// y el lector del libro original, si lo había.
    /// </summary>
    /// <returns>El libro, listo para guardarlo.</returns>
    /// <exception cref="InvalidOperationException">Si la ficha tiene errores.</exception>
    public Libro CrearLibro()
    {
        throw new NotImplementedException();
    }
}
