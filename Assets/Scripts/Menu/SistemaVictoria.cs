using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Saucy.Modules.XP;

public class SistemaVictoria : MonoBehaviour
{
    public SistemaTurnos sistemaTurnos;

    [SerializeField] private GameObject logoVictoria;
    [SerializeField] private GameObject logoDerrota;

    [Header("Estados")]
    [SerializeField] private GameObject estado1;
    [SerializeField] private GameObject estado2;
    [SerializeField] private GameObject estado3;
    [SerializeField] private GameObject estado4;

    [Header("Tipo de Final")]
    public string tipo;

    private void Start()
    {
        sistemaTurnos = GameObject.FindGameObjectWithTag("SistemaTurnos").GetComponent<SistemaTurnos>();
    }

    private void Update()
    {
        if(tipo!=null)
            Actualizar();
    }

    public void CargarPanelVictoria()
    {
        tipo = "victoria";

        Actualizar();

        logoVictoria.SetActive(false);

        transform.Find("PantallaResultados").gameObject.SetActive(true);

        int personaje = Random.Range(0, sistemaTurnos.heroesCombate.Count);

        transform.Find("PantallaResultados").Find("PanelAbajo").Find("LogoVictoriaMiniatura").gameObject.SetActive(true);
        transform.Find("PantallaResultados").Find("PanelAbajo").Find("Icon").GetComponent<SVGImage>().sprite = sistemaTurnos.heroesCombate[personaje].GetComponent<HeroeStateMachine>().heroe.icon;
        //Sustituir por la frase del heroe
        transform.Find("PantallaResultados").Find("PanelAbajo").Find("FraseVictoriaDerrota").Find("Texto").GetComponent<TextoAutomatico>().frase = sistemaTurnos.heroesCombate[personaje].GetComponent<HeroeStateMachine>().heroe.frasesVictoria[Random.Range(0,2)];
        StartCoroutine(transform.Find("PantallaResultados").Find("PanelAbajo").Find("FraseVictoriaDerrota").Find("Texto").GetComponent<TextoAutomatico>().IniciarTextoAutomatico());
    }

    public void CargarPanelDerrota()
    {
        tipo = "derrota";

        Actualizar();

        logoDerrota.SetActive(false);

        transform.Find("PantallaResultados").gameObject.SetActive(true);

        int personaje = Random.Range(0, 4);

        transform.Find("PantallaResultados").Find("PanelAbajo").Find("LogoDerrotaMiniatura").gameObject.SetActive(true);
        transform.Find("PantallaResultados").Find("PanelAbajo").Find("Icon").GetComponent<SVGImage>().sprite = sistemaTurnos.heroesCombate[personaje].GetComponent<HeroeStateMachine>().heroe.icon;
        //Sustituir por la frase del heroe
        transform.Find("PantallaResultados").Find("PanelAbajo").Find("FraseVictoriaDerrota").Find("Texto").GetComponent<TextoAutomatico>().frase = sistemaTurnos.heroesCombate[personaje].GetComponent<HeroeStateMachine>().heroe.frasesDerrota[Random.Range(0, 2)];
        StartCoroutine(transform.Find("PantallaResultados").Find("PanelAbajo").Find("FraseVictoriaDerrota").Find("Texto").GetComponent<TextoAutomatico>().IniciarTextoAutomatico());
    }

    private void Actualizar()
    {
        estado1.transform.Find("Nombre").GetComponent<TextMeshProUGUI>().text = sistemaTurnos.heroesCombate[0].GetComponent<HeroeStateMachine>().heroe.nombre;
        estado2.transform.Find("Nombre").GetComponent<TextMeshProUGUI>().text = sistemaTurnos.heroesCombate[1].GetComponent<HeroeStateMachine>().heroe.nombre;
        estado3.transform.Find("Nombre").GetComponent<TextMeshProUGUI>().text = sistemaTurnos.heroesCombate[2].GetComponent<HeroeStateMachine>().heroe.nombre;
        estado4.transform.Find("Nombre").GetComponent<TextMeshProUGUI>().text = sistemaTurnos.heroesCombate[3].GetComponent<HeroeStateMachine>().heroe.nombre;

        estado1.transform.Find("Nivel").GetComponent<TextMeshProUGUI>().text = "Nvl. " + sistemaTurnos.heroesCombate[0].GetComponent<HeroeStateMachine>().heroe.nivel;
        estado2.transform.Find("Nivel").GetComponent<TextMeshProUGUI>().text = "Nvl. " + sistemaTurnos.heroesCombate[1].GetComponent<HeroeStateMachine>().heroe.nivel;
        estado3.transform.Find("Nivel").GetComponent<TextMeshProUGUI>().text = "Nvl. " + sistemaTurnos.heroesCombate[2].GetComponent<HeroeStateMachine>().heroe.nivel;
        estado4.transform.Find("Nivel").GetComponent<TextMeshProUGUI>().text = "Nvl. " + sistemaTurnos.heroesCombate[3].GetComponent<HeroeStateMachine>().heroe.nivel;

        estado1.transform.Find("BarraExperiencia").Find("BarraExp").GetComponent<Image>().fillAmount = sistemaTurnos.heroesCombate[0].GetComponent<HeroeStateMachine>().heroe.cantidadXP;
        estado2.transform.Find("BarraExperiencia").Find("BarraExp").GetComponent<Image>().fillAmount = sistemaTurnos.heroesCombate[1].GetComponent<HeroeStateMachine>().heroe.cantidadXP;
        estado3.transform.Find("BarraExperiencia").Find("BarraExp").GetComponent<Image>().fillAmount = sistemaTurnos.heroesCombate[2].GetComponent<HeroeStateMachine>().heroe.cantidadXP;
        estado4.transform.Find("BarraExperiencia").Find("BarraExp").GetComponent<Image>().fillAmount = sistemaTurnos.heroesCombate[3].GetComponent<HeroeStateMachine>().heroe.cantidadXP;
    }

    public void Volver()
    {
        switch(tipo)
        {
            case "victoria":

                //Guardar Break
                sistemaTurnos.sistemaGuardado.SaveBreak(sistemaTurnos.sistemaMenus.sistemaBreak.cantidadBreak, sistemaTurnos.sistemaMenus.sistemaBreak.barra.fillAmount);

                //Guardar Heroes
                for (int i = 0; i < sistemaTurnos.heroesCombate.Count; i++)
                    sistemaTurnos.sistemaGuardado.SaveHeroe(sistemaTurnos.heroesCombate[i].GetComponent<HeroeStateMachine>().heroe);

                //Volver al mapa
                SceneManager.LoadScene("Mapa");
                break;
            case "derrota":
                //Cambiar por volver al menu principal
                SceneManager.LoadScene("MenuPrincipal");
                break;
        }
    }
}
