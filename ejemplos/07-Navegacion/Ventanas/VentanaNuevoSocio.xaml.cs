using System.Windows;
using System.Windows.Controls;
using Navegacion.Modelos;

namespace Navegacion.Ventanas;

/// <summary>
/// Diálogo modal para dar de alta un socio.
/// </summary>
public partial class VentanaNuevoSocio : Window
{
    public VentanaNuevoSocio()
    {
        InitializeComponent();
        Loaded += (_, _) => txtNombre.Focus();
    }

    /// <summary>
    /// El socio creado, o <c>null</c> si el usuario ha cancelado.
    /// </summary>
    public Socio? SocioCreado { get; private set; }

    private void btnAceptar_Click(object sender, RoutedEventArgs e)
    {
        var nombre = txtNombre.Text.Trim();
        if (nombre.Length == 0)
        {
            MessageBox.Show(this, "Escribe el nombre del socio.", "Falta un dato", MessageBoxButton.OK, MessageBoxImage.Warning);
            txtNombre.Focus();
            return;
        }

        if (!int.TryParse(txtAntiguedad.Text, out var antiguedad) || antiguedad < 0)
        {
            MessageBox.Show(this, "La antigüedad debe ser un número entero de años, 0 o más.", "Dato incorrecto", MessageBoxButton.OK, MessageBoxImage.Warning);
            txtAntiguedad.Focus();
            txtAntiguedad.SelectAll();
            return;
        }

        SocioCreado = new Socio
        {
            Nombre = nombre,
            Categoria = (string)((ComboBoxItem)cmbCategoria.SelectedItem).Content,
            Antiguedad = antiguedad
        };

        // Asignar DialogResult cierra la ventana y es lo que devuelve ShowDialog
        DialogResult = true;
    }
}
