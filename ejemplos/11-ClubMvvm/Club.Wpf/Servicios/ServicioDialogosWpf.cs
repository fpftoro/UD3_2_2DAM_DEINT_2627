using System.Windows;
using Club.Nucleo.Servicios;

namespace Club.Wpf.Servicios;

/// <summary>
/// Implementación de <see cref="IServicioDialogos"/> con <see cref="MessageBox"/>.
/// </summary>
public class ServicioDialogosWpf : IServicioDialogos
{
    /// <inheritdoc />
    public bool Confirmar(string mensaje, string titulo) =>
        MessageBox.Show(
            Application.Current.MainWindow,
            mensaje,
            titulo,
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning,
            MessageBoxResult.No) == MessageBoxResult.Yes;

    /// <inheritdoc />
    public void MostrarError(string mensaje, string titulo) =>
        MessageBox.Show(Application.Current.MainWindow, mensaje, titulo, MessageBoxButton.OK, MessageBoxImage.Error);
}
