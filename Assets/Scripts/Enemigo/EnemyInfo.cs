using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EnemyInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private GameObject enemigo;

    //Datos del prefab
    [SerializeField] private EnemigoBase enemigoBase;
    [SerializeField] private TextMeshProUGUI nombreText;
    [SerializeField] private TextMeshProUGUI vidaText;
    [SerializeField] private SVGImage icon;

    [Header("Sonido")]
    [SerializeField] private AudioClip clipSonidoPE;
    [SerializeField] private AudioClip clipSonidoPC;

    public GameObject Enemigo { get => enemigo; set => enemigo = value; }

    public void CargarInfoEnemigo()
    {
        //Rellenar datos del prefab [icono, nombre, vida, seleccionado]
        enemigoBase = Enemigo.GetComponent<EnemigoStateMachine>().enemigo;
        icon.sprite = enemigoBase.icon;
        nombreText.text = enemigoBase.nombre;
        vidaText.text = "<color=#f43224>hp:<color=white> " + enemigoBase.vidaActual;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayAudio("PointerEnter");

        enemigo.transform.Find("Selector").gameObject.SetActive(true);
        //gameObject.transform.Find("Selector").gameObject.SetActive(true);
        nombreText.color = Color.white;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DesactivarCursor();
        //gameObject.transform.Find("Selector").gameObject.SetActive(false);
        nombreText.color = Color.gray;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayAudio("PointerClick");

        //transform.parent.GetComponent<SelectEnemy>().EnemigoSeleccionado(enemigo);
        transform.parent.GetComponent<SistemaSeleccion>().SeleccionadoHeroeEnemigo(enemigo);
        DesactivarCursor();
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

    public void DesactivarCursor()
    {
        enemigo.transform.Find("Selector").gameObject.SetActive(false);
    }
}
