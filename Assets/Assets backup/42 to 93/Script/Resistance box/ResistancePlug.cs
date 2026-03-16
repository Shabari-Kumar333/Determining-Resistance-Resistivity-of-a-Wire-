using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class ResistancePlug : MonoBehaviour
{
    [Header("Resistance")]
    public float ohmsValue;   // ✔ correct name

    [Header("Managers")]
    public ResistanceSystem boxManager;   // combined system

    [Header("Movement Targets")]
    public Transform tablePoint;

    [Header("Animation")]
    public float duration = 0.5f;
    public AnimationCurve motionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [HideInInspector]
    public bool isPluggedIn;

    // Internal state
    bool isInserted;
    bool isAnimating;

    Vector3 startLocalPos;
    Quaternion startLocalRot;

    Collider col;
    Rigidbody rb;

    // ================= INIT =================
    void Awake()
    {
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = true;
        rb.useGravity = false;

        startLocalPos = transform.localPosition;
        startLocalRot = transform.localRotation;
    }

    void Start()
    {
        isInserted = IsInsidePlugHole();
        isPluggedIn = isInserted;

        // 🔒 IMPORTANT:
        // Only notify the box ONCE at start if plug is removed
        if (!isInserted && boxManager != null)
        {
            boxManager.OnPlugRemoved(ohmsValue);
        }
    }

    // ================= USER ACTION =================
    public void TogglePlug()
    {
        if (isAnimating) return;

        StopAllCoroutines();

        col.enabled = false;
        rb.isKinematic = true;

        if (isInserted)
        {
            // Remove plug → table
            StartCoroutine(AnimateTo(tablePoint.position, tablePoint.rotation));
            SetInserted(false);
        }
        else
        {
            // Insert plug → hole
            Vector3 homeWorldPos = transform.parent.TransformPoint(startLocalPos);
            Quaternion homeWorldRot = transform.parent.rotation * startLocalRot;
            StartCoroutine(AnimateTo(homeWorldPos, homeWorldRot));
            SetInserted(true);
        }
    }

    // ================= ANIMATION =================
    IEnumerator AnimateTo(Vector3 endPos, Quaternion endRot)
    {
        isAnimating = true;

        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        float t = 0f;

        while (t < duration)
        {
            float k = motionCurve.Evaluate(t / duration);
            transform.position = Vector3.Lerp(startPos, endPos, k);
            transform.rotation = Quaternion.Lerp(startRot, endRot, k);
            t += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        transform.rotation = endRot;

        col.enabled = true;
        isAnimating = false;
    }

    // ================= INSERT / REMOVE =================
    void SetInserted(bool inserted)
    {
        if (isInserted == inserted) return;

        isInserted = inserted;
        isPluggedIn = inserted;

        if (boxManager == null) return;

        if (inserted)
            boxManager.OnPlugInserted(ohmsValue);
        else
            boxManager.OnPlugRemoved(ohmsValue);
    }

    // ================= DETECTION =================
    bool IsInsidePlugHole()
    {
        Collider[] hits = Physics.OverlapBox(
            col.bounds.center,
            col.bounds.extents * 0.9f,
            Quaternion.identity
        );

        foreach (var hit in hits)
        {
            if (hit.CompareTag("PlugHole"))
                return true;
        }
        return false;
    }
}
