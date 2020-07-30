using UnityEngine;
using UnityEngine.UI;

public class SistemaBreak : MonoBehaviour
{
    public Image barra;
    public int cantidadBreak=0;

    private void Start()
    {
        gameObject.transform.Find("UsarBreak").GetComponent<TooltipTLK>().infoLeft = "<b>Cantidad Break: " + "~" + cantidadBreak + "</b>"; ;
    }

    public void IncrementarBarra(float cantidad)
    {
        if ((barra.fillAmount + cantidad) >= 1f)
            barra.fillAmount = 1f;
        else
            barra.fillAmount += cantidad;

        if (barra.fillAmount == 1)
        {
            if (cantidadBreak < 3)
            {
                AddBreak();
            }
        }
    }

    private void AddBreak()
    {
        switch (cantidadBreak)
        {
            case 0:
                transform.Find("Break1").gameObject.SetActive(true);
                break;
            case 1:
                transform.Find("Break2").gameObject.SetActive(true);
                break;
            case 2:
                transform.Find("Break3").gameObject.SetActive(true);
                break;
        }

        cantidadBreak++;

        gameObject.transform.Find("UsarBreak").GetComponent<TooltipTLK>().infoLeft = "<b>Cantidad Break: " + "~" + cantidadBreak + "</b>"; ;

        barra.fillAmount = 0;
    }

    public void RemoveBreak()
    {
        switch (cantidadBreak)
        {
            case 1:
                transform.Find("Break1").gameObject.SetActive(false);
                break;
            case 2:
                transform.Find("Break2").gameObject.SetActive(false);
                break;
            case 3:
                transform.Find("Break3").gameObject.SetActive(false);
                break;
        }

        cantidadBreak--;

        gameObject.transform.Find("UsarBreak").GetComponent<TooltipTLK>().infoLeft = "Cantidad Break: " + "<color=#59b2d9><b>" + cantidadBreak;

        if (barra.fillAmount == 1)
            AddBreak();
    }

    public void Actualizar()
    {
        switch (cantidadBreak)
        {
            case 1:
                transform.Find("Break1").gameObject.SetActive(true);
                break;
            case 2:
                transform.Find("Break1").gameObject.SetActive(true);
                transform.Find("Break2").gameObject.SetActive(true);
                break;
            case 3:
                transform.Find("Break1").gameObject.SetActive(true);
                transform.Find("Break2").gameObject.SetActive(true);
                transform.Find("Break3").gameObject.SetActive(true);
                break;
        }

        gameObject.transform.Find("UsarBreak").GetComponent<TooltipTLK>().infoLeft = "Cantidad Break: " + "<color=#59b2d9><b>" + cantidadBreak;

        if (barra.fillAmount == 1 && cantidadBreak<3)
            AddBreak();
    }
}
