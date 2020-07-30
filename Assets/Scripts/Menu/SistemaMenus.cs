using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SistemaMenus : MonoBehaviour
{
    /*** Inicio de variables para el menu de Comandos ***/

    [Header("Menu Comandos")]
    //Gameobject del menu de Comandos
    public GameObject menuComandosBatalla;
    public GameObject menuComandosHoguera;
    public GameObject menuComandosTesoro;
    //Comandos
    public List<GameObject> comandos;

    /*** Fin de variables para el menu de Comandos ***/

    [Header("Menu Lista")]
    /*** Inicio Menu lista **/
    [SerializeField] private GameObject menuLista;
    [SerializeField] private GameObject panelTesoro;
    [SerializeField] private GameObject prefabBotonEnemigo;
    public GameObject prefabBotonMagia;
    [SerializeField] private GameObject prefabBotonHeroe;
    public GameObject prefabBotonObjeto;
    [SerializeField] private GameObject prefabBotonTesoro;
    //public GameObject prefabBotonTesoro;
    /*** Fin Menu lista **/

    [Header("Sistemas / State Machines")]
    public HeroeStateMachine HSM;
    public SistemaTurnos sistemaTurnos;
    public SistemaBreak sistemaBreak;
    public SistemaEstado sistemaEstado;
    /*** Fin State Machines ***/

    [Header("Panel Turno Actual")]
    //Panel que muestra el icono del personaje/enemigo que este realizando el turno
    public GameObject panelTurnoActual;
    public Sprite panelTurnoNormal;
    public Sprite panelTurnoBreak;

    private void Start()
    {
        menuLista = GameObject.FindGameObjectWithTag("ListaEnemigos");
        sistemaTurnos = GetComponent<SistemaTurnos>();
        sistemaBreak = GameObject.FindGameObjectWithTag("Break").GetComponent<SistemaBreak>();
        sistemaEstado = GameObject.FindGameObjectWithTag("Estado").GetComponent<SistemaEstado>();

        if(SceneManager.GetActiveScene().name.Equals("Hoguera"))
        {
            menuComandosHoguera.SetActive(true);

            //Añadir los selectores de enemigos
            for (int i = 1; i < menuComandosHoguera.transform.childCount; i++)
            {
                comandos.Add(menuComandosHoguera.transform.GetChild(i).gameObject);
            }

            comandos[0].GetComponent<Button>().onClick.AddListener(DescansarHoguera);
            comandos[1].GetComponent<Button>().onClick.AddListener(ContinuarAMapa);
            comandos[2].GetComponent<Button>().onClick.AddListener(IniciarHuida);
        }
        else if(SceneManager.GetActiveScene().name.Equals("Tesoro"))
        {
            menuComandosTesoro.SetActive(true);

            //Añadir los selectores de enemigos
            for (int i = 1; i < menuComandosTesoro.transform.childCount; i++)
            {
                comandos.Add(menuComandosTesoro.transform.GetChild(i).gameObject);
            }

            comandos[0].GetComponent<Button>().onClick.AddListener(AbrirCofre);
            comandos[1].GetComponent<Button>().onClick.AddListener(ContinuarAMapa);
            comandos[2].GetComponent<Button>().onClick.AddListener(IniciarHuida);
        }
        else
        {
            menuComandosBatalla.SetActive(true);

            //Añadir los selectores de enemigos
            for (int i = 1; i < menuComandosBatalla.transform.childCount; i++)
            {
                comandos.Add(menuComandosBatalla.transform.GetChild(i).gameObject);
            }

            comandos[0].GetComponent<Button>().onClick.AddListener(IniciarAtaque);
            comandos[1].GetComponent<Button>().onClick.AddListener(IniciarDefensa);
            comandos[2].GetComponent<Button>().onClick.AddListener(IniciarMagia);
            comandos[3].GetComponent<Button>().onClick.AddListener(IniciarObjetos);
            comandos[4].GetComponent<Button>().onClick.AddListener(IniciarEstado);
            comandos[5].GetComponent<Button>().onClick.AddListener(IniciarHuida);
        }

        DesactivarMenu();
    }

    public void DesactivarMenu()
    {
        GameObject.FindGameObjectWithTag("Break").transform.Find("UsarBreak").GetComponent<Button>().interactable = false;

        for (int i = 0; i < comandos.Count; i++)
        {
            comandos[i].GetComponent<SelectCommand>().Desactivar();
        }
    }

    public void VolverMenu()
    {
        panelTurnoActual.SetActive(true);

        GameObject.FindGameObjectWithTag("Break").transform.Find("UsarBreak").GetComponent<Button>().interactable = true;
        HSM.estadoMenu = HeroeStateMachine.HeroeInputMenu.NINGUNO;

        for (int i = 0; i < comandos.Count; i++)
        {
            comandos[i].GetComponent<SelectCommand>().Activar();
        }
    }

    public void ActualizarPanelTurnoActual(GameObject objeto)
    {
        panelTurnoActual.transform.Find("BG").GetComponent<Image>().sprite = panelTurnoNormal;
        panelTurnoActual.transform.Find("Background").GetComponent<Image>().color = Color.black;

        if (objeto.CompareTag("Player"))
        {
            panelTurnoActual.transform.Find("Icon").gameObject.GetComponent<SVGImage>().sprite = objeto.GetComponent<HeroeStateMachine>().heroe.icon;

            panelTurnoActual.GetComponent<TooltipTLK>().infoLeft = "<b>Turno actual:";
            panelTurnoActual.GetComponent<TooltipTLK>().infoRight = "~" + objeto.GetComponent<HeroeStateMachine>().heroe.nombre+ "</b>";
        }
        else if (objeto.CompareTag("Enemigo"))
        {
            panelTurnoActual.transform.Find("Icon").gameObject.GetComponent<SVGImage>().sprite = objeto.GetComponent<EnemigoStateMachine>().enemigo.icon;

            panelTurnoActual.GetComponent<TooltipTLK>().infoLeft = "<b>Turno actual:";
            panelTurnoActual.GetComponent<TooltipTLK>().infoRight = "~" + objeto.GetComponent<EnemigoStateMachine>().enemigo.nombre + "</b>";
        }

        StartCoroutine(sistemaTurnos.ActualizarPanelTurnos());
    }

    //Iniciar Menu de Ataque
    public void IniciarAtaque()
    {
        DesactivarMenu();

        HSM.estadoMenu = HeroeStateMachine.HeroeInputMenu.ATACAR;
        SeleccionarObjetivo();
    }

    //Iniciar Menu de Defensa
    public void IniciarDefensa()
    {
        DesactivarMenu();

        HSM.estadoMenu = HeroeStateMachine.HeroeInputMenu.DEFENDER;
        HSM.Defender();
    }

    //Iniciar Menu de Magia
    public void IniciarMagia()
    {
        DesactivarMenu();

        HSM.estadoMenu = HeroeStateMachine.HeroeInputMenu.MAGIA;

        menuLista.GetComponent<SistemaSeleccion>().HSM = HSM;
        menuLista.GetComponent<SistemaSeleccion>().sistemaMenus = this;
        menuLista.GetComponent<SistemaSeleccion>().menuAnterior = HeroeStateMachine.HeroeInputMenu.NINGUNO;
        menuLista.GetComponent<SistemaSeleccion>().Inicializar(RellenarListaMagias(HSM.heroe.magias), "magias");
    }

    //Iniciar Menu de Objetos
    public void IniciarObjetos()
    {
        DesactivarMenu();

        HSM.estadoMenu = HeroeStateMachine.HeroeInputMenu.OBJETOS;

        menuLista.GetComponent<SistemaSeleccion>().HSM = HSM;
        menuLista.GetComponent<SistemaSeleccion>().sistemaMenus = this;
        menuLista.GetComponent<SistemaSeleccion>().menuAnterior = HeroeStateMachine.HeroeInputMenu.NINGUNO;
        menuLista.GetComponent<SistemaSeleccion>().Inicializar(RellenarListaObjetos(sistemaTurnos.inventario.Objetos), "objetos");
    }

    public void IniciarEstado()
    {
        DesactivarMenu();

        HSM.estadoMenu = HeroeStateMachine.HeroeInputMenu.ESTADO;

        sistemaEstado.sistemaMenus = this;
        sistemaEstado.Inicializar();
    }

    public void IniciarHuida()
    {
        DesactivarMenu();

        HSM.estadoMenu = HeroeStateMachine.HeroeInputMenu.HUIR;

        StartCoroutine(GameObject.FindGameObjectWithTag("Escenas").GetComponent<Escenas>().CargarEscena("MenuPrincipal"));
    }

    public void DescansarHoguera()
    {
        DesactivarMenu();

        HSM.estadoMenu = HeroeStateMachine.HeroeInputMenu.HUIR;

        int cantidadVida = Mathf.RoundToInt(sistemaTurnos.heroesCombate[0].GetComponent<HeroeStateMachine>().heroe.vidaBase / 2);
        int cantidadMana = Mathf.RoundToInt(sistemaTurnos.heroesCombate[0].GetComponent<HeroeStateMachine>().heroe.manaBase / 2);
        sistemaTurnos.sistemaDMG.Curar(cantidadVida, sistemaTurnos.heroesCombate[0]);
        sistemaTurnos.sistemaDMG.RestablecerMana(cantidadMana, sistemaTurnos.heroesCombate[0]);

        cantidadVida = Mathf.RoundToInt(sistemaTurnos.heroesCombate[1].GetComponent<HeroeStateMachine>().heroe.vidaBase / 2);
        cantidadMana = Mathf.RoundToInt(sistemaTurnos.heroesCombate[1].GetComponent<HeroeStateMachine>().heroe.manaBase / 2);
        sistemaTurnos.sistemaDMG.Curar(cantidadVida, sistemaTurnos.heroesCombate[1]);
        sistemaTurnos.sistemaDMG.RestablecerMana(cantidadMana, sistemaTurnos.heroesCombate[1]);

        cantidadVida = Mathf.RoundToInt(sistemaTurnos.heroesCombate[2].GetComponent<HeroeStateMachine>().heroe.vidaBase / 2);
        cantidadMana = Mathf.RoundToInt(sistemaTurnos.heroesCombate[2].GetComponent<HeroeStateMachine>().heroe.manaBase / 2);
        sistemaTurnos.sistemaDMG.Curar(cantidadVida, sistemaTurnos.heroesCombate[2]);
        sistemaTurnos.sistemaDMG.RestablecerMana(cantidadMana, sistemaTurnos.heroesCombate[2]);

        cantidadVida = Mathf.RoundToInt(sistemaTurnos.heroesCombate[3].GetComponent<HeroeStateMachine>().heroe.vidaBase / 2);
        cantidadMana = Mathf.RoundToInt(sistemaTurnos.heroesCombate[3].GetComponent<HeroeStateMachine>().heroe.manaBase / 2);
        sistemaTurnos.sistemaDMG.Curar(cantidadVida, sistemaTurnos.heroesCombate[3]);
        sistemaTurnos.sistemaDMG.RestablecerMana(cantidadMana, sistemaTurnos.heroesCombate[3]);

        for (int i = 1; i < comandos.Count-1; i++)
        {
            comandos[i].GetComponent<SelectCommand>().Activar();
        }
    }

    public void AbrirCofre()
    {
        DesactivarMenu();

        HSM.estadoMenu = HeroeStateMachine.HeroeInputMenu.HUIR;

        //Activar Panel
        panelTesoro.SetActive(true);

        //Generar Tesoro
        List<ObjetoBase> tesoros = sistemaTurnos.inventario.GenerarTesoro();

        //Animacion del cofre
        GameObject.FindGameObjectWithTag("Cofre").transform.Find("1_chest_top").GetComponent<Animator>().SetBool("open", true);

        //Generar Panel de tesoro con boton de aceptar o click para recoger

        //Cargamos los objetos
        foreach (ObjetoBase objeto in tesoros)
        {
            prefabBotonTesoro.GetComponent<ObjectInfo>().objeto = objeto;

            GameObject tesoroPrefab;

            tesoroPrefab = Instantiate(prefabBotonTesoro) as GameObject;

            tesoroPrefab.transform.SetParent(GameObject.FindGameObjectWithTag("ListaTesoros").transform);
            tesoroPrefab.transform.localScale = Vector3.one;

            tesoroPrefab.GetComponent<ObjectInfo>().CargarInfoObjeto();
        }

        panelTesoro.transform.Find("Button").GetComponent<Button>().onClick.AddListener(CofreAceptar);

    }

    public void CofreAceptar()
    {
        GameObject listaTesoros = GameObject.FindGameObjectWithTag("ListaTesoros");

        for (int i = 0; i < listaTesoros.transform.childCount; i++)
        {
            Destroy(listaTesoros.transform.GetChild(i));
        }

        panelTesoro.SetActive(false);

        for (int i = 1; i < comandos.Count-1; i++)
        {
            comandos[i].GetComponent<SelectCommand>().Activar();
        }
    }

    public void ContinuarAMapa()
    {
        sistemaTurnos.sistemaGuardado.SaveHeroe(sistemaTurnos.heroesCombate[0].GetComponent<HeroeStateMachine>().heroe);
        sistemaTurnos.sistemaGuardado.SaveHeroe(sistemaTurnos.heroesCombate[1].GetComponent<HeroeStateMachine>().heroe);
        sistemaTurnos.sistemaGuardado.SaveHeroe(sistemaTurnos.heroesCombate[2].GetComponent<HeroeStateMachine>().heroe);
        sistemaTurnos.sistemaGuardado.SaveHeroe(sistemaTurnos.heroesCombate[3].GetComponent<HeroeStateMachine>().heroe);
        SceneManager.LoadScene("Mapa");
    }

    public List<GameObject> RellenarListaEnemigos(List<GameObject> enemigosCombate)
    {
        LimpiarLista();

        List<GameObject> enemigos = new List<GameObject>();

        //Cargamos los enemigos
        foreach (GameObject enemigo in enemigosCombate)
        {
            prefabBotonEnemigo.GetComponent<EnemyInfo>().Enemigo = enemigo;

            GameObject objetoPrefab;

            objetoPrefab = Instantiate(prefabBotonEnemigo) as GameObject;

            objetoPrefab.transform.SetParent(menuLista.transform);
            objetoPrefab.transform.localScale = Vector3.one;

            enemigos.Add(objetoPrefab);
        }

        return enemigos;
    }

    public List<GameObject> RellenarListaHeroes(List<GameObject> heroesCombate)
    {
        LimpiarLista();

        List<GameObject> heroes = new List<GameObject>();

        //Cargamos los enemigos
        foreach (GameObject heroe in heroesCombate)
        {
            prefabBotonHeroe.GetComponent<HeroeInfo>().Heroe = heroe;

            GameObject objetoPrefab;

            objetoPrefab = Instantiate(prefabBotonHeroe) as GameObject;

            objetoPrefab.transform.SetParent(menuLista.transform);
            objetoPrefab.transform.localScale = Vector3.one;

            heroes.Add(objetoPrefab);
        }

        return heroes;
    }

    public List<GameObject> RellenarListaMagias(List<AtaqueBase> magias)
    {
        LimpiarLista();

        List<GameObject> listaMagias = new List<GameObject>();

        //Cargamos las magias
        foreach (AtaqueBase magia in magias)
        {
            prefabBotonMagia.GetComponent<MagicInfo>().ataqueMagico = magia;

            GameObject objetoPrefab;

            objetoPrefab = Instantiate(prefabBotonMagia) as GameObject;

            objetoPrefab.transform.SetParent(menuLista.transform);
            objetoPrefab.transform.localScale = Vector3.one;

            listaMagias.Add(objetoPrefab);
        }

        return listaMagias;
    }

    public List<GameObject> RellenarListaObjetos(List<ObjetoBase> objetos)
    {
        LimpiarLista();

        List<GameObject> listaObjetos = new List<GameObject>();

        //Cargamos los objetos
        foreach (ObjetoBase objeto in objetos)
        {
            prefabBotonObjeto.GetComponent<ObjectInfo>().objeto = objeto;

            GameObject objetoPrefab;

            objetoPrefab = Instantiate(prefabBotonObjeto) as GameObject;

            objetoPrefab.transform.SetParent(menuLista.transform);
            objetoPrefab.transform.localScale = Vector3.one;

            listaObjetos.Add(objetoPrefab);
        }

        return listaObjetos;
    }

    //Seleccionar Objetivo -> Ataque
    public void SeleccionarObjetivo()
    {
        List<GameObject> enemigosInstanciados = RellenarListaEnemigos(sistemaTurnos.enemigosCombate);

        menuLista.GetComponent<SistemaSeleccion>().menuAnterior = HeroeStateMachine.HeroeInputMenu.NINGUNO;
        menuLista.GetComponent<SistemaSeleccion>().HSM = HSM;
        menuLista.GetComponent<SistemaSeleccion>().sistemaMenus = this;
        menuLista.GetComponent<SistemaSeleccion>().Inicializar(enemigosInstanciados, "enemigos");
    }

    //Seleccionar Objetivo -> Magia
    public void SeleccionarObjetivo(AtaqueBase magia)
    {
        List<GameObject> enemigosInstanciados = RellenarListaEnemigos(sistemaTurnos.enemigosCombate);

        menuLista.GetComponent<SistemaSeleccion>().menuAnterior = HeroeStateMachine.HeroeInputMenu.MAGIA;
        menuLista.GetComponent<SistemaSeleccion>().HSM = HSM;
        menuLista.GetComponent<SistemaSeleccion>().sistemaMenus = this;
        menuLista.GetComponent<SistemaSeleccion>().magia = magia;
        menuLista.GetComponent<SistemaSeleccion>().Inicializar(enemigosInstanciados, "enemigos");
    }

    //Seleccionar Objetivo -> Objeto
    public void SeleccionarObjetivo(ObjetoBase objeto)
    {
        menuLista.GetComponent<SistemaSeleccion>().menuAnterior = HeroeStateMachine.HeroeInputMenu.OBJETOS;
        menuLista.GetComponent<SistemaSeleccion>().HSM = HSM;
        menuLista.GetComponent<SistemaSeleccion>().sistemaMenus = this;
        menuLista.GetComponent<SistemaSeleccion>().objeto = objeto;

        if (objeto.tipoAccion.Equals(ObjetoBase.TipoAccion.CURAR) || objeto.tipoAccion.Equals(ObjetoBase.TipoAccion.MANA) || objeto.tipoAccion.Equals(ObjetoBase.TipoAccion.REVIVIR))
        {
            List<GameObject> heroesInstaciados = RellenarListaHeroes(sistemaTurnos.heroesCombate);

            menuLista.GetComponent<SistemaSeleccion>().Inicializar(heroesInstaciados, "heroes");
        }
        else if (objeto.tipoAccion.Equals(ObjetoBase.TipoAccion.HACERDMG))
        {
            List<GameObject> enemigosInstanciados = RellenarListaEnemigos(sistemaTurnos.enemigosCombate);

            menuLista.GetComponent<SistemaSeleccion>().Inicializar(enemigosInstanciados, "enemigos");
        }
    }

    public void UsarBreak()
    {
        if (sistemaBreak.cantidadBreak > 0 && !sistemaTurnos.turnosPersonajes[0].GetComponent<HeroeStateMachine>().turnoBreak)
        {
            sistemaTurnos.turnosPersonajes[0].GetComponent<HeroeStateMachine>().turnoBreak = true;

            panelTurnoActual.transform.Find("BG").GetComponent<Image>().sprite = panelTurnoBreak;
            panelTurnoActual.transform.Find("Background").GetComponent<Image>().color = Color.white;

            sistemaBreak.RemoveBreak();
        }
    }

    private void LimpiarLista()
    {
        //Eliminamos todos los gameobjects que estaban en el estado anterior
        foreach (Transform child in menuLista.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
