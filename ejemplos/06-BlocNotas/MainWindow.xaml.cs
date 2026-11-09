using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;

namespace BlocNotas;

/// <summary>
/// Lógica de interacción para MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private const string NombreAplicacion = "Bloc de notas";

    private string? _ruta;
    private bool _modificado;

    public MainWindow()
    {
        InitializeComponent();
        ActualizarTitulo();
    }

    // --- Eventos del cuadro de texto ---

    private void txtContenido_TextChanged(object sender, TextChangedEventArgs e)
    {
        _modificado = true;
        ActualizarTitulo();
        txtCaracteres.Text = $"{txtContenido.Text.Length} caracteres";
    }

    private void txtContenido_SelectionChanged(object sender, RoutedEventArgs e)
    {
        var linea = txtContenido.GetLineIndexFromCharacterIndex(txtContenido.CaretIndex);
        if (linea < 0)
        {
            return;
        }

        var columna = txtContenido.CaretIndex - txtContenido.GetCharacterIndexFromLineIndex(linea);
        txtPosicion.Text = $"Línea {linea + 1}, columna {columna + 1}";
    }

    private void txtContenido_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key != Key.F5)
        {
            return;
        }

        // F5 inserta la fecha y la hora en la posición del cursor
        var texto = DateTime.Now.ToString("HH:mm dd/MM/yyyy");
        var inicio = txtContenido.SelectionStart;
        txtContenido.SelectedText = texto;
        txtContenido.CaretIndex = inicio + texto.Length;
        e.Handled = true;
    }

    private void txtContenido_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        // Ctrl + rueda cambia el tamaño de la letra; sin Ctrl, la rueda desplaza el texto como siempre
        if (Keyboard.Modifiers != ModifierKeys.Control)
        {
            return;
        }

        var tamano = txtContenido.FontSize + (e.Delta > 0 ? 1 : -1);
        txtContenido.FontSize = Math.Clamp(tamano, 8, 48);
        e.Handled = true;
    }

    // --- Comandos de Archivo ---

    private void Nuevo_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (!ConfirmarCambios())
        {
            return;
        }

        txtContenido.Clear();
        _ruta = null;
        _modificado = false;
        ActualizarTitulo();
    }

    private void Abrir_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (!ConfirmarCambios())
        {
            return;
        }

        var dialogo = new OpenFileDialog { Filter = "Texto (*.txt)|*.txt|Todos los ficheros (*.*)|*.*" };
        if (dialogo.ShowDialog(this) != true)
        {
            return;
        }

        try
        {
            txtContenido.Text = File.ReadAllText(dialogo.FileName);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            MostrarError("No se ha podido abrir el fichero.", ex);
            return;
        }

        // Asignar Text lanza TextChanged, que marca el documento como modificado: se corrige después
        _ruta = dialogo.FileName;
        _modificado = false;
        ActualizarTitulo();
    }

    private void Guardar_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        Guardar();
    }

    private void Guardar_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = _modificado;
    }

    private void mnuSalir_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    // --- Cierre de la ventana ---

    private void Window_Closing(object? sender, CancelEventArgs e)
    {
        if (!ConfirmarCambios())
        {
            e.Cancel = true;
        }
    }

    // --- Apoyo ---

    /// <summary>
    /// Si hay cambios sin guardar, pregunta qué hacer con ellos.
    /// </summary>
    /// <returns><see langword="true"/> si se puede continuar; <see langword="false"/> si el usuario cancela.</returns>
    private bool ConfirmarCambios()
    {
        if (!_modificado)
        {
            return true;
        }

        var respuesta = MessageBox.Show(
            "Hay cambios sin guardar. ¿Quieres guardarlos?",
            NombreAplicacion,
            MessageBoxButton.YesNoCancel,
            MessageBoxImage.Question,
            MessageBoxResult.Cancel);

        return respuesta switch
        {
            MessageBoxResult.Yes => Guardar(),
            MessageBoxResult.No => true,
            _ => false
        };
    }

    private bool Guardar()
    {
        if (_ruta is not null)
        {
            return Escribir(_ruta);
        }

        var dialogo = new SaveFileDialog
        {
            Filter = "Texto (*.txt)|*.txt|Todos los ficheros (*.*)|*.*",
            DefaultExt = ".txt"
        };

        return dialogo.ShowDialog(this) == true && Escribir(dialogo.FileName);
    }

    private bool Escribir(string ruta)
    {
        try
        {
            File.WriteAllText(ruta, txtContenido.Text);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            MostrarError("No se ha podido guardar el fichero.", ex);
            return false;
        }

        _ruta = ruta;
        _modificado = false;
        ActualizarTitulo();
        return true;
    }

    private void MostrarError(string mensaje, Exception ex)
    {
        MessageBox.Show($"{mensaje}\n\n{ex.Message}", NombreAplicacion, MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private void ActualizarTitulo()
    {
        var nombre = _ruta is null ? "Sin título" : Path.GetFileName(_ruta);
        Title = $"{(_modificado ? "*" : string.Empty)}{nombre} - {NombreAplicacion}";
    }
}
