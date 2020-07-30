using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

class Comparador : IComparer<GameObject>
{
    public int Compare(GameObject x, GameObject y)
    {
        int vel1 = 0, vel2 = 0;

        if(x.CompareTag("Player"))
            vel1 = x.GetComponent<HeroeStateMachine>().heroe.velocidad;
        else if(x.CompareTag("Enemigo"))
            vel1 = x.GetComponent<EnemigoStateMachine>().enemigo.velocidad;

        if (y.CompareTag("Player"))
            vel2 = y.GetComponent<HeroeStateMachine>().heroe.velocidad;
        else if (y.CompareTag("Enemigo"))
            vel2 = y.GetComponent<EnemigoStateMachine>().enemigo.velocidad;

        return vel2.CompareTo(vel1);
    }
}

public class SistemaTurnos : MonoBehaviour
{
    //Acciones del menu
    public enum Accion
    {
        VICTORIA,
        DERROTA,
        ESPERA,
        VACIO
    }

    [Header("Variables del Sistema de Batalla")]
    public Accion accionActual;
    //Array para saber a quien le toca el turno
    public List<GameObject> turnosPersonajes = new List<GameObject>();
    //Heroes en combate
    public List<GameObject> heroesCombate = new List<GameObject>();
    //Enemigos en Combate
    public List<GameObject> enemigosCombate = new List<GameObject>();
    private int cantEnemigosCombate;
    //Lista de acciones
    public List<HandleAction> listaAcciones = new List<HandleAction>();

    [Header("Sistema Controlador de Menus")]
    //SistemaMenus
    public SistemaMenus sistemaMenus;

    [Header("Sistema de Daño")]
    //DamageHandler
    public SistemaDMG sistemaDMG;

    [Header("Sistema de Guardado")]
    //SaveHandler
    public SistemaGuardado sistemaGuardado;
    public bool nuevaRun = true;

    [Header("Referencia Inventario")]
    //Inventario
    public Inventario inventario;

    [Header("Pantalla de Victoria / Derrota")]
    //Pantalla de Victoria / Derrota
    public GameObject pantallaFinalVD;
    public AudioClip musicaVictoria;
    public AudioClip musicaDerrota;

    [Header("Prefab Panel Turno")]
    public GameObject prefabTurno;

    private void Start()
    {
        nuevaRun = GameObject.FindGameObjectWithTag("Escenas").GetComponent<Escenas>().nuevaRun;
        Iniciar();
    }

    void Iniciar()
    {
        sistemaMenus = GetComponent<SistemaMenus>();
        sistemaDMG = GetComponent<SistemaDMG>();
        sistemaGuardado = transform.Find("SaveManager").GetComponent<SistemaGuardado>();
        inventario = GameObject.FindGameObjectWithTag("Inventario").GetComponent<Inventario>();

        //Carga las opciones
        sistemaGuardado.LoadOpciones();

        accionActual = Accion.ESPERA;

        enemigosCombate.AddRange(GameObject.FindGameObjectsWithTag("Enemigo"));
        heroesCombate.AddRange(GameObject.FindGameObjectsWithTag("Player"));

        //Carga datos de los heroes
        if (!nuevaRun)
        {
            //Carga el sistema break
            sistemaGuardado.LoadBreak(sistemaMenus.sistemaBreak);

            //Carga los heroes
            for (int i = 0; i < heroesCombate.Count; i++)
                heroesCombate[i].GetComponent<HeroeStateMachine>().heroe = sistemaGuardado.LoadHeroe(heroesCombate[i].GetComponent<HeroeStateMachine>().heroe);

            //sistemaDMG.statHandler.ActualizarInfo();
        }

        //Y los guarda
        for (int i = 0; i < heroesCombate.Count; i++)
            sistemaGuardado.SaveHeroe(heroesCombate[i].GetComponent<HeroeStateMachine>().heroe);

        turnosPersonajes.AddRange(enemigosCombate);
        turnosPersonajes.AddRange(heroesCombate);

        cantEnemigosCombate = enemigosCombate.Count;

        //Ordenar por velocidad
        Comparador comparador = new Comparador();
        turnosPersonajes.Sort(comparador);

        //Crear Inventario si no esta hecho
        if(inventario.Objetos.Count==0)
            inventario.CrearInventario();
    }

