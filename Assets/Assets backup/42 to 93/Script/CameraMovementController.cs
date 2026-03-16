//using UnityEngine;
//using System;

//public class CameraMovementController : MonoBehaviour
//{
//    [Header("Camera")]
//    public Transform cameraTransform;
//    public Transform[] cameraTargets;

//    [Header("Smooth Movement Settings")]
//    public float positionSmoothTime = 0.45f;
//    public float rotationSmoothTime = 0.35f;

//    [Header("State")]
//    [SerializeField] private bool isMoving = false;

//    public static event Action OnCameraBusy;
//    public static event Action OnCameraFree;

//    public bool IsBusy => isMoving;

//    private int currentIndex = 0;

//    private Vector3 positionVelocity;
//    private Vector3 rotationVelocity;

//    void Start()
//    {
//        cameraTransform.SetPositionAndRotation(
//            cameraTargets[currentIndex].position,
//            cameraTargets[currentIndex].rotation
//        );
//    }

//    // ✅ LateUpdate avoids render-order jitter
//    void LateUpdate()
//    {
//        if (!isMoving) return;

//        Transform target = cameraTargets[currentIndex];

//        // Smooth position
//        cameraTransform.position = Vector3.SmoothDamp(
//            cameraTransform.position,
//            target.position,
//            ref positionVelocity,
//            positionSmoothTime
//        );

//        // Smooth rotation
//        Vector3 currentEuler = cameraTransform.rotation.eulerAngles;
//        Vector3 targetEuler = target.rotation.eulerAngles;

//        cameraTransform.rotation = Quaternion.Euler(
//            Mathf.SmoothDampAngle(currentEuler.x, targetEuler.x, ref rotationVelocity.x, rotationSmoothTime),
//            Mathf.SmoothDampAngle(currentEuler.y, targetEuler.y, ref rotationVelocity.y, rotationSmoothTime),
//            Mathf.SmoothDampAngle(currentEuler.z, targetEuler.z, ref rotationVelocity.z, rotationSmoothTime)
//        );

//        // ✅ SOFT STOP — NO SNAP
//        if (
//            Vector3.SqrMagnitude(cameraTransform.position - target.position) < 0.0005f &&
//            Quaternion.Angle(cameraTransform.rotation, target.rotation) < 0.1f
//        )
//        {
//            // Kill velocity → kills shake
//            positionVelocity = Vector3.zero;
//            rotationVelocity = Vector3.zero;

//            isMoving = false;
//            OnCameraFree?.Invoke();
//        }
//    }

//    public void Next()
//    {
//        if (isMoving || currentIndex >= cameraTargets.Length - 1) return;
//        currentIndex++;
//        StartMovement();
//    }

//    public void Previous()
//    {
//        if (isMoving || currentIndex <= 0) return;
//        currentIndex--;
//        StartMovement();
//    }

//    private void StartMovement()
//    {
//        positionVelocity = Vector3.zero;
//        rotationVelocity = Vector3.zero;

//        isMoving = true;
//        OnCameraBusy?.Invoke();
//    }
//}
using UnityEngine;
using System;

public class CameraMovementController : MonoBehaviour
{
    public Transform[] cameraTargets;

    public static event Action OnCameraBusy;
    public static event Action OnCameraFree;

    bool isMoving;

    public void MoveToIndex(int index)
    {
        if (isMoving) return;
        if (index < 0 || index >= cameraTargets.Length) return;
        if (!GlobalCameraController.Instance) return;

        isMoving = true;
        OnCameraBusy?.Invoke();

        GlobalCameraController.Instance.MoveTo(cameraTargets[index]);

        // simple unlock (optional delay if you want)
        Invoke(nameof(Unlock), 0.6f);
    }

    void Unlock()
    {
        isMoving = false;
        OnCameraFree?.Invoke();
    }
}
