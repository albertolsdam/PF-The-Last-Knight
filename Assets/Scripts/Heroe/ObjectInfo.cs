using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ObjectInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    //Datos del prefab
    [SerializeField] public ObjetoBase objeto;
    [SerializeField] private TextMeshProUGUI nombreText;
    [SerializeField] private TextMeshProUGUI cantidadText;
    [SerializeField] private Image icon;
    [SerializeField] private Image effectIcon;
    [SerializeField] private TextMeshProUGUI effectText;

    [SerializeField] private Sprite spriteCura;
    [SerializeField] private Sprite spriteDMG;
    [SerializeField] private Sprite spriteMana;

    [Header("Sonido")]
    [SerializeField] private AudioClip clipSonidoPE;
    [SerializeField] private AudioClip clipSonidoPC;

    public void CargarInfoObjeto()
    {
        if (objeto != null)
        {
            icon.sprite = objeto.icon;
            nombreText.text = objeto.nombreObjeto;

            if(cantidadText!=null)
                cantidadText.text = "<color=white>x" + objeto.cantidad;

            if (objeto.tipoAccion.Equals(ObjetoBase.TipoAccion.CURAR))
                effectIcon.sprite = spriteCura;
            else if(objeto.tipoAccion.Equals(ObjetoBase.TipoAccion.HACERDMG))
                effectIcon.sprite = spriteDMG;
            else if(objeto.tipoAccion.Equals(ObjetoBase.TipoAccion.MANA))
                effectIcon.sprite = spriteMana;
            else if(objeto.tipoAccion.Equals(ObjetoBase.TipoAccion.REVIVIR))
                effectIcon.sprite = spriteCura;

            if (!objeto.tipoAccion.Equals(ObjetoBase.TipoAccion.REVIVIR))
                effectText.text = "" + objeto.cantidadEfecto;
            else
                effectText.text = "1/2";
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayAudio("PointerClick");

        if (transform.parent.CompareTag("ListaEnemigos"))
        {
            transform.parent.GetComponent<SistemaSeleccion>().ObjetoSeleccionado(objeto);
        }
        else if (transform.parent.CompareTag("ListaObjetosMagia"))
        {
            transform.parent.GetComponent<ListaObjetos>().ObjetoSeleccionado(objeto);
        }
        else if (transform.parent.CompareTag("ListaTesoros"))
        {
            //Play sonido
            GameObject.FindGameObjectWithTag("Inventario").GetComponent<Inventario>().AddtoInvt(objeto);
            Destroy(this.gameObject);
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayAudio("PointerEnter");
        nombreText.color = Color.white;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
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
