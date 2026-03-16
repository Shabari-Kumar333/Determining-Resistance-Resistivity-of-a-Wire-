using UnityEngine;
using UnityEngine.UI;

public class ResistanceManager : MonoBehaviour
{
    [Header("UI Settings")]
    public Button nextSlideButton;

    [Header("The Plugs")]
    public ResistancePlugs[] allPlugs;

    [Header("Goal Settings")]
    public float minResistance = 1.5f;
    public float maxResistance = 6.0f;

    void OnEnable()
    {
        CheckProgress();
    }

    void OnDisable()
    {
        if (nextSlideButton != null) nextSlideButton.interactable = true;
    }

    public void CheckProgress()
    {
        if (nextSlideButton == null)
        {
            Debug.LogError("CRITICAL ERROR: 'Next Slide Button' is not assigned!");
            return;
        }

        float currentTotalResistance = 0f;

        foreach (ResistancePlugs plug in allPlugs)
        {
            if (plug == null) continue;

            // In a Resistance Box: 
            // Plug IN = 0 resistance (Short circuit)
            // Plug REMOVED = Adds resistance
            if (!plug.isPluggedIn)
            {
                currentTotalResistance += plug.ohmValue;
            }
        }

        Debug.Log($"Current Resistance: {currentTotalResistance} Ohms");

        // Check if we are within the target range (1.5 to 6)
        if (currentTotalResistance >= minResistance && currentTotalResistance <= maxResistance)
        {
            Debug.Log("Success! Resistance is valid.");
            nextSlideButton.interactable = true;
        }
        else
        {
            Debug.Log("Locked. Resistance must be between 1.5 and 6.");
            nextSlideButton.interactable = false;
        }
    }
}