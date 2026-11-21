using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Microsoft.VisualBasic.CompilerServices;

namespace esBattagliaNavale.View;

public partial class Inserimento : UserControl
{
    private readonly Button[,] bottoni = new Button[CCampo.Dim, CCampo.Dim];
    private readonly CCampo campo;

    public Inserimento(CCampo campo)
    {
        InitializeComponent();
        this.campo = campo;

        for (int x = 0; x < CCampo.Dim; x++)
        {
            for (int y = 0; y < CCampo.Dim; y++)
            {
                Button b = new Button();
                b.Background = Brushes.LightSteelBlue;
                b.HorizontalAlignment = HorizontalAlignment.Stretch;
                b.VerticalAlignment = VerticalAlignment.Stretch;
                b.Click += Cella_Click;
                Grid.SetRow(b, y);
                Grid.SetColumn(b, x);
                GrdCampo.Children.Add(b);
                bottoni[x, y] = b;
            }
        }
    }

    private void Cella_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button b)
        {
            try
            {
                TxtErrore.Text = "";
                int x = Grid.GetColumn(b);
                int y = Grid.GetRow(b);
                int lung = campo.ProssimaNave;

                campo.Posiziona(x, y);
                ColoraNave(x, y, lung);
                AggiornaTesti();
            }
            catch (Exception ex)
            {
                TxtErrore.Text = ex.Message;
            }
        }
    }

    private void CambiaOrientamento(object? sender, RoutedEventArgs e)
    {
        TxtErrore.Text = "";
        campo.CambiaOrientamento();
        AggiornaTesti();
    }

    private void Ricomincia(object? sender, RoutedEventArgs e)
    {
        TxtErrore.Text = "";
        campo.Reset();

        foreach (var b in bottoni)
        {
            b.Background = Brushes.LightSteelBlue;
        }

        AggiornaTesti();
    }

    public void ColoraNave(int x, int y, int lunghezza)
    {
        for (int i = 0; i < lunghezza; i++)
        {
            if (campo.Orizzontale)
            {
                bottoni[x + i, y].Background = Brushes.SteelBlue;
            }
            else
            {
                bottoni[x, y + i].Background = Brushes.SteelBlue;
            }
        }
    }

    public void AggiornaTesti()
    {
        TxtErrore.Text = "";
        if (campo.FlottaCompleta)
        {
            TxtProssima.Text = "Flotta completa! Premi GIOCA per iniziare";
        }
        else
        {
            TxtProssima.Text = $"Prossima nave: da {campo.ProssimaNave} celle";
        }

        if (campo.Orizzontale)
        {
            BtnOrienta.Content = "Orizzontale";
        }
        else
        {
            BtnOrienta.Content = "Verticale";
        }
    }
}