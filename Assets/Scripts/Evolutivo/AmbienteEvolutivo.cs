using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienteEvolutivo : MonoBehaviour
{
    public CompanionEvolutivo companion;
    public AlgoritmoEvolutivo evolutivo;
    PlayerHealth saludJugador;
    public float tiempoDePrueba;
    public float tiempoPruebaOriginal;
    public bool reiniciar=false;
    public bool listo = false;
    public bool termine=false;

    void Start()
    {
        companion= BusquedaHijos.buscarHijoPorTag(gameObject, "Companion").GetComponent<CompanionEvolutivo>();
        saludJugador = BusquedaHijos.buscarHijoPorTag(gameObject, "Player").GetComponent<PlayerHealth>();
        evolutivo= GameObject.FindGameObjectWithTag("Evolutivo").GetComponent<AlgoritmoEvolutivo>();
    }

    public void NuevoTimer(float newTimer)
    {
        tiempoPruebaOriginal = newTimer;
        tiempoDePrueba = newTimer;
        listo = true;
    }


    // Update is called once per frame
    void Update()
    {
        if (listo)
        {
            if (tiempoDePrueba > 0 && saludJugador.currentHealth > 0)
            {
                tiempoDePrueba -= Time.deltaTime;
            }
            else
            {
                float tiempoTotal = tiempoPruebaOriginal - tiempoDePrueba;
                companion.setTiempoVida(tiempoTotal);
                companion.Evaluar();
                //reiniciar = true;
                termine = true;
                listo = false;
            }
        }
        
    }
}
