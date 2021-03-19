using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusquedaHijos : MonoBehaviour {

    public static GameObject buscarHijoPorTag(GameObject gameObj ,string Tag){
        int cantidadHijos = gameObj.transform.childCount;
        GameObject retorno = null;
        for (int i = 0; i < cantidadHijos; i++){
            if (gameObj.transform.GetChild(i).gameObject.CompareTag(Tag)){
                retorno = gameObj.transform.GetChild(i).gameObject;
            }
        }
        return retorno;
    }

}
