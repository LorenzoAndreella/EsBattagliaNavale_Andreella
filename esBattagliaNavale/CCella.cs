using System;

namespace esBattagliaNavale;

public delegate void DCella(CCella cella);

public class CCella
{
    public int X { get; }
    public int Y { get; }
    public CNave? Nave { get; set; }
    public bool Colpita { get; set; }

    public event DCella? Sparata;
    public bool HaNave
    {
        get
        {
            if (Nave == null)
            {
                return false;
            }

            return true;
        }
    }

    public CCella(int x, int y)
    {
        X = x;
        Y = y;
        Nave = null;
        Colpita = false;
    }

    public void Spara()
    {
        if (Colpita)
        {
            throw new ArgumentException("Cella già colpita");
        }

        Colpita = true;

        Sparata?.Invoke(this);
    }

    public override string ToString()
    {
        return $"({X + 1},{Y + 1})";
    }
}