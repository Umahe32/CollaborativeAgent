using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParametrosAmbiente
{
	public int estados_tamaño { get; set; }
	public int accion_tamaño { get; set; }
	public int observaciones_tamaño { get; set; }
	public List<string> accion_descripcion { get; set; }
	public string nombre_ambiente { get; set; }
	// public string action_space_type { get; set; }
	// public string state_space_type { get; set; }
	public int num_agentes { get; set; }
}


public abstract class AmbienteSarsa : MonoBehaviour
{
	public float recompensa;
	public bool hecho;
	public int maxPasos;
	public int pasoActual;
	public bool inicio;
	public bool aceptacionPasos;
	/// <summary>
	/// /////// objetos del juego pripios
	/// </summary>
	public float[] enemigos;
	public float[] obstaculos;
	public float[] items;



	public Agente agente;
	public int comPort;
	public int frameToSkip;
	public int framesSinceAction;
	// public string currentPythonCommand;
	public bool skippingFrames;
	public float[] acciones;
	public float tiempoEspera;
	public int recuentoEpisodios;
    public GridAI ambienteGeneral;
	// public bool humanControl;

	// public int bumper; --> parachoques

	public ParametrosAmbiente parametrosAmbiente;

	public virtual void SetUp()
	{
		parametrosAmbiente = new ParametrosAmbiente()
		{
			observaciones_tamaño = 0,
			estados_tamaño = 0,
			accion_descripcion = new List<string>(),
			accion_tamaño = 0,
			nombre_ambiente = "Null",
			//action_space_type = "discrete",
			//state_space_type = "discrete",
			num_agentes = 1
		};
		inicio = false;
		aceptacionPasos = true;
	}

	// Update is called once per frame
	void Update()
	{

	}

	public virtual List<float> recolectarEstados()
	{
        List<float> estado = ambienteGeneral.ConstruirEntradasSarsa();
		return estado;
	}

	public virtual void Paso()
	{
		aceptacionPasos = false;
		
		pasoActual += 1;
		if (pasoActual >= maxPasos)
		{
			hecho = true;
		}

		recompensa = 0;
		acciones = agente.ObtenerAccion();
		framesSinceAction = 0;

		int enviarAccion = Mathf.FloorToInt(acciones[0]);
		PasoIntermedio(enviarAccion);

		//StartCoroutine(WaitStep());
	}

	public virtual void PasoIntermedio(int accion)
	{

	}

	public virtual void PasoIntermedio(float[] accion)
	{

	}

	public IEnumerator PasoDeEspera()
	{
		yield return new WaitForSeconds(tiempoEspera);
		PasoFinal();
	}

	public virtual void PasoFinal()
	{
		agente.EnviarEstado(recolectarEstados(), recompensa, hecho);
		skippingFrames = false;
		aceptacionPasos = true;
	}

	public virtual void Reset()
	{
		recompensa = 0;
		pasoActual = 0;
		recuentoEpisodios++;
		hecho = false;
		aceptacionPasos = false;
	}

	public virtual void EndReset()
	{
		agente.EnviarEstado(recolectarEstados(), recompensa, hecho);
		skippingFrames = false;
		aceptacionPasos = true;
		inicio = true;
		framesSinceAction = 0;
	}

	public virtual void RunMdp()
	{
		if (aceptacionPasos == true)
		{
			if (hecho == false)
			{
				Paso();
			}
			else
			{
				Reset();
			}
		}
	}


}
