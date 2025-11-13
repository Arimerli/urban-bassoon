using UnityEngine;
using TMPro;

public class setPortOnRequest : MonoBehaviour
{
    [Tooltip("The TMP_Text where the user input should be written")]
    public TMP_Text directoryText;
    public RectTransform submitTransform;

    
    public void SetDirectoryText(string text)
    {
        if (directoryText != null)
        {
            directoryText.text = text;

            directoryText.GetComponent<RectTransform>().sizeDelta = new Vector2(directoryText.preferredWidth + 15, 30);
        }
        else
            Debug.LogWarning($"{name}: directoryText not set!");
    }
}
