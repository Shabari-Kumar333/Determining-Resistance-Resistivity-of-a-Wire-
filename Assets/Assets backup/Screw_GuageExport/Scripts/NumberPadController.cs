using UnityEngine;
using TMPro;

public class NumberPadController : MonoBehaviour
{
    TMP_InputField activeInput;

    public void SetActiveInput(TMP_InputField field)
    {
        activeInput = field;
    }

    public void AddDigit(string digit)
    {
        if (activeInput == null || !activeInput.interactable) return;
        activeInput.text += digit;
        activeInput.ForceLabelUpdate();
    }

    public void AddDecimal()
    {
        if (activeInput == null || !activeInput.interactable) return;
        if (!activeInput.text.Contains("."))
            activeInput.text += ".";
    }

    public void Backspace()
    {
        if (activeInput == null || !activeInput.interactable) return;
        if (activeInput.text.Length > 0)
            activeInput.text =
                activeInput.text.Substring(0, activeInput.text.Length - 1);
    }
}
