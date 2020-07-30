using System.Collections.Generic;
using UnityEngine;

public class ListaObjetos : MonoBehaviour
{

    //Prefabs para mostrar el enemigo a seleccionar
    [SerializeField] private List<GameObject> objetosEnLista;

    [SerializeField] private bool seleccionable = false;
    public bool enDescripcion = false;

    [SerializeField] private GameObject panelDescpObjetos;
    [SerializeField] private GameObject panelEstado;

    public void Inicializar(List<GameObject> objetos)
    {
        //Añadir los selectores de objetos
        for (int i = 0; i < objetos.Count; i++)
        {
            objetosEnLista.Add(objetos[i]);
        }

        for (int i = 0; i < objetosEnLista.Count; i++)
        {
            //Actualizar vida
            objetosEnLista[i].GetComponent<ObjectInfo>().CargarInfoObjeto();
        }

        seleccionable = true;
    }

    private void Update()
    {
        if (seleccionable)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                ResetMenuSeleccion();
            }
        }
        else if (enDescripcion)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                OcultarDescripcion();
            }
        }
    }

    public void ObjetoSeleccionado(ObjetoBase objeto)
    {
        enDescripcion = true;
        seleccionable = false;

        //Mostrar panel de descripcion del objetos
        panelDescpObjetos.SetActive(true);
        panelDescpObjetos.transform.Find("Objeto").gameObject.SetActive(true);
        panelDescpObjetos.transform.Find("Objeto").GetComponent<RellenarObjetos>().Rellenar(objeto);
    }

    private void OcultarDescripcion()
    {
        panelDescpObjetos.transform.Find("Objeto").gameObject.SetActive(false);
        panelDescpObjetos.SetActive(false);

        enDescripcion = false;
        seleccionable = true;
    }

    private void ResetMenuSeleccion()
    {
        seleccionable = false;
        transform.parent.parent.gameObject.SetActive(false);

        //Vaciar lista de enemigos a seleccionar
        objetosEnLista.RemoveRange(0, objetosEnLista.Count);

        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        panelEstado.GetComponent<SistemaEstado>().otroMenu = false;
        panelEstado.GetComponent<SistemaEstado>().ActivarComandos();
    }
}
