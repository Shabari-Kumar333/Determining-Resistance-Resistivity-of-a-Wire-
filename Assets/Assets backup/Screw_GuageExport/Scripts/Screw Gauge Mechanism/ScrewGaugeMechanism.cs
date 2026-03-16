
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//public class ScrewGaugeMechanism : MonoBehaviour
//{
//    // =====================================================
//    // 🔢 MEASUREMENT POSITIONS (0–5)
//    // =====================================================
//    [Header("Measurement Position (0–5)")]
//    [Range(0, 5)]
//    public int currentPosition = 0;

//    [Header("Slider Inward Limits Per Position")]
//    public float[] positionSliderLimits = new float[6]
//    {
//        0.15f,
//        0.28f,
//        0.42f,
//        0.55f,
//        0.70f,
//        0.85f
//    };

//    [Header("Expected Values Per Position (mm)")]
//    public float[] expectedValues = new float[6]
//    {
//        0.75f,
//        1.40f,
//        2.10f,
//        2.75f,
//        3.50f,
//        4.25f
//    };

//    // =====================================================
//    // REFERENCES
//    // =====================================================
//    public Transform trimble;
//    public Transform spindle;

//    public Slider rotationSlider;
//    public TMP_Text readingText;

//    [Header("Settings")]
//    public float maxRotations = 5f;
//    public float pitch = 0.5f;
//    public float spindleMoveRange = 0.02f;

//    // =====================================================
//    // 🔊 AUDIO (ADDED)
//    // =====================================================
//    [Header("SFX")]
//    public AudioSource dragAudioSource;
//    public AudioClip dragLoopSFX;

//    // =====================================================
//    // INTERNAL
//    // =====================================================
//    float startSpindleX;
//    float startTrimbleX;
//    float startTrimbleRotX;

//    float lastSliderValue;
//    bool freeMode = false;
//    bool limitReached = false;

//    bool isDragging = false;

//    // 🔊 drag-stop detection
//    float dragStopTimer = 0f;
//    const float DRAG_STOP_DELAY = 0.05f;

//    // =====================================================
//    // UNITY
//    // =====================================================
//    void Start()
//    {
//        startSpindleX = spindle.localPosition.x;
//        startTrimbleX = trimble.localPosition.x;
//        startTrimbleRotX = trimble.localEulerAngles.x;

//        rotationSlider.onValueChanged.AddListener(UpdateMechanism);

//        lastSliderValue = rotationSlider.value;
//        ApplyMechanism(rotationSlider.value);

//        // Audio setup
//        if (dragAudioSource && dragLoopSFX)
//        {
//            dragAudioSource.clip = dragLoopSFX;
//            dragAudioSource.loop = true;
//            dragAudioSource.playOnAwake = false;
//        }
//    }

//    void Update()
//    {
//        if (!isDragging) return;

//        dragStopTimer -= Time.deltaTime;

//        if (dragStopTimer <= 0f)
//        {
//            StopDragSFX();
//        }
//    }

//    // =====================================================
//    // 🔑 POSITION API
//    // =====================================================
//    public void SetMeasurementPosition(int index)
//    {
//        currentPosition = Mathf.Clamp(index, 0, 5);
//        freeMode = false;
//        limitReached = false;

//        lastSliderValue = rotationSlider.value;
//        ApplyMechanism(rotationSlider.value);

//        MeasurementSession.Instance.expectedGaugeValue =
//            expectedValues[currentPosition];

//        Debug.Log($"[Gauge] Position {currentPosition} | Expected = {expectedValues[currentPosition]}");
//    }

//    // =====================================================
//    // LEGACY API (SAFE)
//    // =====================================================
//    public void EnableMeasurementMode()
//    {
//        freeMode = false;
//        limitReached = false;
//    }

//    public void EnableFreeMode()
//    {
//        freeMode = true;
//        limitReached = false;
//    }

//    public void ResetMeasurement()
//    {
//        lastSliderValue = rotationSlider.value;
//        ApplyMechanism(rotationSlider.value);
//    }

//    // =====================================================
//    // CORE MECHANISM
//    // =====================================================
//    void UpdateMechanism(float sliderValue)
//    {
//        bool moving = !Mathf.Approximately(sliderValue, lastSliderValue);

//        // 🔊 DRAG SFX CONTROL (FIXED)
//        if (moving)
//        {
//            dragStopTimer = DRAG_STOP_DELAY;
//            StartDragSFX();
//        }

