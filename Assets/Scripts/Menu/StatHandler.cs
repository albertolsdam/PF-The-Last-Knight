using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatHandler : MonoBehaviour
{
    public List<GameObject> party;
    public List<GameObject> stats;

    [SerializeField] private float cantidadVida;
    [SerializeField] private float cantidadMana;

    // Start is called before the first frame update
    void Start()
    {
        stats.AddRange(GameObject.FindGameObjectsWithTag("Estadisticas"));
        party.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        cantidadVida = 0f;
        cantidadMana = 0f;

        ActualizarInfo();
    }

    public void ActualizarInfo()
    {
        for (int i = 0; i < 4; i++)
        {
            //Rojo #db5c67 | #f43224
            //Verde #59d97b
            //Azul #59b2d9 | #00bdff
            stats[i].GetComponent<StatInfo>().nombre.text = party[i].GetComponent<HeroeStateMachine>().heroe.nombre;
            stats[i].GetComponent<StatInfo>().vida.text = "<color=#f43224><b>hp<color=white>" + party[i].GetComponent<HeroeStateMachine>().heroe.vidaActual+"</b>";
            stats[i].GetComponent<StatInfo>().mana.text = "<color=#00bdff><b>mp<color=white>" + party[i].GetComponent<HeroeStateMachine>().heroe.manaActual+"</b>";

            cantidadVida = calcularVidaActual(party[i]);
            cantidadMana = calcularManaActual(party[i]);

            //Para Sprites
            //stats[i].GetComponent<StatInfo>().barraVida.transform.localScale = new Vector3(cantidadVida, 1f, 1f);
            //stats[i].GetComponent<StatInfo>().barraMana.transform.localScale = new Vector3(cantidadMana, 1f, 1f);

            //Para imagenes
            stats[i].GetComponent<StatInfo>().barraVida.GetComponent<Image>().fillAmount = cantidadVida;
            stats[i].GetComponent<StatInfo>().barraMana.GetComponent<Image>().fillAmount = cantidadMana;
        }
    }

    private float calcularVidaActual(GameObject party)
    {
        float vidaActual = party.GetComponent<HeroeStateMachine>().heroe.vidaActual;
        float vidaBase = party.GetComponent<HeroeStateMachine>().heroe.vidaBase;
        return vidaActual / vidaBase;
    }

    private float calcularManaActual(GameObject party)
    {
        float manaActual = party.GetComponent<HeroeStateMachine>().heroe.manaActual;
        float manaBase = party.GetComponent<HeroeStateMachine>().heroe.manaBase;
        return manaActual / manaBase;
    }
}
