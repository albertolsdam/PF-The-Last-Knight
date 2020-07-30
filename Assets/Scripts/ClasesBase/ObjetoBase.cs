using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Nuevo Objeto", menuName = "Objeto")]
public class ObjetoBase : ScriptableObject
{
    public enum TipoAccion
    {
        CURAR,
        MANA,
        HACERDMG,
        REVIVIR
    }

    public TipoAccion tipoAccion;

    public string nombreObjeto;
    public int cantidad;
    public string descripcion;
    public int cantidadEfecto;
    public Sprite icon;
    public GameObject prefab;
}
