using System.Windows;

namespace LaboratorioBinding;

/// <summary>
/// Lógica de interacción para MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly DatosLaboratorio _datos = new();

    public MainWindow()
    {
        InitializeComponent();

        // Todos los {Binding} sin ElementName buscan sus propiedades en este objeto
        DataContext = _datos;
    }

    private void btnAnadir_Click(object sender, RoutedEventArgs e)
    {
        _datos.AnadirExtra(new Extra("Agua", 1.5m));
        btnAnadir.IsEnabled = false;
    }
}
