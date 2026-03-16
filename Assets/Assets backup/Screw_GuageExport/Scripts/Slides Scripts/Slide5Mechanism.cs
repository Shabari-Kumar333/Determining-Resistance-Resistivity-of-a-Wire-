////using UnityEngine;
////using UnityEngine.UI;

////public class Slide5Mechanism : MonoBehaviour, ISlideRotateHandler
////{
////    public int slideIndex = 5;

////    public SharedUIController sharedUI;
////    public SharedObjectController sharedObjects;

////    public WirePoseController wire;
////    public Transform pos1;   // target pose for rotate (if needed)

////    public Slider rotationSlider;

////    void OnEnable()
////    {
////        // UI
////        rotationSlider.gameObject.SetActive(true);
////        rotationSlider.interactable = true;

////        sharedObjects.SetMeasurementMode();

////        // 🔑 SAME PATTERN AS SLIDE 11
////        sharedUI.SetRotateHandler(this);
////        wire.OnSlideEnter(slideIndex);

////        Debug.Log("[SLIDE 5] Entered, wire state preserved");
////    }

////    void OnDisable()
////    {
////        sharedUI.ClearRotateHandler();
////    }

////    // 🔄 Rotate ONLY when button pressed
////    public void HandleRotate()
////    {
////        wire.RotateOnSlide(slideIndex, pos1);
////    }
////}
////using UnityEngine;
////using UnityEngine.UI;

////public class Slide5Mechanism : MonoBehaviour, ISlideRotateHandler
////{
////    public int slideIndex = 5;

////    public SharedUIController sharedUI;
////    public SharedObjectController sharedObjects;

////    public WirePoseController wire;
////    public Transform pos1;

////    public Slider rotationSlider;

////    [Header("Measurement Lock")]
////    public float tolerance = 0.01f;   // acceptable measurement error

////    bool completed = false;

////    void OnEnable()
////    {
////        completed = false;

////        // UI
////        rotationSlider.gameObject.SetActive(true);
////        rotationSlider.interactable = true;

////        // Put gauge into measurement mode
////        sharedObjects.SetMeasurementMode();

////        sharedUI.SetRotateHandler(this);
////        wire.OnSlideEnter(slideIndex);

////        Debug.Log("[SLIDE 5] Entered, wire state preserved");
////    }

////    void OnDisable()
////    {
////        if (sharedUI)
////            sharedUI.ClearRotateHandler();
////    }

////    void Update()
////    {
////        if (completed) return;

////        float current = MeasurementSession.Instance.currentGaugeValue;
////        float expected = MeasurementSession.Instance.expectedGaugeValue;

////        if (Mathf.Abs(current - expected) <= tolerance)
////        {
////            CompleteSlide();
////        }
////    }

////    // 🔄 Rotate ONLY when button pressed
////    public void HandleRotate()
////    {
////        wire.RotateOnSlide(slideIndex, pos1);
////    }

////    void CompleteSlide()
////    {
////        completed = true;

////        Debug.Log($"✅ Slide {slideIndex} completed: Measurement matched");

////        SlideProgressManager.Instance.MarkCompleted(slideIndex);
////    }
////}
//using UnityEngine;
//using UnityEngine.UI;

//public class Slide5Mechanism : MonoBehaviour, ISlideRotateHandler
//{
//    public int slideIndex = 5;

//    public SharedUIController sharedUI;
//    public SharedObjectController sharedObjects;

//    public WirePoseController wire;
//    public Transform pos1;

//    public Slider rotationSlider;
//    public ScrewGaugeMechanism screwGauge;

//    [Header("Measurement Settings")]
//    [Tooltip("Measurement position index used for this slide (0–5)")]
//    public int measurementPositionIndex = 0;

//    [Tooltip("Allowed measurement tolerance")]
//    public float tolerance = 0.01f;

//    bool completed = false;

//    void OnEnable()
//    {
//        completed = false;

