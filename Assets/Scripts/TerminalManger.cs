using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Rendering.RenderGraphModule;

public class TerminalManager : MonoBehaviour
{
    [Header("Terminal UI References")]
    public TMP_InputField terminalInput;
    public GameObject userInputLine;
    public GameObject directoryLine;
    public GameObject responseLine;
    public ScrollRect sr;
    public GameObject msgList;

    Interpreter interpreter;

    private void Start()
    {
        interpreter = GetComponent<Interpreter>();
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
        AddInterpreterLines(interpreter.InterpretCommand(userInput));

        // Move user input line to bottom
        if (userInputLine != null)
            userInputLine.transform.SetAsLastSibling();

        // Scroll to bottom
        Canvas.ForceUpdateCanvases();
        if (sr != null)
            sr.verticalNormalizedPosition = 0f;
    }

    private void AddDirectoryLine(string userInput)
    {
        GameObject msg = Instantiate(directoryLine, msgList.transform);
        msg.transform.SetSiblingIndex(msgList.transform.childCount - 1);

        DirectoryLine dir = msg.GetComponent<DirectoryLine>();
        if (dir != null)
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

            ReponseLine rsp = res.GetComponent<ReponseLine>();
            if (rsp != null)
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