using UnityEngine;
using UnityEngine.UI;

public class CheckButtonRouter : MonoBehaviour
{
    public Button checkButton;

    [Header("Handlers")]
    public ValidatorController normalValidator;
    public FinalMeanValidator finalValidator;

    void Awake()
    {
        if (checkButton == null)
            checkButton = GetComponent<Button>();
    }

    // ===============================
    // CALL THIS FOR SLIDES 1–24
    // ===============================
    public void UseNormalValidation()
    {
        checkButton.onClick.RemoveAllListeners();
        checkButton.onClick.AddListener(normalValidator.OnCheckPressed);

        Debug.Log("[CheckButton] Normal validation enabled");
    }

    // ===============================
    // CALL THIS FOR SLIDE 25
    // ===============================
    public void UseFinalValidation()
    {
        checkButton.onClick.RemoveAllListeners();
        checkButton.onClick.AddListener(finalValidator.OnCheckPressed);

        Debug.Log("[CheckButton] Final mean validation enabled");
    }
}
