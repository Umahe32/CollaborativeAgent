using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agente : MonoBehaviour
{
	public virtual void EnviarParametros(ParametrosAmbiente env)
	{

	}

	public virtual string Recibir()
	{
		return "";
	}

	public virtual float[] ObtenerAccion()
	{
		return new float[1] { 0.0f };
	}

	public virtual float[] ObtenerValor()
	{
		float[] valor = new float[1];
		return valor;
	}

	public virtual void EnviarEstado(List<float> estado, float recompensa, bool d)
	{

	}

}



