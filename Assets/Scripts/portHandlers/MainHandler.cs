using System;
using System.Collections.Generic;
using UnityEngine;


public class MainHandler
{
    Dictionary<string, Action> comandi = new()
    {
        {"help", HelpTest},
    };

    public List<string> InterpretCommand(string userInput)
    {
        string[] args = userInput.Split();

        if (comandi.ContainsKey(args[0]))
        {
            comandi[args[0]]();
            return MainGlobals.risposta;
        }
        else
        {
            MainGlobals.risposta.Clear();

            MainGlobals.risposta.Add("errore - comando non esistente");

            return MainGlobals.risposta;
        }
    }

    public static void HelpTest()
    {
        MainGlobals.risposta.Clear();

        MainGlobals.risposta.Add("Terminale del Server | Online");
        MainGlobals.risposta.Add("Scrivi `help --list` per una lista di comandi");
    }
}

public class MainGlobals
{
    public static List<string> risposta = new();
}