using UnityEngine;

public class ToggleObjectButton : MonoBehaviour
{
    [Header("Target To Toggle")]
    public GameObject targetObject;

    void Start()
    {
        if (targetObject != null)
            targetObject.SetActive(false); // start hidden
    }

    // 🔘 Button OnClick
    public void ToggleObject()
    {
        if (targetObject == null) return;

        bool isActive = targetObject.activeSelf;
        targetObject.SetActive(!isActive);
    }
}
