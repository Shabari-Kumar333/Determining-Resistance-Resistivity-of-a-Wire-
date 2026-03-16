////using UnityEngine;
////using System.Collections;

////public class WireSimpleMover : MonoBehaviour
////{
////    public Transform[] targets;
////    public float moveDuration = 1.2f;

////    // 🔑 GLOBAL shared index
////    public static int CurrentIndex = -1;

////    Coroutine routine;

////    public void MoveNext(int requiredStartIndex)
////    {
////        // 🔒 FIRST: ensure we are at the correct start index for this slide
////        if (CurrentIndex < requiredStartIndex)
////        {
////            CurrentIndex = requiredStartIndex;
////        }
////        else
////        {
////            CurrentIndex++;
////        }

////        CurrentIndex = Mathf.Clamp(CurrentIndex, 0, targets.Length - 1);

////        if (routine != null)
////            StopCoroutine(routine);

////        routine = StartCoroutine(MoveRoutine(targets[CurrentIndex]));
////    }

////    IEnumerator MoveRoutine(Transform t)
////    {
////        Vector3 sp = transform.position;
////        Quaternion sr = transform.rotation;

////        Vector3 ep = t.position;
////        Quaternion er = t.rotation;

////        float k = 0f;
////        while (k < 1f)
////        {
////            k += Time.deltaTime / moveDuration;
////            transform.position = Vector3.Lerp(sp, ep, k);
////            transform.rotation = Quaternion.Slerp(sr, er, k);
////            yield return null;
////        }

////        transform.SetPositionAndRotation(ep, er);
////    }
////}
//using UnityEngine;
//using System.Collections;

//public class WireSimpleMover : MonoBehaviour
//{
//    public Transform[] targets;

//    [Header("Bypass (Used only for pos 1 → Element 0)")]
//    public Transform bypassTransform;

//    public float moveDuration = 1.2f;

//    public static int CurrentIndex = -1;

//    Coroutine routine;
//    bool isMoving = false;

//    public void MoveNext(int requiredStartIndex)
//    {
//        if (isMoving)
//            return;

//        if (CurrentIndex < requiredStartIndex)
//            CurrentIndex = requiredStartIndex;
//        else
//            CurrentIndex++;

//        CurrentIndex = Mathf.Clamp(CurrentIndex, 0, targets.Length - 1);

//        Debug.Log($"[WIRE] MoveNext → Index {CurrentIndex} ({targets[CurrentIndex].name})");

//        if (routine != null)
//            StopCoroutine(routine);

//        // ✅ FIX: BYPASS FOR ELEMENT 0 (pos 1)
//        if (CurrentIndex == 0 && bypassTransform != null)
//        {
//            routine = StartCoroutine(BypassSequence());
//        }
//        else
//        {
//            routine = StartCoroutine(MoveRoutine(targets[CurrentIndex], "DIRECT"));
//        }
//    }

//    IEnumerator BypassSequence()
//    {
//        isMoving = true;

//        Debug.Log("[WIRE] BYPASS SEQUENCE START");

//        yield return StartCoroutine(MoveRoutine(bypassTransform, "BYPASS"));
//        yield return StartCoroutine(MoveRoutine(targets[0], "FINAL"));

//        Debug.Log("[WIRE] BYPASS SEQUENCE COMPLETE");

//        isMoving = false;
//        routine = null;
//    }

//    IEnumerator MoveRoutine(Transform target, string phase)
//    {
//        Vector3 startPos = transform.position;
//        Quaternion startRot = transform.rotation;

//        Vector3 endPos = target.position;
//        Quaternion endRot = target.rotation;

//        float t = 0f;
//        while (t < 1f)
//        {
//            t += Time.deltaTime / moveDuration;
//            transform.position = Vector3.Lerp(startPos, endPos, t);
//            transform.rotation = Quaternion.Slerp(startRot, endRot, t);
//            yield return null;
//        }

//        transform.SetPositionAndRotation(endPos, endRot);

//        Debug.Log($"[WIRE] REACHED → {phase} ({target.name})");
//    }

//    void OnDrawGizmos()
//    {
//        if (bypassTransform != null)
//        {
//            Gizmos.color = Color.yellow;
//            Gizmos.DrawSphere(bypassTransform.position, 0.003f);
//            Gizmos.DrawLine(transform.position, bypassTransform.position);
//        }
//    }
//}
//using UnityEngine;
//using System.Collections;

//public class WireSimpleMover : MonoBehaviour
//{
//    public Transform[] targets;

//    [Header("Bypass (used before first entry to pos 1)")]
//    public Transform bypassTransform;

//    public float moveDuration = 1.2f;

//    public static int CurrentIndex = -1;

//    bool isMoving = false;
//    bool hasEnteredPos1 = false;   // ⭐ KEY FIX

//    Coroutine routine;

//    public void MoveNext(int requiredStartIndex)
//    {
//        if (isMoving)
//            return;

//        // Index logic (unchanged)
//        if (CurrentIndex < requiredStartIndex)
//            CurrentIndex = requiredStartIndex;
//        else
//            CurrentIndex++;

//        CurrentIndex = Mathf.Clamp(CurrentIndex, 0, targets.Length - 1);

