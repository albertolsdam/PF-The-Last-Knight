using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnMapa : MonoBehaviour
{
    public bool enOpciones = false;
    public GameObject panelOpciones;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("Escenas").GetComponent<SistemaGuardado>().LoadOpciones();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name.Equals("Mapa") && !enOpciones)
        {
            enOpciones = true;
            panelOpciones.SetActive(true);
            panelOpciones.GetComponent<SistemaOpciones>().Inicializar();
        }
    }
}
