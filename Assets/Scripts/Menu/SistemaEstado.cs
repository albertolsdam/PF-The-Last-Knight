using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Saucy.Modules.XP;

public class SistemaEstado : MonoBehaviour
{
    [SerializeField] private GameObject opciones;
    [SerializeField] private List<GameObject> listaComandos;

    [SerializeField] private GameObject estado1;
    [SerializeField] private GameObject estado2;
    [SerializeField] private GameObject estado3;
    [SerializeField] private GameObject estado4;

    public SistemaTurnos sistemaTurnos;

    public SistemaMenus sistemaMenus;
    public bool otroMenu = false;
    public bool seleccionable = false;

    public GameObject panelEstadisticas;
    public GameObject listaMagiaObjetos;

    private HeroeBase caballero;
    private HeroeBase arquera;
    private HeroeBase hechicero;
    private HeroeBase ingeniero;

    public void Inicializar()
    {
        sistemaTurnos = GameObject.Find("BattleHandler").GetComponent<SistemaTurnos>();

        transform.Find("Contenido").gameObject.SetActive(true);

        listaComandos = new List<GameObject>();

        for (int i = 0; i < transform.Find("Contenido").Find("Comandos").childCount; i++)
        {
            listaComandos.Add(transform.Find("Contenido").Find("Comandos").GetChild(i).gameObject);
        }

        seleccionable = true;

        ActivarComandos();

        ActualizarEstadisticas();
    }

