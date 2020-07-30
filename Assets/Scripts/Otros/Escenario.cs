using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Escenario
{
    public Sprite fondo;
    public string nombreMapa;
    public List<string> escenasEnemigos;
    public List<string> escenasEnemigosElites;
    public List<string> escenasTesoros;
    public List<string> escenasBosses;
    public string escenaHoguera;
}
