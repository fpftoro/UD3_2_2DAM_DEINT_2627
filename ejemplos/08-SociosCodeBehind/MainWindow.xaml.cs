using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace SociosCodeBehind;

/// <summary>
/// Lógica de interacción para MainWindow.xaml.
/// Toda la aplicación está aquí, a propósito: es el punto de partida del tema 8.
/// </summary>
public partial class MainWindow : Window
{
    private const string Fichero = "socios.csv";

    private readonly List<Socio> _socios = [];

    // Socios que se ven en la lista ahora mismo (cambia con el buscador)
    private List<Socio> _visibles = [];

    // Socio que se está editando, o null si el formulario es de alta
    private Socio? _editado;

    private bool _hayCambios;
    private bool _cargando;

    public MainWindow()
    {
        InitializeComponent();
        CargarFichero();
        RellenarLista();
        LimpiarFormulario();
        ActualizarResumen();
        ActualizarBotones();
    }

    // --- Lista y buscador ---

    private void RellenarLista()
    {
        var texto = txtBuscar.Text.Trim().ToLower();
        _visibles = _socios
            .Where(s => s.Nombre.ToLower().Contains(texto))
            .OrderBy(s => s.Nombre)
            .ToList();

        lstSocios.Items.Clear();
        foreach (var socio in _visibles)
        {
            lstSocios.Items.Add($"{socio.Nombre} · {socio.Categoria} · {socio.Antiguedad} años");
        }
    }

    private void txtBuscar_TextChanged(object sender, TextChangedEventArgs e)
    {
        RellenarLista();
        LimpiarFormulario();
        ActualizarBotones();
    }

    private void lstSocios_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (lstSocios.SelectedIndex < 0)
        {
            return;
        }

        // La lista tiene textos: el socio se busca por la posición en _visibles
        _editado = _visibles[lstSocios.SelectedIndex];

        _cargando = true;
        txtTituloFicha.Text = "Editar socio";
        txtNombre.Text = _editado.Nombre;
        cmbCategoria.SelectedIndex = _editado.Categoria switch
        {
            "Infantil" => 0,
            "Senior" => 2,
            _ => 1
        };
        txtAntiguedad.Text = _editado.Antiguedad.ToString();
        _cargando = false;

