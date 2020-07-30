using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroeInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private GameObject heroe;

    //Datos del prefab
    [SerializeField] private HeroeBase heroeBase;
    [SerializeField] private TextMeshProUGUI nombreText;
    [SerializeField] private TextMeshProUGUI vidaText;
    [SerializeField] private TextMeshProUGUI manaText;
    [SerializeField] private SVGImage icon;

    [Header("Sonido")]
    [SerializeField] private AudioClip clipSonidoPE;
    [SerializeField] private AudioClip clipSonidoPC;

    public GameObject Heroe { get => heroe; set => heroe = value; }

    public void CargarInfoHeroe()
    {
        //Rellenar datos del prefab [icono, nombre, vida, seleccionado]
        heroeBase = Heroe.GetComponent<HeroeStateMachine>().heroe;
        icon.sprite = heroeBase.icon;
        nombreText.text = "<b>" + heroeBase.nombre + "</b>";
        vidaText.text = "<color=#f43224><b>hp:<color=white> " + heroeBase.vidaActual + "</b>";
        manaText.text = "<color=#00bdff><b>mp:<color=white> " + heroeBase.manaActual + "</b>";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayAudio("PointerClick");

        //transform.parent.GetComponent<SelectHeroe>().HeroeSeleccionado(heroe);
        transform.parent.GetComponent<SistemaSeleccion>().SeleccionadoHeroeEnemigo(heroe);
        DesactivarCursor();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayAudio("PointerEnter");

        heroe.transform.Find("Selector").gameObject.SetActive(true);
        //gameObject.transform.Find("Selector").gameObject.SetActive(true);
        nombreText.color = Color.white;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DesactivarCursor();
        //gameObject.transform.Find("Selector").gameObject.SetActive(false);
        nombreText.color = Color.gray;
    }

    public void DesactivarCursor()
    {
        heroe.transform.Find("Selector").gameObject.SetActive(false);
    }

    private void PlayAudio(string tipo)
    {
        switch (tipo)
        {
            case "PointerEnter":

                if (clipSonidoPE != null)
                {
                    GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>().clip = clipSonidoPE;
                    GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>().Play();
                }

                break;

            case "PointerClick":

                if (clipSonidoPC != null)
                {
                    GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>().clip = clipSonidoPC;
                    GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>().Play();
                }

                break;
        }
    }
}
