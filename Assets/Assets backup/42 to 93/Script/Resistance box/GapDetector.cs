using UnityEngine;
using UnityEngine.Events;

public class GapDetector : MonoBehaviour
{
    public bool isRightGap;   // Tick this only on the RIGHT gap

    public static bool boxOnRight;
    public static bool boxOnLeft;
    public UnityEvent OnInterChanged;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("ResistanceBox")) return;

        if (isRightGap)
        {
            boxOnRight = true;
            boxOnLeft = false;
            
            Debug.Log("Is rightgap is worked");
        }
        else
        {
            boxOnLeft = true;
            boxOnRight = false;
            Debug.Log("Is leftgap is worked");
            OnInterChanged.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("ResistanceBox")) return;

        if (isRightGap)
            boxOnRight = false;
        else
            boxOnLeft = false;
    }
}
