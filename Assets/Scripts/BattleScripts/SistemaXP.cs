using Saucy.Modules.XP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SistemaXP : MonoBehaviour
{
    public SistemaTurnos sistemaTurnos;

    private static SistemaXP instancia;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (instancia == null)
            instancia = this;
        else
            Destroy(gameObject);

    }

    public void RepartirXP()
    {
        if (!SceneManager.GetActiveScene().name.Equals("Mapa") && !SceneManager.GetActiveScene().name.Equals("MenuPrincipal"))
        {
            sistemaTurnos = GameObject.FindGameObjectWithTag("SistemaTurnos").GetComponent<SistemaTurnos>();
        }

        foreach (GameObject heroe in sistemaTurnos.heroesCombate)
        {
            if (heroe != null)
            {
                //heroe.GetComponent<HeroeStateMachine>().heroe.cantidadXP = ((float)GetComponentInChildren<XPReceiver>().CurrentXP / (float)GetComponentInChildren<XPReceiver>().RequiredXP);
                heroe.GetComponent<HeroeStateMachine>().heroe.cantidadXP = GetComponentInChildren<XPReceiver>().Progress;
            }
        }
    }

    public void SubirLvl()
    {
        if (!SceneManager.GetActiveScene().name.Equals("Mapa") && !SceneManager.GetActiveScene().name.Equals("MenuPrincipal"))
        {
            sistemaTurnos = GameObject.FindGameObjectWithTag("SistemaTurnos").GetComponent<SistemaTurnos>();
        }

        foreach (GameObject heroe in sistemaTurnos.heroesCombate)
        {
            if (heroe != null)
            {
                heroe.GetComponent<HeroeStateMachine>().SubirNivel();
            }
        }
    }
}
