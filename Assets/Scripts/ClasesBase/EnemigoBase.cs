using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemigoBase : ClaseBase
{
    //Tipos de elementos
    public enum Elemento
    {
        FUEGO,
        AGUA,
        PLANTA,
        ELECTRICO,
        OSCURIDAD,
        LUZ,
        NEUTRAL
    }

    //Elemento del enemigo
    public Elemento elemento;
}
