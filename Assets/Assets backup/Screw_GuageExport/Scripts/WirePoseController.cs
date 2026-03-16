using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WirePoseController : MonoBehaviour
{
    public float moveDuration = 1f;

    Coroutine routine;

    // 🔑 SlideIndex → Target Pose (completed work)
    static Dictionary<int, Transform> completedSlides =
        new Dictionary<int, Transform>();

    // ================= SLIDE ENTER =================
    public void OnSlideEnter(int slideIndex)
    {
        if (completedSlides.TryGetValue(slideIndex, out Transform pose))
        {
            // 🔄 Restore pose for review
            transform.SetPositionAndRotation(pose.position, pose.rotation);
        }
    }

    // ================= ROTATE =================
    public void RotateOnSlide(int slideIndex, Transform target)
    {
        if (target == null) return;

        // ✅ Mark slide as completed
        completedSlides[slideIndex] = target;

        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(MoveRoutine(target));
    }

    IEnumerator MoveRoutine(Transform target)
    {
        Vector3 sp = transform.position;
        Quaternion sr = transform.rotation;

        Vector3 ep = target.position;
        Quaternion er = target.rotation;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / moveDuration;
            transform.position = Vector3.Lerp(sp, ep, t);
            transform.rotation = Quaternion.Slerp(sr, er, t);
            yield return null;
        }

        transform.SetPositionAndRotation(ep, er);
        routine = null;
    }
}
