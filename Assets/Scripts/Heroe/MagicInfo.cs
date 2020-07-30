using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class MagicInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    //Datos del prefab
    [SerializeField] public AtaqueBase ataqueMagico;
    [SerializeField] private TextMeshProUGUI nombreText;
    [SerializeField] private TextMeshProUGUI manaText;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI dmgText;
    [SerializeField] private Image dmgIcon;
    [SerializeField] private Sprite spriteCura;
    [SerializeField] private Sprite spriteDMG;

    [Header("Sonido")]
    [SerializeField] private AudioClip clipSonidoPE;
    [SerializeField] private AudioClip clipSonidoPC;

    public void CargarInfoMagia()
    {
        if(ataqueMagico != null)
        {
            //Rellenar datos del prefab [icono, nombre, vida, seleccionado]

            if(ataqueMagico.icon!=null)
                icon.sprite = ataqueMagico.icon;

            nombreText.text = ataqueMagico.nombreAtaque;
            manaText.text = "<color=#00bdff>mp:<color=white> " + ataqueMagico.coste;
            dmgText.text = ""+ataqueMagico.damage;

            if (ataqueMagico.tipoAtaque.Equals(AtaqueBase.TipoAtaque.CURA))
                dmgIcon.sprite = spriteCura;
            else
                dmgIcon.sprite = spriteDMG;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayAudio("PointerClick");

        if (transform.parent.CompareTag("ListaEnemigos"))
        {
            //transform.parent.GetComponent<SelectMagic>().MagiaSeleccionada(ataqueMagico);
            transform.parent.GetComponent<SistemaSeleccion>().MagiaSeleccionada(ataqueMagico);
        }
        else if (transform.parent.CompareTag("ListaObjetosMagia"))
        {
            transform.parent.GetComponent<ListaMagias>().MagiaSeleccionada(ataqueMagico);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayAudio("PointerEnter");
        //gameObject.transform.Find("Selector").gameObject.SetActive(true);
        nombreText.color = Color.white;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //gameObject.transform.Find("Selector").gameObject.SetActive(false);
        nombreText.color = Color.gray;
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
