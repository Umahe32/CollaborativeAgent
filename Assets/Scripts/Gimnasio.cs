using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gimnasio : MonoBehaviour
{
    public GameObject ambienteBase;
    public float tiempoPruebas=60f;
    public bool termino;
    public AlgoritmoEvolutivo evolutivo;

    private void Start()
    {
        evolutivo = GameObject.FindGameObjectWithTag("Evolutivo").GetComponent<AlgoritmoEvolutivo>();
    }

    void Update()
    {
       /* if (!Termino())
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<AmbienteEvolutivo>().reiniciar)
                {
                    RecargarAmbiente(i);
                }
            }
        }*/
    }

    void RecargarAmbiente(int i) {
        Transform ambienteARefrescar=transform.GetChild(i);
        GameObject ambienteNuevo= Instantiate(ambienteBase, ambienteARefrescar);
        ambienteNuevo.transform.SetParent(ambienteARefrescar.parent);
        Destroy(transform.GetChild(i).gameObject);
        ambienteNuevo.GetComponentInChildren<CompanionEvolutivo>().evolutivo = evolutivo;
        evolutivo.cambiarCromosoma(ambienteNuevo.GetComponentInChildren<CompanionEvolutivo>());
        ambienteNuevo.GetComponent<AmbienteEvolutivo>().NuevoTimer(tiempoPruebas);
    }

    public void RecargarTodo() {
        for (int i = 0; i < transform.childCount; i++)
        {
            RecargarAmbiente(i);
        }
    }

    public bool Termino() {
        bool salida=false;
        int porTerminar = 100;
        for (int i = 0; i < transform.childCount; i++)
        {
            salida=transform.GetChild(i).GetComponent<AmbienteEvolutivo>().termine;
            if (salida) {
                porTerminar--;
            }
            
        }
        Debug.Log("Aun por acabar:" + porTerminar);
        if (porTerminar == 0)
        {
            salida = true;
        }
        else {
            salida = false;
        }
        return salida;
    }
}
