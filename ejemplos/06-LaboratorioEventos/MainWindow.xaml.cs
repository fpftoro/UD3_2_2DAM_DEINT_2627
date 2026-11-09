using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LaboratorioEventos;

/// <summary>
/// Lógica de interacción para MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // Suscribirse desde código es equivalente a escribir el atributo en el XAML
        bordeInterior.PreviewMouseDown += BordeInterior_DetenerSiProcede;
    }

    // --- 1. Eventos enrutados ---

    private void Rastro(object sender, MouseButtonEventArgs e)
    {
        var receptor = ((FrameworkElement)sender).Name;
        var origen = (e.OriginalSource as FrameworkElement)?.Name;

        lstRastro.Items.Add($"{e.RoutedEvent.Name} en {receptor} (origen: {origen})");
    }

    private void BordeInterior_DetenerSiProcede(object sender, MouseButtonEventArgs e)
    {
        if (chkDetener.IsChecked == true)
        {
            e.Handled = true;
        }
    }

    private void Limpiar_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        lstRastro.Items.Clear();
    }

    // --- 2. Un manejador para varios controles ---

    private void Categoria_Click(object sender, RoutedEventArgs e)
    {
        if (e.OriginalSource is not Button boton)
        {
            return;
        }

        txtCuota.Text = $"{boton.Content}: {boton.Tag} € al mes";
    }

    // --- 3. Teclado ---

    private void txtSocios_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        // Si el texto escrito no son solo cifras, se descarta antes de que llegue al cuadro
        e.Handled = !e.Text.All(char.IsDigit);
    }

    // --- 4. Ratón ---

    private void cnvPista_MouseMove(object sender, MouseEventArgs e)
    {
        var posicion = e.GetPosition(cnvPista);
        txtPosicion.Text = $"x = {posicion.X:0}, y = {posicion.Y:0}";
    }

    private void cnvPista_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        var posicion = e.GetPosition(cnvPista);
        var punto = new Ellipse { Width = 16, Height = 16, Fill = Brushes.SteelBlue };

        Canvas.SetLeft(punto, posicion.X - 8);
        Canvas.SetTop(punto, posicion.Y - 8);
        cnvPista.Children.Add(punto);
    }

    private void cnvPista_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        cnvPista.Children.Clear();
    }
}
