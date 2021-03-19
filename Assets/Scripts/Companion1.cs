using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Companion1 : MonoBehaviour {
	
	private Animator anim;
    private GameObject[] ppOriginales;
	private int contador;
//	private Vector2 direccion;
    private List<Nodo> ruta;
    private Nodo puntoActual;
    private Visualizacion ojos;
    private int auxiliar = 0;
    private bool deteccion = false;
    private Arma arma;
    private bool isDead = false;
    private bool isWalking = false;

    public float alcanceVisual = 7f;
    public float  velocidad=5;
	public int salud=10;
   // public Pathfinding guia;
    public GameObject[] puntosDePatrulla;
    

    void Start () {
		anim = GetComponent<Animator>();
		contador=0;
        GameObject aux = BusquedaHijos.buscarHijoPorTag(gameObject, "Weapon");
        arma =  aux != null ? aux.GetComponent<Arma>() : null;
        ppOriginales = puntosDePatrulla != null ? puntosDePatrulla : null;
        ruta = new List<Nodo>();
        puntoActual = new Nodo();
        puntoActual.worldPosition = transform.position;
        ojos = GetComponentInChildren<Visualizacion>();
        //ojos.setAlcanceVisual(alcanceVisual);
    }

	void Update () {
		if(!Murio()){
            deteccion = ojos.ComprobrarVisulizacion();
            //ojos.setAlerta(deteccion);
            //anim.SetBool("Alert", deteccion);
            if (!deteccion){
				patrullar();
				
			}else{
				isWalking=false;
				atacar();	
			}
			anim.SetBool("isWalking", isWalking);
			anim.SetBool("attack",deteccion);
		}
	}
	
	bool Murio(){
		if(salud<=0){
			isDead=true;
			anim.SetBool("isDead", isDead);//el enemigo Murio
            arma.gameObject.GetComponent<FixedJoint2D>().enabled = false;
            arma.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            arma.gameObject.transform.SetParent(null);// soltar arma
            
            Invoke("destructor", 3f);
		}
		return isDead;
	}
	
	void patrullar(){
		if(puntosDePatrulla.Length==0){
				isWalking=false;	
		}
        if (contador >= puntosDePatrulla.Length)
        {
            contador = 0;
        }
        else if (puntosDePatrulla[contador] != null)
        {
            isWalking = true;
            
            if (Mathf.Round(transform.position.x) == Mathf.Round(puntosDePatrulla[contador].transform.position.x) && Mathf.Round(transform.position.y) == Mathf.Round(puntosDePatrulla[contador].transform.position.y)) 
            {
                contador++;//introducir el cambio de direccion de hacia el otro punto patrulla 
            }
            else //movimiento basa en path finding A*
            {
                //Debug.Log("nodos de la ruta: " + ruta.Count);
                /*
                if (ruta.Count <= 0)
                {
                   // Debug.Log("nodos de la ruta: " +0);
                    Vector3 lugarPP = puntosDePatrulla[contador].transform.position;
                    ruta = guia.ResolverCamino(transform.position, lugarPP);

                } else if (!(Mathf.Round(transform.position.x) == Mathf.Round(puntoActual.worldPosition.x )&& Mathf.Round(transform.position.y) == Mathf.Round(puntoActual.worldPosition.y))) {
                    moverse(puntoActual);
                }else {
                    
                    puntoActual = ruta[0];
                    ruta.RemoveAt(0);
                    moverse(puntoActual);
                }*/
            }
        }
        else {
            contador++;
        }
		
	}
	
	void atacar(){
		if(deteccion){
            procesoAlarma();
            if (arma != null)
            //arma.reposicionar();
            {

                if (arma.IsBobyToBody())
                {
                    //corra al enemigo usar A* 
                    if (puntosDePatrulla != null){
                        patrullar();
                    }
                    arma.ataque();
                }
                else if (arma.getMunicionDisponible() != 0)
                {
                   // if (ojos.esAlcanzableArma(arma.getAlcance())) { anim.SetBool("attack", true); arma.ataque(); }
                }
                else
                {
                    arma.gameObject.transform.SetParent(null);
                    arma = null;
                }
            }
           
        }
	}
	void destructor(){
        //SpriteRenderer renderizadorSprite = arma.gameObject.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        //renderizadorSprite.enabled = false;
        Destroy(this.gameObject);
    }

    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy" && !Murio())
        {//si quien entra en colision es el jugador
            if (!other.gameObject.GetComponent<EnemyHealth>().Murio())
            {
                auxiliar = 0;
            }
        }
    }
    void OnTriggerStay2D(Collider2D other){
		if(other.tag=="Enemy"&&!Murio()){//si quien entra en colision es el jugador
            Vector2 objetivo = new Vector2((other.transform.position.x - transform.position.x), (other.transform.position.y - transform.position.y));
            if (!other.gameObject.GetComponent<EnemyHealth>().Murio()){//&&ojos.esVisible(objetivo,alcanceVisual)){
                rotar(objetivo);
                // Debug.Log("Paso 2");
                deteccion = true;
                procesoAlarma(other.gameObject);

            }
            else
            {
                deteccion = false;
                puntosDePatrulla = ppOriginales;
            }
        }
	}
	
	void OnTriggerExit2D(Collider2D other){
		if(other.tag=="Enemy"&&!Murio()){
			deteccion=false;
            if (arma != null)
            {
                if (arma.IsBobyToBody())
                {
                    puntosDePatrulla = ppOriginales;
                }
            }
        }
        
    }

	public void aplicarDanio(int danio){
		salud-=danio;
	}
	private void moverse(Nodo punto)
    {
        Vector2 intro = new Vector2((punto.worldPosition.x - transform.position.x), (punto.worldPosition.y - transform.position.y));//Se traza un vector en la direccion del punto de ruta
        rotar(intro);
        Vector3 v = new Vector3(intro.x, intro.y, 0).normalized * Time.deltaTime;//se normaliza por el delta de tiempo para que el movimiento vaya acorde al tiempo de la escena
        v = v * velocidad;                                                                         //v = v * velocidad;//Para poder encontrar personajes con diferentes velocidades
        transform.position += v;
    }
    private void rotar(Vector2 objetivo)
    {
        float angulo = Vector2.Angle(Vector2.up, objetivo);
        if (objetivo.x >= 0)
        {
            angulo = 180 + (180 - angulo);//debido a que el motor busca la forma mas corta de calcular el angulo se debe corregir cuando se pasa hacia la parte positiva de las X
        }
        Quaternion target = Quaternion.Euler(0, 0, angulo);//Define la rotacion a aplicar en el eje Z
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * velocidad);//Rota el enemigo
    }
    private void procesoAlarma()
    {
        //GameObject other = ojos.getObjetivo();
       // procesoAlarma(other);
    }
    private void procesoAlarma(GameObject other)
    {
        if (other != null && !other.GetComponent<EnemyHealth>().Murio())
        {
            Vector2 objetivo = new Vector2((other.transform.position.x - transform.position.x), (other.transform.position.y - transform.position.y));//se traza un vector hacia el objetivo
            rotar(objetivo);
            if (arma != null)
            {
                if (arma.IsBobyToBody())
                {
                    if (auxiliar % 100 == 0)
                    {
                        ruta = new List<Nodo>();
                        puntoActual.worldPosition = transform.position;
                    }

                    puntosDePatrulla = new GameObject[1];
                    puntosDePatrulla[0] = other.gameObject;
                    auxiliar++;
                }
            }
        }
        else
        {
            puntosDePatrulla = ppOriginales;
        }
    }
}