using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListaMagias : MonoBehaviour
{
    public HeroeStateMachine HSM;
    public SistemaMenus sistemaMenus;

    //Prefabs para mostrar el enemigo a seleccionar
    [SerializeField] private List<GameObject> magiasEnLista;

    //Gameobjects con el tag enemigo
    [SerializeField] private bool seleccionable = false;

    public bool enDescripcion = false;
    [SerializeField] private GameObject panelDescpMagias;
    [SerializeField] private GameObject panelEstado;

    public void Inicializar(List<GameObject> magias)
    {
        //Añadir los selectores de magias
        for (int i = 0; i < magias.Count; i++)
        {
            magiasEnLista.Add(magias[i]);
        }

        for (int i = 0; i < magiasEnLista.Count; i++)
        {
            //Actualizar vida
            magiasEnLista[i].GetComponent<MagicInfo>().CargarInfoMagia();
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

    private void OcultarDescripcion()
    {
        panelDescpMagias.transform.Find("Magia").gameObject.SetActive(false);
        panelDescpMagias.SetActive(false);

        enDescripcion = false;
        seleccionable = true;
    }

    public void MagiaSeleccionada(AtaqueBase ataqueMagico)
    {
        enDescripcion = true;
        seleccionable = false;

        //Mostrar panel de descripcion del objetos
        panelDescpMagias.SetActive(true);
        panelDescpMagias.transform.Find("Magia").gameObject.SetActive(true);
        panelDescpMagias.transform.Find("Magia").GetComponent<RellenarMagias>().Rellenar(ataqueMagico);
    }

    private void ResetMenuSeleccion()
    {
        seleccionable = false;
        transform.parent.parent.gameObject.SetActive(false);

        //Vaciar lista de enemigos a seleccionar
        magiasEnLista.RemoveRange(0, magiasEnLista.Count);

        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        panelEstado.GetComponent<SistemaEstado>().otroMenu = false;
        panelEstado.GetComponent<SistemaEstado>().ActivarComandos();
    }
}
