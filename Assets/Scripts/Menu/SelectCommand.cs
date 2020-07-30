using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectCommand : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI texto;
    [SerializeField] private bool seleccionable = false;

    [Header("Sonido")]
    [SerializeField] private AudioClip clipSonidoPE;
    [SerializeField] private AudioClip clipSonidoPC;

    public bool Seleccionable { get => seleccionable; set => seleccionable = value; }

    // Start is called before the first frame update
    void Start()
    {
        texto = gameObject.transform.Find("Texto").GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (seleccionable)
        {
            PlayAudio("PointerEnter");
            texto.color = Color.white;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(seleccionable)
            texto.color = Color.gray;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (seleccionable)
            PlayAudio("PointerClick");
    }

    public void Desactivar()
    {
        Seleccionable = false;
        texto.color = Color.gray;
        GetComponent<Button>().interactable = false;
    }

    public void Activar()
    {
        GetComponent<Button>().interactable = true;
        Seleccionable = true;
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
