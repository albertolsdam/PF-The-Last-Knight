using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class HeroeBase : ClaseBase
{
    public int nivel;
    public List<AtaqueBase> magias;

    public List<string> frasesVictoria;
    public List<string> frasesDerrota;

    public float cantidadXP = 0f;

    public List<int> listaAumentos;
}