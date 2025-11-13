using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Rendering.RenderGraphModule;
using Unity.VisualScripting;

public class TerminalManager : MonoBehaviour
{
    [Header("Terminal UI References")]
    public TMP_InputField terminalInput;
    public GameObject userInputLine;
    public GameObject directoryLine;
    public GameObject responseLine;
    public GameObject msgList;

    Interpreter interpreter;


    private void Start()
    {
        interpreter = new Interpreter();

        if (interpreter == null)
            Debug.LogError("Interpreter component NOT FOUND on this GameObject!");
    }

    void Awake()
    {
        if (terminalInput == null)
        {
            Debug.LogError("TerminalManager: terminalInput is not assigned!");
            return;
        }

        // Make sure the input field is single line so Enter submits
        terminalInput.lineType = TMP_InputField.LineType.SingleLine;

        // Listen for submissions
        terminalInput.onSubmit.AddListener(OnSubmitCommand);
    }

    void OnDestroy()
    {
        if (terminalInput != null)
        {
            terminalInput.onSubmit.RemoveListener(OnSubmitCommand);
            terminalInput.onEndEdit.RemoveListener(OnSubmitCommand);
        }
    }

    private void OnSubmitCommand(string userInput)
    {
        if (string.IsNullOrEmpty(userInput))
            return;

        //Debug.Log($"[Terminal] Submitted: \"{userInput}\"");

        HandleSubmit(userInput);

        // Clear and refocus
        terminalInput.text = "";
        terminalInput.ActivateInputField();
        terminalInput.Select();
    }

    private void HandleSubmit(string userInput)
    {
        AddDirectoryLine(userInput);
        AddInterpreterLines(interpreter.Interpret(userInput));

        // Move user input line to bottom
        if (userInputLine != null)
        {
            string[] args = userInput.Split();
            if (args[0] == "port")
            {
                userInputLine.TryGetComponent<setPortOnRequest>(out var dirText);
                dirText.SetDirectoryText("admin@server/" + args[0] + "_" + args[1] + " >");
            } 
            userInputLine.transform.SetAsLastSibling();
        }
        // Scroll to bottom
        Canvas.ForceUpdateCanvases();
    }

    private void AddDirectoryLine(string userInput)
    {
        GameObject msg = Instantiate(directoryLine, msgList.transform);
        msg.transform.SetSiblingIndex(msgList.transform.childCount - 1);
 
        if (msg.TryGetComponent<DirectoryLine>(out var dir))
            dir.SetText(userInput);
        else
        {
            // fallback: try to assign automatically as before
            var texts = msg.GetComponentsInChildren<TMP_Text>();
            if (texts.Length > 1)
                texts[1].text = userInput;
            else if (texts.Length == 1)
                texts[0].text = userInput;
            else
                Debug.LogWarning("DirectoryLine prefab has no TMP_Text component and no DirectoryLine script!");
        }
    }

    private void AddInterpreterLines(List<string> rispostaTerminale)
    {
        for (int i = 0; i < rispostaTerminale.Count; i++)
        {
            GameObject res = Instantiate(responseLine, msgList.transform);
            res.transform.SetAsLastSibling();

            if (res.TryGetComponent<ReponseLine>(out var rsp))
                rsp.SetText(rispostaTerminale[i]);
            else
            {
                // fallback: try to assign automatically as before
                var responses = res.GetComponentsInChildren<TMP_Text>();
                if (responses.Length > 1)
                    responses[1].text = rispostaTerminale[i];
                else if (responses.Length == 1)
                    responses[0].text = rispostaTerminale[i];
                else
                    Debug.LogWarning("ReponseLine prefab has no TMP_Text component and no DirectoryLine script!");
            }
        }
    }
}