        MostrarCuota();
        ActualizarBotones();
    }

    // --- Formulario ---

    private void Formulario_Changed(object sender, RoutedEventArgs e)
    {
        // Durante InitializeComponent los controles de más abajo aún no existen
        if (txtCuota is null || _cargando)
        {
            return;
        }

        MostrarCuota();
        ActualizarBotones();
    }

    private void MostrarCuota()
    {
        if (!int.TryParse(txtAntiguedad.Text, out var antiguedad) || antiguedad < 0)
        {
            txtCuota.Text = "Cuota: —";
            return;
        }

        var categoria = ((ComboBoxItem)cmbCategoria.SelectedItem).Content.ToString();
        decimal cuota = categoria switch
        {
            "Infantil" => 15m,
            "Senior" => 25m,
            _ => 35m
        };
        if (antiguedad >= 10)
        {
            cuota *= 0.9m; // 10 % de descuento a partir de 10 años
        }

        txtCuota.Text = $"Cuota: {cuota:0.00} € al mes";
    }

    private void LimpiarFormulario()
    {
        _editado = null;
        _cargando = true;
        txtTituloFicha.Text = "Nuevo socio";
        txtNombre.Text = "";
        cmbCategoria.SelectedIndex = 1;
        txtAntiguedad.Text = "0";
        lstSocios.SelectedIndex = -1;
        _cargando = false;
        MostrarCuota();
    }

    private void ActualizarBotones()
    {
        btnGuardar.IsEnabled = txtNombre.Text.Trim().Length > 0;
        btnEliminar.IsEnabled = _editado is not null;
    }

    private void ActualizarResumen()
    {
        decimal total = 0;
        foreach (var socio in _socios)
        {
            // Mismo cálculo que en MostrarCuota, copiado aquí
            decimal cuota = socio.Categoria switch
            {
                "Infantil" => 15m,
                "Senior" => 25m,
                _ => 35m
            };
            if (socio.Antiguedad >= 10)
            {
                cuota *= 0.9m;
            }

            total += cuota;
        }

        txtResumen.Text = $"{_socios.Count} socios · Cuotas: {total:0.00} € al mes";
        Title = _hayCambios ? "*Socios del club" : "Socios del club";
    }

    // --- Botones ---

    private void btnNuevo_Click(object sender, RoutedEventArgs e)
    {
        LimpiarFormulario();
        ActualizarBotones();
        txtNombre.Focus();
    }

    private void btnGuardar_Click(object sender, RoutedEventArgs e)
    {
        var nombre = txtNombre.Text.Trim();
        if (nombre.Length == 0)
        {
            MessageBox.Show("Escribe el nombre del socio.", "Falta un dato", MessageBoxButton.OK, MessageBoxImage.Warning);
            txtNombre.Focus();
            return;
        }

        if (!int.TryParse(txtAntiguedad.Text, out var antiguedad) || antiguedad < 0 || antiguedad > 100)
        {
            MessageBox.Show("La antigüedad debe ser un número de años entre 0 y 100.", "Dato incorrecto", MessageBoxButton.OK, MessageBoxImage.Warning);
            txtAntiguedad.Focus();
            return;
        }

        if (_socios.Any(s => s != _editado && s.Nombre.Equals(nombre, StringComparison.CurrentCultureIgnoreCase)))
        {
            MessageBox.Show("Ya hay un socio con ese nombre.", "Socio repetido", MessageBoxButton.OK, MessageBoxImage.Warning);
            txtNombre.Focus();
            return;
        }

        var socio = _editado ?? new Socio();
        socio.Nombre = nombre;
        socio.Categoria = ((ComboBoxItem)cmbCategoria.SelectedItem).Content.ToString()!;
        socio.Antiguedad = antiguedad;
        if (_editado is null)
        {
            _socios.Add(socio);
        }

        _hayCambios = true;
        GuardarFichero();
        RellenarLista();
        LimpiarFormulario();
        ActualizarResumen();
        ActualizarBotones();
    }

    private void btnEliminar_Click(object sender, RoutedEventArgs e)
    {
        if (_editado is null)
        {
            return;
        }

        var respuesta = MessageBox.Show(
            $"¿Seguro que quieres eliminar a {_editado.Nombre}?",
            "Eliminar socio",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning,
            MessageBoxResult.No);
        if (respuesta != MessageBoxResult.Yes)
        {
            return;
        }

        _socios.Remove(_editado);
        _hayCambios = true;
        GuardarFichero();
        RellenarLista();
        LimpiarFormulario();
        ActualizarResumen();
        ActualizarBotones();
    }

    // --- Fichero ---

    private void CargarFichero()
    {
        if (!File.Exists(Fichero))
        {
            _socios.Add(new Socio { Nombre = "Lucía Pérez", Categoria = "Adulto", Antiguedad = 12 });
            _socios.Add(new Socio { Nombre = "Mario Gil", Categoria = "Infantil", Antiguedad = 2 });
            _socios.Add(new Socio { Nombre = "Carmen Ruiz", Categoria = "Senior", Antiguedad = 25 });
            return;
        }

        try
        {
            foreach (var linea in File.ReadAllLines(Fichero))
            {
                var partes = linea.Split(';');
                if (partes.Length == 3 && int.TryParse(partes[2], out var antiguedad))
                {
                    _socios.Add(new Socio { Nombre = partes[0], Categoria = partes[1], Antiguedad = antiguedad });
                }
            }
        }
        catch (IOException ex)
        {
            MessageBox.Show($"No se han podido leer los socios: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void GuardarFichero()
    {
        try
        {
            File.WriteAllLines(Fichero, _socios.Select(s => $"{s.Nombre};{s.Categoria};{s.Antiguedad}"));
            _hayCambios = false;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            MessageBox.Show($"No se han podido guardar los socios: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Window_Closing(object? sender, CancelEventArgs e)
    {
        if (_hayCambios)
        {
            GuardarFichero();
        }
    }
}
