using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Draggable3DObject : MonoBehaviour
{
    public DragAndPlaceManager manager;

    [Header("Settings")]
    public float returnSpeed = 5f;
    public float liftHeight = 0.2f; // Slight lift when dragging so it doesn't clip

    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 offset;
    private bool isDragging = false;
    private Plane dragPlane; 

    void Start()
    {
        if (manager == null) manager = FindFirstObjectByType<DragAndPlaceManager>();
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void OnMouseDown()
    {
        isDragging = true;
        
        // 1. Create an invisible plane at the object's height
        // This ensures we drag along a flat surface, not into the floor
        dragPlane = new Plane(Vector3.up, transform.position);

        // 2. Calculate offset
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float planeDist;
        if (dragPlane.Raycast(camRay, out planeDist))
        {
            offset = transform.position - camRay.GetPoint(planeDist);
        }

        if (manager != null) manager.StartDragging(this.gameObject);
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;

        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float planeDist;

        // 3. Move object along the invisible plane
        if (dragPlane.Raycast(camRay, out planeDist))
        {
            Vector3 targetPos = camRay.GetPoint(planeDist) + offset;
            
            // Apply slight lift to avoid Z-fighting (flickering on table)
            targetPos.y = startPosition.y + liftHeight; 
            
            transform.position = targetPos;
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
        bool success = false;

        if (manager != null) success = manager.CheckDrop(this.gameObject);

        if (!success) StartCoroutine(LerpBack());
    }

    private IEnumerator LerpBack()
    {
        float elapsed = 0f;
        Vector3 currentPos = transform.position;
        Quaternion currentRot = transform.rotation;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * returnSpeed;
            transform.position = Vector3.Lerp(currentPos, startPosition, elapsed);
            transform.rotation = Quaternion.Lerp(currentRot, startRotation, elapsed);
            yield return null;
        }
        transform.position = startPosition;
        transform.rotation = startRotation;
    }
}