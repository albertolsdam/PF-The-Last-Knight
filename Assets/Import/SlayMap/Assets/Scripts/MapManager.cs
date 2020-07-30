using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class MapManager : MonoBehaviour
{
    public MapConfig config;
    public MapView view;

    public Map CurrentMap { get; private set; }

    public GameObject textoEscenario;

    public GameObject contenidoVictoria;

    private void Start()
    {
        StartCoroutine(SetUp());
    }

    public IEnumerator SetUp() //The Last Knight
    {
        if (PlayerPrefs.HasKey("MapaAntEst") && GameObject.FindGameObjectWithTag("Escenas").GetComponent<Escenas>().cargar)
        {
            GameObject.FindGameObjectWithTag("Escenas").GetComponent<Escenas>().cargar = false;

            var mapJson = PlayerPrefs.GetString("MapaAntEst");
            var map = JsonConvert.DeserializeObject<Map>(mapJson);

            textoEscenario.GetComponent<CargarEscenario>().Cargar(); //The Last Knight

            yield return new WaitForSeconds(1f);

            CurrentMap = map;
            // player has not reached the boss yet, load the current map
            view.ShowMap(map);

        }
        else if (PlayerPrefs.HasKey("Map") && !GameObject.FindGameObjectWithTag("Escenas").GetComponent<Escenas>().nuevaRun)
        {
            var mapJson = PlayerPrefs.GetString("Map");
            var map = JsonConvert.DeserializeObject<Map>(mapJson);

            if((map.path.Any(p => p.Equals(map.GetBossNode().point))) && GameObject.FindGameObjectWithTag("Escenas").GetComponent<Escenas>().EsUltimaEscena())
            {
                contenidoVictoria.SetActive(true);

                //Mover los objetos
                MoverLogoVictoria();
            }
            // using this instead of .Contains()
            else if (map.path.Any(p => p.Equals(map.GetBossNode().point)))
            {
                //Cargar siguiente escenario
                GameObject.FindGameObjectWithTag("Escenas").GetComponent<Escenas>().SiguienteEscenario(); //The Last Knight
                textoEscenario.GetComponent<CargarEscenario>().Cargar(); //The Last Knight

                yield return new WaitForSeconds(2f);

                // payer has already reached the boss, generate a new map
                GenerateNewMap();
            }
            else
            {
                textoEscenario.GetComponent<CargarEscenario>().Cargar(); //The Last Knight

                yield return new WaitForSeconds(1f);

                CurrentMap = map;
                // player has not reached the boss yet, load the current map
                view.ShowMap(map);
            }
        }
        else
        {
            textoEscenario.GetComponent<CargarEscenario>().Cargar(); //The Last Knight

            yield return new WaitForSeconds(2f);

            GenerateNewMap();
        }

        yield return null;
    }

    private void MoverLogoVictoria()
    {
        contenidoVictoria.transform.Find("LogoVictoriaFinal").transform.Find("EspadaIzq").transform.DOMove(new Vector3(0, 0.5f, 0), 1);
        contenidoVictoria.transform.Find("LogoVictoriaFinal").transform.Find("EspadaDer").transform.DOMove(new Vector3(0, 0.5f, 0), 1);
        contenidoVictoria.transform.Find("LogoVictoriaFinal").transform.Find("TextoVictoria").transform.DOMove(new Vector3(0, 0.5f, 0), 1);
        contenidoVictoria.transform.Find("LogoVictoriaFinal").transform.Find("Boton").transform.DOMove(new Vector3(0,0.5f,0), 1);

        contenidoVictoria.transform.Find("LogoVictoriaFinal").transform.Find("Boton").Find("BotonContinuar").GetComponent<Button>().onClick.AddListener(GameObject.FindGameObjectWithTag("Escenas").GetComponent<Escenas>().TerminarRun);
    }

    public void GenerateNewMap()
    {
        var map = MapGenerator.GetMap(view.allMapConfigs[UnityEngine.Random.Range(0, view.allMapConfigs.Count)]);
        CurrentMap = map;
        Debug.Log(map.ToJson());
        view.ShowMap(map);
    }

    public void SaveMap()
    {
        if(CurrentMap == null) return;

        var json = JsonConvert.SerializeObject(CurrentMap);

        PlayerPrefs.SetString("Map", CurrentMap.ToJson());
        PlayerPrefs.Save();
    }

    public void SaveMap(string tipo)
    {
        if (CurrentMap == null) return;

        var json = JsonConvert.SerializeObject(CurrentMap);

        PlayerPrefs.SetString(tipo, CurrentMap.ToJson());
        PlayerPrefs.Save();
    }

    private void OnApplicationQuit()
    {
        SaveMap();
    }
}
