using UnityEngine;
using TigerForge;
using System.Collections.Generic;

public class SistemaGuardado : MonoBehaviour
{
    [SerializeField] private List<HeroeBase> listaHeroes;

    public void Reiniciar(List<HeroeBase> lista)
    {
        lista.RemoveRange(0, lista.Count);
        lista.AddRange(listaHeroes);
    }

    public void SaveOpciones(float valorMusica, float valorSFX)
    {
        EasyFileSave miarchivo = new EasyFileSave("Opciones");

        miarchivo.Add("Musica", valorMusica);
        miarchivo.Add("SFX", valorSFX);

        miarchivo.Save();
        Debug.Log("Opciones Guardado");
    }

    public void LoadOpciones()
    {
        EasyFileSave miarchivo = new EasyFileSave("Opciones");

        if (miarchivo.Load() && miarchivo.FileExists())
        {
            
            if(miarchivo.KeyExists("Musica"))
                GameObject.FindGameObjectWithTag("Musica").GetComponent<AudioSource>().volume = miarchivo.GetFloat("Musica");

            if (miarchivo.KeyExists("SFX"))
                GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>().volume = miarchivo.GetFloat("SFX");
            
            Debug.Log("Opciones Cargado");

            miarchivo.Dispose();
        }
        else
        {
            Debug.LogWarning("Fallo al cargar Opciones");
        }
    }

    public void SaveHeroe(HeroeBase heroe)
    {
        EasyFileSave miarchivo = new EasyFileSave(heroe.nombre);

        miarchivo.Add("nombre", heroe.nombre);
        miarchivo.Add("HeroeVidaActual", heroe.vidaActual);
        miarchivo.Add("HeroeVidaBase", heroe.vidaBase);
        miarchivo.Add("HeroeManaActual", heroe.manaActual);
        miarchivo.Add("HeroeManaBase", heroe.manaBase);
        miarchivo.Add("HeroeFuerza", heroe.fuerza);
        miarchivo.Add("HeroeInteligencia", heroe.inteligencia);
        miarchivo.Add("HeroeResistenciaF", heroe.resistenciaF);
        miarchivo.Add("HeroeResistenciaM", heroe.resistenciaM);
        miarchivo.Add("HeroeVelocidad", heroe.velocidad);

        miarchivo.Add("HeroeXP", heroe.cantidadXP);
        miarchivo.Add("HeroeNivel", heroe.nivel);

        miarchivo.Save();

        Debug.Log(heroe.nombre+" Guardado");
    }

    public HeroeBase LoadHeroe(HeroeBase heroe)
    {
        EasyFileSave miarchivo = new EasyFileSave(heroe.nombre);

        if(miarchivo.Load())
        {
            heroe.vidaActual = miarchivo.GetInt("HeroeVidaActual");
            heroe.vidaBase = miarchivo.GetInt("HeroeVidaBase");
            heroe.manaActual = miarchivo.GetInt("HeroeManaActual");
            heroe.manaBase = miarchivo.GetInt("HeroeManaBase");
            heroe.fuerza = miarchivo.GetInt("HeroeFuerza");
            heroe.inteligencia = miarchivo.GetInt("HeroeInteligencia");
            heroe.resistenciaF = miarchivo.GetInt("HeroeResistenciaF");
            heroe.resistenciaM = miarchivo.GetInt("HeroeResistenciaM");
            heroe.velocidad = miarchivo.GetInt("HeroeVelocidad");

            heroe.cantidadXP = miarchivo.GetFloat("HeroeXP");
            heroe.nivel = miarchivo.GetInt("HeroeNivel");

            Debug.Log(heroe.nombre + " Cargado");

            miarchivo.Dispose();
        }
        else
        {
            Debug.LogWarning("Fallo al cargar "+heroe.nombre);
        }

        return heroe;
    }

    public void SaveBreak(int cantidad, float barra)
    {
        EasyFileSave miarchivo = new EasyFileSave("Break");

        miarchivo.Add("barra", barra);
        miarchivo.Add("cantidad", cantidad);

        miarchivo.Save();
    }

    public void LoadBreak(SistemaBreak sistemaBreak)
    {
        EasyFileSave miarchivo = new EasyFileSave("Break");

        if(miarchivo.Load())
        {
            sistemaBreak.cantidadBreak = miarchivo.GetInt("cantidad");
            sistemaBreak.barra.fillAmount = miarchivo.GetFloat("barra");
            sistemaBreak.Actualizar();
        }
        else
        {
            Debug.LogWarning("Fallo al cargar Sistema Break");
        }
    }
}
