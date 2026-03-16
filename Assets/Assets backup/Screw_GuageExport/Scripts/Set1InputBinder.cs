using UnityEngine;
using TMPro;

public class Set1InputBinder : MonoBehaviour
{
    public TMP_InputField inputField;
    public int setIndex = 0;
    public int stepIndex = 0;

    public void OnInputSelected()
    {
        AutoFillController_Set1.Instance.RegisterInput(
            inputField,
            setIndex,
            stepIndex   
        );
    }
}
