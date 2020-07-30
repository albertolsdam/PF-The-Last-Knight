using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SistemaOpciones : MonoBehaviour
{
    [SerializeField] private GameObject musicOpt;
    [SerializeField] private GameObject effectOpt;

    [SerializeField] private GameObject musicHandler;
    [SerializeField] private GameObject sfxHandler;
    [SerializeField] private bool seleccionable;

    public MapManager mapManager;

    public void Inicializar()
    {
        musicHandler = GameObject.FindGameObjectWithTag("Musica");
        sfxHandler = GameObject.FindGameObjectWithTag("Audio");

        musicOpt.transform.Find("Slider").GetComponent<Slider>().value = musicHandler.GetComponent<AudioSource>().volume;
        effectOpt.transform.Find("Slider").GetComponent<Slider>().value = sfxHandler.GetComponent<AudioSource>().volume;

        seleccionable = true;
    }

    private void Update()
    {
        if(seleccionable)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                ResetMenuOpciones();
            }
        }
    }

    public void ActualizarValores()
    {
        musicOpt.transform.Find("Valor").GetComponent<TextMeshProUGUI>().text = ""+ System.Math.Round(musicOpt.transform.Find("Slider").GetComponent<Slider>().value,2);
        effectOpt.transform.Find("Valor").GetComponent<TextMeshProUGUI>().text = "" + System.Math.Round(effectOpt.transform.Find("Slider").GetComponent<Slider>().value,2);
    }

    public void MenuPrincipal()
    {
        mapManager.SaveMap("MapaAntEst");
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void ResetMenuOpciones()
    {
        seleccionable = false;

        if (SceneManager.GetActiveScene().name.Equals("MenuPrincipal"))
        {
            GameObject.FindGameObjectWithTag("Escenas").GetComponent<SistemaGuardado>().SaveOpciones((float)System.Math.Round(musicOpt.transform.Find("Slider").GetComponent<Slider>().value, 2), (float)System.Math.Round(effectOpt.transform.Find("Slider").GetComponent<Slider>().value, 2));
            gameObject.SetActive(false);

            transform.GetComponentInParent<MenuPrincipal>().ActivarComandos();
        }
        else if(SceneManager.GetActiveScene().name.Equals("Mapa"))
        {
            GameObject.FindGameObjectWithTag("Escenas").GetComponent<SistemaGuardado>().SaveOpciones((float)System.Math.Round(musicOpt.transform.Find("Slider").GetComponent<Slider>().value, 2), (float)System.Math.Round(effectOpt.transform.Find("Slider").GetComponent<Slider>().value, 2));
            GameObject.FindGameObjectWithTag("Opciones").GetComponent<EnMapa>().enOpciones = false;
            gameObject.SetActive(false);
        }
        else
        {
            //QuickSave de Opciones
            GameObject.Find("BattleHandler").GetComponent<SistemaTurnos>().sistemaGuardado.SaveOpciones((float)System.Math.Round(musicOpt.transform.Find("Slider").GetComponent<Slider>().value, 2), (float)System.Math.Round(effectOpt.transform.Find("Slider").GetComponent<Slider>().value, 2));
            transform.parent.GetComponentInParent<SistemaEstado>().ActivarComandos();
            gameObject.SetActive(false);

            transform.parent.GetComponentInParent<SistemaEstado>().otroMenu = false;
        }
    }
}
