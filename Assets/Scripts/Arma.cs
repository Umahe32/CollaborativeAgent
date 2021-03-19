using UnityEngine;
using System.Collections;
using UnityEngine.UI;


/**
 * Representa las funcionalidades de las armas tales como: arcos, mosquetes, pistolas, hachas y demas
 * NOTA: toda arma a distacia debe tener un objeto hijo, disparador.
 **/
public class Arma : MonoBehaviour {
	// private Animator anim;///permite animar el arma
    protected bool isBodyToBody = false;
    public float alcance = 1f;
    public float[] posicionPorDefecto = new float[3];///Posicion donde se acomoda el arma cuando es recogida por el jugador

	public bool isThrowable=false;///se puede lanzar? ej: lanza
   	
	// Use this for initialization
	void Start () {
        //anim = GetComponent<Animator>();
	}

    public virtual void ataque() { }

    public virtual string enviarTextoPantalla() {
        return "";
    }
    public virtual string enviarMunicionDisponibleAPantalla(){
        return "";
    }
    public virtual int getMunicionDisponible(){
        return 1;
    }

    public bool IsBobyToBody() {
        return isBodyToBody;
    }
    public void reposicionar()//Reacomoda el arma cuando es recogida
    {
        transform.localPosition = new Vector3(posicionPorDefecto[0], posicionPorDefecto[1], posicionPorDefecto[2]);
        transform.Rotate(0, 0, (-1*transform.localEulerAngles.z));
        
    }
    public float getAlcance() {
        return alcance;
    }

}