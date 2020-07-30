using DG.Tweening;
using Saucy.Modules.XP;
using System.Collections;
using UnityEngine;

public class EnemigoStateMachine : MonoBehaviour
{
    /*** Sistema de turnos ***/
    public enum EstadoTurno
    {
        ESPERANDO,
        ACCION,
        MITURNO,
        MUERTO
    }

    public SistemaTurnos sistemaTurnos;
    //Estado del turno
    public EstadoTurno estadoTurno;

    /*** Fin de Sistema de turnos ***/

    //Datos del enemigo
    public EnemigoBase enemigo;
    public Vector3 posicionInicial;

    //Accion del enemigo
    public bool accionComenzada = false;
    private GameObject objetivo;
    private float velAnimacion = 30f;
    public HandleAction accion;

    //Animar Muerte
    public Material material;
    private float fade = 1f;
    

    private void OnMouseEnter()
    {
        GetComponent<TooltipTLK>().infoLeft = "<b>" + enemigo.nombre + "</b>"; 
        GetComponent<TooltipTLK>().infoRight = "@<b><smallcaps>hp</smallcaps><color=white>" + enemigo.vidaActual+ "</b>";
    }

    void Start()
    {
        sistemaTurnos = GameObject.Find("BattleHandler").GetComponent<SistemaTurnos>();
        posicionInicial = transform.position;
        material = GetComponent<SpriteRenderer>().material;
    }

    void Update()
    {

        if (estadoTurno.Equals(EstadoTurno.MUERTO) && fade <= 0f)
        {
            Destroy(this.gameObject);
        }

        if (enemigo.vidaActual<=0 && !estadoTurno.Equals(EstadoTurno.MUERTO))
        {
            estadoTurno = EstadoTurno.MUERTO;
            //Quitar de la lista de enemigos
            sistemaTurnos.ComprobarEstadoBatalla();
        }

        switch (estadoTurno)
        {
            case EstadoTurno.MITURNO:
                JugarTurno();
                break;
            case EstadoTurno.ACCION:
                StartCoroutine(RealizarAccion());
                estadoTurno = EstadoTurno.ESPERANDO;
                break;
            case EstadoTurno.ESPERANDO:
                //UpdateMeToca();
                estadoTurno = EstadoTurno.ESPERANDO;
                break;
            case EstadoTurno.MUERTO:
                material.SetFloat("_Fade", fade);
                fade -= Time.deltaTime;
                break;
        }
    }

    private void OnDestroy()
    {
        if (transform.Find("XP Granter (Enemy)") != null)
        {
            transform.Find("XP Granter (Enemy)").GetComponent<XPGranter>().GrantXP();
        }
    }

    public void MeToca()
    {
        estadoTurno = EstadoTurno.MITURNO;
    }

    private void JugarTurno()
    {
        //Creamos una accion para el enemigo
        accion = new HandleAction();
        accion.atacante = enemigo.nombre;
        accion.tipoAtacante = HandleAction.TipoAtacante.ENEMIGO;
        accion.atacanteGameObj = this.gameObject;
        accion.ataque = enemigo.ataque;

        //Seleccionamos un objetivo aleatorio para atacar
        int eleccion = Random.Range(0, sistemaTurnos.heroesCombate.Count);

        while(sistemaTurnos.heroesCombate[eleccion].CompareTag("Muerto"))
        {
            eleccion = Random.Range(0, sistemaTurnos.heroesCombate.Count);
        }

        objetivo = sistemaTurnos.heroesCombate[eleccion];
        accion.objetivoGameObj = objetivo;

        //Añadimos la accion al sistema de batalla
        sistemaTurnos.RealizarAccion(accion);
    }

    public void AcabarTurno()
    {
        //Colocamos la accion a espera
        estadoTurno = EstadoTurno.ESPERANDO;

        //Colocar al enemigo el ultimo del array
        var temp = sistemaTurnos.turnosPersonajes[0];

        for (int i = 0; i < sistemaTurnos.turnosPersonajes.Count-1; i++)
        {
            sistemaTurnos.turnosPersonajes[i] = sistemaTurnos.turnosPersonajes[i + 1]; 
        }

        sistemaTurnos.turnosPersonajes[sistemaTurnos.turnosPersonajes.Count-1] = temp;
    }

