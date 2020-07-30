using UnityEngine;

[CreateAssetMenu(fileName = "Nuevo Ataque", menuName = "Ataque")]
public class AtaqueBase : ScriptableObject
{
    public enum TipoAtaque
    {
        FISICO,
        MAGICO,
        CURA
    }

    public enum ElementoAtaque
    {
        FUEGO,
        AGUA,
        PLANTA,
        ELECTRICO,
        OSCURIDAD,
        LUZ,
        NEUTRAL
    }

    public enum Rango
    {
        CORTO,
        MEDIO,
        LARGO
    }


    public string nombreAtaque;
    public int damage;
    public string descripcion;
    public int coste;
    public TipoAtaque tipoAtaque;
    public Sprite icon;
    public ElementoAtaque elementoAtaque;

    public Rango rango;
    public GameObject prefab;
    public bool seMueve;
}
