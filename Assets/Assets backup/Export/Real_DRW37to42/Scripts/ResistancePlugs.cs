using UnityEngine;
using System.Collections;

public class ResistancePlugs : MonoBehaviour
{
    [Header("Resistance Value (IMPORTANT: Set this in Inspector)")]
    public float ohmValue = 1.0f; // Set this to 0.1, 2, 5, etc. for each plug

    [Header("Manager (Auto-Finds if empty)")]
    public ResistanceManager manager;

    [Header("Target Settings")]
    public Transform tablePoint;

    [Header("Animation Settings")]
    public float duration = 0.5f;
    public AnimationCurve motionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    public bool isPluggedIn = true;

    // Internal memory
    private Vector3 startLocalPos;
    private Quaternion startLocalRot;
    private Collider col;
    private Rigidbody rb;

    void Start()
    {
        if (manager == null)
        {
            manager = FindObjectOfType<ResistanceManager>();
        }

        startLocalPos = transform.localPosition;
        startLocalRot = transform.localRotation;
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    public void TogglePlugState()
    {
        isPluggedIn = !isPluggedIn;
        StopAllCoroutines();

        // Physics Cleanup
        if (col) col.enabled = false;
        if (rb) rb.isKinematic = true;

        if (!isPluggedIn)
        {
            // Removed: Move to Table
            StartCoroutine(AnimateToWorld(tablePoint.position, tablePoint.rotation));
        }
        else
        {
            // Inserted: Go Home
            if (transform.parent != null)
            {
                Vector3 homeWorldPos = transform.parent.TransformPoint(startLocalPos);
                Quaternion homeWorldRot = transform.parent.rotation * startLocalRot;
                StartCoroutine(AnimateToWorld(homeWorldPos, homeWorldRot));
            }
        }

        // Notify the Manager to recalculate total resistance
        if (manager != null)
        {
            manager.CheckProgress();
        }
        else
        {
            Debug.LogError("Manager missing! Make sure ResistanceManager is in the scene.");
        }
    }

    IEnumerator AnimateToWorld(Vector3 endPos, Quaternion endRot)
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            float curveValue = motionCurve.Evaluate(t);
            transform.position = Vector3.Lerp(startPos, endPos, curveValue);
            transform.rotation = Quaternion.Lerp(startRot, endRot, curveValue);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        transform.rotation = endRot;

        if (isPluggedIn && col) col.enabled = true;
    }
}