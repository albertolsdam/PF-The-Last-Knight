using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HeroeStateMachine : MonoBehaviour
{
    //Estado del turno del jugador
    public enum EstadoTurno
    {
        ESPERANDO,
        ACCION,
        MITURNO,
        MUERTO,
    }

    //Acciones del jugador
    public enum HeroeInputMenu
    {
        NINGUNO,
        ATACAR,
        DEFENDER,
        MAGIA,
        OBJETOS,
        ESTADO,
        HUIR
    }

    public SistemaTurnos sistemaTurnos;
    public SistemaMenus sistemaMenus;

    //Estadisticas del hero
    public HeroeBase heroe;
    public Dictionary<string, int> heroeBackup; //Arreglar
    public Vector3 posicionInicial;

    //Estado del turno
    public EstadoTurno estadoTurno;
    public bool turnoBreak=false;

    //Estado del menu
    public HeroeInputMenu estadoMenu;
    private bool accionComenzada;

    //Accion
    public HandleAction ataque;
    private float velAnimacion = 50f;

    void Start()
    {
        heroeBackup = new Dictionary<string, int>();
        BackupEstadisticas();

        sistemaTurnos = GameObject.Find("BattleHandler").GetComponent<SistemaTurnos>();
        sistemaMenus = GameObject.Find("BattleHandler").GetComponent<SistemaMenus>();
        estadoMenu = HeroeInputMenu.NINGUNO;
        estadoTurno = EstadoTurno.ESPERANDO;
        posicionInicial = transform.position;
    }

    private void BackupEstadisticas()
    {
        heroeBackup.Add("vidaActual", heroe.vidaActual);
        heroeBackup.Add("manaActual", heroe.manaActual);
        heroeBackup.Add("resistenciaF", heroe.resistenciaF);
        heroeBackup.Add("resistenciaM", heroe.resistenciaM);
        heroeBackup.Add("velocidad", heroe.velocidad);
        heroeBackup.Add("fuerza", heroe.fuerza);
        heroeBackup.Add("inteligencia", heroe.inteligencia);
    }

    void Update()
    {
        if (heroe.vidaActual <= 0)
            Muerte();
    }

    public void MeToca()
    {
        estadoTurno = EstadoTurno.MITURNO;
        ResetEstadisticas();

        //Nuevas Modificaciones
        sistemaMenus.HSM = this;
        sistemaMenus.VolverMenu();
    }

    private void ResetEstadisticas()
    {
        heroe.resistenciaF = heroeBackup["resistenciaF"];
        heroe.resistenciaM = heroeBackup["resistenciaM"];
        heroe.velocidad = heroeBackup["velocidad"];
        heroe.fuerza = heroeBackup["fuerza"];
        heroe.inteligencia = heroeBackup["inteligencia"];
    }

    public void Atacar(GameObject enemigo)
    {
        ataque = new HandleAction();
        ataque.atacante = heroe.nombre;
        ataque.tipoAtacante = HandleAction.TipoAtacante.PERSONAJE;
        ataque.atacanteGameObj = this.gameObject;
        ataque.ataque = heroe.ataque;

        //Seleccionamos el objetivo
        ataque.objetivoGameObj = enemigo;

        //Añadimos la accion al sistema de batalla
        sistemaTurnos.RealizarAccion(ataque);

        StartCoroutine(Accion(enemigo));

        estadoTurno = EstadoTurno.ESPERANDO;
    }

    public void Defender()
    {
        float resistenciaF = heroe.resistenciaF;
        estadoMenu = HeroeInputMenu.DEFENDER;

        resistenciaF *= 1.5f;

        Mathf.RoundToInt(resistenciaF);
        heroe.resistenciaF = (int)resistenciaF;
        AcabarTurno();
    }

    public void LanzarMagia(AtaqueBase magia, GameObject enemigo)
    {
        ataque = new HandleAction();
        ataque.atacante = heroe.nombre;
        ataque.tipoAtacante = HandleAction.TipoAtacante.PERSONAJE;
        ataque.atacanteGameObj = gameObject;
        ataque.ataque = magia;

        //Seleccionamos el objetivo
        ataque.objetivoGameObj = enemigo;

        //Eliminar Mana
        heroe.manaActual -= magia.coste;
        sistemaTurnos.sistemaDMG.GastarMana();

        //Añadimos la accion al sistema de batalla
        sistemaTurnos.RealizarAccion(ataque);

        StartCoroutine(Accion(enemigo));
    }

    public void LanzarObjeto(ObjetoBase objeto, GameObject objetivo)
    {
        //Hacer animacion objeto
        StartCoroutine(AccionObjeto(objetivo, objeto));

        if (objeto.tipoAccion.Equals(ObjetoBase.TipoAccion.CURAR))
        {
            sistemaTurnos.sistemaDMG.Curar(objeto.cantidadEfecto, objetivo);
        }
        else if (objeto.tipoAccion.Equals(ObjetoBase.TipoAccion.MANA))
        {
            sistemaTurnos.sistemaDMG.RestablecerMana(objeto.cantidadEfecto, objetivo);
        }
        else if (objeto.tipoAccion.Equals(ObjetoBase.TipoAccion.HACERDMG))
        {
            sistemaTurnos.sistemaDMG.HacerDMGaEnemigo(objeto, objetivo);
        }
        else if(objeto.tipoAccion.Equals(ObjetoBase.TipoAccion.REVIVIR))
        {
            if (objetivo.CompareTag("Muerto"))
            {
                objetivo.GetComponent<HeroeStateMachine>().Revivir();
                sistemaTurnos.sistemaDMG.Curar(Mathf.RoundToInt(objetivo.GetComponent<HeroeStateMachine>().heroe.vidaBase/2), objetivo);
            }
        }

        objeto.cantidad -= 1;

        if (objeto.cantidad == 0)
            sistemaTurnos.inventario.RmvfromInvt(objeto);

        AcabarTurno();
    }

    public IEnumerator AccionObjeto(GameObject objetivo, ObjetoBase objeto)
    {
        if (accionComenzada)
            yield break;

        //Comenzamos la accion
        accionComenzada = true;

        if (objeto.prefab != null)
        {
            GameObject projectile = Instantiate(objeto.prefab, objetivo.transform) as GameObject;

            projectile.transform.SetParent(objetivo.transform.parent);
            projectile.transform.localScale = objeto.prefab.transform.localScale;

            yield return new WaitForSeconds(2f);

            Destroy(projectile);
        }
        else
        {
            yield return new WaitForSeconds(2f);
        }

        //Esperar
        yield return new WaitForSeconds(1.5f);

        accionComenzada = false;
    }

    public IEnumerator Accion(GameObject objetivo)
    {
        if (accionComenzada)
            yield break;

        //Comenzamos la accion
        accionComenzada = true;

        estadoTurno = EstadoTurno.ACCION;

        if (ataque.ataque.rango.Equals(AtaqueBase.Rango.CORTO))
        {
            //Si el ataca es de corto rango se mueve hasta el objetivo

            //Mover enemigo cerca del personaje a atacar
            //Vector3 posicionPersonaje = new Vector3(objetivo.transform.position.x - 15f, objetivo.transform.position.y, objetivo.transform.position.z);

            Vector3 posicionPersonaje = new Vector3(transform.position.x + 3f, transform.position.y, transform.position.z);

            while (MoverHaciaPosicion(this.gameObject, posicionPersonaje, velAnimacion))
            {
                yield return null;
            }

            //Volver a la posicion original
            while (MoverHaciaPosicion(this.gameObject, posicionInicial, velAnimacion))
            {
                yield return null;
            }

            if (ataque.ataque.prefab != null)
            {
                GameObject projectile = Instantiate(ataque.ataque.prefab, ataque.objetivoGameObj.transform) as GameObject;

                projectile.transform.SetParent(ataque.objetivoGameObj.transform.parent);
                projectile.transform.localScale = ataque.ataque.prefab.transform.localScale;

                yield return new WaitForSeconds(0.5f);

                Destroy(projectile);
            }
        }
        else if(ataque.ataque.rango.Equals(AtaqueBase.Rango.LARGO))
        {
            if (ataque.ataque.prefab != null)
            {
                if (ataque.ataque.seMueve)
                {
                    //lanzar prefab
                    GameObject projectile = Instantiate(ataque.ataque.prefab, ataque.atacanteGameObj.transform) as GameObject;

                    projectile.transform.right = (objetivo.transform.position - projectile.transform.position);

                    while (MoverHaciaPosicion(projectile, objetivo.transform.position, 50f))
                    {
                        yield return null;
                    }

                    Destroy(projectile);
                }
                else
                {
                    Vector3 posicionPersonaje = new Vector3(transform.position.x + 3f, transform.position.y, transform.position.z);

                    while (MoverHaciaPosicion(this.gameObject, posicionPersonaje, velAnimacion))
                    {
                        yield return null;
                    }

                    //Volver a la posicion original
                    while (MoverHaciaPosicion(this.gameObject, posicionInicial, velAnimacion))
                    {
                        yield return null;
                    }

                    GameObject projectile = Instantiate(ataque.ataque.prefab, ataque.objetivoGameObj.transform) as GameObject;

                    Destroy(projectile, 1.5f);
                }
            }
        }


        //Hacer daño
        sistemaTurnos.sistemaDMG.HacerDMG(ataque);

        //Esperar
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

    public void AcabarTurno()
    {
        //Desactivar menu de comandos
        sistemaMenus.DesactivarMenu();
        estadoMenu = HeroeInputMenu.NINGUNO;

        //Colocamos la accion a espera
        estadoTurno = EstadoTurno.ESPERANDO;

        if (turnoBreak)
        {
            turnoBreak = false;
        }
        else
        {
            //Colocar al personaje el ultimo del array
            var temp = sistemaTurnos.turnosPersonajes[0];

            for (int i = 0; i < sistemaTurnos.turnosPersonajes.Count - 1; i++)
            {
                sistemaTurnos.turnosPersonajes[i] = sistemaTurnos.turnosPersonajes[i + 1];
            }

            sistemaTurnos.turnosPersonajes[sistemaTurnos.turnosPersonajes.Count - 1] = temp;
        }
    }

    public void AnimacionRecibirDMG()
    {
        transform.DOPunchPosition(new Vector3(-0.5f,0,0), 1.5f, 3, 0.2f, false);
    }

    public void Muerte()
    {
        transform.tag = "Muerto";
        estadoTurno = EstadoTurno.MUERTO;
        //Cambiar shader a gris
        this.gameObject.GetComponent<SpriteRenderer>().material.color = Color.gray;
        this.gameObject.transform.Find("Death").gameObject.SetActive(true);

        //sistemaTurnos.heroesCombate.Remove(this.gameObject);
        sistemaTurnos.turnosPersonajes.Remove(this.gameObject);
    }

    public void Revivir()
    {
        transform.tag = "Player";
        estadoTurno = EstadoTurno.ESPERANDO;
        //Cambiar shader a normal
        this.gameObject.GetComponent<SpriteRenderer>().material.color = Color.white;
        this.gameObject.transform.Find("Death").gameObject.SetActive(false);

        //sistemaTurnos.heroesCombate.Add(this.gameObject);
        sistemaTurnos.turnosPersonajes.Add(this.gameObject);
    }

    public void SubirNivel()
    {
        heroe.nivel++;

        heroe.vidaBase += heroe.listaAumentos[0];
        heroe.manaBase += heroe.listaAumentos[1];

        heroe.fuerza += heroe.listaAumentos[2];
        heroe.inteligencia += heroe.listaAumentos[3];
        heroe.velocidad += heroe.listaAumentos[4]; ;
        heroe.resistenciaF += heroe.listaAumentos[5];
        heroe.resistenciaM += heroe.listaAumentos[6];

        heroe.vidaActual = heroe.vidaBase;
        heroe.manaActual = heroe.manaBase;

        //Spawn aumento nivel
    }
}
