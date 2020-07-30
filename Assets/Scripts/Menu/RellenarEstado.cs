using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RellenarEstado : MonoBehaviour
{
    [SerializeField] private GameObject panelEstado;
    public bool seleccionable=false;

    private void Update()
    {
        if (seleccionable)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                OcultarEstado();
            }
        }
    }

    private void OcultarEstado()
    {
        panelEstado.GetComponent<SistemaEstado>().ActivarComandos();
        panelEstado.GetComponent<SistemaEstado>().otroMenu = false;

        seleccionable = false;
        gameObject.SetActive(false);
    }

    public void Rellenar(HeroeBase heroe)
    {
        GameObject personaje = transform.Find("Personaje").gameObject;
        GameObject estadisticas = transform.Find("Estadisticas").gameObject;

        personaje.transform.Find("Icon").GetComponent<SVGImage>().sprite = heroe.icon;
        personaje.transform.Find("Nombre").GetComponent<TextMeshProUGUI>().text = heroe.nombre;
        personaje.transform.Find("NombreAtaque").GetComponent<TextMeshProUGUI>().text = heroe.ataque.nombreAtaque;
        personaje.transform.Find("IconoAtaque").GetComponent<Image>().sprite = heroe.ataque.icon;
        personaje.transform.Find("DMG").GetComponent<TextMeshProUGUI>().text = ""+heroe.ataque.damage;
        personaje.transform.Find("Nivel").GetComponent<TextMeshProUGUI>().text = "Nvl. " + heroe.nivel;

        estadisticas.transform.Find("Vida").GetComponent<TextMeshProUGUI>().text = "<color=#f43224><b>hp<color=white>   " + heroe.vidaActual + "/" + heroe.vidaBase + "</b>";
        estadisticas.transform.Find("Mana").GetComponent<TextMeshProUGUI>().text = "<color=#59b2d9><b>mp<color=white>   " + heroe.manaActual + "/" + heroe.manaBase + "</b>";
        estadisticas.transform.Find("Fuerza").GetComponent<TextMeshProUGUI>().text = "<color=#f27424><b>Fue<color=white>   " + heroe.fuerza + "</b>";
        estadisticas.transform.Find("Inteligencia").GetComponent<TextMeshProUGUI>().text = "<color=#59b2d9><b>Int<color=white>   " + heroe.inteligencia + "</b>";
        estadisticas.transform.Find("Velocidad").GetComponent<TextMeshProUGUI>().text = "<color=#24f27d><b>Vel<color=white>   " + heroe.velocidad + "</b>";
        estadisticas.transform.Find("ResistenciaF").GetComponent<TextMeshProUGUI>().text = "<color=#f0d030><b>ResF.<color=white>   " + heroe.resistenciaF + "</b>";
        estadisticas.transform.Find("ResistenciaM").GetComponent<TextMeshProUGUI>().text = "<color=#ca72fc><b>ResM.<color=white>   " + heroe.resistenciaM + "</b>";
    }
}
