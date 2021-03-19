using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puntoPatrulla : MonoBehaviour {
    public GridAI guia;
    // Use this for initialization
    private void Awake()
    {
        //guia = GetComponent<GridAI>();
    }
    void Update() {
        transform.position=guia.NodoDeUnPunto(transform.position).worldPosition;
	}
	

}
