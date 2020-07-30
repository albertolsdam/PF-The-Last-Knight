using UnityEngine;
using TMPro;

public class CargarEscenario : MonoBehaviour
{
    public GameObject textos;
    public GameObject fondo;
    public Escenas escenas;
    
    public void Cargar()
    {
        escenas = GameObject.FindGameObjectWithTag("Escenas").GetComponent<Escenas>();
        fondo.GetComponent<SpriteRenderer>().sprite = escenas.escenarioActual.fondo;

        textos.transform.Find("TextoMapa").GetComponent<TextMeshProUGUI>().text = "Mapa "+escenas.numEscenarioActual;
        textos.transform.Find("TextoNombreMapa").GetComponent<TextMeshProUGUI>().text = escenas.escenarioActual.nombreMapa;
        textos.SetActive(true);
    }
}
