using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IAPlayer : MonoBehaviour {

    public NavMeshAgent Agente;
    public AIShooting disparador;
    public PlayerHealth player;
    public bool EsEntrenador = false;
    private Collider colisionador;
    CompanionEvolutivo companionEvolutivo;

    // Use this for initialization
    void Start() {
        Agente = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (EsEntrenador)
        {
            player = BusquedaHijos.buscarHijoPorTag(transform.parent.parent.gameObject, "Player").GetComponent<PlayerHealth>();
        }
        else {
            player =GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        }
        colisionador = gameObject.GetComponent<CapsuleCollider>();
        companionEvolutivo = GameObject.FindGameObjectWithTag("Companion").GetComponent<CompanionEvolutivo>();
        //Debug.Log("" + disparador.GetInstanceID());
        //Invoke("movimientoAleatorioV2", 1.0f);
        //Invoke("disparoNoApuntadoAleatorio", 2.0f);
    }



    private void movimientoAleatorio() {
        Vector3 posicionAleatoria = new Vector3(Random.Range(-23.0f, 17.0f), 0, Random.Range(-18.0f, 27.0f));//acorde al tamaño de la escena 
        Agente.SetDestination(posicionAleatoria);
        Invoke("movimientoAleatorio", 1.0f);
    }
    private void disparoNoApuntadoAleatorio() {
        disparador.dispararIA();
        Invoke("disparoNoApuntadoAleatorio", 2.0f);
    }

    private void movimientoAleatorioV2() {
        int r = Random.Range(0, 5);
        switch (r) {
            case 0:
                Adelante();
                break;
            case 1:
                Atras();
                break;
            case 2:
                Izquierda();
                break;
            case 3:
                Derecha();
                break;
            case 4:
                Disparar();
                break;
        }
        Invoke("movimientoAleatorioV2", 1.0f);
    }

    public void Derecha()
    {
        Vector3 direccion = transform.position + (Vector3.right * 2);
        Agente.SetDestination(direccion);
    }
    public void Atras()
    {
        Vector3 direccion = transform.position + (Vector3.back * 2);
        Agente.SetDestination(direccion);
    }
    public void Izquierda()
    {
        Vector3 direccion = transform.position + (Vector3.left * 2);
        Agente.SetDestination(direccion);
    }
    public void Adelante() {
        Vector3 direccion = transform.position + (Vector3.forward * 2);
        Agente.SetDestination(direccion);
    }
    public void Disparar() {
        disparador.dispararIA();
    }
    public void Rotar45() {
        gameObject.transform.Rotate(0,45,0,Space.Self);
    }
    public void Rotar45N()
    {
        gameObject.transform.Rotate(0,-45, 0, Space.Self);
    }
    public void curar() {
        player.TakeDamage(-10);
        Debug.Log("Aqui deberia estar curando a alguien");
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(companionEvolutivo!=null)
            companionEvolutivo.aplicarCastigo((int)CompanionEvolutivo.Categoria.MOVIMIENTO);
    }
    float timer = 0;
    private void OnCollisionStay(Collision collision)
    {
        timer += Time.deltaTime;
        if (timer >= 0.2f) {
            if (companionEvolutivo != null)
                companionEvolutivo.aplicarCastigo((int)CompanionEvolutivo.Categoria.MOVIMIENTO);
            timer = 0;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        timer = 0;
    }

}
