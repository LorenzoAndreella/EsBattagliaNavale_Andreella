using System;
using System.Collections.Generic;

namespace esBattagliaNavale;

public class CCampo
{
    public const int Dim = 10;
    public static readonly int[] Flotta = { 4, 3, 3, 2, 2, 1 };

    private int naviPosizionate = 0;
    public CCella[,] Celle { get; }
    public List<CNave> Navi { get; }
    public bool Orizzontale { get; set; } = true;

    public CCampo()
    {
        Celle = new CCella[Dim, Dim];
        Navi = new List<CNave>();

        for (int x = 0; x < Dim; x++)
        {
            for (int y = 0; y < Dim; y++)
            {
                Celle[x, y] = new CCella(x, y);
            }
        }
    }

    public bool FlottaCompleta
    {
        get
        {
            if (naviPosizionate == Flotta.Length)
            {
                return true;
            }

            return false;
        }
    }

    public int ProssimaNave
    {
        get
        {
            if (FlottaCompleta)
            {
                return 0;
            }

            return Flotta[naviPosizionate];
        }
    }

    public int NaviNonAffondate
    {
        get
        {
            int n = 0;
            foreach (var nave in Navi)
            {
                if (!nave.Affondata)
                {
                    n++;
                }
            }

            return n;
        }
    }

    public bool CambiaOrientamento()
    {
        Orizzontale = !Orizzontale;
        return Orizzontale;
    }

    public void Reset()
    {
        foreach (var c in Celle)
        {
            c.Nave = null;
            c.Colpita = false;
        }

        naviPosizionate = 0;
        Navi.Clear();
    }

    public bool PuoPosizionare(int x, int y, int lunghezza)
    {
        int xUltima = x;
        int yUltima = y;
        if (Orizzontale)
        {
            xUltima = x + lunghezza - 1;
        }
        else
        {
            yUltima = y + lunghezza - 1;
        }

        if (x < 0 || y < 0 || xUltima >= Dim || yUltima >= Dim)
        {
            return false;
        }

        for (int i = x - 1; i <= xUltima + 1; i++)
        {
            for (int j = y - 1; j <= yUltima + 1; j++)
            {
                if (i >= 0 && i < Dim && j >= 0 && j < Dim && Celle[i, j].HaNave)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void Posiziona(int x, int y)
    {
        if (FlottaCompleta)
        {
            throw new ArgumentException("Flotta completa");
        }

        int lunghezza = ProssimaNave;

        if (!PuoPosizionare(x, y, lunghezza))
        {
            throw new ArgumentException("Posizione non valida");
        }

        CNave nave = new CNave(lunghezza);
        for (int i = 0; i < lunghezza; i++)
        {
            CCella c;
            if (Orizzontale)
            {
                c = Celle[x + i, y];
            }
            else
            {
                c = Celle[x, y + i];
            }

            c.Nave = nave;
            nave.Celle.Add(c);
        }

        Navi.Add(nave);
        naviPosizionate++;
    }
}
