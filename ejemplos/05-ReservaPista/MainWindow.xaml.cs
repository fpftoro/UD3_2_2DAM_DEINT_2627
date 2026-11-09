using System.Windows;
using System.Windows.Controls;

namespace ReservaPista;

/// <summary>
/// Lógica de interacción para MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        dgSocios.ItemsSource = new List<Socio>
        {
            new("Lucía Pérez", "Adulto", 4),
            new("Mateo Ruiz", "Infantil", 1),
            new("Carmen Gil", "Senior", 12)
        };

        // El foco inicial se pone cuando la ventana ya está cargada
        Loaded += (_, _) => txtSocio.Focus();
    }

    private void btnReservar_Click(object sender, RoutedEventArgs e)
    {
        var socio = txtSocio.Text.Trim();
        if (socio.Length == 0)
        {
            MessageBox.Show("Escribe el nombre del socio.", "Falta un dato", MessageBoxButton.OK, MessageBoxImage.Warning);
            txtSocio.Focus();
            return;
        }

        if (dtpFecha.SelectedDate is not { } fecha)
        {
            MessageBox.Show("Elige la fecha de la reserva.", "Falta un dato", MessageBoxButton.OK, MessageBoxImage.Warning);
            dtpFecha.Focus();
            return;
        }

        if (pwdPin.Password.Length is > 0 and < 4)
        {
            MessageBox.Show("El PIN debe tener 4 cifras.", "PIN incompleto", MessageBoxButton.OK, MessageBoxImage.Warning);
            pwdPin.Focus();
            return;
        }

        var pista = ((ComboBoxItem)cmbPista.SelectedItem).Content;
        var tipo = rbIndividual.IsChecked == true ? "individual" : "dobles";

        var extras = new List<string>();
        if (chkRaqueta.IsChecked == true)
        {
            extras.Add("raqueta");
        }

        if (chkLuz.IsChecked == true)
        {
            extras.Add("luz");
        }

        var linea = $"{fecha:dd/MM/yyyy} · {pista} · {sldDuracion.Value:0} min · {tipo} · {socio}";
        if (extras.Count > 0)
        {
            linea += $" · extras: {string.Join(", ", extras)}";
        }

        lstReservas.Items.Add(linea);
        txtEstado.Text = $"Reserva de {socio} añadida.";
    }

    private void btnNueva_Click(object sender, RoutedEventArgs e)
    {
        txtSocio.Clear();
        cmbPista.SelectedIndex = 0;
        dtpFecha.SelectedDate = null;
        sldDuracion.Value = 60;
        rbIndividual.IsChecked = true;
        chkTodos.IsChecked = false;
        chkRaqueta.IsChecked = false;
        chkLuz.IsChecked = false;
        pwdPin.Clear();
        txtEstado.Text = "Formulario vacío.";
        txtSocio.Focus();
    }

    private void mnuSalir_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void mnuAcercaDe_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Reserva de pistas · ejemplo del tema 5", "Acerca de", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void sldDuracion_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        // Este evento se lanza ya durante InitializeComponent, antes de que exista txtDuracion
        if (txtDuracion is null)
        {
            return;
        }

        txtDuracion.Text = $"{e.NewValue:0} min";
    }

    private void chkTodos_Click(object sender, RoutedEventArgs e)
    {
        var marcado = chkTodos.IsChecked == true;
        chkRaqueta.IsChecked = marcado;
        chkLuz.IsChecked = marcado;
    }

    private void chkExtra_Click(object sender, RoutedEventArgs e)
    {
        chkTodos.IsChecked = (chkRaqueta.IsChecked, chkLuz.IsChecked) switch
        {
            (true, true) => true,
            (false, false) => false,
            _ => null
        };
    }
}
