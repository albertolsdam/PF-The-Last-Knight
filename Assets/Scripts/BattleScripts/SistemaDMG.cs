using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SistemaDMG : MonoBehaviour
{
    //Manejador de las estadisticas de los heroes
    public StatHandler statHandler;
    public SistemaMenus sistemaMenus;

    public GameObject prefabTextoDMG;

    [Header("Sprites DMG/VIDA/MANA")]
    public Sprite dmgSprite;
    public Sprite vidaSprite;
    public Sprite manaSprite;

    public void HacerDMG(HandleAction accion)
    {
        HeroeBase heroe;
        EnemigoBase enemigo;
        int daño = 0;
        AtaqueBase ataque = accion.ataque;

        float modificadorFuerza = 0.5f;
        float modificadorInteligencia = 0.5f;

        if (accion.atacanteGameObj.CompareTag("Player"))
        {
            heroe = accion.atacanteGameObj.GetComponent<HeroeStateMachine>().heroe;
            enemigo = accion.objetivoGameObj.GetComponent<EnemigoStateMachine>().enemigo;

            if (accion.ataque.tipoAtaque.Equals(AtaqueBase.TipoAtaque.FISICO))
                daño = Mathf.RoundToInt((((heroe.fuerza * modificadorFuerza) + accion.ataque.damage) - (enemigo.resistenciaF)));
            else if (accion.ataque.tipoAtaque.Equals(AtaqueBase.TipoAtaque.MAGICO))
            {
                daño = Mathf.RoundToInt((((heroe.inteligencia * modificadorInteligencia) + accion.ataque.damage) - (enemigo.resistenciaM)) * calcularModificador(ataque, enemigo));
            }

            if (daño < 0)
                daño = 0;

            accion.damage = daño;
            accion.objetivoGameObj.GetComponent<EnemigoStateMachine>().enemigo.vidaActual -= accion.damage;

            if (accion.objetivoGameObj.GetComponent<EnemigoStateMachine>().enemigo.vidaActual < 0)
                accion.objetivoGameObj.GetComponent<EnemigoStateMachine>().enemigo.vidaActual = 0;

            accion.objetivoGameObj.GetComponent<EnemigoStateMachine>().AnimacionRecibirDMG();
            statHandler.ActualizarInfo();
        }
        else if (accion.atacanteGameObj.CompareTag("Enemigo"))
        {
            enemigo = accion.atacanteGameObj.GetComponent<EnemigoStateMachine>().enemigo;
            heroe = accion.objetivoGameObj.GetComponent<HeroeStateMachine>().heroe;

            if (accion.ataque.tipoAtaque.Equals(AtaqueBase.TipoAtaque.FISICO))
                daño = Mathf.RoundToInt((((enemigo.fuerza * modificadorFuerza) + accion.ataque.damage) - (heroe.resistenciaF)));
            else if (accion.ataque.tipoAtaque.Equals(AtaqueBase.TipoAtaque.MAGICO))
            {
                daño = Mathf.RoundToInt((((enemigo.inteligencia * modificadorInteligencia) + accion.ataque.damage) - (heroe.resistenciaM)));
            }

            if (daño < 0)
                daño = 0;

            accion.damage = daño;
            accion.objetivoGameObj.GetComponent<HeroeStateMachine>().heroe.vidaActual -= accion.damage;

            if (accion.objetivoGameObj.GetComponent<HeroeStateMachine>().heroe.vidaActual < 0)
                accion.objetivoGameObj.GetComponent<HeroeStateMachine>().heroe.vidaActual = 0;

            accion.objetivoGameObj.GetComponent<HeroeStateMachine>().AnimacionRecibirDMG();
        }

        SpawnDMG(daño, accion.objetivoGameObj, "daño");
        sistemaMenus.sistemaBreak.IncrementarBarra(daño*0.015f);
    }

    public void HacerDMGaEnemigo(ObjetoBase objeto, GameObject objetivo)
    {
        objetivo.GetComponent<EnemigoStateMachine>().enemigo.vidaActual -= Mathf.RoundToInt(objeto.cantidadEfecto*0.75f);

        objetivo.GetComponent<EnemigoStateMachine>().AnimacionRecibirDMG();

        SpawnDMG(objeto.cantidadEfecto, objetivo, "daño");
    }

    private float calcularModificador(AtaqueBase ataque, EnemigoBase enemigo)
    {
        if (ataque.elementoAtaque.Equals(AtaqueBase.ElementoAtaque.AGUA) && enemigo.elemento.Equals(EnemigoBase.Elemento.FUEGO)
            || ataque.elementoAtaque.Equals(AtaqueBase.ElementoAtaque.FUEGO) && enemigo.elemento.Equals(EnemigoBase.Elemento.PLANTA)
            || ataque.elementoAtaque.Equals(AtaqueBase.ElementoAtaque.PLANTA) && enemigo.elemento.Equals(EnemigoBase.Elemento.AGUA)
            || ataque.elementoAtaque.Equals(AtaqueBase.ElementoAtaque.ELECTRICO) && enemigo.elemento.Equals(EnemigoBase.Elemento.AGUA)
            || ataque.elementoAtaque.Equals(AtaqueBase.ElementoAtaque.LUZ) && enemigo.elemento.Equals(EnemigoBase.Elemento.OSCURIDAD)
            || ataque.elementoAtaque.Equals(AtaqueBase.ElementoAtaque.OSCURIDAD) && enemigo.elemento.Equals(EnemigoBase.Elemento.LUZ))
        {
            return 2f;
        }
        else if (ataque.elementoAtaque.Equals(AtaqueBase.ElementoAtaque.FUEGO) && enemigo.elemento.Equals(EnemigoBase.Elemento.AGUA)
            || ataque.elementoAtaque.Equals(AtaqueBase.ElementoAtaque.PLANTA) && enemigo.elemento.Equals(EnemigoBase.Elemento.FUEGO)
            || ataque.elementoAtaque.Equals(AtaqueBase.ElementoAtaque.AGUA) && enemigo.elemento.Equals(EnemigoBase.Elemento.PLANTA)
            || ataque.elementoAtaque.Equals(AtaqueBase.ElementoAtaque.AGUA) && enemigo.elemento.Equals(EnemigoBase.Elemento.ELECTRICO))
        {
            return 0.5f;
        }
        else
            return 1f;
    }

    public void SpawnDMG(int daño, GameObject objetivo, string tipo)
    {
        GameObject spawnDMG = GameObject.FindGameObjectWithTag("SpawnDMG");

        if(tipo.Equals("daño"))
            prefabTextoDMG.transform.Find("Icon").GetComponent<Image>().sprite = dmgSprite;
        else if(tipo.Equals("curar"))
            prefabTextoDMG.transform.Find("Icon").GetComponent<Image>().sprite = vidaSprite;
        else if(tipo.Equals("mana"))
            prefabTextoDMG.transform.Find("Icon").GetComponent<Image>().sprite = manaSprite;

        prefabTextoDMG.transform.Find("Texto").GetComponent<TextMeshProUGUI>().text = ""+daño;
        GameObject dmg = Instantiate(prefabTextoDMG) as GameObject;
        dmg.transform.SetParent(spawnDMG.transform);
        dmg.gameObject.transform.localScale = Vector3.one;
        spawnDMG.transform.position = objetivo.transform.position;

        Destroy(dmg, 1f);
    }

    public void GastarMana()
    {
        statHandler.ActualizarInfo();
    }

    public void Curar(int cantidad, GameObject objetivo)
    {
        if(objetivo.CompareTag("Player"))
        {
            HeroeBase heroe = objetivo.GetComponent<HeroeStateMachine>().heroe;
            if ((heroe.vidaActual + cantidad) >= heroe.vidaBase)
                heroe.vidaActual = heroe.vidaBase;
            else
                heroe.vidaActual += cantidad;

            statHandler.ActualizarInfo();
        }
        else if(objetivo.CompareTag("Enemigo"))
        {
            EnemigoBase enemigo = objetivo.GetComponent<EnemigoStateMachine>().enemigo;
            if ((enemigo.vidaActual + cantidad) >= enemigo.vidaBase)
                enemigo.vidaActual = enemigo.vidaBase;
            else
                enemigo.vidaActual += cantidad;
        }

        if(!SceneManager.GetActiveScene().name.Equals("Hoguera"))
            SpawnDMG(cantidad, objetivo, "curar");
    }

    public void RestablecerMana(int cantidad, GameObject objetivo)
    {
        if (objetivo.CompareTag("Player"))
        {
            HeroeBase heroe = objetivo.GetComponent<HeroeStateMachine>().heroe;
            if ((heroe.manaActual + cantidad) >= heroe.manaBase)
                heroe.manaActual = heroe.manaBase;
            else
                heroe.manaActual += cantidad;

            statHandler.ActualizarInfo();
        }
        else if (objetivo.CompareTag("Enemigo"))
        {
            EnemigoBase enemigo = objetivo.GetComponent<EnemigoStateMachine>().enemigo;
            if ((enemigo.manaActual + cantidad) >= enemigo.manaBase)
                enemigo.manaActual = enemigo.manaBase;
            else
                enemigo.manaActual += cantidad;
        }

        if (!SceneManager.GetActiveScene().name.Equals("Hoguera"))
            SpawnDMG(cantidad, objetivo, "mana");
    }
}
