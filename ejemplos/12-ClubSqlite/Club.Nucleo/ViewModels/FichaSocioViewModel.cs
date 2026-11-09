using System.Collections;
using System.ComponentModel;
using Club.Nucleo.Modelos;
using Club.Nucleo.Mvvm;

namespace Club.Nucleo.ViewModels;

/// <summary>
/// Formulario de alta o edición de un socio, con validación.
/// Trabaja sobre sus propios campos: el socio original no cambia hasta que se guarda.
/// </summary>
public class FichaSocioViewModel : ObjetoObservable, INotifyDataErrorInfo
{
    private readonly Dictionary<string, List<string>> _errores = [];
    private readonly int _id;
    private string _nombre;
    private string _categoria;
    private string _antiguedadTexto;

    /// <summary>
    /// Crea la ficha.
    /// </summary>
    /// <param name="socio">El socio que se edita, o <c>null</c> para dar de alta uno nuevo.</param>
    public FichaSocioViewModel(Socio? socio)
    {
        _id = socio?.Id ?? 0;
        _nombre = socio?.Nombre ?? "";
        _categoria = socio?.Categoria ?? "Adulto";
        _antiguedadTexto = (socio?.Antiguedad ?? 0).ToString();
        ValidarTodo();
    }

    /// <summary>Se lanza cuando cambian los errores de una propiedad.</summary>
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    /// <summary>Título del formulario.</summary>
    public string Titulo => EsNuevo ? "Nuevo socio" : "Editar socio";

    /// <summary>Si la ficha es de un socio que aún no está guardado.</summary>
    public bool EsNuevo => _id == 0;

    /// <summary>Categorías que puede elegir el usuario.</summary>
    public IReadOnlyList<string> Categorias => CalculadoraCuotas.Categorias;

    /// <summary>Nombre completo. Obligatorio, de 60 caracteres como máximo.</summary>
    public string Nombre
    {
        get => _nombre;
        set
        {
            if (Asignar(ref _nombre, value))
            {
                Validar(nameof(Nombre));
            }
        }
    }

    /// <summary>Categoría del socio.</summary>
    public string Categoria
    {
        get => _categoria;
        set
        {
            if (Asignar(ref _categoria, value))
            {
                OnPropertyChanged(nameof(Cuota));
            }
        }
    }

    /// <summary>
    /// Antigüedad tal como la escribe el usuario. Es texto para poder validar también
    /// lo que no es un número («abc»).
    /// </summary>
    public string AntiguedadTexto
    {
        get => _antiguedadTexto;
        set
        {
            if (Asignar(ref _antiguedadTexto, value))
            {
                Validar(nameof(AntiguedadTexto));
                OnPropertyChanged(nameof(Cuota));
            }
        }
    }

    /// <summary>Cuota con los datos actuales, o <c>null</c> si la antigüedad no es válida.</summary>
    public decimal? Cuota =>
        LeerAntiguedad() is int antiguedad ? CalculadoraCuotas.Calcular(Categoria, antiguedad) : null;

    /// <summary>Si hay algún error de validación.</summary>
    public bool HasErrors => _errores.Count > 0;

    /// <summary>Devuelve los errores de una propiedad (o de todas, si es <c>null</c> o vacía).</summary>
    /// <param name="propertyName">Nombre de la propiedad.</param>
    /// <returns>Los mensajes de error.</returns>
    public IEnumerable GetErrors(string? propertyName)
    {
        if (string.IsNullOrEmpty(propertyName))
        {
            return _errores.Values.SelectMany(e => e).ToList();
        }

        return _errores.TryGetValue(propertyName, out var errores) ? errores : Array.Empty<string>();
    }

    /// <summary>
    /// Añade un error que no se puede comprobar dentro de la ficha (por ejemplo, un nombre repetido).
    /// </summary>
    /// <param name="propiedad">Propiedad con el error.</param>
    /// <param name="mensaje">Mensaje para el usuario.</param>
    public void AnadirError(string propiedad, string mensaje)
    {
        _errores[propiedad] = [mensaje];
        AvisarErrores(propiedad);
    }

    /// <summary>
    /// Crea el modelo con los datos del formulario. Solo se debe llamar si no hay errores.
    /// </summary>
    /// <returns>El socio, con el mismo Id que el original.</returns>
    public Socio CrearSocio() => new()
    {
        Id = _id,
        Nombre = Nombre.Trim(),
        Categoria = Categoria,
        Antiguedad = LeerAntiguedad() ?? 0
    };

    private int? LeerAntiguedad() =>
        int.TryParse(AntiguedadTexto, out var antiguedad) && antiguedad is >= 0 and <= 100 ? antiguedad : null;

    private void ValidarTodo()
    {
        Validar(nameof(Nombre));
        Validar(nameof(AntiguedadTexto));
    }

    private void Validar(string propiedad)
    {
        string? error = propiedad switch
        {
            nameof(Nombre) when string.IsNullOrWhiteSpace(Nombre) => "El nombre es obligatorio.",
            nameof(Nombre) when Nombre.Trim().Length > 60 => "El nombre no puede pasar de 60 caracteres.",
            nameof(AntiguedadTexto) when LeerAntiguedad() is null => "La antigüedad debe ser un número de años entre 0 y 100.",
            _ => null
        };

        var teniaError = _errores.Remove(propiedad);
        if (error is not null)
        {
            _errores[propiedad] = [error];
        }

        if (teniaError || error is not null)
        {
            AvisarErrores(propiedad);
        }
    }

    private void AvisarErrores(string propiedad)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propiedad));
        OnPropertyChanged(nameof(HasErrors));
    }
}
