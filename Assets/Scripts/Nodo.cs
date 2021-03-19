using System.Collections;
using UnityEngine;

public class Nodo {
    public bool walkable; //Indica si un nodo dibujado en pantalla es caminable o no
    public Vector3 worldPosition;//posicion en la escena
    public bool peligroso;
    public bool jugador;
    public bool companion;
    public int gridX, gridY;
    public bool caminable;
    public bool obstaculo;
    public bool item;
    public bool enemigo;
    public bool aliado;
    //A*
    public int costoG;
    public int costoH;


    public Nodo nodoPadre;

    //ACO
    public double nivelDeFeromonas = 0.0;
    public double tasaDeCambioDeFeromonas = 0.0;

    public enum TIPONODO {
        recurso = 1,
        peligro = 2
    }
    public int tipoNodo=0;
    public int nroHormigas;
    
    public enum TIPONODOSARSA
    {
        caminable = 1,
        obstaculo =2,
        recurso =3,
        peligro = 4,
        aliado = 6,
    }

    public Nodo() { }

    public Nodo(bool walkable, Vector3 worldPosition, int gridX, int gridY){
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public int costoF{
        get{
            return costoG + costoH;
        }
    }

    /*public bool tieneHormigas() {
        return nroHormigas >= 1;
    }*/
}
