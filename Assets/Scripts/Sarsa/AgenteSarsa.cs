using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class AgenteInterno : Agente{

    /*
    public float[][] q_tabla;   // Matriz que contiene los valores estimados.
    float tasa_aprendizaje = 0.5f; // Tasa de actualización de estimaciones por valor dada una recompensa. 
    int accion = -1;
    float gamma = 0.99f; // Factor de descuento para calcular Q-Objetivo.
    float epsilon = 1; // Epsilon inicial para la selección de acciones aleatorias.
    float eMin = 0.1f; // Límite inferior de épsilon.
    int recuentoPasos = 2000; // Número de pasos para bajar Epsilon a eMin.
    int ultimoEstado;

    public override void EnviarParametros(ParametrosAmbiente env)
    {
        q_tabla = new float[env.estados_tamaño][];
        accion = 0;
        for (int i = 0; i < env.estados_tamaño; i++)
        {
            q_tabla[i] = new float[env.accion_tamaño];
            for (int j = 0; j < env.accion_tamaño; j++)
            {
                q_tabla[i][j] = 0.0f;
            }
        }
    }

    /// <summary> 
    /// Selecciona una acción para realizar desde su estado actual. 
    /// </summary> 
    /// <returns> La opción elegida por la política del agente </returns> 

    public override float[] ObtenerAccion()
    {
        //accion = q_tabla[ultimoEstado].ToList().IndexOf(q_tabla[ultimoEstado].Max());
        if (Random.Range(0f, 1f) < epsilon) { accion = Random.Range(0, 3); }
        if (epsilon > eMin) { epsilon = epsilon - ((1f - eMin) / (float)recuentoPasos); }
        //GameObject.Find("ETxt").GetComponent<Text>().text = "Epsilon: " + epsilon.ToString("F2");
        float currentQ = q_tabla[ultimoEstado][accion];
        //GameObject.Find("QTxt").GetComponent<Text>().text = "Current Q-value: " + currentQ.ToString("F2");
        return new float[1] { accion };
    }


    /// <resumen>
    /// Actualiza la matriz de estimación de valor dada una nueva experiencia (estado, acción, recompensa).
    /// </summary>
    /// <param name = "estado"> El estado del entorno en el que ocurrió la experiencia. </param>
    /// <param name = "recompensa"> La recompensa recibida por el agente del entorno por su acción. </param>
    /// <param name = "hecho"> Si el episodio ha terminado </param>

    public override void EnviarEstado(List<float> estado, float recompesa, bool hecho)
    {
        //int nextState = Mathf.FloorToInt(estado.First());
        float proximoEstado;
        if (accion != -1)
        {
            if (hecho == true)
            {
                q_tabla[ultimoEstado][accion] += tasa_aprendizaje * (recompesa - q_tabla[ultimoEstado][accion]);
            }
            else
            {
             //   q_tabla[ultimoEstado][accion] += tasa_aprendizaje * (recompesa + gamma * q_tabla[nextState].Max() - q_tabla[ultimoEstado][accion]);
            }
        }
        //ultimoEstado = proximoEstado;
    }
    */

    public void generarArchivo() {
        string rutaCompleta = @"D:\Prueba\generacion.txt";
        using (StreamWriter mylogs = File.AppendText(rutaCompleta)) {
            mylogs.WriteLine("1,2,3,4,5,6,7,8,9,10,11");
            mylogs.Close();
        }
    }


    public virtual void EnviarParametros(ParametrosAmbiente env)
    {

    }

    public virtual string Recibir()
    {
        return "";
    }

    public virtual float[] ObtenerAccion()
    {
        return new float[1] { 0.0f };
    }

    public virtual float[] ObtenerValor()
    {
        float[] valor = new float[1];
        return valor;
    }

    public virtual void EnviarEstado(List<float> estado, float recompensa, bool d)
    {

    }


}
