using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GenomaNEAT : ScriptableObject
{
    public List<GenNEAT> genes=new List<GenNEAT>();
    public GridAI vision;
    private List<Neurona> redNeural;
    public int maximoNeuronas;
    public int maxNodos;
    private int numeroEspecie;
    public float fitness;
    public int tamanioEntrada;
    public int tamanioSalida;
    public int tamañoRed;

    private int posicionSalida = 0;
    public void setGenes(List<GenNEAT> red) {
        genes = red;
    }

    public void SetRedInicial(List<Neurona> red) {
        tamañoRed = red.Count;
        foreach (Neurona neurona in red)
        {
            foreach (GenNEAT gen in neurona.entrante)
            {
                gen.peso = Random.Range(-1.0f, 1.0f);
            }
        }
        foreach (GenNEAT gen in genes)
        {
            gen.peso = Random.Range(-1.0f, 1.0f);
        }
        redNeural = new List<Neurona>(red);
    }

    public List<Neurona> RedBase(int maxNeuronas) {
        List<Neurona> neuronas = new List<Neurona>();
        maxNodos = maxNeuronas;
        //float[] entradas = vision.construirEntradas();
        for (int i = 0; i < tamanioEntrada; i++)
        {//inputs
            Neurona Aux = CreateInstance<Neurona>();
            Aux.idTipo = -1;
            neuronas.Add(Aux);
            maximoNeuronas = i;
        }
        posicionSalida = maximoNeuronas+1;
        for (int o = 0; o < tamanioSalida; o++)
        {//outputs
            Neurona Aux = CreateInstance<Neurona>();
            Aux.idTipo = 1;
            neuronas.Add(Aux);
        }
        int count = 0;
        for (int i = 0; i < tamanioEntrada; i++) {
            for (int o = 0; o < tamanioSalida; o++) {
                GenNEAT nuevoGen = CreateInstance<GenNEAT>();
                
                nuevoGen.entrada = i;
                nuevoGen.salida = posicionSalida + o;
                nuevoGen.peso = Random.Range(-1f, 1f);
                
                nuevoGen.innovacion = count;
                count++;
                nuevoGen.habilitado = true;
                genes.Add(nuevoGen);
            }
        }
       // Debug.Log("Tamaño Genes:"+ genes.Count+"innovacion"+count);
        foreach (GenNEAT gen in genes)
        {
            
            neuronas[gen.salida].entrante.Add(gen);
        }

        //   Debug.Log(count + " Genes Generados");
        redNeural = neuronas;
        tamañoRed = neuronas.Count;
        return neuronas;
    }


    public void GenerarRed(int maxNeuronas)
    {
        List<Neurona> neuronas = new List<Neurona>();
        maxNodos = maxNeuronas;


        if (genes.Count != 0)
        {
            //Debug.Log("ya habia genes");
            float[] entradas = vision.construirEntradas();
            for (int i = 0; i < entradas.Length; i++)
            {//inputs
                Neurona Aux = CreateInstance<Neurona>();
                Aux.idTipo = -1;
                neuronas.Add(Aux);
                maximoNeuronas = i;
            }
            posicionSalida = maximoNeuronas+1;
            for (int o = 0; o < tamanioSalida; o++)
            {//outputs
                Neurona Aux = CreateInstance<Neurona>();
                Aux.idTipo = 1;
                neuronas.Add(Aux);
            }
            foreach (GenNEAT gen in genes)
            {
                if (gen.habilitado)
                {
                    //   Debug.Log("salida=" + gen.salida);
                    try
                    {
                        if (gen.salida>=neuronas.Count)
                        {
                            gen.salida = neuronas.Count;
                            neuronas.Add(CreateInstance<Neurona>());
                           
                        }
                        neuronas[gen.salida].entrante.Add(gen);
                        if (gen.entrada>= neuronas.Count)
                        {
                            gen.entrada = neuronas.Count;
                            neuronas.Add(CreateInstance<Neurona>());
                            
                        }
                    }
                    catch (System.Exception e) {
                        Debug.Log("salida=" + e.Data.ToString());
                      //  Debug.Log("entrada=" + gen.entrada);
                    }
                }
            }
        }
        else {
           // Debug.Log("hubo que regenerar");
           neuronas=RedBase(maxNeuronas);
        }
       // Debug.Log("Red Generada Exitosamente" );

        //string prueba = neuronas[2000] != null ? "tiene algo" : "Es Null";
        redNeural =neuronas;
       // Debug.Log("PosicionSalida: " + posicionSalida + " tamaño Red: " + redNeural.Count + " tamaño Salida: " + tamanioSalida);
    }

    public float[] EvaluarEntorno()
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
                if (neurona.idTipo == 1) {
                   // Debug.Log("Sigmoide a la salida=" + suma);
                }
            }
        }
        float[]salida =new float[8];
       
        int limite = posicionSalida + tamanioSalida;
        int count = 0;
        for (int i = posicionSalida; i < limite; i++)
        {//outputs
            salida[count] = redNeural[i].valor;
            count++;

        }
        return salida;
    }

    public float Sigmoide(float x)
    {
        float relu = Mathf.Max(0, x);
        float soft = Mathf.Log((1+Mathf.Exp(x)));
        float sig= 1 / (1 + Mathf.Exp(-.1f * x));
        return relu;
    }

    public void setNumEspecie(int especie) {
        numeroEspecie = especie;
    }
    public int getNumEspecie()
    {
        return numeroEspecie;
    }
    public Neurona kEsimaNeurona(int k) {
        return redNeural[k];
    }
}
