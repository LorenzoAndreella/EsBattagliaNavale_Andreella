using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;

namespace esBattagliaNavale.View;

public partial class Gioca : UserControl
{
    private readonly Button[,] bottoni = new Button[CCampo.Dim, CCampo.Dim];
    private readonly CCampo campo;
    private readonly List<string> log = new List<string>();
    private int mosse;

    public Gioca(CCampo campo)
    {
        InitializeComponent();
        this.campo = campo;
        LstLog.ItemsSource = new List<string>(log);

        for (int x = 0; x < CCampo.Dim; x++)
        {
            for (int y = 0; y < CCampo.Dim; y++)
            {
                Button b = new Button();
                b.Background = Brushes.LightSteelBlue;
                b.HorizontalAlignment = HorizontalAlignment.Stretch;
                b.VerticalAlignment = VerticalAlignment.Stretch;
                b.HorizontalContentAlignment = HorizontalAlignment.Center;
                b.VerticalContentAlignment = VerticalAlignment.Center;
                b.Click += Cella_Click;
                Grid.SetRow(b, y);
                Grid.SetColumn(b, x);
                GrdCampo.Children.Add(b);
                bottoni[x, y] = b;

            }
        }
    }

    public void Inizializza()
    {
        mosse = 0;
        foreach (var c in campo.Celle)
        {
            c.Sparata -= CellaNave;
            c.Sparata -= CellaAcqua;
            
            if (c.HaNave)
            {
                c.Sparata += CellaNave;
            }
            else
            {
                c.Sparata += CellaAcqua;
            }
            if(c.Colpita)
            {
                mosse++;
            }

            ColoraCella(c);
        }

        if (mosse == 0)
        {
            log.Clear();
        }

        TxtErrore.Text = "";
        AggiornaTesti();
    }

    private void Cella_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button b)
        {
            TxtErrore.Text = "";

            if (campo.NaviNonAffondate == 0)
            {
                TxtErrore.Text = $"Hai vinto in {mosse} mosse!";
                return;
            }

            try
            {
                campo.Celle[Grid.GetColumn(b), Grid.GetRow(b)].Spara();
            }
            catch (Exception ex)
            {
                TxtErrore.Text = ex.Message;
            }
        }
    }

    private void CellaNave(CCella cella)
    {
        TxtErrore.Text = "";
        mosse++;
        log.Add($"Colpito in posizione {cella.ToString()}");
        ColoraCella(cella);

        if (cella.Nave != null && cella.Nave.Affondata)
        {
            log.Add($"Affondata nave di lunghezza {cella.Nave.Lunghezza} celle");
            foreach (var c in cella.Nave.Celle)
            {
                ColoraCella(c);
            }
        }

        if (campo.NaviNonAffondate == 0)
        {
            TxtErrore.Text = $"Partita terminata, hai vinto in {mosse} mosse!";
        }

        AggiornaTesti();
    }

    private void CellaAcqua(CCella cella)
    {
        mosse++;
        log.Add($"Acqua !!");
        ColoraCella(cella);
        AggiornaTesti();
    }

    public void ColoraCella(CCella cella)
    {
        Button b = bottoni[cella.X, cella.Y];

        if (!cella.Colpita)
        {
            b.Background = Brushes.LightSteelBlue;
            b.Content = "";
        }
        else if (cella.HaNave)
        {
            if (cella.Nave != null && cella.Nave.Affondata)
            {
                b.Background = Brushes.DarkRed;
            }
            else
            {
                b.Background = Brushes.OrangeRed;
            }

            b.Content = "X";
        }
        else
        {
            b.Background = Brushes.White;
            b.Content = "-";
        }
    }

    private void AggiornaTesti()
    {
        TxtMosse.Text = $"Mosse: {mosse}";
        TxtNavi.Text = $"Navi non affondate: {campo.NaviNonAffondate}";
        LstLog.ItemsSource = new List<string>(log);

        if (log.Count > 0)
        {
            LstLog.ScrollIntoView(log.Count-1);
        }
    }
}