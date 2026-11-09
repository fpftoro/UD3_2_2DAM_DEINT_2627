using System.Collections.ObjectModel;
using System.ComponentModel;

namespace LaboratorioBinding;

/// <summary>
/// Objeto que usa la ventana como DataContext: los bindings leen y escriben sus propiedades.
/// </summary>
public class DatosLaboratorio : ObjetoObservable
{
    private string _nombre = "Lucía";
    private string _comentario = "";

    /// <summary>
    /// Crea los datos con tres extras de partida.
    /// </summary>
    public DatosLaboratorio()
    {
        Extras = [];
        foreach (var extra in new[] { new Extra("Luz", 4m), new Extra("Raquetas", 3m), new Extra("Pelotas", 2.5m) })
        {
            AnadirExtra(extra);
        }
    }

    /// <summary>Nombre que comparten las cajas de la sección «Modos».</summary>
    public string Nombre
    {
        get => _nombre;
        set => Asignar(ref _nombre, value);
    }

    /// <summary>Comentario que escribe la caja OneWayToSource.</summary>
    public string Comentario
    {
        get => _comentario;
        set => Asignar(ref _comentario, value);
    }

    /// <summary>Extras disponibles. Al ser observable, la lista de la ventana sigue sus altas.</summary>
    public ObservableCollection<Extra> Extras { get; }

    /// <summary>Precio de los extras marcados.</summary>
    public decimal Total => Extras.Where(e => e.Seleccionado).Sum(e => e.Precio);

    /// <summary>
    /// Estado de la casilla «Todos»: <c>true</c> si están todos marcados,
    /// <c>false</c> si no hay ninguno y <c>null</c> (indeterminado) si hay algunos.
    /// </summary>
    public bool? TodosSeleccionados
    {
        get
        {
            var marcados = Extras.Count(e => e.Seleccionado);
            return marcados == 0 ? false : marcados == Extras.Count ? true : null;
        }
        set
        {
            // Al pulsar «Todos», una casilla de tres estados pasaría a indeterminado: se trata como «marcar»
            var marcar = value != false;
            foreach (var extra in Extras)
            {
                extra.Seleccionado = marcar;
            }
        }
    }

    /// <summary>
    /// Añade un extra y se suscribe a sus cambios para recalcular el total.
    /// </summary>
    /// <param name="extra">El extra que se añade.</param>
    public void AnadirExtra(Extra extra)
    {
        extra.PropertyChanged += Extra_PropertyChanged;
        Extras.Add(extra);
        AvisarResumen();
    }

    private void Extra_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Extra.Seleccionado))
        {
            AvisarResumen();
        }
    }

    private void AvisarResumen()
    {
        // Total y TodosSeleccionados se calculan: hay que avisar cuando cambian los datos de los que dependen
        OnPropertyChanged(nameof(Total));
        OnPropertyChanged(nameof(TodosSeleccionados));
    }
}
