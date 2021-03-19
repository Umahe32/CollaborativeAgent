using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GridAI : MonoBehaviour
{

    public LayerMask unwalkableMask;
    public LayerMask maskPlayer;
    public LayerMask maskCompanion;
    public LayerMask maskRecurso;
    public LayerMask maskPeligro;
    public bool IsSarsa = false;
    public Vector3 gridWorldSize;//para definir el tamaño de la grilla a nivel total de la escena
    public float radioDelNodo;
    Nodo[,] grid;
    float diametroDelNodo;
    public int tamanioGridX, tamanioGridY;
    private bool jugadorAlAlcance;
    public Visualizacion[] Lidars;
    public Transform companion;


    void Awake()
    {
        diametroDelNodo = radioDelNodo * 2;
        tamanioGridX = Mathf.RoundToInt(gridWorldSize.x / diametroDelNodo);
        tamanioGridY = Mathf.RoundToInt(gridWorldSize.z / diametroDelNodo);
        CrearGrilla();

    }

    private void Update()
    {
        if (IsSarsa)
        {
            RefreshGrid();
            prueba();
            ConstruirEntradasSarsa();

           
        }
    }
    void CrearGrilla()
    {
        grid = new Nodo[tamanioGridX, tamanioGridY];
        Vector3 izquierdaInferiorMundo = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.z / 2; //apunta a la esquina inferior izquierda de la grilla
        for (int i = 0; i < tamanioGridX; i++)
        {
            for (int j = 0; j < tamanioGridY; j++)
            {
                Vector3 puntoMundial = izquierdaInferiorMundo + Vector3.right * (i * diametroDelNodo + radioDelNodo) + Vector3.forward * (j * diametroDelNodo + radioDelNodo);
                bool walkable = !(Physics.CheckSphere(puntoMundial, radioDelNodo, unwalkableMask));
                bool recurso = (Physics.CheckSphere(puntoMundial, radioDelNodo, maskRecurso));
                bool peligro = (Physics.CheckSphere(puntoMundial, radioDelNodo, maskPeligro));
                jugadorAlAlcance = (Physics.CheckSphere(puntoMundial, radioDelNodo, maskPlayer));
                grid[i, j] = new Nodo(walkable, puntoMundial, i, j);
                grid[i, j].tipoNodo = recurso ? 1 : 0;
                grid[i, j].peligroso = peligro;
                grid[i, j].jugador = jugadorAlAlcance;
                grid[i, j].companion = (Physics.CheckSphere(puntoMundial, radioDelNodo, maskCompanion));
            }

        }

    }
    public List<Nodo> ruta;

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, gridWorldSize.z));
        if (grid != null)
        {

            foreach (Nodo n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.gray : Color.black;
                if (!IsSarsa && n.walkable)
                {
                    Gizmos.color = Color.white;
                }
                if (n.peligroso)
                {
                    Gizmos.color = Color.red;
                }
                else if (n.jugador)
                {
                    Gizmos.color = Color.blue;
                }
                else if (n.companion)
                {
                    Gizmos.color = Color.green;
                }
                /*  if(n.nivelDeFeromonas>=0.0001)Gizmos.color = Color.HSVToRGB(0.34f, (float)n.nivelDeFeromonas*(10f), 0.4f);//nivel de feromonas
                  if (n.tipoNodo ==1) {//Recursos 
                      Gizmos.color = Color.magenta;
                  }

                  if (ruta!= null)
                      if (ruta.Contains(n))
                      {
                          Gizmos.color = Color.blue;
                      }*/
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (diametroDelNodo - .1f));

                /*if (n.tieneHormigas()) 

                    for(int i = 1; i <= n.nroHormigas; i++)
                    {
                        Gizmos.color = Color.blue;
                        Vector3 posicionHormiga = new Vector3(n.worldPosition.x+(0.2f*i), n.worldPosition.y+0.2f, n.worldPosition.z+(-0.2f*i));
                        Gizmos.DrawWireSphere(posicionHormiga,0.2f );
                    }*/
            }
        }
    }


    public Nodo NodoDeUnPunto(Vector3 posicionMundial)
    {
        float porcetajeX = (posicionMundial.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float porcetajeY = (posicionMundial.z + gridWorldSize.z / 2) / gridWorldSize.z;
        porcetajeX = Mathf.Clamp01(porcetajeX);
        porcetajeY = Mathf.Clamp01(porcetajeY);

        int x = Mathf.RoundToInt((tamanioGridX - 1) * porcetajeX);
        int y = Mathf.RoundToInt((tamanioGridY - 1) * porcetajeY);
        return grid[x, y];
    }
    public List<Nodo> getNodosVecinos(Nodo nodo)
    {
        List<Nodo> vecinos = new List<Nodo>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }
                int checkX = nodo.gridX + x;
                int checkY = nodo.gridY + y;
                
                if (checkX >= 0 && checkX < tamanioGridX && checkY >= 0 && checkY < tamanioGridY)
                {
                    Debug.Log("vecino agregado check x:" + checkX + " check y:" + checkY);
                    vecinos.Add(grid[checkX, checkY]);
                }
            }
        }
        return vecinos;
    }
    public void RefreshGrid()
    {
        grid = null;
        CrearGrilla();
        // construirEntradasGUI();
    }

    /// <summary>
    /// Transforma la grilla de observacion en datos numericos para su procesamiento
    /// </summary>
    /// <returns>Un arreglo de flotantes bidimensional, que representa el espacio observado por el agente, 0 para lugar caminable, 1 para obstaculo,2 para peligro,3 para jugador</returns>
    public float[,] construirEntradasGUI()
    {
        float[,] entradas = new float[tamanioGridX, tamanioGridY];
        //string salida="";
        for (int i = 0; i < tamanioGridX; i++)
        {
            for (int j = 0; j < tamanioGridY; j++)
            {
                entradas[j, i] = ValorNodo(i, j);
                // salida+=valorNodo + " ";
            }
            //salida+="\n";
        }

        return entradas;
    }

    public int ValorNodo(int posX, int posY)
    {
        Nodo nodoLocal = grid[posX, posY];
        int valorNodo = (nodoLocal.walkable) ? 1 : 0; //caminable u obstaculo
        if (nodoLocal.peligroso)
        {
            valorNodo = 2;
        }
        else if (nodoLocal.jugador)
        {
            valorNodo = 3;
        }
        return valorNodo;
    }
    public int ValorNodoSarsa(int posX, int posY)
    {
        Nodo nodoLocal = grid[posX, posY];
        int valorNodo = (nodoLocal.walkable) ? 0 : 2; //caminable u obstaculo
        List<Nodo> vecinos = getNodosVecinos(nodoLocal);

        foreach (Nodo vecino in vecinos)
        {
            valorNodo = (nodoLocal.walkable) ? 1 : 2;
        }

        if (nodoLocal.peligroso)
        {
            valorNodo = 4;
        }
        else if (nodoLocal.jugador)
        {
            valorNodo = 6;
        }
        else if (nodoLocal.item) {
            valorNodo = 3;
        }

        return valorNodo;
    }

    /// <summary>
    /// Transforma la grilla de observacion en datos numericos para su procesamiento
    /// </summary>
    /// <returns>Un arreglo de flotantes bidimensional, que representa el espacio observado por el agente, 0 para lugar caminable, 1 para obstaculo,2 para peligro,3 para jugador</returns>
    public float[] construirEntradas()
    {
        float[] entradas = new float[tamanioGridX * tamanioGridY];
        int aux = 0;
        for (int i = 0; i < tamanioGridX; i++)
        {
            for (int j = 0; j < tamanioGridY; j++)
            {

                if (IsSarsa)
                {
                    entradas[aux] = ValorNodoSarsa(i, j);

                }
                else
                {
                    entradas[aux] = ValorNodo(i, j);
                }
                aux++;
            }
        }


        return entradas;
    }
    public List<float> ConstruirEntradasSarsa()
    {
        List<float> entradas = new List<float>();
        /*for (int i = 0; i < tamanioGridX; i++)
        {
            for (int j = 0; j < tamanioGridY; j++)
            {
                entradas.Add(ValorNodoSarsa(i, j));
            }
        }*/
        List<Nodo> listaNodo = getNodosVecinos(NodoDeUnPunto(companion.position));
        Debug.Log("nodos vecinos:"+listaNodo.Count);
        foreach (Nodo n in listaNodo)
        {
            entradas.Add(ValorNodoSarsa(n.gridX, n.gridY));
        }
        //Debug.Log(entradas.Count);
        for (int i = 0; i < Lidars.Length; i++)
        {
            entradas.Add(Lidars[i].Distancia);
        }
        string salida = "";
        foreach (float valor in entradas)
        {
            salida += valor + " ";
        }
        Debug.Log(salida);
        return entradas;
    }

    public float YoDecidoElControlador(List<float> entradas)
    {
        float salida = 0f;
        foreach (float n in entradas)
        {
            //Debug.Log("entrada: " + n);
            salida += n;
        }
        salida = salida / entradas.Count;
        Debug.Log("Cuenta: " + entradas.Count);
        return salida;
    }


    internal void YoDecidoElControlador(GridAI ambienteLocal)
    {
        throw new NotImplementedException();
    }


    void prueba()
    {
        Debug.Log(YoDecidoElControlador(ConstruirEntradasSarsa()));

    }
}
