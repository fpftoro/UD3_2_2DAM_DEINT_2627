namespace MvvmToolkit;

/// <summary>
/// DataContext de la ventana: reúne los dos ViewModels, que no se conocen entre sí.
/// </summary>
public class PrincipalViewModel
{
    /// <summary>Lista de socios.</summary>
    public SociosViewModel Socios { get; } = new();

    /// <summary>Registro de actividad.</summary>
    public RegistroViewModel Registro { get; } = new();
}
