using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SistemaSeleccion : MonoBehaviour
{
    //Prefabs para mostrar el enemigo a seleccionar
    [SerializeField] private List<GameObject> gameobjEnLista;
    [SerializeField] private bool seleccionable = false;

    public HeroeStateMachine.HeroeInputMenu menuAnterior;

    public HeroeStateMachine HSM;
    public SistemaMenus sistemaMenus;

    public AtaqueBase magia;
    public ObjetoBase objeto;

    public void Inicializar(List<GameObject> instanciados, string tipo)
    {
        for (int i = 0; i < instanciados.Count; i++)
        {
            gameobjEnLista.Add(instanciados[i]);
        }

        for (int i = 0; i < gameobjEnLista.Count; i++)
        {
            switch (tipo)
            {
                case "heroes":
                        gameobjEnLista[i].GetComponent<HeroeInfo>().CargarInfoHeroe();
                    break;

                case "enemigos":
                        gameobjEnLista[i].GetComponent<EnemyInfo>().CargarInfoEnemigo();
                    break;

                case "objetos":
                        gameobjEnLista[i].GetComponent<ObjectInfo>().CargarInfoObjeto();
                    break;

                case "magias":
                        gameobjEnLista[i].GetComponent<MagicInfo>().CargarInfoMagia();
                    break;
            }
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

                switch(menuAnterior)
                {
                    case HeroeStateMachine.HeroeInputMenu.NINGUNO:
                        sistemaMenus.VolverMenu();
                        break;
                    case HeroeStateMachine.HeroeInputMenu.MAGIA:
                        sistemaMenus.IniciarMagia();
                        break;
                    case HeroeStateMachine.HeroeInputMenu.OBJETOS:
                        sistemaMenus.IniciarObjetos();
                        break;
                }
            }
        }
    }

    private void ResetMenuSeleccion()
    {
        seleccionable = false;

        //Vaciar lista de heroes a seleccionar
        gameobjEnLista.RemoveRange(0, gameobjEnLista.Count);

        

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.TryGetComponent(out HeroeInfo heroeInfo))
                heroeInfo.DesactivarCursor();
            else if (transform.GetChild(i).gameObject.TryGetComponent(out EnemyInfo enemyInfo))
                enemyInfo.DesactivarCursor();


            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public void SeleccionadoHeroeEnemigo(GameObject gameobj)
    {
        //Desactiva el menu de comandos
        sistemaMenus.DesactivarMenu();

        //Mandar orden de ataque
        switch (menuAnterior)
        {
            case HeroeStateMachine.HeroeInputMenu.NINGUNO:
                HSM.Atacar(gameobj);
                break;
            case HeroeStateMachine.HeroeInputMenu.MAGIA:
                //Mandar orden de ataque
                HSM.LanzarMagia(magia, gameobj);
                break;
            case HeroeStateMachine.HeroeInputMenu.OBJETOS:
                HSM.LanzarObjeto(objeto, gameobj);
                break;
        }

        //Reset menu selector de heroes
        ResetMenuSeleccion();
    }

    public void ObjetoSeleccionado(ObjetoBase objeto)
    {
        //Desactiva el menu de comandos
        sistemaMenus.DesactivarMenu();

        if (objeto.cantidad > 0)
        {
            //Reset menu selector de objetos
            ResetMenuSeleccion();

            sistemaMenus.SeleccionarObjetivo(objeto);
        }
    }

    public void MagiaSeleccionada(AtaqueBase ataqueMagico)
    {
        if (HSM.heroe.manaActual >= ataqueMagico.coste)
        {
            //Reset menu selector de magias
            ResetMenuSeleccion();

            sistemaMenus.SeleccionarObjetivo(ataqueMagico);
        }
    }
}
