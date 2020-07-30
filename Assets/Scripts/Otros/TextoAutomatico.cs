using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TextoAutomatico : MonoBehaviour
{
    public string frase;
    [SerializeField] private TextMeshProUGUI texto;
    [SerializeField] private int contador;
    public bool terminado;

    void Start()
    {
        texto.text = "";
        contador = 0;
        terminado = false;
    }

    //private void Update()
    //{
    //    if(texto.text.Length != frase.Length)
    //    {
    //        texto.text += frase[contador];
    //        contador++;
    //    }
    //}

    public IEnumerator IniciarTextoAutomatico()
    {
        while(!AddLetra())
        {
            contador++;
            yield return null;
        }

        terminado = true;

        yield return new WaitForSeconds(0.5f);
    }

    private bool AddLetra()
    {
        texto.text += frase[contador];

        return (texto.text.Length == frase.Length);
    }
}
