using System;
using System.Collections.Generic;

namespace esBattagliaNavale;

public class CNave
{
    public int Lunghezza { get; }
    public List<CCella> Celle { get; }

    public CNave(int lunghezza)
    {
        if (lunghezza <= 0)
        {
            throw new ArgumentException("Lunghezza nave non valida");
        }

        Lunghezza = lunghezza;
        Celle = new List<CCella>();
    }
    public bool Affondata
    {
        get
        {
            if (Celle.Count == 0)
            {
                return false;
            }

            foreach (var c in Celle)
            {
                if (!c.Colpita)
                {
                    return false;
                }
            }

            return true;
        }
    }

    public override string ToString()
    {
        return $"Nave da {Lunghezza} celle";
    }
}