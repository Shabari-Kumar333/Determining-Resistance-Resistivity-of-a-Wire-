using UnityEngine;
using UnityEngine.UI;

public class SharedUIController : MonoBehaviour
{
    [Header("UI References")]
    public Slider rotationSlider;    // A
    public GameObject numPad;        // B
    public GameObject tablePanel;    // C
    public Button rotateButton;      // D

    [Header("Settings")]
    public float rotateEnableThreshold = 0.6f;

    private ISlideRotateHandler rotateHandler;
    private bool rotateUnlocked = false;
    private bool isGatedMode = false;

    void Awake()
    {
        if (rotateButton)
            rotateButton.onClick.AddListener(OnRotateClicked);

        if (rotationSlider)
            rotationSlider.onValueChanged.AddListener(OnSliderChanged);
    }

    // ================= CORE CONFIG =================
    public void ConfigureUI(SlideUIRule rule)
    {
        isGatedMode = rule.sliderUnlocksButton;

        // 🔑 IMPORTANT:
        // Do NOT reset slider value here
        // Preserve screw gauge state
        rotateUnlocked = !isGatedMode;

        if (rotationSlider)
        {
            rotationSlider.gameObject.SetActive(rule.showSlider);

            // 🛡 Preserve value WITHOUT triggering screw reset
            rotationSlider.SetValueWithoutNotify(rotationSlider.value);
        }

        if (numPad) numPad.SetActive(rule.showNumpad);
        if (tablePanel) tablePanel.SetActive(rule.showTable);

        if (rotateButton)
        {
            if (rule.showButton)
                rotateButton.gameObject.SetActive(!isGatedMode);
            else
                rotateButton.gameObject.SetActive(false);
        }
    }

    // ================= LEGACY SUPPORT =================
    public void ShowSlider(bool show)
    {
        if (rotationSlider)
            rotationSlider.gameObject.SetActive(show);
    }

    public void ShowNumPad(bool show) => numPad?.SetActive(show);
    public void ShowCheckButton(bool show) => tablePanel?.SetActive(show);
    public void ShowRotateButton(bool show) => rotateButton?.gameObject.SetActive(show);
    public void ClearRotateHandler() => rotateHandler = null;
    public void SetRotateHandler(ISlideRotateHandler h) => rotateHandler = h;

    // ================= INTERNAL LOGIC =================
    void OnSliderChanged(float value)
    {
        if (!isGatedMode) return;

        if (value >= rotateEnableThreshold && !rotateUnlocked)
        {
            rotateUnlocked = true;
            rotateButton.gameObject.SetActive(true);
        }
        else if (value < rotateEnableThreshold && rotateUnlocked)
        {
            rotateUnlocked = false;
            rotateButton.gameObject.SetActive(false);
        }
    }

    void OnRotateClicked()
    {
        Debug.Log("Rotate clicked. Handler = " + rotateHandler);

        if (rotateUnlocked && rotateHandler != null)
            rotateHandler.HandleRotate();
    }


    // ================= HIDE UI ONLY =================
    public void HideAll()
    {
        if (rotationSlider) rotationSlider.gameObject.SetActive(false);
        if (numPad) numPad.SetActive(false);
        if (tablePanel) tablePanel.SetActive(false);
        if (rotateButton) rotateButton.gameObject.SetActive(false);

        // UI state reset is OK
        rotateUnlocked = false;
        isGatedMode = false;
    }

    // ================= OPTIONAL (EXPLICIT RESET ONLY) =================
    // Call this ONLY if you WANT to reset the screw gauge
    public void ResetSliderValue()
    {
        if (rotationSlider)
            rotationSlider.value = 0f; // Explicit reset
    }


}
