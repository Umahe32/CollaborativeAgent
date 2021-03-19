using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CompanionSarsa : MonoBehaviour {
	//TO-DO AI
    private GameObject[] ppOriginales;
	private int contador;
    private List<Nodo> ruta;
    private Nodo puntoActual;
    public float velocidad = 5;
    public UnityEngine.AI.NavMeshAgent Agente;

    public Pathfinding guia;
    public GameObject[] puntosDePatrulla;
    

    void Start () {
	//	anim = GetComponent<Animator>();
		contador=0;
    //    GameObject aux = BusquedaHijos.buscarHijoPorTag(gameObject, "Weapon");
        //  arma =  aux != null ? aux.GetComponent<Arma>() : null;
        ppOriginales = puntosDePatrulla != null ? puntosDePatrulla : null;
        ruta = new List<Nodo>();
        puntoActual = new Nodo();
        puntoActual.worldPosition = transform.position;
    }
    private void Update()
    {
        patrullar();
    }
    int aux = 0;
    void patrullar(){
		if(puntosDePatrulla.Length==0){
			//	isWalking=false;	
		}
        if (contador >= puntosDePatrulla.Length)
        {
            contador = 0;
        }
        else if (puntosDePatrulla[contador] != null)
        {
            
            if (Mathf.Round(Agente.gameObject.transform.position.x) == Mathf.Round(puntosDePatrulla[contador].transform.position.x) && Mathf.Round(Agente.gameObject.transform.position.z) == Mathf.Round(puntosDePatrulla[contador].transform.position.z)) 
            {
                contador++;//introducir el cambio de direccion de hacia el otro punto patrulla 
            }
            else //movimiento basa en path finding A*
            {
                Vector3 lugarPP = puntosDePatrulla[contador].transform.position;
                ruta = guia.ResolverCamino(Agente.gameObject.transform.position, lugarPP);
                Agente.SetDestination(lugarPP);
                aux++; 
                // ultima modificacion
                /*if (aux % 100 == 0) {
                    contador++;
                }*/
                /* //Debug.Log("nodos de la ruta: " + ruta.Count);
                 if (ruta.Count <= 0)
                 {
                    Debug.Log("nodos de la ruta Antes: " + ruta.Count);
                     Vector3 lugarPP = puntosDePatrulla[contador].transform.position;
                     ruta = guia.ResolverCamino(transform.position, lugarPP);
                     Agente.SetDestination(lugarPP);
                     Debug.Log("nodos de la ruta Despues: " + ruta.Count);
                 }
                /* else if(transform.position.x <= (Mathf.Round(puntoActual.worldPosition.x )+ 1f) && transform.position.x >= (Mathf.Round(puntoActual.worldPosition.x )- 1f) && transform.position.z <= (Mathf.Round(puntoActual.worldPosition.z) + 1f) && transform.position.z >= (Mathf.Round(puntoActual.worldPosition.z )- 1f))
                 {
                     Debug.Log("alcanzado");
                     puntoActual = ruta[0];
                     ruta.RemoveAt(0);

                     //moverse(puntoActual);
                     Debug.Log("nodos de la ruta: " + ruta.Count);
                 } else 
                 {
                     //moverse(ruta[0]);
                     Debug.Log("posicion: " + transform.position);
                     Debug.Log("nodo objectivo: " + ruta[0].worldPosition);
                 }
                 */
            }
        }
        else {
            contador++;
        }
		
	}

    /*private void rotar(Vector3 objetivo)
    {
        float angulo = Vector3.Angle(Vector3.forward, objetivo);
        if (objetivo.x >= 0)
        {
            angulo = 180 + (180 - angulo);//debido a que el motor busca la forma mas corta de calcular el angulo se debe corregir cuando se pasa hacia la parte positiva de las X
        }
        Quaternion target = Quaternion.Euler(0, angulo, 0);//Define la rotacion a aplicar en el eje Z
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * velocidad);//Rota el enemigo
    }
    */
    private void moverse(Nodo punto)
    {
        /* Vector3 intro = new Vector3((punto.worldPosition.x - transform.position.x), 0 , (punto.worldPosition.z - transform.position.z));//Se traza un vector en la direccion del punto de ruta
         rotar(intro);
         Vector3 v = new Vector3(intro.x, 0 ,intro.z).normalized * Time.deltaTime;//se normaliza por el delta de tiempo para que el movimiento vaya acorde al tiempo de la escena
         v = v * velocidad;                                                                         //v = v * velocidad;//Para poder encontrar personajes con diferentes velocidades
         transform.position += v;
     */

        Agente.SetDestination(punto.worldPosition);
    }
    
}