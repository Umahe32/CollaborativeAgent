using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AlgoritmoEvolutivo : MonoBehaviour {
    public static AlgoritmoEvolutivo algoritmoEvolutivo;
    private List<GenomaNEAT> poblacion;
    public List<List<GenomaNEAT>> especies;
    private List<float[]> matingPool;
    private float[] fitness;
    public float[,] resultados;
    private int numeroGeneracion=1;
    private int numeroIndividuo=0;
    private int numeroEspecie = 1;
    public float tasaMutacion=.01f;
    public float probabilidadCruce = .4f;
    public int tamanioPoblacion=100;
    public int generacionLimite=100000;
    public int maximoNeuronas = 1000000;
    private int tamañoEntradas=100;
    private int tamañoSalidas = 8;
    private float fitnessPromedio = 0;
    public float tiempoDePrueba=60;
    private float tiempoPruebaOriginal=60;
    public CompanionEvolutivo companionEvolutivo;
    private int innovacion=8;
    private float MaxFitness=0;


    public float DeltaDesconexion=2.0f;
    public float deltaPeso = 0.4f;
    public float DeltaLimite = 1.0f;

    public float puntosPorDaño=1;
    public float puntosPorMovimiento=0.002f;
    public int puntosPorTiempo=1;
    public float puntosPorEliminacion=2;
    public int puntosPorCuracion = 5;
    public float castigoPorDaño=-1;
    public float castigoPorColision=-0.002f;
    GameObject ambientes;

    private List<int> posicionesElite;
    private float totalfitnesspromedio;
    // Use this for initialization
    private void Awake()
    {
        // tiempoPruebaOriginal = tiempoPruebaOriginal;
        if (algoritmoEvolutivo == null)
        {
            algoritmoEvolutivo = this;
            DontDestroyOnLoad(gameObject);
            MaxFitness = 1.2f*((tiempoPruebaOriginal / 0.2f) - (tiempoPruebaOriginal / 15f)) * 1 + ((tiempoPruebaOriginal / 15f) * puntosPorCuracion);
            if (poblacion == null)
            {


                poblacion = new List<GenomaNEAT>();
                matingPool = new List<float[]>();
                especies = new List<List<GenomaNEAT>>();
                fitness = new float[tamanioPoblacion];
                resultados = new float[tamanioPoblacion, 5];

                crearPoblacion();
               
                
                float aux = tasaMutacion;  

                tasaMutacion = 1.0f;
                 foreach (List<GenomaNEAT> especie in especies) {
                     foreach(GenomaNEAT individuo in especie)
                     {
                        for(int i=0;i<10;i++)
                        mutacion(individuo);
                         //Debug.Log("cambie un gen");
                     }

                 }
                tasaMutacion = aux;
                 

            }

        }
        else if(algoritmoEvolutivo!=this) {
            Destroy(gameObject);
        }
    }

    void Start () {

        ambientes = GameObject.FindGameObjectWithTag("Ambiente");
        for (int i = 0; i < ambientes.transform.childCount; i++) {
            companionEvolutivo = BusquedaHijos.buscarHijoPorTag(ambientes.transform.GetChild(i).gameObject, "Companion").GetComponent<CompanionEvolutivo>();
            //Debug.Log("numero inviduo: "+ numeroIndividuo);
            GenomaNEAT aux = poblacion[numeroIndividuo];
            companionEvolutivo.individuo = numeroIndividuo;
            companionEvolutivo.setRed(aux);
            tiempoDePrueba = tiempoPruebaOriginal;
            numeroIndividuo ++;
            AmbienteEvolutivo ambienteLocal=ambientes.transform.GetChild(i).GetComponent<AmbienteEvolutivo>();
            ambienteLocal.tiempoPruebaOriginal = tiempoPruebaOriginal;
            ambienteLocal.tiempoDePrueba = tiempoPruebaOriginal;
            ambienteLocal.listo = true;
        }
        companionEvolutivo = GameObject.FindGameObjectWithTag("Companion").GetComponent<CompanionEvolutivo>();


    }

    void Update()
    {
        UIManager.generacion = numeroGeneracion;
        UIManager.individuo = numeroIndividuo + 1;
        UIManager.especie = numeroEspecie;

        if (companionEvolutivo == null && numeroIndividuo<tamanioPoblacion && ambientes==null) {///si el de cada ambiente 
            /*companionEvolutivo = GameObject.FindGameObjectWithTag("Companion").GetComponent<CompanionEvolutivo>();
            companionEvolutivo.setRed(poblacion[numeroIndividuo]);
            companionEvolutivo.setNumeroIndividuo(numeroIndividuo);
           // companionEvolutivo.GenerarRed(maximoNeuronas);*/
            ambientes = GameObject.FindGameObjectWithTag("Ambiente");
            for (int i = 0; i < ambientes.transform.childCount; i++)
            {
                companionEvolutivo = BusquedaHijos.buscarHijoPorTag(ambientes.transform.GetChild(i).gameObject, "Companion").GetComponent<CompanionEvolutivo>();
               // Debug.Log("numero inviduo: " + numeroIndividuo);
                GenomaNEAT aux = poblacion[numeroIndividuo];
                companionEvolutivo.individuo = numeroIndividuo;
                companionEvolutivo.setRed(aux);
                tiempoDePrueba = tiempoPruebaOriginal;
                numeroIndividuo++;
                AmbienteEvolutivo ambienteLocal = ambientes.transform.GetChild(i).GetComponent<AmbienteEvolutivo>();
                ambienteLocal.tiempoPruebaOriginal = tiempoPruebaOriginal;
                ambienteLocal.tiempoDePrueba = tiempoPruebaOriginal;
                ambienteLocal.listo = true;
            }
            companionEvolutivo = GameObject.FindGameObjectWithTag("Companion").GetComponent<CompanionEvolutivo>();
        }


        if (numeroIndividuo >= tamanioPoblacion && ambientes.GetComponent<Gimnasio>().Termino()) 
        {
            //Debug.Log("Siguiente paso");
            calificar();
            seleccion();
            List<GenomaNEAT> nuevaPoblacion= new List<GenomaNEAT>();
            foreach (List<GenomaNEAT> especie in especies) {
                 nuevaPoblacion.Add(cruceEspecie(especie));
            }
            while (nuevaPoblacion.Count < tamanioPoblacion) {
                int seleccion = Random.Range(0, especies.Count - 1);
               //Debug.Log("Especies: " + especies.Count + " Seleccion: " + seleccion);
                nuevaPoblacion.Add(cruceEspecie(especies[seleccion]));
            }
            especies = new List<List<GenomaNEAT>>();
            foreach (GenomaNEAT hijo in nuevaPoblacion) {
                aniadirAEspecies(hijo); 
            }
            numeroGeneracion++;
            numeroIndividuo = 0;
            Debug.Log(estadoPoblacion());
            poblacion = nuevaPoblacion;
            //aniadirAEspecies(todos)
           // ambientes.GetComponent<Gimnasio>().RecargarTodo();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
           // Debug.Log("Siguiente paso 2");
        }

    }


    
    private void crearPoblacion() {
        GenomaNEAT cromosoma,CromosomaBase;
        CromosomaBase = ScriptableObject.CreateInstance<GenomaNEAT>();
        CromosomaBase.tamanioEntrada = tamañoEntradas;
        CromosomaBase.tamanioSalida = tamañoSalidas;
        List<Neurona> red= CromosomaBase.RedBase(maximoNeuronas);
        for (int i = 0; i < tamanioPoblacion; i++){
            cromosoma = ScriptableObject.CreateInstance<GenomaNEAT>();
            cromosoma.tamanioEntrada = tamañoEntradas;
            cromosoma.tamanioSalida = tamañoSalidas;
           // cromosoma.genes = new List<GenNEAT>(CromosomaBase.genes);
            // cromosoma.setGenes(CromosomaBase.genes);
            //cromosoma.SetRedInicial(red);
            cromosoma.RedBase(maximoNeuronas);
            cromosoma = mutacion(cromosoma);
            poblacion.Add(cromosoma);
            aniadirAEspecies(cromosoma);
        }
        numeroIndividuo = 0;
        Debug.Log(estadoPoblacion());
    }

    public string estadoPoblacion() {
        string salida = "Generacion: "+ numeroGeneracion + "\n";
        int pos = 0;
        string rutaCompleta = @"D:\Prueba\generacion"+numeroGeneracion+".csv";
        using (StreamWriter mylogs = File.AppendText(rutaCompleta))         //se crea el archivo
        {
            mylogs.WriteLine("Generacion: " + numeroGeneracion + "\n");
            foreach (GenomaNEAT cromosoma in poblacion) {
                salida += "[";
                
                mylogs.WriteLine("Invidividuo:" + (pos + 1) + " Especie:" + cromosoma.getNumEspecie() + " Fitness: " + fitness[pos] + "\n");
                foreach (GenNEAT gen in cromosoma.genes)
                {
                    string habilitado = gen.habilitado ? "#t" : "#f";
                    string a =gen.entrada + ","+ gen.salida+","+ System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0.0000}",gen.peso) +","+habilitado+","+gen.innovacion;
                    mylogs.WriteLine(a);
                }
                salida += "] Especie:"+cromosoma.getNumEspecie() +" Fitness: "+fitness[pos]+"\n";
                pos++;
            }
            
            mylogs.Close();
        }
        salida += MaxFitness;
        return salida;
    }
    
    public void calificar() {
        float suma;
        
        string rutaCompleta = @"D:\Prueba\Top\top.csv";
        using (StreamWriter mylogs = File.AppendText(rutaCompleta))
        {
            for (int i = 0; i < tamanioPoblacion; i++) {
                suma = 0;
                suma = resultados[i, 0];//puntos por segundo vivo
                suma += resultados[i, 1];//puntos curacion
                suma += resultados[i, 2];//desactivacion de peligros ambientales
                suma += resultados[i, 3] ;//daño a enemigos
                suma += resultados[i, 4];
                fitness[i] = suma/ MaxFitness; //suma de puntos sobre maximo esperado
                fitnessPromedio += fitness[i];
                poblacion[i].fitness = fitness[i];
            }

            
            fitnessPromedio = fitnessPromedio / tamanioPoblacion;
            float MejorFitness = Mathf.Max(fitness);
            int posicion = System.Array.IndexOf(fitness,MejorFitness);


            mylogs.WriteLine(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0.0000}",MejorFitness )+ " , " + System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0.0000}",fitnessPromedio)+ " , " + System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0.00}", MaxFitness));
            mylogs.Close();
        }

    }

    public void RemoverEspeciesDebiles() {
        List<List<GenomaNEAT>> especiesSupervivientes = new List<List<GenomaNEAT>>();
        List<GenomaNEAT> MejorEspecie = new List<GenomaNEAT>();
        Stack<List<GenomaNEAT>> especiesElite = new Stack<List<GenomaNEAT>>();
        float max = 0;
        for (int i = 0; i < especies.Count; i++) {
            float fitnessEspecie = CalcularFitnessPromedio(especies[i]);
            if (fitnessEspecie > max) {
                especiesElite.Push(especies[i]);
                max = fitnessEspecie;
            }
            float probabilidadDeSupervivencia = Mathf.Floor(fitnessEspecie/fitnessPromedio);
            //Debug.Log("PS: "+probabilidadDeSupervivencia);
            if (probabilidadDeSupervivencia >= 1) {
                especiesSupervivientes.Add(especies[i]);
            }
        }
        if (especiesSupervivientes.Count <= 0) {
            if (especiesElite.Count > 10) {
                for (int i = 0; i <10; i++) {
                    especiesSupervivientes.Add(especiesElite.Pop());   
                }
                Debug.Log("pasan los 5 mejores");
            }
        }
        especies = especiesSupervivientes;
        Debug.Log("numero especies:" +especies.Count);
    }
    public float CalcularFitnessPromedio(List<GenomaNEAT>especie) {
        int aux = 0;
        float promedio = 0f;
        foreach (GenomaNEAT individuo in especie) {

            promedio += individuo.fitness;
            
            aux++;
        }
        promedio = promedio / aux;
        return promedio;
    }

    public void calificarUno(int i)
    {
        float suma;
        suma = 0;
        suma = resultados[i, 0];//puntos por segundo vivo
        suma += resultados[i, 1];//puntos curacion
        suma += resultados[i, 2];//desactivacion de peligros ambientales
        suma += resultados[i, 3];//daño a enemigos
        suma += resultados[i, 4];
        fitness[i] = suma /MaxFitness; //suma de puntos sobre maximo esperado
        UIManager.fitnessActual = fitness[i];
        UIManager.puntaje = suma;
    }



    public void seleccion() {
        float suma=0;
        for (int i = 0; i < tamanioPoblacion; i++) {
            suma += fitness[i];
        }
        float[] probabilidad = new float[tamanioPoblacion];
        for(int i = 0; i < tamanioPoblacion; i++)
        {
            probabilidad[i] = fitness[i] / suma;
        }

        float[] q = new float[tamanioPoblacion + 1];
        q[0] = 0;

        for (int k = 1; k < tamanioPoblacion; k++)
        {
            q[k] = probabilidad[k] + q[k - 1];
        }

        for (int j = 0; j < (int) Mathf.Floor(tamanioPoblacion/2); j++)
        {
            float rj = Random.value;
            int seleccionado = 0;
            for (int i = 1; i < tamanioPoblacion; i++)
            {
                if (rj > q[i - 1] && rj < q[i])
                {
                    seleccionado = i - 1;
                    break;
                }
                if (rj > q[tamanioPoblacion])
                {
                    seleccionado = tamanioPoblacion - 1;
                }
            }
            //  matingPool.Add(poblacion[seleccionado]);
           
            RemoverEspeciesDebiles();
            
        }

    }
    public GenomaNEAT cruceEspecie(List<GenomaNEAT> especie) {
        GenomaNEAT hijo = ScriptableObject.CreateInstance<GenomaNEAT>();

        if (Random.Range(0f, 1f) < probabilidadCruce) {
            int seleccion = Random.Range(0, especie.Count - 1);
            int seleccion2= Random.Range(0, especie.Count - 1);
            //Debug.Log("Cant. Especies" + especie.Count+"Especie1: "+seleccion+" Especie2: "+seleccion2);
            GenomaNEAT genoma1 = especie[seleccion];
            GenomaNEAT genoma2 = especie[seleccion2];
            hijo = cruceGenoma(genoma1, genoma2);
        }
        else
        {
            int seleccion = Random.Range(0, especie.Count - 1);
            //Debug.Log("Cant. Especies" + especie.Count+" Especie1: " + seleccion);
            GenomaNEAT genoma1 = especie[seleccion];
            hijo = genoma1;
        }
        mutacion(hijo);

        return hijo;
    }

    public GenomaNEAT cruceGenoma(GenomaNEAT cromosoma1, GenomaNEAT cromosoma2) {
        List<GenNEAT> innovaciones2=new List<GenNEAT>();
        GenomaNEAT hijo = ScriptableObject.CreateInstance<GenomaNEAT>();
        if (cromosoma1.fitness > cromosoma2.fitness) {
            GenomaNEAT tempg = cromosoma1;
            cromosoma1 = cromosoma2;
            cromosoma2 = tempg;
         }
        foreach (GenNEAT gen in cromosoma2.genes) {
            
            innovaciones2.Add(gen);
        }
        foreach (GenNEAT gen in cromosoma1.genes)
        {
            GenNEAT otroGen;
            if (gen.innovacion < innovaciones2.Count)
            {
                otroGen = innovaciones2[gen.innovacion];
                
            }
            else
            {
                //Debug.Log("otro gen");
                otroGen = gen;
            }
            if (otroGen != null && Random.Range(1, 2) == 1 && otroGen.habilitado)
                hijo.genes.Add(otroGen);
            else
                hijo.genes.Add(gen);
        }  
        hijo.maximoNeuronas = Mathf.Max(cromosoma1.maximoNeuronas, cromosoma2.maximoNeuronas);
        
        return hijo;
    }



    /*public void cruce() {
        float mutar;
        float[] hijo1 = new float[5];
        float[] hijo2 = new float[5];
        List<float[]> hijos = new List<float[]>();
        for (int i = 0; i < matingPool.Count; i = i + 2)
        {
            hijo1[0] = matingPool[i][0];
            hijo1[1] = matingPool[i+1][1];
            hijo1[2] = matingPool[i][2];
            hijo1[3] = matingPool[i+1][3];
            hijo1[4] = matingPool[i][4];
            mutar = Random.value;
            //Debug.Log(mutar);
            if (mutar <= tasaMutacion) {
                hijo1=mutacion(hijo1);
            }

            hijo2[0] = matingPool[i+1][0];
            hijo2[1] = matingPool[i][1];
            hijo2[2] = matingPool[i+1][2];
            hijo2[3] = matingPool[i][3];
            hijo2[4] = matingPool[i+1][4];
            mutar = Random.value;
            if (mutar <= tasaMutacion)
            {
                hijo2=mutacion(hijo2);
            }
            hijos.Add(hijo1);
            hijos.Add(hijo2);
        }
        poblacion.Clear();
       // Debug.Log("# poblacion " + poblacion.Count);
     /*   foreach (float[] cromosoma in matingPool) {
            poblacion.Add(cromosoma);
        }
        foreach (float[] cromosoma in hijos) {
            poblacion.Add(cromosoma);
        }*/
    /*    matingPool.Clear();
      //  Debug.Log("# poblacion " + poblacion.Count +"Mating pool "+matingPool.Count);
        numeroGeneracion++;
        
        numeroIndividuo = 0;
    }*/


    private GenomaNEAT mutacion(GenomaNEAT cromosoma)
    {
        int muto = 0;
        int posicion = Random.Range(0, 1);
        if (Random.Range(0f, 1f) < tasaMutacion) {//conexiones
            mutarPesos(cromosoma);
            muto++;
        }
        if (Random.Range(0f, 1f) < tasaMutacion) //Link
        {
            mutarLink(cromosoma,false);
            muto++;
        }
        if (Random.Range(0f, 1f) < tasaMutacion) //Bias
        {
            mutarLink(cromosoma, true);
            muto++;
        }
        if (Random.Range(0f, 1f) < tasaMutacion)//node
        {
            mutarNodo(cromosoma);
            muto++;
        }
        if (Random.Range(0f, 1f) < tasaMutacion)//enable
        {
            MutarHabilitarDesHab(cromosoma, true);
            muto++;
        }
        if (Random.Range(0f, 1f) < tasaMutacion)//enable
        {
            MutarHabilitarDesHab(cromosoma, false);
            muto++;
        }
        //Debug.Log("No. Mutacion:"+muto);
        return cromosoma;
        
    }

    public void CambiarCromosoma() {//cambiar 
        
        float tiempoTotal = tiempoPruebaOriginal - tiempoDePrueba;
        companionEvolutivo.setTiempoVida(tiempoTotal);
        companionEvolutivo.Evaluar();
       
        this.numeroIndividuo++;
        tiempoDePrueba = tiempoPruebaOriginal;
        numeroEspecie=numeroIndividuo;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public GenomaNEAT cambiarCromosoma(CompanionEvolutivo companion) {
        companionEvolutivo = companion;
        numeroIndividuo++;
        if (numeroIndividuo <tamanioPoblacion) {
            companionEvolutivo.setRed(poblacion[numeroIndividuo]);
            companionEvolutivo.individuo = numeroIndividuo;
        }
        return null; 
    }



    //mutaciones especiales

    /// <summary>
    /// Toma un cromosoma y crea una mutacion, que añade un link entre neuronas
    /// </summary>
    /// <param name="cromosoma">Es la estructura que representa 1 individuo de la poblacion</param>
    /// <param name="forzarBias">Forzar conexion, para el nodo BIAS(sesgo),nodo que se conecta a toda la red neuronal</param>
    public void mutarLink(GenomaNEAT cromosoma,bool forzarBias) {
        int neuro1 = Random.Range(0, tamañoEntradas);
        int neuro2 = Random.Range(tamañoEntradas, cromosoma.tamañoRed);
        GenNEAT link = ScriptableObject.CreateInstance<GenNEAT>();
        if (neuro1 <= tamañoEntradas && neuro2 <= tamañoEntradas) {
            return;
        }
        if (neuro2 <= tamañoEntradas) {
            int temp = neuro1;
            neuro1 = neuro2;
            neuro2 = temp;
        }
        link.entrada = neuro1;
        link.salida = neuro2;
        if (forzarBias) {
            link.entrada = tamañoEntradas;
        }
        if (contieneLink(cromosoma.genes, link)) {
            return;
        }
        link.innovacion = cromosoma.genes.Count;
        link.peso = Random.Range(-1.0f, 1.0f);
        cromosoma.genes.Add(link);
    }

    /// <summary>
    /// Toma un cromosoma y realiza una mutacion de nodo, añadiendo una nueva neurona a la red
    /// </summary>
    /// <param name="cromosoma">Es la estructura que representa 1 individuo de la poblacion</param>
    public void mutarNodo(GenomaNEAT cromosoma)
    {
        if (cromosoma.genes.Count == 0) {
            return;
        }
        cromosoma.maximoNeuronas++;
        GenNEAT gen = cromosoma.genes[Random.Range(1, cromosoma.genes.Count-1)];
        if (!gen.habilitado) {
            return;
        }
        gen.habilitado = false;
        GenNEAT gen1 = GenNEAT.copiarGen(gen);
        gen1.salida= cromosoma.maximoNeuronas;
        gen1.peso = Random.Range(-1f,1f);
        gen1.innovacion = cromosoma.genes.Count;
        gen1.habilitado = true;
        cromosoma.genes.Add(gen1);


        GenNEAT gen2 = GenNEAT.copiarGen(gen);
        gen1.entrada = cromosoma.maximoNeuronas;
        gen2.innovacion = cromosoma.genes.Count;
        gen2.habilitado = true;
        cromosoma.genes.Add(gen2);
    }

    /// <summary>
    /// toma un cromosoma y muta los pesos de la red neuronal actual
    /// </summary>
    /// <param name="cromosoma">Es la estructura que representa 1 individuo de la poblacion</param>
    public void mutarPesos(GenomaNEAT cromosoma) {
        foreach (GenNEAT gen in cromosoma.genes) {
            float probabilidad= Random.Range(0.0f, 1.0f);
            if (probabilidad < tasaMutacion)
            {
                gen.peso = Random.Range(-1f, 1f);
                if (gen.peso > 1f) {
                    gen.peso = 1f;
                }
                if (gen.peso < -1f)
                {
                    gen.peso = -1f;
                }
            }
        }
    }

    /// <summary>
    /// Toma un cromsoma y  un valor de habilitacion, y muta las posibilidades de conexion 
    /// </summary>
    /// <param name="cromosoma">Es la estructura que representa 1 individuo de la poblacion</param>
    /// <param name="habilitar">habilita las conexion negando la entrada</param>
    public void MutarHabilitarDesHab(GenomaNEAT cromosoma,bool habilitar) {
        List<GenNEAT> candidatos = new List<GenNEAT>(); ; 
        foreach (GenNEAT gen in cromosoma.genes) {
            if (gen.habilitado != habilitar)
                candidatos.Add(gen);
        }
        if (candidatos.Count==0) {
		    return;
        }
        int indice=Random.Range(0, (candidatos.Count - 1));
        candidatos[indice].habilitado = !candidatos[indice].habilitado;
    }
    /// <summary>
    /// Verifica si en una lista de genes, existe un gen que contiene un gen link
    /// </summary>
    /// <param name="genes">Lista de genes sobre la que se busca</param>
    /// <param name="link">gen link buscado</param>
    /// <returns></returns>
    public bool contieneLink(List<GenNEAT> genes, GenNEAT link) {
        foreach (GenNEAT gen in genes) {
            if (gen.salida == link.salida && gen.entrada == link.entrada) {
                return true;
            }
        }
        return false;
    }


    /// <summary>
    /// Devuelve el indice que identifica una neurona 
    /// </summary>
    /// <param name="cromosoma"></param>
    /// <param name="NoInput"></param>
    /// <returns></returns>
    public int neuroAleatorio(GenomaNEAT cromosoma , bool NoInput) {
        List<bool> neurona = new List<bool>();
        neurona.Capacity=cromosoma.tamañoRed;
        for (int i = 0; i < neurona.Capacity; i++) {
            neurona.Add(false);
        }
        if (!NoInput) {
            for (int i = 0; i < tamañoEntradas; i++) {
                neurona[i]= true;
            }
        }
        for (int o = tamañoEntradas; o < (tamañoEntradas+tamañoSalidas); o++) {
            neurona[o] = true;
        }
        foreach (GenNEAT gen in cromosoma.genes) {
            if (!NoInput || gen.entrada > tamañoEntradas) {
                neurona[gen.entrada] = true;
            }
            if (!NoInput || gen.salida > tamañoEntradas)
            {
                neurona[gen.salida] = true;
            }
        }
        int n =Random.Range(0, neurona.Count);
        int k = 0;
        foreach (bool valor in neurona) {
            n--;
            k++;
            if (n == 0) {
                return k;
            }
        }
      

        return 0;
    }
    /// <summary>
    /// añade cromosomas al set de especies
    /// </summary>
    /// <param name="cromosoma"></param>
    public void aniadirAEspecies(GenomaNEAT cromosoma)
    {
        bool especieEncontrada = false;
        int especieID = 0;
        if (especies != null)
        {
            for (int i = 0; i < especies.Count; i++)
            {
                if (especies[i] != null)
                {
                    List<GenomaNEAT> especieAux = especies[i];

                    if (!especieEncontrada && mismaEspecie(cromosoma, especieAux[0]))
                    {
                        cromosoma.setNumEspecie(i);
                        especies[i].Add(cromosoma);
                    }

                }
            }
            especieID = especies.Count;
        }
        else
        {
            especieID++;   

        }

        if (!especieEncontrada)
        {
            List<GenomaNEAT> especie = new List<GenomaNEAT>();
            cromosoma.setNumEspecie(especies.Count);
            especie.Add(cromosoma);
            especies.Add(especie);
        }
    }

    /// <summary>
    /// Revision por estructura, computacionalmente costosa
    /// </summary>
    /// <param name="cromosoma1"></param>
    /// <param name="cromosoma2"></param>
    /// <returns></returns>
    public bool mismaEspecie(GenomaNEAT cromosoma1, GenomaNEAT cromosoma2)
    {
        float VaricionDesunion = DeltaDesconexion * disjunciones(cromosoma1.genes, cromosoma2.genes);
        float variacionPeso = deltaPeso * Pesos(cromosoma1.genes, cromosoma2.genes);
        return VaricionDesunion + variacionPeso < DeltaLimite;
    }

    public float Pesos(List<GenNEAT> genes1, List<GenNEAT> genes2)
    {
        List<GenNEAT> genAux = new List<GenNEAT>();
        foreach (GenNEAT gen in genes2)
        {
            genAux.Add(gen);
        }

        float sum = 0;
        float coincidencia = 0;
        foreach (GenNEAT gen in genes1)
        {
            if (gen.innovacion < genAux.Count)
            {
                if (genAux[gen.innovacion] != null)
                {
                    sum += Mathf.Abs(gen.peso - genAux[gen.innovacion].peso);
                    coincidencia++;

                }
            }
        }
        return sum / coincidencia;
    }

    public float disjunciones(List<GenNEAT> genes1, List<GenNEAT> genes2)
    {
        List<bool> i1 = new List<bool>();
        List<bool> i2 = new List<bool>();
        int genesDisconexos = 0;

        foreach (GenNEAT gen in genes1)
        {
            i1.Add(true);
        }
        foreach (GenNEAT gen in genes2)
        {
            i2.Add(true);
        }

        foreach (GenNEAT gen in genes1)
        {
            if (gen.innovacion < i2.Count)
            {
                if (!i2[gen.innovacion])
                {
                    genesDisconexos++;
                }
            }else
            {
                genesDisconexos++;
            }
        }

        foreach (GenNEAT gen in genes2)
        {
            if (gen.innovacion < i1.Count)
            {
                if (!i1[gen.innovacion])
                {
                    genesDisconexos++;
                }
            }
            else {
                genesDisconexos++;
            }
        }
        float n = Mathf.Max(genes1.Count, genes2.Count);
        if (n <= 0) {
            n = 0.1f;
        }
        return genesDisconexos / n;
        
    }
    public bool ultimoInvidividuo() {
        return numeroIndividuo >= tamanioPoblacion;
    }
}
