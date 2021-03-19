using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neurona : ScriptableObject
{
    public List<GenNEAT> entrante=new List<GenNEAT>();
    public float valor = 0.0f;
    public int idTipo=0;//-1 entrada, 0 oculta, 1 salida

    //para sarsa 

    public float[,] entrada ;
    public float[] salida ;
    public int indice;
}
