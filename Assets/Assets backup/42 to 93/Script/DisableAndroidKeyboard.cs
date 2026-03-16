using TMPro;
using UnityEngine;

public class DisableAndroidKeyboard : MonoBehaviour
{
    void Awake()
    {
        TMP_InputField input = GetComponent<TMP_InputField>();

        if (input != null)
        {
            input.shouldHideMobileInput = true; // 🔒 KEY LINE
            input.readOnly = true;              // Optional (safer)
        }
    }
}
