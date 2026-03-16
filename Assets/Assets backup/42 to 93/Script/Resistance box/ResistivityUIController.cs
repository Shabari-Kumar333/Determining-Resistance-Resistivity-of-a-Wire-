using UnityEngine;
using UnityEngine.UI;

public class ResistivityUIController : MonoBehaviour
{
    //[Header("UI Settings")]
    //public Button nextSlideButton;

    [Header("The Plugs")]
    public ResistancePlug[] allPlugs;

    void OnEnable()
    {
        Debug.Log("Resistance Slide Opened: Locking Button.");
        CheckProgress();
    }

    void OnDisable()
    {
        //if (nextSlideButton != null) nextSlideButton.interactable = true;
    }

    public void CheckProgress()
    {
        //if (nextSlideButton == null)
        {
            Debug.LogError("CRITICAL ERROR: 'Next Slide Button' is not assigned in ResistanceManager!");
            return;
        }

        bool allRemoved = true;

        foreach (ResistancePlug plug in allPlugs)
        {
            if (plug == null) continue; // Skip if a slot is empty

            // Log exactly which plug is causing the problem
            if (plug.isPluggedIn)
            {
                Debug.Log($"Button Locked: Plug '{plug.name}' is still in.");
                allRemoved = false;
                // We don't break here so we can see ALL plugs that are stuck
            }
        }

        //nextSlideButton.interactable = allRemoved;

        if (allRemoved)
        {
            Debug.Log("SUCCESS: All plugs removed. Button Unlocked!");
        }
    }
}