    private void Update()
    {
        if (!otroMenu && seleccionable)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                sistemaMenus.VolverMenu();

                ResetMenuOpciones();
            }
        }
    }

    public void Volver()
    {
        sistemaMenus.VolverMenu();

        ResetMenuOpciones();
    }

    public void ActualizarEstadisticas()
    {
        //Actualizar vida y mana
        HeroeBase heroe;

        for (int i=0; i < sistemaTurnos.heroesCombate.Count; i++)
        {
            switch(sistemaTurnos.heroesCombate[i].GetComponent<HeroeStateMachine>().heroe.nombre)
            {
                case "Caballero":
                    heroe = sistemaTurnos.heroesCombate[i].GetComponent<HeroeStateMachine>().heroe;

                    caballero = heroe;

                    estado1.transform.Find("Estadisticas").Find("Vida").GetComponent<TextMeshProUGUI>().text = "<color=#f43224><b>hp <color=white>" + heroe.vidaActual + "/" + heroe.vidaBase + "</b>";
                    estado1.transform.Find("Estadisticas").Find("Mana").GetComponent<TextMeshProUGUI>().text = "<color=#00bdff><b>mp <color=white>" + heroe.manaActual + "/" + heroe.manaBase + "</b>";
                    estado1.transform.Find("Button").Find("Fondo").Find("Icono").GetComponent<SVGImage>().sprite = heroe.spriteCompleto;
                    estado1.transform.Find("Nivel").GetComponent<TextMeshProUGUI>().text = "Nvl. " + sistemaTurnos.heroesCombate[i].GetComponent<HeroeStateMachine>().heroe.nivel;
                    estado1.transform.Find("BarraExperiencia").Find("BarraExp").GetComponent<Image>().fillAmount = sistemaTurnos.heroesCombate[i].GetComponent<HeroeStateMachine>().heroe.cantidadXP;
                    break;

                case "Arquera":
                    heroe = sistemaTurnos.heroesCombate[i].GetComponent<HeroeStateMachine>().heroe;

                    arquera = heroe;

                    estado2.transform.Find("Estadisticas").Find("Vida").GetComponent<TextMeshProUGUI>().text = "<color=#f43224><b>hp <color=white>" + heroe.vidaActual + "/" + heroe.vidaBase + "</b>";
                    estado2.transform.Find("Estadisticas").Find("Mana").GetComponent<TextMeshProUGUI>().text = "<color=#00bdff><b>mp <color=white>" + heroe.manaActual + "/" + heroe.manaBase + "</b>";
                    estado2.transform.Find("Button").Find("Fondo").Find("Icono").GetComponent<SVGImage>().sprite = heroe.spriteCompleto;
                    estado2.transform.Find("Nivel").GetComponent<TextMeshProUGUI>().text = "Nvl. " + sistemaTurnos.heroesCombate[i].GetComponent<HeroeStateMachine>().heroe.nivel;
                    estado2.transform.Find("BarraExperiencia").Find("BarraExp").GetComponent<Image>().fillAmount = sistemaTurnos.heroesCombate[i].GetComponent<HeroeStateMachine>().heroe.cantidadXP;
                    break;

                case "Hechicero":
                    heroe = sistemaTurnos.heroesCombate[i].GetComponent<HeroeStateMachine>().heroe;

                    hechicero = heroe;

                    estado3.transform.Find("Estadisticas").Find("Vida").GetComponent<TextMeshProUGUI>().text = "<color=#f43224><b>hp <color=white>" + heroe.vidaActual + "/" + heroe.vidaBase + "</b>";
                    estado3.transform.Find("Estadisticas").Find("Mana").GetComponent<TextMeshProUGUI>().text = "<color=#00bdff><b>mp <color=white>" + heroe.manaActual + "/" + heroe.manaBase + "</b>";
                    estado3.transform.Find("Button").Find("Fondo").Find("Icono").GetComponent<SVGImage>().sprite = heroe.spriteCompleto;
                    estado3.transform.Find("Nivel").GetComponent<TextMeshProUGUI>().text = "Nvl. " + sistemaTurnos.heroesCombate[i].GetComponent<HeroeStateMachine>().heroe.nivel;
                    estado3.transform.Find("BarraExperiencia").Find("BarraExp").GetComponent<Image>().fillAmount = sistemaTurnos.heroesCombate[i].GetComponent<HeroeStateMachine>().heroe.cantidadXP;
                    break;

                case "Ingeniero":
                    heroe = sistemaTurnos.heroesCombate[i].GetComponent<HeroeStateMachine>().heroe;

                    ingeniero = heroe;

                    estado4.transform.Find("Estadisticas").Find("Vida").GetComponent<TextMeshProUGUI>().text = "<color=#f43224><b>hp <color=white>" + heroe.vidaActual + "/" + heroe.vidaBase + "</b>";
                    estado4.transform.Find("Estadisticas").Find("Mana").GetComponent<TextMeshProUGUI>().text = "<color=#00bdff><b>mp <color=white>" + heroe.manaActual + "/" + heroe.manaBase + "</b>";
                    estado4.transform.Find("Button").Find("Fondo").Find("Icono").GetComponent<SVGImage>().sprite = heroe.spriteCompleto;
                    estado4.transform.Find("Nivel").GetComponent<TextMeshProUGUI>().text = "Nvl. " + sistemaTurnos.heroesCombate[i].GetComponent<HeroeStateMachine>().heroe.nivel;
                    estado4.transform.Find("BarraExperiencia").Find("BarraExp").GetComponent<Image>().fillAmount = sistemaTurnos.heroesCombate[i].GetComponent<HeroeStateMachine>().heroe.cantidadXP;
                    break;
            }
        }
    }

    public void HabilitarOpciones()
    {
        otroMenu = true;

        opciones.SetActive(true);
        opciones.GetComponent<SistemaOpciones>().Inicializar();
        DesactivarComandos();
    }

    public void ActivarComandos()
    {
        foreach(GameObject comando in listaComandos)
        {
            comando.GetComponent<SelectCommand>().Activar();
        }
    }

    public void DesactivarComandos()
    {
        foreach (GameObject comando in listaComandos)
        {
            comando.GetComponent<SelectCommand>().Desactivar();
        }
    }

    private void ResetMenuOpciones()
    {
        otroMenu = false;
        DesactivarComandos();
        transform.Find("Contenido").gameObject.SetActive(false);
    }

    public void Salir()
    {
        Application.Quit();
    }

    public void ActivarListaObjetos()
    {
        DesactivarComandos();

        otroMenu = true;
        listaMagiaObjetos.SetActive(true);

        List<GameObject> listaObjetos = new List<GameObject>();

        //Cargamos los objetos
        foreach (ObjetoBase objeto in sistemaTurnos.inventario.Objetos)
        {
            sistemaMenus.prefabBotonObjeto.GetComponent<ObjectInfo>().objeto = objeto;

            GameObject objetoPrefab;

            objetoPrefab = Instantiate(sistemaMenus.prefabBotonObjeto) as GameObject;

            objetoPrefab.transform.SetParent(listaMagiaObjetos.transform.Find("Viewport").Find("GridView").transform);
            objetoPrefab.transform.localScale = Vector3.one;

            listaObjetos.Add(objetoPrefab);
        }

        listaMagiaObjetos.transform.Find("Viewport").Find("GridView").GetComponent<ListaObjetos>().Inicializar(listaObjetos);
    }

    public void ActivarListaMagias()
    {
        DesactivarComandos();

        otroMenu = true;
        listaMagiaObjetos.SetActive(true);

        List<GameObject> listaMagias = new List<GameObject>();

        for (int i=0; i < sistemaTurnos.heroesCombate.Count; i++)
        {
            //Cargamos los objetos
            foreach (AtaqueBase ataque in sistemaTurnos.heroesCombate[i].GetComponent<HeroeStateMachine>().heroe.magias)
            {
                sistemaMenus.prefabBotonMagia.GetComponent<MagicInfo>().ataqueMagico = ataque;

                GameObject magiaPrefab;

                magiaPrefab = Instantiate(sistemaMenus.prefabBotonMagia) as GameObject;

                magiaPrefab.transform.SetParent(listaMagiaObjetos.transform.Find("Viewport").Find("GridView").transform);
                magiaPrefab.transform.localScale = Vector3.one;

                listaMagias.Add(magiaPrefab);
            }
        }

        listaMagiaObjetos.transform.Find("Viewport").Find("GridView").GetComponent<ListaMagias>().Inicializar(listaMagias);
    }

    public void ListarEstado1()
    {
        panelEstadisticas.gameObject.SetActive(true);
        DesactivarComandos();
        otroMenu = true;

        //Desactivar comandos

        //HeroeBase heroe = GetComponent<SistemaEstado>().sistemaTurnos.heroesCombate[0].GetComponent<HeroeStateMachine>().heroe;

        panelEstadisticas.GetComponent<RellenarEstado>().seleccionable = true;
        panelEstadisticas.GetComponent<RellenarEstado>().Rellenar(caballero);
    }

    public void ListarEstado2()
    {
        panelEstadisticas.gameObject.SetActive(true);
        DesactivarComandos();
        otroMenu = true;

        //Desactivar comandos
        //HeroeBase heroe = GetComponent<SistemaEstado>().sistemaTurnos.heroesCombate[1].GetComponent<HeroeStateMachine>().heroe;

        panelEstadisticas.GetComponent<RellenarEstado>().seleccionable = true;
        panelEstadisticas.GetComponent<RellenarEstado>().Rellenar(arquera);
    }

    public void ListarEstado3()
    {
        panelEstadisticas.gameObject.SetActive(true);
        DesactivarComandos();
        otroMenu = true;

        //Desactivar comandos
        //HeroeBase heroe = GetComponent<SistemaEstado>().sistemaTurnos.heroesCombate[2].GetComponent<HeroeStateMachine>().heroe;

        panelEstadisticas.GetComponent<RellenarEstado>().seleccionable = true;
        panelEstadisticas.GetComponent<RellenarEstado>().Rellenar(hechicero);
    }

    public void ListarEstado4()
    {
        panelEstadisticas.gameObject.SetActive(true);
        DesactivarComandos();
        otroMenu = true;

        //Desactivar comandos
        //HeroeBase heroe = GetComponent<SistemaEstado>().sistemaTurnos.heroesCombate[3].GetComponent<HeroeStateMachine>().heroe;

        panelEstadisticas.GetComponent<RellenarEstado>().seleccionable = true;
        panelEstadisticas.GetComponent<RellenarEstado>().Rellenar(ingeniero);
    }
}
