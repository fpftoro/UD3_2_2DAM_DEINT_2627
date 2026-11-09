using System.Windows;
using System.Windows.Controls;
using Biblioteca.Nucleo.Modelos;

namespace Biblioteca.Wpf;

/// <summary>
/// Ventana de la biblioteca, con toda la lógica en el code-behind. Funciona, pero es el punto de partida
/// del ejercicio: hay que reestructurarla con MVVM.
/// </summary>
public partial class MainWindow : Window
{
    private readonly List<Libro> _libros = [];
    private readonly List<Libro> _visibles = []; // lo que se ve en la lista, en el mismo orden
    private int _ultimoId;
    private Libro? _seleccionado;
    private bool _esNuevo;

    public MainWindow()
    {
        InitializeComponent();

        foreach (var genero in Generos.Todos)
        {
            cmbGenero.Items.Add(genero);
        }

        _libros.Add(new Libro { Id = ++_ultimoId, Titulo = "Cien años de soledad", Autor = "Gabriel García Márquez", Anio = 1967 });
        _libros.Add(new Libro { Id = ++_ultimoId, Titulo = "Don Quijote de la Mancha", Autor = "Miguel de Cervantes", Anio = 1605, Lector = "Lucía Pérez" });
        _libros.Add(new Libro { Id = ++_ultimoId, Titulo = "Platero y yo", Autor = "Juan Ramón Jiménez", Anio = 1914, Genero = "Poesía" });
        _libros.Add(new Libro { Id = ++_ultimoId, Titulo = "Breve historia del tiempo", Autor = "Stephen Hawking", Anio = 1988, Genero = "Divulgación", Lector = "Mario Gil" });
        _libros.Add(new Libro { Id = ++_ultimoId, Titulo = "Mafalda 1", Autor = "Quino", Anio = 1966, Genero = "Cómic" });
        _libros.Add(new Libro { Id = ++_ultimoId, Titulo = "La sombra del viento", Autor = "Carlos Ruiz Zafón", Anio = 2001 });

        RefrescarLista();
        LimpiarFicha();
        ActualizarResumen();
        ActualizarBotones();
    }

    private void RefrescarLista()
    {
        var buscar = txtBuscar.Text.Trim().ToLower();
        _visibles.Clear();
        lstLibros.Items.Clear();
        foreach (var libro in _libros.OrderBy(l => l.Titulo))
        {
            if (libro.Titulo.ToLower().Contains(buscar) || libro.Autor.ToLower().Contains(buscar))
            {
                _visibles.Add(libro);
                var estado = libro.Lector == null ? "disponible" : "prestado a " + libro.Lector;
                lstLibros.Items.Add(libro.Titulo + " · " + libro.Autor + " · " + libro.Anio + " · " + estado);
            }
        }
    }

    private void txtBuscar_TextChanged(object sender, TextChangedEventArgs e)
    {
        RefrescarLista();
        LimpiarFicha();
        ActualizarBotones();
    }

    private void lstLibros_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (lstLibros.SelectedIndex < 0)
        {
            _seleccionado = null;
            ActualizarBotones();
            return;
        }

