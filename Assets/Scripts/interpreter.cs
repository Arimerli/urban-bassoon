using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using UnityEngine;

public class Interpreter 
{
    MainHandler main = new MainHandler();
    int portaCorrente = 0;

    Dictionary<int, Func<string, List<string>>> comandi;
    List<string> tempRisp;

    public List<string> Interpret(string userInput)
    {
        comandi = new Dictionary<int, Func<string, List<string>>>()
        {
            {0, main.InterpretCommand}
        };
        
        string[] args = userInput.Split();
        GameGlobals.risposta.Clear();

        if (portaCorrente == 0)
        {
            if (args[0] == "port")
            {
                portaCorrente = int.Parse(args[1]);
                return GameGlobals.risposta;
            }
            tempRisp = comandi[0](args[0]);
            for (int x = 0;x<tempRisp.Count;x++)
            {
                GameGlobals.risposta.Add(tempRisp[x]);
            }
            return GameGlobals.risposta;
        }
        else
        {
            return GameGlobals.risposta;
        }
    }
}

public class GameGlobals
{
    public static List<string> risposta = new();
}