//        Debug.Log($"[WIRE] MoveNext → Index {CurrentIndex}");

//        if (routine != null)
//            StopCoroutine(routine);

//        // ⭐ FIRST TIME ONLY → DEFAULT → BYPASS → POS 1
//        if (CurrentIndex == 0 && !hasEnteredPos1 && bypassTransform != null)
//        {
//            routine = StartCoroutine(FirstEntrySequence());
//        }
//        else
//        {
//            routine = StartCoroutine(MoveRoutine(targets[CurrentIndex]));
//        }
//    }

//    IEnumerator FirstEntrySequence()
//    {
//        isMoving = true;

//        Debug.Log("[WIRE] FIRST ENTRY TO POS 1 → USING BYPASS");

//        // Phase 1: default → bypass
//        yield return StartCoroutine(MoveRoutine(bypassTransform));

//        // Phase 2: bypass → pos 1
//        yield return StartCoroutine(MoveRoutine(targets[0]));

//        hasEnteredPos1 = true;   // ⭐ mark done

//        Debug.Log("[WIRE] POS 1 ENTRY COMPLETE");

//        isMoving = false;
//        routine = null;
//    }

//    IEnumerator MoveRoutine(Transform target)
//    {
//        Vector3 startPos = transform.position;
//        Quaternion startRot = transform.rotation;

//        Vector3 endPos = target.position;
//        Quaternion endRot = target.rotation;

//        float t = 0f;
//        while (t < 1f)
//        {
//            t += Time.deltaTime / moveDuration;
//            transform.position = Vector3.Lerp(startPos, endPos, t);
//            transform.rotation = Quaternion.Slerp(startRot, endRot, t);
//            yield return null;
//        }

//        transform.SetPositionAndRotation(endPos, endRot);

//        Debug.Log($"[WIRE] Reached → {target.name}");
//    }

//    // 🧭 Scene debug
//    void OnDrawGizmos()
//    {
//        if (bypassTransform != null)
//        {
//            Gizmos.color = Color.yellow;
//            Gizmos.DrawSphere(bypassTransform.position, 0.003f);
//        }
//    }
//}
using UnityEngine;
using System.Collections;

public class WireSimpleMover : MonoBehaviour
{
    public Transform[] targets;

    [Header("Bypass (The 'Safety' Waypoint)")]
    public Transform bypassTransform;

    public float moveDuration = 1.0f;

    // We use a private variable to track if we are still on the table
    private static bool isFirstMoveFromTable = true;
    public static int CurrentIndex = -1;

    private bool isMoving = false;

    void Start()
    {
        // Reset logic so testing in Editor works every time
        isFirstMoveFromTable = true;
        CurrentIndex = -1;
        Debug.Log("<color=white>[WIRE] Script Started: Wire is currently at Default Position (Table).</color>");
    }

    public void MoveNext(int requiredStartIndex)
    {
        if (isMoving) return;

        // Determine which target index we are heading to
        if (CurrentIndex < requiredStartIndex)
            CurrentIndex = requiredStartIndex;
        else
            CurrentIndex++;

        CurrentIndex = Mathf.Clamp(CurrentIndex, 0, targets.Length - 1);

        // --- THE CORE FIX ---
        if (isFirstMoveFromTable && CurrentIndex == 0)
        {
            // The wire is going from the table to the first gauge position
            if (bypassTransform != null)
            {
                Debug.Log("<color=cyan>[WIRE] DETECTED: Table -> Bypass -> Pos 1. Avoiding Body Clipping.</color>");
                StartCoroutine(TableToGaugeSequence());
            }
            else
            {
                Debug.LogError("[WIRE] No Bypass assigned! Wire will clip through the gauge.");
                StartCoroutine(SingleMoveRoutine(targets[0]));
            }
        }
        else
        {
            // Normal movement between targets already inside/near the gauge
            Debug.Log($"[WIRE] Normal Move to Target {CurrentIndex}: {targets[CurrentIndex].name}");
            StartCoroutine(SingleMoveRoutine(targets[CurrentIndex]));
        }
    }

    IEnumerator TableToGaugeSequence()
    {
        isMoving = true;

        // PHASE 1: Table to Bypass
        Debug.Log("<color=yellow>[WIRE] STEP 1: Moving from Table to Bypass.</color>");
        yield return StartCoroutine(LerpToTarget(bypassTransform));

        // PHASE 2: Bypass to Pos 1
        Debug.Log("<color=green>[WIRE] STEP 2: Moving from Bypass into Gauge (Pos 1).</color>");
        yield return StartCoroutine(LerpToTarget(targets[0]));

        isFirstMoveFromTable = false; // Never use the bypass again until game restarts
        isMoving = false;
    }

    IEnumerator SingleMoveRoutine(Transform target)
    {
        isMoving = true;
        yield return StartCoroutine(LerpToTarget(target));
        isMoving = false;
    }

    IEnumerator LerpToTarget(Transform target)
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        Vector3 endPos = target.position;
        Quaternion endRot = target.rotation;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / moveDuration;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }
        transform.SetPositionAndRotation(endPos, endRot);
    }
}