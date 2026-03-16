using UnityEngine;
using System.Collections.Generic;

public class slide39_Mechanism : MonoBehaviour
{
    [Header("--- 1. Objects to Drag ---")]
    public List<GameObject> draggableModels;

    [Header("--- 2. Drop Zones (Slots) ---")]
    public List<Collider> slotColliders;

    [Header("--- 3. Gray Placeholders (To Hide) ---")]
    public List<GameObject> grayPlaceholders;

    [Header("--- Visual Settings ---")]
    public Material glowMaterial;
    public Vector3 snapOffset = new Vector3(0, 0.05f, 0);

    // Internal State
    private Camera mainCamera;
    private GameObject currentObject;
    private int currentIndex = -1;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 dragOffset;
    private Plane dragPlane;
    private bool isDragging = false;

    private void Start()
    {
        mainCamera = Camera.main;

        // Hide all slot highlights initially
        foreach (var c in slotColliders)
        {
            if (c != null) c.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        // --- MOUSE DOWN (Start Drag) ---
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (draggableModels.Contains(hit.collider.gameObject))
                {
                    StartDrag(hit.collider.gameObject);
                }
            }
        }

        // --- MOUSE DRAG (Move Object) ---
        if (isDragging && Input.GetMouseButton(0) && currentObject != null)
        {
            PerformDrag();
        }

        // --- MOUSE UP (Drop Object) ---
        if (isDragging && Input.GetMouseButtonUp(0))
        {
            EndDrag();
        }
    }

    private void StartDrag(GameObject obj)
    {
        currentObject = obj;
        currentIndex = draggableModels.IndexOf(obj);

        // Save original spot
        startPosition = currentObject.transform.position;
        startRotation = currentObject.transform.rotation;

        // Setup Drag Plane
        dragPlane = new Plane(Vector3.up, currentObject.transform.position);

        // Calculate Offset
        Ray camRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (dragPlane.Raycast(camRay, out float planeDist))
        {
            dragOffset = currentObject.transform.position - camRay.GetPoint(planeDist);
        }

        // Show Highlight
        if (currentIndex < slotColliders.Count && slotColliders[currentIndex] != null)
        {
            Collider targetSlot = slotColliders[currentIndex];
            targetSlot.gameObject.SetActive(true);

            Renderer slotRenderer = targetSlot.GetComponentInChildren<Renderer>();
            if (slotRenderer != null && glowMaterial != null)
            {
                slotRenderer.material = glowMaterial;
            }
        }

        isDragging = true;
    }

    private void PerformDrag()
    {
        Ray camRay = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (dragPlane.Raycast(camRay, out float planeDist))
        {
            // Drag flat on the plane (no lift)
            Vector3 targetPos = camRay.GetPoint(planeDist) + dragOffset;
            currentObject.transform.position = targetPos;
        }
    }

    private void EndDrag()
    {
        isDragging = false;
        bool droppedSuccessfully = false;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);

        foreach (RaycastHit hit in hits)
        {
            if (currentIndex < slotColliders.Count && hit.collider == slotColliders[currentIndex])
            {
                SuccessDrop();
                droppedSuccessfully = true;
                break;
            }
        }

        if (!droppedSuccessfully)
        {
            // Instant snap back (no animation)
            currentObject.transform.position = startPosition;
            currentObject.transform.rotation = startRotation;

            // Hide the slot highlight
            if (currentIndex < slotColliders.Count && slotColliders[currentIndex] != null)
            {
                slotColliders[currentIndex].gameObject.SetActive(false);
            }
        }

        if (droppedSuccessfully)
        {
            currentObject = null;
            currentIndex = -1;
        }
    }

    private void SuccessDrop()
    {
        // Snap to destination
        currentObject.transform.position = slotColliders[currentIndex].transform.position + snapOffset;
        currentObject.transform.rotation = slotColliders[currentIndex].transform.rotation;

        // Hide Highlight
        slotColliders[currentIndex].gameObject.SetActive(false);

        // Hide Gray Placeholder
        if (grayPlaceholders != null && currentIndex < grayPlaceholders.Count)
        {
            if (grayPlaceholders[currentIndex] != null)
                grayPlaceholders[currentIndex].SetActive(false);
        }

        // Disable collider so it can't be moved again
        Collider col = currentObject.GetComponent<Collider>();
        if (col) col.enabled = false;
    }
}