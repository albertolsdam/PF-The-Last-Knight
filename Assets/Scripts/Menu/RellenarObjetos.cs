using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RellenarObjetos : MonoBehaviour
{
    [SerializeField] private Sprite spriteCura;
    [SerializeField] private Sprite spriteDMG;
    [SerializeField] private Sprite spriteMana;

    public void Rellenar(ObjetoBase objeto)
    {
        transform.Find("Icon").GetComponent<Image>().sprite = objeto.icon;
        transform.Find("Nombre").GetComponent<TextMeshProUGUI>().text = objeto.nombreObjeto;
        transform.Find("Cantidad").GetComponent<TextMeshProUGUI>().text = "x"+objeto.cantidad;
        transform.Find("DMG").GetComponent<TextMeshProUGUI>().text = "" + objeto.cantidadEfecto;

        if (objeto.tipoAccion.Equals(ObjetoBase.TipoAccion.CURAR))
            transform.Find("DMG").Find("Image").GetComponent<Image>().sprite = spriteCura;
        else if (objeto.tipoAccion.Equals(ObjetoBase.TipoAccion.HACERDMG))
            transform.Find("DMG").Find("Image").GetComponent<Image>().sprite = spriteDMG;
        else if (objeto.tipoAccion.Equals(ObjetoBase.TipoAccion.MANA))
            transform.Find("DMG").Find("Image").GetComponent<Image>().sprite = spriteMana;

        transform.Find("Descripcion").GetComponent<TextMeshProUGUI>().text = objeto.descripcion;
    }
}
