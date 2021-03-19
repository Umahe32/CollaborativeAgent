using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seguir : MonoBehaviour
{

    public GameObject companion;
    public Transform izquierda;
    public Transform derecha;
    public Transform centro;

    // Start is called before the first frame update
    private void Start()
    {
        //companion.transform.parent = null;
        izquierda.SetParent(companion.transform);
        derecha.SetParent(companion.transform);
        centro.SetParent(companion.transform);
    }

}
