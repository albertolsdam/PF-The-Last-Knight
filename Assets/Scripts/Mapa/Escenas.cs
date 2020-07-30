using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Escenas : MonoBehaviour
{
    public List<Escenario> escenarios;
    public Escenario escenarioActual;
    public int numEscenarioActual;

    public string menuPrincipal;
    public string mapa;
    public bool nuevaRun = false;
    public bool cargar = false;

    private static Escenas instancia;

    //Clip para el nodo del mapa
    public AudioClip clipCirculo;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instancia == null)
            instancia = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        escenarioActual = escenarios[0];
        numEscenarioActual = 1;
    }

    public IEnumerator CargarEscena(string scene)
    {
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(scene);

        yield return null;
    }

    public void SiguienteEscenario()
    {
        if((numEscenarioActual+1)<=escenarios.Count)
        {
            numEscenarioActual++;
            escenarioActual = escenarios[numEscenarioActual - 1];
        }
    }

    public void VolverAlMenuPrincipal()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void TerminarRun()
    {
        PlayerPrefs.DeleteKey("Mapa");
        PlayerPrefs.DeleteKey("MapaAntEst");
        SceneManager.LoadScene("MenuPrincipal");
    }

    public bool EsUltimaEscena()
    {
        return (numEscenarioActual == escenarios.Count);
    }
}
