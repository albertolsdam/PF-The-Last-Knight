using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClaseBase
{
    //Icono para los turnos
    public Sprite icon;
    public Sprite spriteCompleto;

    //Nombre del enemigo
    public string nombre;

    //Stats [vida, mana]
    public int vidaBase;
    public int vidaActual;

    public int manaBase;
    public int manaActual;

    public int fuerza, inteligencia, velocidad, resistenciaF, resistenciaM;

    public AtaqueBase ataque;
}
