////using UnityEngine;
////using System.Collections;

////public class GlobalCameraController : MonoBehaviour
////{
////    public static GlobalCameraController Instance;

////    public Camera mainCam;
////    public float moveSpeed = 2f;

////    Coroutine moveRoutine;

////    void Awake()
////    {
////        if (Instance != null)
////        {
////            Destroy(gameObject);
////            return;
////        }
////        Instance = this;
////    }

////    public void MoveTo(Transform target)
////    {
////        if (!target || !mainCam) return;

////        if (moveRoutine != null)
////            StopCoroutine(moveRoutine);

////        moveRoutine = StartCoroutine(SmoothMove(target));
////    }

////    IEnumerator SmoothMove(Transform target)
////    {
////        Vector3 startPos = mainCam.transform.position;
////        Quaternion startRot = mainCam.transform.rotation;

////        float t = 0f;
////        while (t < 1f)
////        {
////            t += Time.deltaTime * moveSpeed;
////            mainCam.transform.position = Vector3.Lerp(startPos, target.position, t);
////            mainCam.transform.rotation = Quaternion.Slerp(startRot, target.rotation, t);
////            yield return null;
////        }
////    }
////}
//using UnityEngine;
//using System.Collections;

//public class GlobalCameraController : MonoBehaviour
//{
//    public static GlobalCameraController Instance;

//    public Camera mainCam;
//    public float moveSpeed = 2f;

//    Coroutine moveRoutine;

//    void Awake()
//    {
//        if (Instance != null)
//        {
//            Destroy(gameObject);
//            return;
//        }
//        Instance = this;
//    }

//    public void MoveTo(Transform target)
//    {
//        if (!target || !mainCam) return;

//        if (moveRoutine != null)
//            StopCoroutine(moveRoutine);

//        moveRoutine = StartCoroutine(SmoothMove(target));
//    }

//    IEnumerator SmoothMove(Transform target)
//    {
//        Vector3 startPos = mainCam.transform.position;
//        Quaternion startRot = mainCam.transform.rotation;

//        float t = 0f;
//        while (t < 1f)
//        {
//            t += Time.deltaTime * moveSpeed;
//            mainCam.transform.position = Vector3.Lerp(startPos, target.position, t);
//            mainCam.transform.rotation = Quaternion.Slerp(startRot, target.rotation, t);
//            yield return null;
//        }
//    }
//}
using UnityEngine;
using System.Collections;

public class GlobalCameraController : MonoBehaviour
{
    public static GlobalCameraController Instance;

    public Camera mainCam;
    public float moveSpeed = 2f;

    Coroutine moveRoutine;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void MoveTo(Transform target)
    {
        if (!target || !mainCam) return;

        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        moveRoutine = StartCoroutine(SmoothMove(target));
    }

    IEnumerator SmoothMove(Transform target)
    {
        Vector3 startPos = mainCam.transform.position;
        Quaternion startRot = mainCam.transform.rotation;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            mainCam.transform.position = Vector3.Lerp(startPos, target.position, t);
            mainCam.transform.rotation = Quaternion.Slerp(startRot, target.rotation, t);
            yield return null;
        }
    }
}
