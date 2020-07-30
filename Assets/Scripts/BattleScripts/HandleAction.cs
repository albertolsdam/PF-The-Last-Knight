using UnityEngine;

[System.Serializable]
public class HandleAction
{
    public enum TipoAtacante
    {
        ENEMIGO,
        PERSONAJE
    }

    //Quien ataca
    public GameObject atacanteGameObj;
    public string atacante;
    public TipoAtacante tipoAtacante;
    public int damage;

    //Quien es atacado;
    public GameObject objetivoGameObj;

    public AtaqueBase ataque;
}
