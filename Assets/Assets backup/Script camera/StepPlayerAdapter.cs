using UnityEngine;

public class StepPlayerAdapter : MonoBehaviour
{
    public StepPlayer stepPlayer;
    public Transform[] cameraTargets; // same order as steps

    void OnEnable()
    {
        if (!stepPlayer) return;
        SetStep(stepPlayer.GetCurrentIndex());
    }

    // called by SlideSetController
    public void SetStep(int targetStep)
    {
        if (!stepPlayer) return;

        targetStep = Mathf.Clamp(
            targetStep,
            0,
            stepPlayer.elements.Count - 1
        );

        int current = stepPlayer.GetCurrentIndex();

        while (current < targetStep)
        {
            stepPlayer.PlayForward();
            current++;
        }

        while (current > targetStep)
        {
            stepPlayer.PlayBackward();
            current--;
        }

        // 🔑 GLOBAL CAMERA MOVE
        if (cameraTargets != null &&
            targetStep < cameraTargets.Length &&
            GlobalCameraController.Instance)
        {
            GlobalCameraController.Instance.MoveTo(cameraTargets[targetStep]);
        }
    }
}
