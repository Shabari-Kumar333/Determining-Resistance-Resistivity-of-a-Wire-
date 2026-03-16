using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class ButtonCameraMoveAndEvent : MonoBehaviour
{
    [Header("Camera")]
    public Transform cameraTarget;
    public float moveDuration = 1.5f;

    [Header("Delay Before Move")]
    public float delayBeforeMove = 2f;

    [Header("Events After Camera Arrives")]
    public UnityEvent onCameraArrived;

    private bool isRunning = false;
    private Camera mainCamera;

    void Awake()
    {
        mainCamera = Camera.main;

        if (mainCamera == null)
            Debug.LogError("Main Camera not found!");
    }

    // 🔘 Hook this to Button OnClick
    public void OnButtonPress()
    {
        if (isRunning) return;

        StartCoroutine(CameraSequence());
    }

    private IEnumerator CameraSequence()
    {
        isRunning = true;

        // ⏳ Wait before moving
        yield return new WaitForSeconds(delayBeforeMove);

        Vector3 startPos = mainCamera.transform.position;
        Quaternion startRot = mainCamera.transform.rotation;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / moveDuration;
            mainCamera.transform.position = Vector3.Lerp(startPos, cameraTarget.position, t);
            mainCamera.transform.rotation = Quaternion.Slerp(startRot, cameraTarget.rotation, t);
            yield return null;
        }

        // Hard align (no drift)
        mainCamera.transform.SetPositionAndRotation(
            cameraTarget.position,
            cameraTarget.rotation
        );

        // 🔔 Fire events AFTER arrival
        onCameraArrived?.Invoke();

        isRunning = false;
    }
}
