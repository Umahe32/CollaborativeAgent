using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HormigaAco : MonoBehaviour {
    public ACO aco;
    public Vector3 posicion;
    Nodo direccion;
    private Nodo nodoAnterior;
    public List<Nodo> nodosVisitados=new List<Nodo>();
    private bool recursoEncotrado = false;
    private bool peligroEncontrado = false;
    private float largoDelCamino=0;
    private double nivelDeFeromonas=0;
    public double tasaDeEvaporacion;

    public HormigaAco(Vector3 nido, ACO colonia)
    {
        posicion = nido;
        aco = colonia;
        tasaDeEvaporacion = .5;
    }


    public void MovimientoEnNodos()
    {
        direccion = aco.GrillaMundo.NodoDeUnPunto(posicion);
        nodoAnterior = aco.GrillaMundo.NodoDeUnPunto(posicion);
        direccion = aco.EscogerCamino(nodoAnterior,this);
        posicion = direccion.worldPosition;
        largoDelCamino += aco.getDistancia(nodoAnterior, direccion);

        nodoAnterior.nroHormigas--;
        direccion.nroHormigas++;

        if (!recursoEncotrado && !peligroEncontrado)
        {
            if (direccion.tipoNodo == 1)
            {//recurso
                nodosVisitados.Add(nodoAnterior);
                nodosVisitados.Add(direccion);
                Debug.Log("largo: "+largoDelCamino);
                nivelDeFeromonas = 1 / largoDelCamino;
                Debug.Log("Nivel de feromonas: " + nivelDeFeromonas);
                recursoEncotrado = true;
                direccion.nroHormigas--;
                nodoAnterior.nroHormigas--;
            }
            else if (direccion.tipoNodo == 2)
            {
                nodosVisitados.Add(nodoAnterior);
                nodosVisitados.Add(direccion);
                nivelDeFeromonas = 1 / largoDelCamino;
                peligroEncontrado = true;
            }
            else
            {
                nodosVisitados.Add(nodoAnterior);
                
            }
        }
        else if (recursoEncotrado)
        {
            Debug.Log("Hormiga Saludando desde posicion X: " + posicion.x + " Y: " + posicion.z + "Recursos encontrados"+"mi nivel de feromonas es: "+this.nivelDeFeromonas);
            RepartirFeromonas();
            Debug.Log("feromonas del nodo recurso: " + direccion.nivelDeFeromonas);
            posicion = aco.nido;
            nodosVisitados.Clear();
            recursoEncotrado = false;
            peligroEncontrado = false;
        }
        else if (peligroEncontrado) {
            RepartirFeromonas();
            posicion = aco.nido;
            nodosVisitados.Clear();
            recursoEncotrado = false;
            peligroEncontrado = false;
        }
        
    }

    private void RepartirFeromonas()
    {
        foreach (Nodo visitado in nodosVisitados)
        {
            //Debug.
            visitado.nivelDeFeromonas = visitado.nivelDeFeromonas+((1.0 - tasaDeEvaporacion) *nivelDeFeromonas);
        }
    }
}