    // Update is called once per frame
    void Update()
    {
        sistemaDMG.statHandler.ActualizarInfo();

        ComprobarEstadoBatalla();

        switch(accionActual)
        {
            case Accion.ESPERA:
                if (!SceneManager.GetActiveScene().name.Equals("Hoguera") && !SceneManager.GetActiveScene().name.Equals("Tesoro"))
                {
                    if (listaAcciones.Count > 0)
                    {
                        if (listaAcciones[0].tipoAtacante.Equals(HandleAction.TipoAtacante.ENEMIGO))
                        {
                            //accionActual = Accion.ENEMIGO;
                            EnemigoStateMachine ESM = listaAcciones[0].atacanteGameObj.GetComponent<EnemigoStateMachine>();
                            ESM.estadoTurno = EnemigoStateMachine.EstadoTurno.ACCION;
                        }
                        else if (listaAcciones[0].tipoAtacante.Equals(HandleAction.TipoAtacante.PERSONAJE))
                        {
                            //accionActual = Accion.PERSONAJE;
                            HeroeStateMachine HSM = listaAcciones[0].atacanteGameObj.GetComponent<HeroeStateMachine>();
                            HSM.estadoTurno = HeroeStateMachine.EstadoTurno.ACCION;
                        }
                    }
                    else
                    {
                        if (turnosPersonajes[0] != null && turnosPersonajes[0].CompareTag("Enemigo"))
                        {
                            turnosPersonajes[0].GetComponent<EnemigoStateMachine>().MeToca();
                            sistemaMenus.ActualizarPanelTurnoActual(turnosPersonajes[0]);
                        }
                        else if (turnosPersonajes[0] != null && turnosPersonajes[0].CompareTag("Player"))
                        {
                            if (turnosPersonajes[0].GetComponent<HeroeStateMachine>().estadoTurno.Equals(HeroeStateMachine.EstadoTurno.ESPERANDO))
                            {
                                turnosPersonajes[0].GetComponent<HeroeStateMachine>().MeToca();
                                sistemaMenus.ActualizarPanelTurnoActual(turnosPersonajes[0]);
                            }
                        }
                    }
                }
                else if(SceneManager.GetActiveScene().name.Equals("Hoguera"))
                {
                    GameObject.FindGameObjectWithTag("Break").transform.Find("UsarBreak").GetComponent<Button>().interactable = false;

                    if (turnosPersonajes[0].GetComponent<HeroeStateMachine>().estadoTurno.Equals(HeroeStateMachine.EstadoTurno.ESPERANDO))
                    {
                        turnosPersonajes[0].GetComponent<HeroeStateMachine>().MeToca();
                        sistemaMenus.ActualizarPanelTurnoActual(turnosPersonajes[0]); //Cambiar por hoguera
                    }
                }
                else if (SceneManager.GetActiveScene().name.Equals("Tesoro"))
                {
                    GameObject.FindGameObjectWithTag("Break").transform.Find("UsarBreak").GetComponent<Button>().interactable = false;

                    if (turnosPersonajes[0].GetComponent<HeroeStateMachine>().estadoTurno.Equals(HeroeStateMachine.EstadoTurno.ESPERANDO))
                    {
                        turnosPersonajes[0].GetComponent<HeroeStateMachine>().MeToca();
                        sistemaMenus.ActualizarPanelTurnoActual(turnosPersonajes[0]); //Cambiar por tesoro
                    }
                }

                break;
            case Accion.VICTORIA:
                //Cargar escena de victoria y el mapa
                CargarPantallaFinal("victoria");
                break;
            case Accion.DERROTA:
                CargarPantallaFinal("derrota");
                break;

            case Accion.VACIO: break;
        }
    }

