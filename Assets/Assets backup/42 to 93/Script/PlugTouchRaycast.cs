using UnityEngine;
using UnityEngine.InputSystem;

public class PlugTouchRaycast : MonoBehaviour
{
    private Camera cam;
    private bool pressed;

    void Awake()
    {
        cam = Camera.main;
        if (cam == null)
            Debug.LogError("Main Camera not found! Make sure it is tagged as 'MainCamera'.");
    }

    void Update()
    {
        if (Pointer.current == null) return;

        // Detect new press
        if (!pressed && Pointer.current.press.isPressed)
        {
            pressed = true;

            Vector2 screenPos = Pointer.current.position.ReadValue();
            TryRaycast(screenPos);
        }

        // Reset when released
        if (pressed && !Pointer.current.press.isPressed)
        {
            pressed = false;
        }
    }

    void TryRaycast(Vector2 screenPos)
    {
        Ray ray = cam.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Try to get the PlugAnimation component
            PlugAnimation plugAnim = hit.collider.GetComponent<PlugAnimation>();
            if (plugAnim != null)
            {
                plugAnim.PlayPlugAnimation();
            }
        }
    }
}
