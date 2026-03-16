using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

[System.Serializable]
public class SlideUIRule
{
    public int slideIndex;
    public bool showSlider;
    public bool showNumpad;
    public bool showTable;
    public bool showButton;
    public bool sliderUnlocksButton;
}

public class SlideManager : MonoBehaviour
{
    [Header("References")]
    public SharedUIController sharedUI;
    public CheckButtonRouter checkButtonRouter;

    [Header("Camera Positions")]
    public List<Transform> cameraPositions = new();

    [Header("Slide Panels")]
    public List<GameObject> slidePanels = new();

    [Header("Slide Mechanisms")]
    public List<GameObject> mechanisms = new();

    [Header("Shared Mechanisms")]
    public List<GameObject> sharedMechanisms = new();

    [Header("UI Rules")]
    public List<SlideUIRule> uiRules = new();

    public enum NavigationMode { Clamp, Loop, Exit }
    public NavigationMode navigationMode = NavigationMode.Exit;

    [Header("Navigation Events")]
    public UnityEvent onBackFromFirstSlide;
    public UnityEvent onNextFromLastSlide;

    int currentIndex = 0;

    void Start()
    {
        currentIndex = Mathf.Clamp(currentIndex, 0, GetMaxSlideIndex());
        ApplySlideState();
    }

    void OnEnable()
    {
        ApplySlideState();
    }

    // ================= CORE =================
    void ApplySlideState()
    {
        MoveCamera();

        DisableAll(slidePanels);
        DisableAll(mechanisms, sharedMechanisms);

        Enable(slidePanels, currentIndex);
        ApplySharedUI();
        Enable(mechanisms, currentIndex, sharedMechanisms);

        if (checkButtonRouter != null)
        {
            if (currentIndex == 24)
                checkButtonRouter.UseFinalValidation();
            else
                checkButtonRouter.UseNormalValidation();
        }
    }

    void MoveCamera()
    {
        if (GlobalCameraController.Instance == null) return;
        if (currentIndex >= cameraPositions.Count) return;

        GlobalCameraController.Instance.MoveTo(cameraPositions[currentIndex]);
    }

    void ApplySharedUI()
    {
        if (!sharedUI) return;

        sharedUI.HideAll();
        if (currentIndex < 2) return;

        var rule = uiRules.Find(r => r.slideIndex == currentIndex);
        if (rule != null)
            sharedUI.ConfigureUI(rule);
    }

    int GetMaxSlideIndex()
    {
        int max = cameraPositions.Count - 1;
        if (slidePanels.Count - 1 < max) max = slidePanels.Count - 1;
        if (mechanisms.Count - 1 < max) max = mechanisms.Count - 1;
        return Mathf.Max(0, max);
    }

    // ================= UTIL =================
    void DisableAll(List<GameObject> list, List<GameObject> exclude = null)
    {
        foreach (var obj in list)
            if (obj && (exclude == null || !exclude.Contains(obj)))
                obj.SetActive(false);
    }

    void Enable(List<GameObject> list, int index, List<GameObject> exclude = null)
    {
        if (index >= list.Count) return;
        if (!list[index]) return;
        if (exclude != null && exclude.Contains(list[index])) return;

        list[index].SetActive(true);
    }

    // ================= GLOBAL CONTROL =================
    public void SetSlide(int index)
    {
        currentIndex = Mathf.Clamp(index, 0, GetMaxSlideIndex());
        ApplySlideState();
    }
}
