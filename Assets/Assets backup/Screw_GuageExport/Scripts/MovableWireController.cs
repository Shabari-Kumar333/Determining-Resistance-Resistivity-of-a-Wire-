using UnityEngine;
using System.Collections;

public class MovableWireController : MonoBehaviour
{
    // 🔑 GLOBAL LOCK
    public static bool IsWireMoving = false;

    public float moveDuration = 1.2f;

    Coroutine routine;

    public void MoveTo(Transform target)
    {
        if (target == null) return;

        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(MoveRoutine(target));
    }

    IEnumerator MoveRoutine(Transform target)
    {
        IsWireMoving = true;

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

        IsWireMoving = false;
        routine = null;
    }
}
