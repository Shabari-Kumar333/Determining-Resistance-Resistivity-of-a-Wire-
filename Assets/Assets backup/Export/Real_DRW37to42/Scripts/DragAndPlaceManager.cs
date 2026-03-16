//using UnityEngine;
//using System.Collections.Generic;

//public class DragAndPlaceManager : MonoBehaviour
//{
//    public Camera mainCamera;

//    [Header("1. Draggable 3D Models")]
//    public List<GameObject> draggableModels;

//    [Header("2. Slot Colliders (Destination)")]
//    public List<Collider> slotColliders;

//    [Header("3. Highlight Material")]
//    public Material glowMaterial;

//    [Header("4. Snap Settings")]
//    public Vector3 snapOffset = new Vector3(0, 0.05f, 0);

//    [Header("5. Gray Placeholders (To Hide)")]
//    // NEW: Drag your Gray Pads / Symbol Boxes here!
//    public List<GameObject> grayPlaceholders;

//    private void Start()
//    {
//        if (mainCamera == null) mainCamera = Camera.main;

//        // Hide all slot highlights initially
//        foreach (var c in slotColliders)
//            if (c != null) c.gameObject.SetActive(false);
//    }

//    public void StartDragging(GameObject obj)
//    {
//        int index = draggableModels.IndexOf(obj);

//        if (index != -1 && index < slotColliders.Count)
//        {
//            Collider targetSlot = slotColliders[index];
//            if (targetSlot != null)
//            {
//                targetSlot.gameObject.SetActive(true);

//                // Apply Glow
//                Renderer slotRenderer = targetSlot.GetComponentInChildren<Renderer>();
//                if (slotRenderer != null && glowMaterial != null)
//                {
//                    slotRenderer.material = glowMaterial;
//                }
//            }
//        }
//    }

//    public bool CheckDrop(GameObject obj)
//    {
//        int index = draggableModels.IndexOf(obj);
//        if (index == -1) return false;

//        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

//        // RaycastAll to see through the object we are holding
//        RaycastHit[] hits = Physics.RaycastAll(ray);

//        foreach (RaycastHit hit in hits)
//        {
//            if (hit.collider == slotColliders[index])
//            {
//                // ✅ SUCCESS

//                // 1. Snap Object
//                obj.transform.position = slotColliders[index].transform.position + snapOffset;
//                obj.transform.rotation = slotColliders[index].transform.rotation;

//                // 2. Lock it (Remove script)
//                Destroy(obj.GetComponent<Draggable3DObject>());

//                // 3. Hide Slot Highlight
//                slotColliders[index].gameObject.SetActive(false);

//                // 4. HIDE THE GRAY PLACEHOLDER (New Feature)
//                // We check if the list exists and has an object at this index
//                if (grayPlaceholders != null && index < grayPlaceholders.Count)
//                {
//                    if (grayPlaceholders[index] != null)
//                        grayPlaceholders[index].SetActive(false);
//                }

//                return true;
//            }
//        }

//        // ❌ FAILED
//        if (index < slotColliders.Count && slotColliders[index] != null)
//            slotColliders[index].gameObject.SetActive(false);

//        return false;
//    }
//}
using UnityEngine;
using System.Collections.Generic;

public class DragAndPlaceManager : MonoBehaviour
{
    public Camera mainCamera;

    [Header("1. Draggable 3D Models")]
    public List<GameObject> draggableModels;

    [Header("2. Slot Colliders (Destination)")]
    public List<Collider> slotColliders;

    [Header("3. Highlight Material")]
    public Material glowMaterial;

    [Header("4. Snap Settings")]
    public Vector3 snapOffset = new Vector3(0, 0.05f, 0);

    [Header("5. Gray Placeholders (To Hide)")]
    public List<GameObject> grayPlaceholders;

    // 🔐 LOCK STATE
    int placedCount = 0;
    bool completed = false;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        placedCount = 0;
        completed = false;

        // Hide all slot highlights initially
        foreach (var c in slotColliders)
            if (c != null)
                c.gameObject.SetActive(false);
    }

    public void StartDragging(GameObject obj)
    {
        int index = draggableModels.IndexOf(obj);

        if (index != -1 && index < slotColliders.Count)
        {
            Collider targetSlot = slotColliders[index];
            if (targetSlot != null)
            {
                targetSlot.gameObject.SetActive(true);

                // Apply Glow
                Renderer slotRenderer = targetSlot.GetComponentInChildren<Renderer>();
                if (slotRenderer != null && glowMaterial != null)
                {
                    slotRenderer.material = glowMaterial;
                }
            }
        }
    }

    public bool CheckDrop(GameObject obj)
    {
        if (completed) return false;

        int index = draggableModels.IndexOf(obj);
        if (index == -1) return false;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider == slotColliders[index])
            {
                // ✅ CORRECT DROP

                // 1️⃣ Snap Object
                obj.transform.position =
                    slotColliders[index].transform.position + snapOffset;
                obj.transform.rotation =
                    slotColliders[index].transform.rotation;

                // 2️⃣ Lock object (disable dragging)
                Destroy(obj.GetComponent<Draggable3DObject>());

                // 3️⃣ Hide slot highlight
                slotColliders[index].gameObject.SetActive(false);

                // 4️⃣ Hide gray placeholder
                if (grayPlaceholders != null && index < grayPlaceholders.Count)
                {
                    if (grayPlaceholders[index] != null)
                        grayPlaceholders[index].SetActive(false);
                }

                // 5️⃣ Count success
                placedCount++;

                Debug.Log($"[DRAG] Placed {placedCount}/{draggableModels.Count}");

                // 6️⃣ Check completion
                if (placedCount >= draggableModels.Count)
                {
                    CompleteSlide();
                }

                return true;
            }
        }

        // ❌ FAILED DROP
        if (index < slotColliders.Count && slotColliders[index] != null)
            slotColliders[index].gameObject.SetActive(false);

        return false;
    }

    // ===============================
    // 🔓 UNLOCK NEXT SLIDE
    // ===============================
    void CompleteSlide()
    {
        if (completed) return;

        completed = true;

        int globalSlide = GlobalSlideNavigation.Instance.currentSlide;

        Debug.Log($"✅ Drag & Drop completed → Unlocking Slide {globalSlide}");

        SlideProgressManager.Instance.MarkCompleted(globalSlide);
    }
}