        _seleccionado = _visibles[lstLibros.SelectedIndex];
        _esNuevo = false;
        txtEncabezado.Text = "Editar libro";
        txtTitulo.Text = _seleccionado.Titulo;
        txtAutor.Text = _seleccionado.Autor;
        txtAnio.Text = _seleccionado.Anio.ToString();
        cmbGenero.SelectedItem = _seleccionado.Genero;
        pnlFicha.Visibility = Visibility.Visible;
        ActualizarBotones();
    }

    private void btnNuevo_Click(object sender, RoutedEventArgs e)
    {
        lstLibros.SelectedIndex = -1;
        _seleccionado = null;
        _esNuevo = true;
        txtEncabezado.Text = "Nuevo libro";
        txtTitulo.Text = "";
        txtAutor.Text = "";
        txtAnio.Text = "";
        cmbGenero.SelectedIndex = 0;
        pnlFicha.Visibility = Visibility.Visible;
        txtTitulo.Focus();
        ActualizarBotones();
    }

    private void btnGuardar_Click(object sender, RoutedEventArgs e)
    {
        if (txtTitulo.Text.Trim() == "")
        {
            MessageBox.Show("El título es obligatorio.", "Biblioteca", MessageBoxButton.OK, MessageBoxImage.Error);
            txtTitulo.Focus();
            return;
        }

        if (txtAutor.Text.Trim() == "")
        {
            MessageBox.Show("El autor es obligatorio.", "Biblioteca", MessageBoxButton.OK, MessageBoxImage.Error);
            txtAutor.Focus();
            return;
        }

        if (!int.TryParse(txtAnio.Text, out var anio) || anio < 1450 || anio > DateTime.Today.Year)
        {
            MessageBox.Show("El año debe ser un número entre 1450 y " + DateTime.Today.Year + ".", "Biblioteca",
                MessageBoxButton.OK, MessageBoxImage.Error);
            txtAnio.Focus();
            return;
        }

        Libro libro;
        if (_esNuevo)
        {
            libro = new Libro { Id = ++_ultimoId };
            _libros.Add(libro);
        }
        else if (_seleccionado != null)
        {
            libro = _seleccionado;
        }
        else
        {
            return;
        }

        libro.Titulo = txtTitulo.Text.Trim();
        libro.Autor = txtAutor.Text.Trim();
        libro.Anio = anio;
        libro.Genero = (string)cmbGenero.SelectedItem;

        RefrescarLista();
        lstLibros.SelectedIndex = _visibles.IndexOf(libro);
        txtEstado.Text = "Guardado «" + libro.Titulo + "».";
        ActualizarResumen();
        ActualizarBotones();
    }

    private void btnEliminar_Click(object sender, RoutedEventArgs e)
    {
        if (_seleccionado == null)
        {
            return;
        }

        if (_seleccionado.Lector != null)
        {
            MessageBox.Show("No se puede eliminar «" + _seleccionado.Titulo + "»: está prestado.", "Biblioteca",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (MessageBox.Show("¿Seguro que quieres eliminar «" + _seleccionado.Titulo + "»?", "Biblioteca",
                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
        {
            return;
        }

        var titulo = _seleccionado.Titulo;
        _libros.Remove(_seleccionado);
        _seleccionado = null;
        RefrescarLista();
        LimpiarFicha();
        txtEstado.Text = "Eliminado «" + titulo + "».";
        ActualizarResumen();
        ActualizarBotones();
    }

    private void txtLector_TextChanged(object sender, TextChangedEventArgs e)
    {
        ActualizarBotones();
    }

    private void btnPrestar_Click(object sender, RoutedEventArgs e)
    {
        if (_seleccionado == null || _seleccionado.Lector != null || txtLector.Text.Trim() == "")
        {
            return;
        }

        var libro = _seleccionado;
        libro.Lector = txtLector.Text.Trim();
        txtLector.Text = "";
        RefrescarLista();
        lstLibros.SelectedIndex = _visibles.IndexOf(libro);
        txtEstado.Text = "«" + libro.Titulo + "» prestado a " + libro.Lector + ".";
        ActualizarResumen();
        ActualizarBotones();
    }

    private void btnDevolver_Click(object sender, RoutedEventArgs e)
    {
        if (_seleccionado == null || _seleccionado.Lector == null)
        {
            return;
        }

        var libro = _seleccionado;
        libro.Lector = null;
        RefrescarLista();
        lstLibros.SelectedIndex = _visibles.IndexOf(libro);
        txtEstado.Text = "«" + libro.Titulo + "» devuelto.";
        ActualizarResumen();
        ActualizarBotones();
    }

    private void LimpiarFicha()
    {
        _esNuevo = false;
        txtTitulo.Text = "";
        txtAutor.Text = "";
        txtAnio.Text = "";
        cmbGenero.SelectedIndex = -1;
        pnlFicha.Visibility = Visibility.Collapsed;
    }

    private void ActualizarResumen()
    {
        var prestados = _libros.Count(l => l.Lector != null);
        txtResumen.Text = "Disponibles: " + (_libros.Count - prestados) + " · Prestados: " + prestados;
    }

    private void ActualizarBotones()
    {
        btnGuardar.IsEnabled = pnlFicha.Visibility == Visibility.Visible;
        btnEliminar.IsEnabled = _seleccionado != null && _seleccionado.Lector == null;
        btnPrestar.IsEnabled = _seleccionado != null && _seleccionado.Lector == null && txtLector.Text.Trim() != "";
        btnDevolver.IsEnabled = _seleccionado != null && _seleccionado.Lector != null;
    }
}
