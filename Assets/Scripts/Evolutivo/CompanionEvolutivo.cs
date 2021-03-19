using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CompanionEvolutivo : MonoBehaviour {
    public AlgoritmoEvolutivo evolutivo;
    public float puntosporSegundoVivo = 1;
    public float puntosPorCuracion = 1;
    public float puntosPorPeligros = 1;
    public float puntosPorEliminacion = 1;
    public float puntosPorMovimiento = 0.002f;
    public float castigoPeligro=-0.002f;
    public float castigoPorColision=-0.002f;
    public GridAI DeteccionDeEntorno;
    public IAPlayer companion;

    private float puntosSegundosDeVida;
    private float puntosCuracion;
    private float puntosPeligros;
    private float puntosEliminacion;
    private float puntosMovimiento;
    private GenomaNEAT redNeural;

    public int individuo = 0;

    private float timer = 0.0f;

   //Metodos
    private void Start()
    {
        evolutivo = GameObject.FindGameObjectWithTag("Evolutivo").GetComponent<AlgoritmoEvolutivo>();
        
        //Invoke("Jugar", 0.2f);
    }
    private void Jugar() {
        float[] salida = redNeural.EvaluarEntorno();
        ProcesarOutput(salida);
        Evaluar();
        evolutivo.calificarUno(individuo);
        
        
    }
    
    private void Update()
    {
        timer += Time.deltaTime;
        /*
        float[] s = new float[8];
        for (int i = 0; i < 8; i++) {
            s[i] = 0;
        }
        s[5] = 1;
        ProcesarOutput(s);
        */
    }
   
    public void Evaluar() {
        evolutivo.resultados[individuo, 0] = puntosSegundosDeVida;
        evolutivo.resultados[individuo, 1] = puntosCuracion;
        evolutivo.resultados[individuo, 2] = puntosPeligros;
        evolutivo.resultados[individuo, 3] = puntosEliminacion;
        evolutivo.resultados[individuo, 4] = puntosMovimiento;
    }


    public void ProcesarOutput(float[] ValoresDeConfianza) {
        for (int i = 0; i < ValoresDeConfianza.Length; i++) {
            if (float.IsNaN(ValoresDeConfianza[i])) {
                ValoresDeConfianza[i] = 0;
            }
        }

        float MejorValor=Mathf.Max(ValoresDeConfianza);
        String prueba="[";
        for (int i = 0; i < ValoresDeConfianza.Length; i++) {
            prueba += " " + ValoresDeConfianza[i] + ",";
        }
        prueba += "]";
        int Decision=Array.IndexOf(ValoresDeConfianza, MejorValor);
        //Debug.Log("Decision: "+ Decision+" Mejor valor"+MejorValor+"valores: "+prueba);
        if (Decision == 7 && timer % 10 > 0.5) {
            Decision = UnityEngine.Random.Range(0,6);
        }
        switch (Decision) {
            case 0:
                companion.Rotar45N();
                aplicarPuntos((int)Categoria.MOVIMIENTO);
                break;
            case 1:
                companion.Adelante();
                aplicarPuntos((int)Categoria.MOVIMIENTO);
                break;
            case 2:
                companion.Derecha();
                aplicarPuntos((int)Categoria.MOVIMIENTO);
                break;
            case 3:
                companion.Atras();
                aplicarPuntos((int)Categoria.MOVIMIENTO);
                break;
            
            case 4:
                companion.Izquierda();
                aplicarPuntos((int)Categoria.MOVIMIENTO);
                break;
            case 5:
                companion.Rotar45();
                aplicarPuntos((int)Categoria.MOVIMIENTO);
                break;
            case 6:
                companion.Disparar();
                break;
            case 7:
                companion.curar();
                aplicarPuntos((int)Categoria.CURACION);
                break;

        }

        DeteccionDeEntorno.RefreshGrid();
    }

    public void aplicarPuntos(int categoria) {

        switch (categoria)
        {
            case 0:
                puntosSegundosDeVida = puntosSegundosDeVida * puntosporSegundoVivo;
                break;
            case 1:
                puntosCuracion += puntosPorCuracion;
                break;
            case 2:
                puntosPeligros += puntosPorPeligros;
                break;
            case 3:
                puntosEliminacion += puntosPorEliminacion;
                break;
            case 4:
                puntosMovimiento += puntosPorMovimiento;
                break;
        }
    }
    public void aplicarCastigo(int categoria) {

        switch (categoria) {
            case 2:
                puntosPeligros += castigoPeligro;
                break;
            case 4:
                puntosPorMovimiento += castigoPorColision;
                break;
        }
    }
    public enum Categoria : int
    {
        TIEMPO,
        CURACION,
        PELIGROS,
        ELIMINACIONES,
        MOVIMIENTO
    }


    //Setters
    public void setNumeroIndividuo(int individuo)
    {
        this.individuo = individuo;
    }
    public void setTiempoVida(float timer)
    {
        puntosSegundosDeVida = timer;
        aplicarPuntos((int)Categoria.TIEMPO);
    }
    public void setRed(GenomaNEAT nuevaRed) {
        redNeural = nuevaRed;
        redNeural.vision = DeteccionDeEntorno;
        redNeural.GenerarRed(evolutivo.maximoNeuronas);
        DeteccionDeEntorno.transform.SetParent(companion.transform);
        //Debug.Log("Ha jugar!");
        InvokeRepeating("Jugar", 0.2f, 0.2f);
    }

}