    private void CargarPantallaFinal(string tipo)
    {
        accionActual = Accion.VACIO;

        GameObject.FindGameObjectWithTag("Escenas").GetComponent<Escenas>().nuevaRun = false;

        switch (tipo)
        {
            case "victoria":

                //Musica de Victoria
                GameObject.FindGameObjectWithTag("Musica").GetComponent<AudioSource>().clip = musicaVictoria;
                GameObject.FindGameObjectWithTag("Musica").GetComponent<AudioSource>().loop = false;
                GameObject.FindGameObjectWithTag("Musica").GetComponent<AudioSource>().Play();

                Debug.Log("Victoria");

                pantallaFinalVD.SetActive(true);
                pantallaFinalVD.transform.Find("LogoVictoria").gameObject.SetActive(true);

                //Añadir Musica de victoria

                StartCoroutine(pantallaFinalVD.transform.Find("LogoVictoria").transform.Find("EspadaIzq").GetComponent<AnimarLogoVictoria>().IniciarAnimacion(Vector3.zero));
                StartCoroutine(pantallaFinalVD.transform.Find("LogoVictoria").transform.Find("EspadaDer").GetComponent<AnimarLogoVictoria>().IniciarAnimacion(Vector3.zero));
                StartCoroutine(pantallaFinalVD.transform.Find("LogoVictoria").transform.Find("TextoVictoria").GetComponent<AnimarLogoVictoria>().IniciarAnimacion(Vector3.zero));

                break;

            case "derrota":

                //Musica de Derrota
                GameObject.FindGameObjectWithTag("Musica").GetComponent<AudioSource>().clip = musicaDerrota;
                GameObject.FindGameObjectWithTag("Musica").GetComponent<AudioSource>().loop = false;
                GameObject.FindGameObjectWithTag("Musica").GetComponent<AudioSource>().Play();

                Debug.Log("Derrota");

                pantallaFinalVD.SetActive(true);
                pantallaFinalVD.transform.Find("LogoDerrota").gameObject.SetActive(true);

                //Añadir Musica de derrota

                StartCoroutine(pantallaFinalVD.transform.Find("LogoDerrota").transform.Find("Calavera").GetComponent<AnimarLogoVictoria>().IniciarAnimacion(Vector3.zero));
                StartCoroutine(pantallaFinalVD.transform.Find("LogoDerrota").transform.Find("TextoVictoria").GetComponent<AnimarLogoVictoria>().IniciarAnimacion(Vector3.zero));

                break;
        }

    }

    public void ComprobarEstadoBatalla()
    {
        if (!accionActual.Equals(Accion.VACIO) && !SceneManager.GetActiveScene().name.Equals("Hoguera") && !SceneManager.GetActiveScene().name.Equals("Tesoro"))
        {
            if (enemigosCombate.Count == 0)
            {
                if (!accionActual.Equals(Accion.VACIO))
                {
                    accionActual = Accion.VICTORIA;
                    sistemaMenus.DesactivarMenu();
                }
            }

            if (heroesCombate[0].CompareTag("Muerto") && heroesCombate[1].CompareTag("Muerto") && heroesCombate[2].CompareTag("Muerto") && heroesCombate[3].CompareTag("Muerto"))
            {
                accionActual = Accion.DERROTA;
            }

            if (GameObject.FindGameObjectsWithTag("Enemigo").Length != cantEnemigosCombate)
            {
                enemigosCombate.RemoveRange(0, enemigosCombate.Count);
                enemigosCombate.AddRange(GameObject.FindGameObjectsWithTag("Enemigo"));
            }

            if ((turnosPersonajes[0] == null) || (turnosPersonajes[0].CompareTag("Enemigo") && turnosPersonajes[0].GetComponent<EnemigoStateMachine>().estadoTurno.Equals(EnemigoStateMachine.EstadoTurno.MUERTO)))
            {
                turnosPersonajes.RemoveAt(0);
            }

            for (int i = 0; i < enemigosCombate.Count; i++)
            {
                if (enemigosCombate[i].GetComponent<EnemigoStateMachine>().estadoTurno.Equals(EnemigoStateMachine.EstadoTurno.MUERTO))
                {
                    enemigosCombate.RemoveAt(i);
                }
            }
        }
    }

    public void RealizarAccion(HandleAction accion)
    {
        listaAcciones.Add(accion);
    }


    public IEnumerator ActualizarPanelTurnos()
    {
        GameObject panelTurnos = GameObject.FindGameObjectWithTag("PanelTurnoActual");

        for (int i = 0; i < panelTurnos.transform.childCount; i++)
        {
            Destroy(panelTurnos.transform.GetChild(i).gameObject);
        }

        for(int i=1;i<turnosPersonajes.Count;i++)
        {
            if (turnosPersonajes[i].gameObject != null)
            {
                if (turnosPersonajes[i].CompareTag("Player"))
                    prefabTurno.transform.Find("Icon").GetComponent<SVGImage>().sprite = turnosPersonajes[i].GetComponent<HeroeStateMachine>().heroe.icon;
                else if (turnosPersonajes[i].CompareTag("Enemigo"))
                    prefabTurno.transform.Find("Icon").GetComponent<SVGImage>().sprite = turnosPersonajes[i].GetComponent<EnemigoStateMachine>().enemigo.icon;

                GameObject objetoPrefab = Instantiate(prefabTurno) as GameObject;

                objetoPrefab.transform.SetParent(panelTurnos.transform);
                objetoPrefab.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            }
        }

        yield return null;
    }
}
