using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuronaSarsa : ScriptableObject
{
    public List<Conexion> entrante = new List<Conexion>();
    public float valor = 0.0f;
    public int indice;
    public int idTipo = 0;//-1 entrada, 0 oculta, 1 salida

}