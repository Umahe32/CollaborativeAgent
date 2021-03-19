using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {
    public Transform inicial, objetivo;
    List<Nodo> rutaActual = new List<Nodo>();
    public GridAI grid;
    void Awake()
    {
        grid = GetComponent<GridAI>();
        
    }

    public List<Nodo> ResolverCamino(Vector3 posInicial,Vector3 posObjetivo)
    {
        Nodo nodoInicial = grid.NodoDeUnPunto(posInicial);
        Nodo nodoObjetivo = grid.NodoDeUnPunto(posObjetivo);

        List<Nodo> setAbiertos = new List<Nodo>();
        HashSet<Nodo> setCerrados = new HashSet<Nodo>();
        setAbiertos.Add(nodoInicial);
        while (setAbiertos.Count > 0){
            Nodo nodoActual = setAbiertos[0];
            for(int i=1; i< setAbiertos.Count; i++){
                if (setAbiertos[i].costoF < nodoActual.costoF || setAbiertos[i].costoF == nodoActual.costoF && setAbiertos[i].costoH < nodoActual.costoH) {//siguiente posicion
                    nodoActual = setAbiertos[i];
                }
            }
            setAbiertos.Remove(nodoActual);
            setCerrados.Add(nodoActual);
            if (nodoActual == nodoObjetivo){
                rutaActual =trazarRuta(nodoInicial, nodoObjetivo);
                return rutaActual; //camino encontrado
            }

            //En las siguientes lineas se asigna el set abiertos, los posibles nodos que pueden ocupar el camino 
            foreach (Nodo vecino in grid.getNodosVecinos(nodoActual)) {
                if(!vecino.walkable|| setCerrados.Contains(vecino)){
                    continue;
                }
                int costoNuevoMovimientoAlVecino = nodoActual.costoG + getDistancia(nodoActual, vecino);
                if(costoNuevoMovimientoAlVecino< vecino.costoG || !setAbiertos.Contains(vecino))
                {
                    vecino.costoG = costoNuevoMovimientoAlVecino;
                    vecino.costoH = getDistancia(vecino, nodoObjetivo);
                    vecino.nodoPadre = nodoActual;
                    if (!setAbiertos.Contains(vecino)) {
                        setAbiertos.Add(vecino);
                    }
                }
            }
        }return rutaActual;
    }

    List<Nodo> trazarRuta(Nodo nodoInicio, Nodo nodoFinal) {
        List<Nodo> ruta = new List<Nodo>();
        Nodo nodoActual = nodoFinal;
        while(nodoActual != nodoInicio)
        {
            ruta.Add(nodoActual);
            nodoActual = nodoActual.nodoPadre;
        }
        ruta.Reverse();
        grid.ruta = ruta;
        return ruta;
    }

    // La distancia entre nodo y nodo es de 1 unidad si es horizontal o vertical  en caso diagonal es de raiz de 2, ambas son multiplicadas por un escalar (10)
    int getDistancia(Nodo a,Nodo b)
    {
        int disX = Mathf.Abs(a.gridX - b.gridX);
        int disY = Mathf.Abs(a.gridY - b.gridY);

         
        if(disX > disY)
            return 14 * disY + 10 * (disX - disY);
        return 14 * disX + 10 * (disY - disX);

    }
}