//        bool movingInward = sliderValue < lastSliderValue;

//        if (!freeMode)
//        {
//            float inwardLimit = positionSliderLimits[currentPosition];

//            if (movingInward && sliderValue <= inwardLimit)
//            {
//                rotationSlider.SetValueWithoutNotify(inwardLimit);
//                lastSliderValue = inwardLimit;
//                ApplyMechanism(inwardLimit);
//                StopDragSFX();
//                limitReached = true;
//                return;
//            }
//        }

//        lastSliderValue = sliderValue;
//        ApplyMechanism(sliderValue);
//    }

//    void ApplyMechanism(float sliderValue)
//    {
//        float rotation = sliderValue * maxRotations * 360f;
//        trimble.localRotation = Quaternion.Euler(rotation, 0f, 0f);

//        float moveOffset = -sliderValue * spindleMoveRange;

//        spindle.localPosition = new Vector3(
//            startSpindleX + moveOffset,
//            spindle.localPosition.y,
//            spindle.localPosition.z
//        );

//        trimble.localPosition = new Vector3(
//            startTrimbleX + moveOffset,
//            trimble.localPosition.y,
//            trimble.localPosition.z
//        );

//        float fullRot = sliderValue * maxRotations;
//        float value = fullRot * pitch;

//        readingText.text = value.ToString("F3") + " mm";

//        MeasurementSession.Instance.currentGaugeValue =
//            MeasurementSession.Instance.Round(value);
//    }

//    // =====================================================
//    // 🔊 AUDIO CONTROL
//    // =====================================================
//    void StartDragSFX()
//    {
//        if (isDragging) return;
//        if (!dragAudioSource || !dragLoopSFX) return;

//        isDragging = true;
//        dragAudioSource.Play();
//    }

//    void StopDragSFX()
//    {
//        if (!isDragging) return;
//        if (!dragAudioSource) return;

//        isDragging = false;
//        dragAudioSource.Stop();
//    }
//    // =====================================================
//    // 🔍 STATE QUERY (SAFE – READ ONLY)
//    // =====================================================
//    public bool IsSpindleFullyOut(float tolerance = 0.01f)
//    {
//        return rotationSlider != null && rotationSlider.value <= tolerance;
//    }