//        // ✅ UI
//        if (rotationSlider)
//        {
//            rotationSlider.gameObject.SetActive(true);
//            rotationSlider.interactable = true;
//        }

//        // ✅ Put system into measurement mode
//        if (sharedObjects)
//            sharedObjects.SetMeasurementMode();

//        // 🔑 IMPORTANT FIX: Set expected gauge value
//        if (screwGauge)
//            screwGauge.SetMeasurementPosition(measurementPositionIndex);

//        // Existing behavior (DO NOT TOUCH)
//        if (sharedUI)
//            sharedUI.SetRotateHandler(this);

//        if (wire)
//            wire.OnSlideEnter(slideIndex);

//        Debug.Log($"[SLIDE {slideIndex}] Entered | Expected value set");
//    }

//    void OnDisable()
//    {
//        if (sharedUI)
//            sharedUI.ClearRotateHandler();
//    }

//    void Update()
//    {
//        if (completed) return;

//        float current = MeasurementSession.Instance.currentGaugeValue;
//        float expected = MeasurementSession.Instance.expectedGaugeValue;

//        if (Mathf.Abs(current - expected) <= tolerance)
//        {
//            CompleteSlide();
//        }
//    }

//    // 🔄 Rotate ONLY when button pressed
//    public void HandleRotate()
//    {
//        if (wire)
//            wire.RotateOnSlide(slideIndex, pos1);
//    }

//    void CompleteSlide()
//    {
//        completed = true;

//        Debug.Log($"✅ Slide {slideIndex} completed: Measurement matched");

//        // 🔓 REPORT COMPLETION
//        SlideProgressManager.Instance.MarkCompleted(slideIndex);
//    }
//}
using UnityEngine;
using UnityEngine.UI;

public class Slide5Mechanism : MonoBehaviour, ISlideRotateHandler
{
    public int slideIndex = 5;

    public SharedUIController sharedUI;
    public SharedObjectController sharedObjects;

    public WirePoseController wire;
    public Transform pos1;

    public Slider rotationSlider;
    public ScrewGaugeMechanism screwGauge;

    [Header("Measurement Settings")]
    [Tooltip("Measurement position index used for this slide (0–5)")]
    public int measurementPositionIndex = 0;

    bool completed = false;

    void OnEnable()
    {
        completed = false;

        // UI
        if (rotationSlider)
        {
            rotationSlider.gameObject.SetActive(true);
            rotationSlider.interactable = true;
        }

        // Put system into measurement mode
        if (sharedObjects)
            sharedObjects.SetMeasurementMode();

        // 🔑 Set expected value for this slide
        if (screwGauge)
            screwGauge.SetMeasurementPosition(measurementPositionIndex);

        // Existing behavior
        if (sharedUI)
            sharedUI.SetRotateHandler(this);

        if (wire)
            wire.OnSlideEnter(slideIndex);

        Debug.Log($"[SLIDE {slideIndex}] Entered | Expected value set");
    }

    void OnDisable()
    {
        if (sharedUI)
            sharedUI.ClearRotateHandler();
    }

    void Update()
    {
        if (completed) return;

        // 🔍 Get measurement values
        float current = MeasurementSession.Instance.currentGaugeValue;
        float expected = MeasurementSession.Instance.expectedGaugeValue;

        // 🔑 Match UI precision (3 decimal places)
        current = Mathf.Round(current * 1000f) / 1000f;
        expected = Mathf.Round(expected * 1000f) / 1000f;

        if (Mathf.Approximately(current, expected))
        {
            CompleteSlide();
        }
    }

    // 🔄 Rotate ONLY when button pressed
    public void HandleRotate()
    {
        if (wire)
            wire.RotateOnSlide(slideIndex, pos1);
    }

    void CompleteSlide()
    {
        completed = true;

        Debug.Log($"✅ Slide {slideIndex} completed: Measurement matched");

        // 🔓 REPORT COMPLETION
        SlideProgressManager.Instance.MarkCompleted(slideIndex);
    }
}
