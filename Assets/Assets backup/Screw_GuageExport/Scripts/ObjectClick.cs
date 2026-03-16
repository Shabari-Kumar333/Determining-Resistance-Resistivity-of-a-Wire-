using UnityEngine;
using UnityEngine.InputSystem; // << new input system

public class ObjectClick : MonoBehaviour
{
    public string objectName;

    void Update()
    {
        // --- Mouse click (editor / PC) ---
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            DetectClick(Mouse.current.position.ReadValue());
        }

        // --- Touch input (Android / iOS) ---
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            DetectClick(Touchscreen.current.primaryTouch.position.ReadValue());
        }
    }

    void DetectClick(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null && hit.collider.gameObject == this.gameObject)
            {
                QuizManager.Instance.CheckAnswer(objectName);
            }
        }
    }
}
