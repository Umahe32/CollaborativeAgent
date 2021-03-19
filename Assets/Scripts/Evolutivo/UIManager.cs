using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static int generacion;       
    public static int individuo;
    public static int timer;
    public static int especie;
    public static float fitnessActual;
    public static float puntaje;
    Text textGeneracion;                      


    void Awake()
    {
        textGeneracion = GetComponent<Text>();

        // Reset the score.
        generacion = 1;
        individuo = 1;
        timer = 60;
        especie = 1;
        fitnessActual = 0.0f;
    }


    void Update()
    {
        // Set the displayed text to be the word "Score" followed by the score value.
        textGeneracion.text = "Generacion: " + generacion +"\t Individuo: "+individuo+ "\t Especie: " + especie + "\t Timer: " + timer + "\t Fitness: " +fitnessActual.ToString("0.0000") + "\t Puntaje: " + fitnessActual.ToString("0.00");
    }
}
