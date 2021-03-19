using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TranquiloIA : MonoBehaviour {

    private bool tranquilo;
    private AlertaIA alerta;
    private PeligroIA peligro;
	// Use this for initialization
	void Start () {
        tranquilo = true;
       
       peligro= GetComponent<PeligroIA>();
       alerta = GetComponent<AlertaIA>();
        if (tranquilo) {
            alerta.enabled = false;
            peligro.enabled = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
       	}
}
