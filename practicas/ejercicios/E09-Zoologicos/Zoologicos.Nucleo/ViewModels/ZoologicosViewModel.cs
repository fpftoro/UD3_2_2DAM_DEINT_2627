using System.Collections.ObjectModel;
using Zoologicos.Nucleo.Datos;
using Zoologicos.Nucleo.Modelos;
using Zoologicos.Nucleo.Mvvm;
using Zoologicos.Nucleo.Servicios;

namespace Zoologicos.Nucleo.ViewModels;

/// <summary>
/// La ventana de los zoológicos: la lista de zoológicos (maestro), sus animales (detalle) y todos los animales,
/// con los comandos para añadir, cambiar y eliminar zoológicos y animales y para relacionarlos.
/// </summary>
public class ZoologicosViewModel : ObjetoObservable
{
    /// <summary>
    /// Crea el ViewModel y carga los zoológicos y los animales.
    /// </summary>
    /// <param name="repositorio">Dónde están los datos.</param>
    /// <param name="dialogos">Cómo se pregunta y se avisa al usuario.</param>
    public ZoologicosViewModel(IRepositorioZoos repositorio, IServicioDialogos dialogos)
    {
        throw new NotImplementedException();
    }

    /// <summary>Todos los zoológicos, ordenados por ciudad.</summary>
    public ObservableCollection<Zoo> Zoos => throw new NotImplementedException();

    /// <summary>Los animales del zoológico elegido (vacía si no hay ninguno elegido).</summary>
    public ObservableCollection<Animal> AnimalesDelZoo => throw new NotImplementedException();

    /// <summary>Todos los animales, ordenados por especie.</summary>
    public ObservableCollection<Animal> Animales => throw new NotImplementedException();

    /// <summary>Zoológico elegido. Al cambiarlo se cargan sus animales y su ciudad pasa a <see cref="TextoZoo"/>.</summary>
    public Zoo? ZooSeleccionado
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    /// <summary>Animal elegido en la lista de los del zoológico.</summary>
    public Animal? AnimalDelZooSeleccionado
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    /// <summary>Animal elegido en la lista de todos. Al cambiarlo, su especie pasa a <see cref="TextoAnimal"/>.</summary>
    public Animal? AnimalSeleccionado
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    /// <summary>Ciudad que se escribe para añadir un zoológico o cambiar el elegido.</summary>
    public string TextoZoo
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    /// <summary>Especie que se escribe para añadir un animal o cambiar el elegido.</summary>
    public string TextoAnimal
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    /// <summary>Mensaje con el resultado de la última acción.</summary>
    public string Estado => throw new NotImplementedException();

    /// <summary>Añade un zoológico con <see cref="TextoZoo"/> y lo elige. Solo si hay texto.</summary>
    public RelayCommand AnadirZooCommand => throw new NotImplementedException();

    /// <summary>Cambia la ciudad del elegido por <see cref="TextoZoo"/>. Solo si hay uno elegido y el texto es distinto.</summary>
    public RelayCommand ActualizarZooCommand => throw new NotImplementedException();

    /// <summary>Elimina el zoológico elegido, tras confirmarlo. Solo si hay uno elegido.</summary>
    public RelayCommand EliminarZooCommand => throw new NotImplementedException();

    /// <summary>Añade un animal con <see cref="TextoAnimal"/> y lo elige. Solo si hay texto.</summary>
    public RelayCommand AnadirAnimalCommand => throw new NotImplementedException();

    /// <summary>Cambia la especie del elegido por <see cref="TextoAnimal"/>. Solo si hay uno elegido y el texto es distinto.</summary>
    public RelayCommand ActualizarAnimalCommand => throw new NotImplementedException();

    /// <summary>Elimina el animal elegido de todos los zoológicos, tras confirmarlo. Solo si hay uno elegido.</summary>
    public RelayCommand EliminarAnimalCommand => throw new NotImplementedException();

    /// <summary>Añade el animal elegido al zoológico elegido. Solo si hay los dos y el zoológico no lo tiene ya.</summary>
    public RelayCommand AnadirAlZooCommand => throw new NotImplementedException();

    /// <summary>Quita del zoológico elegido el animal elegido en su lista. Solo si hay los dos.</summary>
    public RelayCommand QuitarDelZooCommand => throw new NotImplementedException();
}
