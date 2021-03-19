using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenNEAT : ScriptableObject
{
    public int entrada = 0;
    public int salida = 0;
    public float peso = 0.0f;
    public bool habilitado = true;
    public int innovacion = 0;

    public static GenNEAT copiarGen(GenNEAT genACopiar) {
        GenNEAT salida = CreateInstance<GenNEAT>();
        salida.entrada = genACopiar.entrada;
        salida.salida = genACopiar.entrada;
        salida.peso = genACopiar.peso;
        salida.habilitado = genACopiar.habilitado;
        salida.innovacion = genACopiar.innovacion;
        return salida;
    }

}
