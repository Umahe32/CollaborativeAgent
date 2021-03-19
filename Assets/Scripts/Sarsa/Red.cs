using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Red : ScriptableObject
{
    public List<Conexion> conexiones = new List<Conexion>();
    public GridAI estado;
    public List<NeuronaSarsa> redNeural;
    public int count = 0;
    public int maximoNeuronas;
    public int maxNodos;
    public int tamanioEntrada;
    public int tamanioSalida;
    public int tamañoRed;

    private int posicionSalida = 0;



    public void SetConexiones(List<Conexion> conex) {
        conexiones = conex;
    }

    public void SetRedInicial(List<NeuronaSarsa> conex)
    {
        tamañoRed = conex.Count;
        foreach (NeuronaSarsa neuronaS in conex)
        {
            foreach (Conexion neurona in neuronaS.entrante)
            {
                neurona.peso = Random.Range(-1.0f, 1.0f);
            }
        }
        foreach (Conexion neurona in conexiones)
        {
            neurona.peso = Random.Range(-1.0f, 1.0f);
        }
        redNeural = new List<NeuronaSarsa>(conex);
    }

    public List<NeuronaSarsa> RedBase(int maxNeuronas)
    {
        List<NeuronaSarsa> neuroSarsa = new List<NeuronaSarsa>();
        maxNodos = maxNeuronas;
        for (int i = 0; i < tamanioEntrada; i++)
        {//inputs
            NeuronaSarsa Aux = CreateInstance<NeuronaSarsa>();
            Aux.idTipo = -1;
            neuroSarsa.Add(Aux);
            maximoNeuronas = i;
        }
        posicionSalida = maximoNeuronas + 1;
        for (int o = 0; o < tamanioSalida; o++)
        {
            NeuronaSarsa Aux = CreateInstance<NeuronaSarsa>();
            Aux.idTipo = 1;
            neuroSarsa.Add(Aux);
        }
        int count = 0;
        for (int i = 0; i < tamanioEntrada; i++)
        {
            for (int o = 0; o < tamanioSalida; o++)
            {
                Conexion nuevaConex = CreateInstance<Conexion>();

                nuevaConex.entrada = i;
                nuevaConex.salida = posicionSalida + o;
                nuevaConex.peso = Random.Range(-1f, 1f);

                count++;
                nuevaConex.habilitado = true;
                conexiones.Add(nuevaConex);
            }
        }

        foreach (Conexion conexion in conexiones)
        {

            // conexiones[conexion.salida].
        }

        redNeural = neuroSarsa;
        tamañoRed = neuroSarsa.Count;
        return neuroSarsa;
    }

    /// <summary>
    /// construye una red base sin capa oculta
    /// </summary>
    
    public void GenerarRed(int maxNeuronas)
    {
        List<float> entradas = estado.ConstruirEntradasSarsa();
        maxNodos = maxNeuronas;
        //entradas
        foreach (float nodo in entradas)
        {
            NeuronaSarsa Aux = CreateInstance<NeuronaSarsa>();
            Aux.indice = count;
            redNeural.Add(Aux);
            count++;
        }
        //salida
        for (int i = 0; i < 8; i++)
        {
            NeuronaSarsa Aux = CreateInstance<NeuronaSarsa>();
            Aux.indice = count;
            redNeural.Add(Aux);
            count++;
        }

        foreach (Conexion conexion in conexiones)
        {
            if (conexion.habilitado)
            {
                try
                {
                    redNeural[conexion.entrada].entrante.Add(conexion);
                }
                catch (System.Exception e)
                {
                    // Debug.Log("salida=" + e.Data.ToString());
                }
            }
        }
    }

    public float[] EvaluarEntorno()
    {
        List<float> entradaSarsa = estado.ConstruirEntradasSarsa();
        for (int i = 0; i < entradaSarsa.Count; i++)
        {//inputs
            redNeural[i].valor = entradaSarsa[i];
        }

        foreach (NeuronaSarsa neuronaS in redNeural)
        {
            float suma = 0;
            foreach (Conexion conex in neuronaS.entrante)
            {
                NeuronaSarsa otraNeurona = redNeural[conex.entrada];
                suma += conex.peso * otraNeurona.valor;
            }

            if (neuronaS.entrante != null)
            {
                neuronaS.valor = FuncionDeActivacion(suma);
                if (neuronaS.idTipo == 1)
                {
                    //actualizo el valor de la neurona en cuestion usando la funcion activacion 
                    //Deberia estar la salida (solo consulta)
                }
            }
        }

        // 8 con respecto a las acciones posibles del companion

        float[] salida = new float[8];

        int limite = posicionSalida + tamanioSalida;
        int count = 0;
        for (int i = posicionSalida; i < limite; i++)
        {//outputs
            salida[count] = redNeural[i].valor;
            count++;
        }
        return salida;
    }

    public NeuronaSarsa kEsimaNeurona(int k)
    {
        return redNeural[k];
    }

    public float FuncionDeActivacion(float x)
    {
        float relu = Mathf.Max(0, x);
        float softplus = Mathf.Log((1 + Mathf.Exp(x)));
        float sigmoide = 1 / (1 + Mathf.Exp(-.1f * x));
        return sigmoide;
    }

    /// <summary>
    /// este procesa la entrada y devuelve los valores de confianza de la salida (accion)
    /// </summary>
    /// <returns></returns>
    /* public float[] EvaluarEntorno()
     {
         float[] entradas = vision.construirEntradas();
         for (int i = 0; i < entradas.Length; i++)
         {//inputs
             redNeural[i].valor = entradas[i];
         }

         foreach (Neurona neurona in redNeural)
         {
             float suma = 0;
             foreach (GenNEAT gen in neurona.entrante)
             {
                 Neurona otraNeurona = redNeural[gen.entrada];
                 suma += gen.peso * otraNeurona.valor;
             }

             if (neurona.entrante != null)
             {
                 neurona.valor = Sigmoide(suma);
                 if (neurona.idTipo == 1)
                 {
                     // Debug.Log("Sigmoide a la salida=" + suma);
                 }
             }
         }
         float[] salida = new float[8];

         int limite = posicionSalida + tamanioSalida;
         int count = 0;
         for (int i = posicionSalida; i < limite; i++)
         {//outputs
             salida[count] = redNeural[i].valor;
             count++;

         }
         return salida;
     }*/



}
