using UnityEngine;
using System.Collections;

public class ScrewGaugePositionController : MonoBehaviour
{
    [Header("Visual Only (Do NOT include spindle / colliders)")]
    public Transform visualRoot;   // 🔥 Assign mesh-only parent

    private Vector3 originalPos;
    private Quaternion originalRot;
    private Vector3 originalVisualScale;

    private Coroutine moveRoutine;

    [Header("Animation Settings")]
    public float moveDuration = 1f;
    public float targetScale = 1.7f;

    private void Awake()
    {
        originalPos = transform.position;
        originalRot = transform.rotation;

        if (visualRoot != null)
            originalVisualScale = visualRoot.localScale;
    }

    // ================== MOVE TO SLIDE ==================
    public void MoveToTarget(Transform target, bool scaleUp = true)
    {
        if (target == null) return;

        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        Vector3 finalScale = scaleUp && visualRoot != null
            ? originalVisualScale * targetScale
            : originalVisualScale;

        moveRoutine = StartCoroutine(
            SmoothMove(target.position, target.rotation, finalScale)
        );
    }

    // ================== RESTORE (FIXES YOUR ERROR) ==================
    public void RestoreOriginal()
    {
        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        moveRoutine = StartCoroutine(
            SmoothMove(originalPos, originalRot, originalVisualScale)
        );
    }

    // ================== CORE ANIMATION ==================
    private IEnumerator SmoothMove(Vector3 targetPos, Quaternion targetRot, Vector3 visualScale)
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        Vector3 startScale = visualRoot != null
            ? visualRoot.localScale
            : Vector3.one;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / moveDuration;
            float s = Mathf.SmoothStep(0f, 1f, t);

            transform.position = Vector3.Lerp(startPos, targetPos, s);
            transform.rotation = Quaternion.Slerp(startRot, targetRot, s);

            if (visualRoot != null)
                visualRoot.localScale = Vector3.Lerp(startScale, visualScale, s);

            yield return null;
        }

        transform.position = targetPos;
        transform.rotation = targetRot;

        if (visualRoot != null)
            visualRoot.localScale = visualScale;
    }
}