    public IEnumerator RealizarAccion()
    {
        if (accionComenzada)
            yield break;

        //Comenzamos la accion
        accionComenzada = true;

        yield return new WaitForSeconds(0.5f);

        if (accion.ataque.rango.Equals(AtaqueBase.Rango.CORTO))
        {
            //Para moverse
            //Mover enemigo cerca del personaje a atacar
            //Vector3 posicionPersonaje = new Vector3(
            //objetivo.transform.Find("Objetivo").transform.position.x + 10f,
            //objetivo.transform.Find("Objetivo").transform.position.y,
            //objetivo.transform.Find("Objetivo").transform.position.z);

            Vector3 posicionPersonaje = new Vector3(transform.position.x - 3f, transform.position.y, transform.position.z);

            while (MoverHaciaPosicion(this.gameObject, posicionPersonaje, velAnimacion))
            {
                yield return null;
            }

            //Volver a la posicion original
            while (MoverHaciaPosicion(this.gameObject, posicionInicial, velAnimacion))
            {
                yield return null;
            }

            if (accion.ataque.prefab != null)
            {
                GameObject projectile = Instantiate(accion.ataque.prefab, accion.objetivoGameObj.transform) as GameObject;

                projectile.transform.SetParent(accion.objetivoGameObj.transform.parent);
                projectile.transform.localScale = accion.ataque.prefab.transform.localScale;

                yield return new WaitForSeconds(0.5f);

                Destroy(projectile);
            }
        }
        else if (accion.ataque.rango.Equals(AtaqueBase.Rango.LARGO))
        {
            if (accion.ataque.prefab != null)
            {
                if (accion.ataque.seMueve)
                {
                    //lanzar prefab
                    GameObject projectile = Instantiate(accion.ataque.prefab, accion.atacanteGameObj.transform) as GameObject;

                    projectile.transform.right = (objetivo.transform.position - projectile.transform.position);

                    while (MoverHaciaPosicion(projectile, objetivo.transform.position, 50f))
                    {
                        yield return null;
                    }

                    Destroy(projectile);
                }
                else
                {
                    Vector3 posicionPersonaje = new Vector3(transform.position.x - 3f, transform.position.y, transform.position.z);

                    while (MoverHaciaPosicion(this.gameObject, posicionPersonaje, velAnimacion))
                    {
                        yield return null;
                    }

                    //Volver a la posicion original
                    while (MoverHaciaPosicion(this.gameObject, posicionInicial, velAnimacion))
                    {
                        yield return null;
                    }

                    GameObject projectile = Instantiate(accion.ataque.prefab, accion.objetivoGameObj.transform) as GameObject;

                    yield return new WaitForSeconds(2f);

                    Destroy(projectile);
                }

            }
        }

        //Hacer daño
        accion.damage = Mathf.RoundToInt(enemigo.fuerza / 2 + accion.ataque.damage);
        sistemaTurnos.sistemaDMG.HacerDMG(accion);

        yield return new WaitForSeconds(1.5f);

        //Eliminar accion de la lista de acciones
        sistemaTurnos.listaAcciones.RemoveAt(0);

        accionComenzada = false;
        //Reset sistemaTurnos -> Espera
        AcabarTurno();
    }

    private bool MoverHaciaPosicion(GameObject objeto, Vector3 objetivo, float velocidad)
    {
        objeto.transform.position = Vector3.MoveTowards(objeto.transform.position, objetivo, velocidad * Time.deltaTime);
        return (objetivo != objeto.transform.position);
    }

    public void AnimacionRecibirDMG()
    {
        transform.DOPunchPosition(new Vector3(0.5f, 0, 0), 1.5f, 3, 0.2f, false);
    }
}