//    void OnDisable()
//    {
//        StopDragSFX();
//    }
//}
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScrewGaugeMechanism : MonoBehaviour
{
    // =====================================================
    // 🔢 MEASUREMENT POSITIONS (0–5)
    // =====================================================
    [Header("Measurement Position (0–5)")]
    [Range(0, 5)]
    public int currentPosition = 0;

    [Header("Slider Inward Limits Per Position")]
    public float[] positionSliderLimits = new float[6]
    {
        0.15f,
        0.28f,
        0.42f,
        0.55f,
        0.70f,
        0.85f
    };

    [Header("Expected Values Per Position (mm)")]
    public float[] expectedValues = new float[6]
    {
        0.75f,
        1.40f,
        2.10f,
        2.75f,
        3.50f,
        4.25f
    };

    // =====================================================
    // REFERENCES
    // =====================================================
    public Transform trimble;
    public Transform spindle;

    public Slider rotationSlider;
    public TMP_Text readingText;

    [Header("Settings")]
    public float maxRotations = 5f;
    public float pitch = 0.5f;
    public float spindleMoveRange = 0.02f;

    // =====================================================
    // 🔊 AUDIO
    // =====================================================
    [Header("SFX")]
    public AudioSource dragAudioSource;
    public AudioClip dragLoopSFX;

    // =====================================================
    // INTERNAL
    // =====================================================
    float startSpindleX;
    float startTrimbleX;
    float startTrimbleRotX;

    float lastSliderValue;
    bool freeMode = false;
    bool limitReached = false;

    bool isDragging = false;

    float dragStopTimer = 0f;
    const float DRAG_STOP_DELAY = 0.05f;

    // =====================================================
    // UNITY
    // =====================================================
    void Start()
    {
        startSpindleX = spindle.localPosition.x;
        startTrimbleX = trimble.localPosition.x;
        startTrimbleRotX = trimble.localEulerAngles.x;

        rotationSlider.onValueChanged.AddListener(UpdateMechanism);

        lastSliderValue = rotationSlider.value;
        ApplyMechanism(rotationSlider.value);

        if (dragAudioSource && dragLoopSFX)
        {
            dragAudioSource.clip = dragLoopSFX;
            dragAudioSource.loop = true;
            dragAudioSource.playOnAwake = false;
        }
    }

    void Update()
    {
        if (!isDragging) return;

        dragStopTimer -= Time.deltaTime;

        if (dragStopTimer <= 0f)
        {
            StopDragSFX();
        }
    }

    // =====================================================
    // 🔑 POSITION API (NEXT SLIDE SAFE)
    // =====================================================
    public void SetMeasurementPosition(int index)
    {
        currentPosition = Mathf.Clamp(index, 0, 5);
        freeMode = false;
        limitReached = false;

        EnableSliderInteraction(); // 🔓 UNLOCK FOR NEXT SLIDE

        lastSliderValue = rotationSlider.value;
        ApplyMechanism(rotationSlider.value);

        MeasurementSession.Instance.expectedGaugeValue =
            expectedValues[currentPosition];

        Debug.Log($"[Gauge] Position {currentPosition} | Expected = {expectedValues[currentPosition]}");
    }

    // =====================================================
    // LEGACY API (SAFE)
    // =====================================================
    public void EnableMeasurementMode()
    {
        freeMode = false;
        limitReached = false;
        EnableSliderInteraction();
    }

    public void EnableFreeMode()
    {
        freeMode = true;
        limitReached = false;
        EnableSliderInteraction();
    }

    public void ResetMeasurement()
    {
        lastSliderValue = rotationSlider.value;
        EnableSliderInteraction();
        ApplyMechanism(rotationSlider.value);
    }

    // =====================================================
    // CORE MECHANISM
    // =====================================================
    void UpdateMechanism(float sliderValue)
    {
        bool moving = !Mathf.Approximately(sliderValue, lastSliderValue);

        if (moving)
        {
            dragStopTimer = DRAG_STOP_DELAY;
            StartDragSFX();
        }

        bool movingInward = sliderValue < lastSliderValue;

        if (!freeMode)
        {
            float inwardLimit = positionSliderLimits[currentPosition];

            if (movingInward && sliderValue <= inwardLimit)
            {
                rotationSlider.SetValueWithoutNotify(inwardLimit);
                lastSliderValue = inwardLimit;
                ApplyMechanism(inwardLimit);
                StopDragSFX();
                limitReached = true;

                DisableSliderInteraction(); // 🔒 LOCK HERE ONLY
                return;
            }
        }

        lastSliderValue = sliderValue;
        ApplyMechanism(sliderValue);
    }

    void ApplyMechanism(float sliderValue)
    {
        float rotation = sliderValue * maxRotations * 360f;
        trimble.localRotation = Quaternion.Euler(rotation, 0f, 0f);

        float moveOffset = -sliderValue * spindleMoveRange;

        spindle.localPosition = new Vector3(
            startSpindleX + moveOffset,
            spindle.localPosition.y,
            spindle.localPosition.z
        );

        trimble.localPosition = new Vector3(
            startTrimbleX + moveOffset,
            trimble.localPosition.y,
            trimble.localPosition.z
        );

        float fullRot = sliderValue * maxRotations;
        float value = fullRot * pitch;

        readingText.text = value.ToString("F3") + " mm";

        MeasurementSession.Instance.currentGaugeValue =
            MeasurementSession.Instance.Round(value);
    }

    // =====================================================
    // 🔊 AUDIO CONTROL
    // =====================================================
    void StartDragSFX()
    {
        if (isDragging) return;
        if (!dragAudioSource || !dragLoopSFX) return;

        isDragging = true;
        dragAudioSource.Play();
    }

    void StopDragSFX()
    {
        if (!isDragging) return;
        if (!dragAudioSource) return;

        isDragging = false;
        dragAudioSource.Stop();
    }

    // =====================================================
    // 🔒 SLIDER LOCK CONTROL (NEW)
    // =====================================================
    void DisableSliderInteraction()
    {
        if (rotationSlider != null)
            rotationSlider.interactable = false;
    }

    public void EnableSliderInteraction()
    {
        if (rotationSlider != null)
            rotationSlider.interactable = true;
    }

    // =====================================================
    // 🔍 STATE QUERY
    // =====================================================
    public bool IsSpindleFullyOut(float tolerance = 0.01f)
    {
        return rotationSlider != null && rotationSlider.value <= tolerance;
    }

    void OnDisable()
    {
        StopDragSFX();
    }
    void OnEnable()
    {
        EnableSliderInteraction();
    }

}
