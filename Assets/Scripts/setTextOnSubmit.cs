using UnityEngine;
using TMPro;

public class DirectoryLine : MonoBehaviour
{
    [Tooltip("The TMP_Text where the user input should be written")]
    public TMP_Text contentText;

    // convenience helper
    public void SetText(string text)
    {
        if (contentText != null)
            contentText.text = text;
        else
            Debug.LogWarning($"{name}: contentText not set!");
    }
}
