using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.Compilation;
using UnityEngine;

public class Interpreter : MonoBehaviour
{
    List<string> risposta = new();

    public List<string> InterpretCommand(string userInput)
    {
        risposta.Clear();

        string[] args = userInput.Split();

        if(args[0] == "help")
        {
            risposta.Add("Terminale del Server | Online");
            risposta.Add("Scrivi `help --list` per una lista di comandi");

            return risposta;
        } else
        {
            risposta.Add("Comando non riconosciuto, digita help per ulteriori aiuti");

            return risposta;
        }
    }
}