using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventario : MonoBehaviour
{
    [SerializeField] private List<ObjetoBase> objetos;
    [SerializeField] private List<ObjetoBase> listaInventario;

    [SerializeField] private List<ObjetoBase> listaTesoros;

    public List<ObjetoBase> Objetos { get => objetos; set => objetos = value; }
    public List<ObjetoBase> ListaInventario { get => listaInventario; set => listaInventario = value; }

    private static Inventario instancia;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instancia == null)
            instancia = this;
        else
            Destroy(gameObject);
    }

    public void CrearInventario()
    {
        foreach(ObjetoBase objeto in ListaInventario)
        {
            Objetos.Add(Instantiate(objeto));
        }
    }

    public void AddtoInvt(ObjetoBase objeto)
    {
        if(Objetos.Find((x => x.nombreObjeto.Equals(objeto.nombreObjeto))) != null)
        {
            Objetos.Find((x => x.nombreObjeto.Equals(objeto.nombreObjeto))).cantidad++;
        }
        else
        {
            Objetos.Add(Instantiate(objeto));
        }
    }

    public void RmvfromInvt(ObjetoBase objeto)
    {
        if (Objetos.Find((x => x.nombreObjeto.Equals(objeto.nombreObjeto))) != null)
        {
            Objetos.Remove(objeto);
        }
    }

    public List<ObjetoBase> GenerarTesoro()
    {
        List<ObjetoBase> tesoros = new List<ObjetoBase>();

        int cantidad = Random.Range(1, 5);

        for(int i=0;i<cantidad;i++)
        {
            tesoros.Add(listaTesoros[Random.Range(0, listaTesoros.Count)]);
        }

        return tesoros;
    }
}
