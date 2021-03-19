using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sarsa : MonoBehaviour
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


    void Jugar() {
        float[] EntradaRed = AmbienteLocal.construirEntradas();
        Debug.Log(EntradaRed.ToString());

        //una vez termina la accion, actualiza entradas 
        AmbienteLocal.RefreshGrid();
        
    }




    void FuncionValorAccionEstado() {
        int i = politica.Length;
        while (i>0) {
            i--;
        }
    }

    void tomarAccion(int id) {
        switch (id) {
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

    void Recompesar(float recompensa) {

    }
    void obtenerEstadoActual() {
        estado=AmbienteLocal.ConstruirEntradasSarsa();
    }

}
