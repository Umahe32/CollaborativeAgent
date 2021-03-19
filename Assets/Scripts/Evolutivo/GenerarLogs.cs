using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GenerarLogs : MonoBehaviour
{
    public bool LogsEnCSV = true;
    public bool LogsEnTXT = false;


    private void Start()
    {
       // GenerarTXT();
    }

    void GenerarTXT()
    {
        string rutaCompleta = @"D:\Prueba\prueba1.csv";
        string texto = "HOLA MUNDO ";

        using (StreamWriter mylogs = File.AppendText(rutaCompleta))         //se crea el archivo
        {
            string a = "otra";

            mylogs.WriteLine(texto +","+a);

            mylogs.Close();


        }
    }

    public void exportarEnCSV(string log) {

    }
    public void exportarEnTXT(string log)
    {

    }
    public void Generar(string Log) {
        if (LogsEnCSV) {
            exportarEnCSV(Log);
        }
        if (LogsEnTXT)
        {
            exportarEnTXT(Log);
        }
    }
    public void Generar(string Log,int categoria)
    {
        if (categoria==1)
        {
            exportarEnCSV(Log);
        }
        if (categoria==2)
        {
            exportarEnTXT(Log);
        }
    }
}
