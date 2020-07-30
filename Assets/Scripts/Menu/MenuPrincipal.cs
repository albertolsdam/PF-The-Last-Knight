using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public GameObject panelOpciones;
    public GameObject panelComandos;
    public GameObject creditos;

    private void Start()
    {
        ActivarComandos();
        GameObject.FindGameObjectWithTag("Escenas").GetComponent<SistemaGuardado>().LoadOpciones();
    }

    public void ActivarComandos()
    {
        for (int i = 0; i < panelComandos.transform.childCount; i++)
        {
            panelComandos.transform.GetChild(i).GetComponent<SelectCommand>().Activar();
        }
    }

    public void DesactivarComandos()
    {
        for (int i = 0; i < panelComandos.transform.childCount; i++)
        {
            panelComandos.transform.GetChild(i).GetComponent<SelectCommand>().Desactivar();
        }
    }

    public void IniciarNuevaRun()
    {
        GameObject.FindGameObjectWithTag("Escenas").GetComponent<Escenas>().cargar = false;
        GameObject.FindGameObjectWithTag("Escenas").GetComponent<Escenas>().nuevaRun = true;
        SceneManager.LoadScene("Mapa");
    }

    public void CargarRun()
    {
        if(PlayerPrefs.HasKey("MapaAntEst"))
        {
            GameObject.FindGameObjectWithTag("Escenas").GetComponent<Escenas>().cargar = true;
            GameObject.FindGameObjectWithTag("Escenas").GetComponent<Escenas>().nuevaRun = false;
            SceneManager.LoadScene("Mapa");
        }
    }

    public void MostrarOpciones()
    {
        DesactivarComandos();
        panelOpciones.SetActive(true);
        panelOpciones.GetComponent<SistemaOpciones>().Inicializar();
    }

    public void MostrarCreditos()
    {
        DesactivarComandos();

        creditos.transform.Find("Texto").transform.localPosition = Vector3.zero;

        creditos.SetActive(true);

        StartCoroutine(MoverCreditos());
    }

    private IEnumerator MoverCreditos()
    {
        yield return new WaitForSeconds(2f);

        Vector3 objetivo = new Vector3(0, 4975, 0);

        while (MoverHaciaPosicion(creditos.transform.Find("Texto").gameObject, objetivo, 2))
        {
            yield return null;
        }

        yield return new WaitForSeconds(3f);

        creditos.SetActive(false);

        ActivarComandos();
    }

    private bool MoverHaciaPosicion(GameObject objeto, Vector3 objetivo, float velocidad)
    {
        Vector3 temp = Vector3.MoveTowards(objeto.transform.position, objetivo, velocidad * Time.deltaTime);

        objeto.transform.position = new Vector3(objeto.transform.position.x, temp.y, objeto.transform.position.z);

        return (objetivo.y >  objeto.transform.localPosition.y);
    }

    public void Salir()
    {
        Application.Quit();
    }
}
