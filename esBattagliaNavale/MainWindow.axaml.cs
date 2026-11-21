using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using esBattagliaNavale.View;

namespace esBattagliaNavale;

public partial class MainWindow : Window
{
    private readonly CCampo campo = new CCampo();
    private readonly Inserimento inserimentoView;
    private readonly Gioca giocaView;

    public MainWindow()
    {
        InitializeComponent();

        inserimentoView = new Inserimento(campo);
        giocaView = new Gioca(campo);

        CntView.Content = inserimentoView;
    }

    private void ShowGiocaView(object? sender, RoutedEventArgs e)
    {
        TxtErrore.Text = "";
        if (!campo.FlottaCompleta)
        {
            TxtErrore.Text = "Non hai posizionato tutte le navi!";
            return;
        }

        giocaView.Inizializza();
        CntView.Content = giocaView;
    }

    private void ShowInserimentoView(object? sender, RoutedEventArgs e)
    {
        TxtErrore.Text = "";
        if (CntView.Content is Gioca && campo.NaviNonAffondate > 0)
        {
            TxtErrore.Text = "Non puoi tornare indietro, hai già iniziato a giocare!";
            return;
        }
        CntView.Content = inserimentoView;
    }
}