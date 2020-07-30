using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RellenarMagias : MonoBehaviour
{
    public void Rellenar(AtaqueBase ataqueMagico)
    {
        if(ataqueMagico.icon!=null)
            transform.Find("Icon").GetComponent<Image>().sprite = ataqueMagico.icon;

        transform.Find("Nombre").GetComponent<TextMeshProUGUI>().text = ataqueMagico.nombreAtaque;
        transform.Find("Mana").GetComponent<TextMeshProUGUI>().text = "<color=#00bdff>mp:<color=white> " + ataqueMagico.coste;
        transform.Find("DMG").GetComponent<TextMeshProUGUI>().text = "" + ataqueMagico.damage;

        transform.Find("Descripcion").GetComponent<TextMeshProUGUI>().text = ataqueMagico.descripcion;
    }
}
