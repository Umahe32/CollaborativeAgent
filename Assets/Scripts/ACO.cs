using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACO : MonoBehaviour {
    public GridAI GrillaMundo;
    public Vector3 nido;
    public int cantidadHormigas=0;
    private HormigaAco[] hormigas; 
	void Awake () {
		 GrillaMundo = GetComponent<GridAI>();
    }


    private void Start()
    {
        hormigas = new HormigaAco[cantidadHormigas];
        for(int i = 0; i < cantidadHormigas; i++){
            hormigas[i] = new HormigaAco(nido,this.gameObject.GetComponent<ACO>());

            Debug.Log("Hormiga: " + i + " Creada");
        }
    }
    private void Update()
    {
        for(int i = 0; i < cantidadHormigas; i++){
            hormigas[i].MovimientoEnNodos();
        }
    }


    public Nodo EscogerCamino(Nodo nodoActual, HormigaAco hormiga) {
        double ruleta = Random.Range(0f, 1f);
        List<Nodo> vecinos = GrillaMundo.getNodosVecinos(nodoActual);
        double[] probabilidadLocal = new double[vecinos.Count];
        double probabilidadVecinos = 0.0;
        Nodo nodoSiguiente = null;
        int indice = 0;
        foreach (Nodo vecino in vecinos) {
            if (vecino == nodoActual.nodoPadre) { indice++; continue; }
            probabilidadLocal[indice] = vecino.nivelDeFeromonas * getDistancia(nodoActual, vecino);
            probabilidadVecinos += probabilidadLocal[indice];
            indice++;
        }
        indice = 0;
        //actualizar probabilidad de seleccion 
        List<double> sum = new List<double>();

        foreach (Nodo vecino in vecinos)
        {
            if (vecino == nodoActual.nodoPadre) { indice++; continue; }
            probabilidadLocal[indice] = probabilidadLocal[indice] / probabilidadVecinos;
            indice++;
        }
        indice = 0;
        foreach (Nodo vecino in vecinos) {
            if (vecino == nodoActual.nodoPadre) { indice++; continue; }
            double suma = 0;
            for (int i = indice; i < probabilidadLocal.Length; i++){
                suma += probabilidadLocal[i];
                
            }
            sum.Add(suma);
            
        }
        sum.Reverse();
        indice = 0;
        foreach (Nodo vecino in vecinos)
        {
            if (vecino == nodoActual.nodoPadre) { indice++; continue; }
            if (indice == 0 )
            {
                if (ruleta <= sum[indice])
                {
                    nodoSiguiente = vecino;
                    nodoSiguiente.nodoPadre = nodoActual;
                }
            }
            else if (ruleta > sum[indice - 1] && ruleta <= sum[indice])
            {
                nodoSiguiente = vecino;
                nodoSiguiente.nodoPadre = nodoActual;
            }
            indice++;
        }
        if (nodoSiguiente == null)
        {
            bool apto=false;
            while (!apto){
                int seleccion = Random.Range(0, vecinos.Count);
                nodoSiguiente = vecinos[seleccion];
                if (nodoSiguiente != nodoActual.nodoPadre && nodoSiguiente.walkable) {
                    apto = true;
                }
            }
        }
        return nodoSiguiente;
    }







    //Auxiliares
    public int getDistancia(Nodo a, Nodo b)
    {
        int disX = Mathf.Abs(a.gridX - b.gridX);
        int disY = Mathf.Abs(a.gridY - b.gridY);


        if (disX > disY)
            return 14 * disY + 10 * (disX - disY);
        return 14 * disX + 10 * (disY - disX);

    }
}
