using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

namespace LaboratorioAsincrono;

/// <summary>
/// Lógica de interacción para MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly DispatcherTimer _reloj = new() { Interval = TimeSpan.FromMilliseconds(100) };
    private readonly Stopwatch _cronometro = Stopwatch.StartNew();
    private CancellationTokenSource? _cancelacion;

    public MainWindow()
    {
        InitializeComponent();

        // El temporizador se ejecuta en el hilo de la interfaz: solo avanza si ese hilo está libre
        _reloj.Tick += (_, _) => txtReloj.Text = _cronometro.Elapsed.ToString(@"mm\:ss\.f");
        _reloj.Start();
    }

    /// <summary>Un cálculo que tarda unos segundos (simulado).</summary>
    private static long CalculoLento()
    {
        Thread.Sleep(3000);
        return 42;
    }

    // 1. El manejador hace el trabajo en el hilo de la interfaz: la ventana se congela
    private void btnBloquear_Click(object sender, RoutedEventArgs e)
    {
        txtResultado.Text = "Calculando…"; // no llega a verse: la ventana no se redibuja hasta el final
        var resultado = CalculoLento();
        txtResultado.Text = $"Resultado: {resultado}. ¿Se ha parado el reloj?";
    }

    // 2. El trabajo va a otro hilo y el manejador espera sin bloquear
    private async void btnAsincrono_Click(object sender, RoutedEventArgs e)
    {
        btnAsincrono.IsEnabled = false;
        txtResultado.Text = "Calculando…";

        var resultado = await Task.Run(CalculoLento);

        // Después del await se vuelve al hilo de la interfaz: se pueden tocar los controles
        txtResultado.Text = $"Resultado: {resultado}. El reloj no se ha parado.";
        btnAsincrono.IsEnabled = true;
    }

    // 3. Un control solo se puede tocar desde el hilo que lo creó
    private async void btnOtroHilo_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            await Task.Run(() => txtResultado.Text = "Esto no se llega a escribir");
        }
        catch (InvalidOperationException ex)
        {
            txtResultado.Text = $"Excepción: {ex.Message}";
        }
    }

    // 4. Desde otro hilo, se pide al Dispatcher que ejecute el cambio en el hilo de la interfaz
    private async void btnDispatcher_Click(object sender, RoutedEventArgs e)
    {
        await Task.Run(() =>
        {
            var hilo = Environment.CurrentManagedThreadId;
            Dispatcher.Invoke(() => txtResultado.Text = $"Escrito desde el hilo {hilo} a través del Dispatcher.");
        });
    }

    // 5. Operación asíncrona con progreso y cancelación
    private async void btnDescargar_Click(object sender, RoutedEventArgs e)
    {
        _cancelacion = new CancellationTokenSource();
        btnDescargar.IsEnabled = false;
        btnCancelar.IsEnabled = true;

        // Progress<T> llama a su acción en el hilo donde se creó: aquí, el de la interfaz
        var progreso = new Progress<int>(porcentaje => prgDescarga.Value = porcentaje);

        try
        {
            await DescargarAsync(progreso, _cancelacion.Token);
            txtResultado.Text = "Descarga terminada.";
        }
        catch (OperationCanceledException)
        {
            txtResultado.Text = "Descarga cancelada.";
            prgDescarga.Value = 0;
        }
        finally
        {
            btnDescargar.IsEnabled = true;
            btnCancelar.IsEnabled = false;
            _cancelacion.Dispose();
            _cancelacion = null;
        }
    }

    private void btnCancelar_Click(object sender, RoutedEventArgs e)
    {
        _cancelacion?.Cancel();
    }

    /// <summary>Simula una descarga de 20 bloques que avisa de su progreso.</summary>
    private static async Task DescargarAsync(IProgress<int> progreso, CancellationToken cancelacion)
    {
        for (var bloque = 1; bloque <= 20; bloque++)
        {
            await Task.Delay(150, cancelacion);
            progreso.Report(bloque * 5);
        }
    }
}
