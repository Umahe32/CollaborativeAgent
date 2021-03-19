using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgoritmoSarsa : MonoBehaviour
{

    private float[] politica;//estado-action
    private float[] trazaEligibilidadEstado;//estado-action
    public float factorDescuento;
    public float tasaAprendizaje;
    public float trazaEligibilidad;
    public IAPlayer companion;
    List<Neurona> RedNeural;
    public GridAI AmbienteLocal;//Deteccion Fisica
    public int tamanioEntradaX;
    public int tamanioEntradaY;
    public int tamamnioSalida;

    public List<float> estado;






    // Start is called before the first frame update
    void Start()
    {

        //Revisar
        /*  RedNeural = new List<Neurona>();
          companion = GameObject.FindGameObjectWithTag("Companion").GetComponent<IAPlayer>();
          AmbienteLocal.IsSarsa = true;
          tamanioEntradaX = AmbienteLocal.tamanioGridX;
          tamanioEntradaY = AmbienteLocal.tamanioGridY;
          int aux = 0;

          //introducir neuronas entrada
          for (int i = 0; i < tamanioEntradaX; i++) {
              for (int j = 0; j < tamanioEntradaY; j++) {
                  Neurona neuronaLocal =ScriptableObject.CreateInstance<Neurona>();
                  neuronaLocal.idTipo = -1;
                  neuronaLocal.indice = aux;
                  RedNeural.Add(neuronaLocal);
                  aux++;
              }
          }

          //introducir neuronas salida
          for (int i = 0; i < tamamnioSalida; i++) {
              Neurona neuronaLocal = new Neurona();
              neuronaLocal.idTipo = 1;
              RedNeural.Add(neuronaLocal);
              aux++;
          }

          //introducir neuronas capa oculta
          for (int i = 0; i < tamanioEntradaX; i++)
          {
              for (int j = 0; j < tamanioEntradaY; j++)
              {
                  Neurona neuronaLocal = ScriptableObject.CreateInstance<Neurona>();
                  neuronaLocal.idTipo = 0;
                  int aux2 = 0;
                  foreach (Neurona NeuronaEntrada in RedNeural)
                  {
                      if (aux2 < (tamanioEntradaX * tamanioEntradaY))
                      {
                          neuronaLocal.entrada[aux2, 0] = NeuronaEntrada.indice;//Enlace
                          neuronaLocal.entrada[aux, 1] = Random.Range(0f, 1f);//Peso
                      }
                      else {
                          neuronaLocal.salida[aux2] = NeuronaEntrada.indice;
                      }
                      aux++;
                  }
                  RedNeural.Add(neuronaLocal);
              }
          }

          InvokeRepeating("Jugar",0.2f,0.2f);*/
    }


    void Jugar()
    {
        float[] EntradaRed = AmbienteLocal.construirEntradas();
        Debug.Log(EntradaRed.ToString());

        //una vez termina la accion, actualiza entradas 
        AmbienteLocal.RefreshGrid();

    }




    void FuncionValorAccionEstado()
    {
        int i = politica.Length;
        while (i > 0)
        {
            i--;
        }
    }

    void tomarAccion(int id)
    {
        switch (id)
        {
            case 0:
                companion.Adelante();
                break;
            case 1:
                companion.Atras();
                break;
            case 2:
                companion.Izquierda();
                break;
            case 3:
                companion.Derecha();
                break;
            case 4:
                companion.Disparar();
                break;

        }
    }

    void Recompesar(float recompensa)
    {

    }
    void obtenerEstadoActual()
    {
        estado = AmbienteLocal.ConstruirEntradasSarsa();
    }




    // Update is called once per frame
    void Update()
    {
        
    }

    public float[][] q_tabla;   // Matriz que contiene los valores estimados.
    float tasa_aprendizaje = 0.5f; // Tasa de actualización de estimaciones por valor dada una recompensa. 
    int accion = -1;
    float gamma = 0.99f; // Factor de descuento para calcular Q-Objetivo.
    float epsilon = 1; // Epsilon inicial para la selección de acciones aleatorias.
    float eMin = 0.1f; // Límite inferior de épsilon.
    int recuentoPasos = 2000; // Número de pasos para bajar Epsilon a eMin.
    int ultimoEstado;

    public void EnviarParametros(ParametrosAmbiente env)
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

    public float[] ObtenerAccion()
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

    public void EnviarEstado(List<float> estado, float recompesa, bool hecho)
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

    void FuncionValorAccionEstad2()
    {
        int i = politica.Length;
        while (i > 0)
        {
            i--;
        }
    }

}
