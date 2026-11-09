using System.Collections.ObjectModel;
using Biblioteca.Nucleo.Modelos;
using Biblioteca.Nucleo.Mvvm;
using Biblioteca.Nucleo.Servicios;

namespace Biblioteca.Nucleo.ViewModels;

/// <summary>
/// La ventana de la biblioteca: lista con buscador, ficha del elegido, préstamos y resumen.
/// No sabe nada de WPF: recibe el servicio de libros y el de diálogos por el constructor.
/// </summary>
public class LibrosViewModel : ObjetoObservable
{
    /// <summary>
    /// Crea el ViewModel, crea sus comandos y carga los libros.
    /// </summary>
    /// <param name="servicio">Dónde están los libros.</param>
    /// <param name="dialogos">Cómo se pregunta y se avisa al usuario.</param>
    public LibrosViewModel(IServicioLibros servicio, IServicioDialogos dialogos)
    {
        throw new NotImplementedException();
    }

    /// <summary>Los libros cuyo título o autor contienen <see cref="Buscar"/>, ordenados por título.</summary>
    public ObservableCollection<Libro> LibrosVisibles => throw new NotImplementedException();

    /// <summary>Texto del buscador (sin distinguir mayúsculas).</summary>
    public string Buscar
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    /// <summary>Libro elegido en la lista. Al cambiarlo se crea su ficha (o se quita, con <c>null</c>).</summary>
    public Libro? Seleccionado
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    /// <summary>Ficha que se está editando (de un libro nuevo o del elegido), o <c>null</c>.</summary>
    public FichaLibroViewModel? Ficha => throw new NotImplementedException();

    /// <summary>Si hay una ficha a la vista.</summary>
    public bool HayFicha => throw new NotImplementedException();

    /// <summary>Nombre de quien se lleva el libro elegido.</summary>
    public string NombreLector
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    /// <summary>Cuántos libros están en la biblioteca (de todos, no solo de los visibles).</summary>
    public int Disponibles => throw new NotImplementedException();

    /// <summary>Cuántos libros están prestados (de todos).</summary>
    public int Prestados => throw new NotImplementedException();

    /// <summary>Mensaje con el resultado de la última acción.</summary>
    public string Estado => throw new NotImplementedException();

    /// <summary>Abre una ficha vacía para un libro nuevo (sin libro elegido).</summary>
    public RelayCommand NuevoCommand => throw new NotImplementedException();

    /// <summary>Guarda la ficha y elige el libro guardado. Solo si la ficha no tiene errores.</summary>
    public RelayCommand GuardarCommand => throw new NotImplementedException();

    /// <summary>Elimina el elegido, tras confirmarlo. Solo si no está prestado.</summary>
    public RelayCommand EliminarCommand => throw new NotImplementedException();

    /// <summary>Presta el elegido a <see cref="NombreLector"/> y vacía el nombre. Solo si está disponible y hay nombre.</summary>
    public RelayCommand PrestarCommand => throw new NotImplementedException();

    /// <summary>Registra la devolución del elegido. Solo si está prestado.</summary>
    public RelayCommand DevolverCommand => throw new NotImplementedException();
}
