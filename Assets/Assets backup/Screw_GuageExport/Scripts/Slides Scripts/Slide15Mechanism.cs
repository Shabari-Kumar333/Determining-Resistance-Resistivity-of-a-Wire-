//using UnityEngine;

//public class Slide15Mechanism : MonoBehaviour, ISlideRotateHandler
//{
//    public SharedUIController sharedUI;
//    public SharedObjectController sharedObjects;
//    public ScrewGaugeMechanism screwGauge;   // 🔑 ADD

//    void OnEnable()
//    {
//        sharedUI.SetRotateHandler(this);

//        // 🔑 Reading 4
//        sharedObjects.SetMeasurementMode();
//        screwGauge.SetMeasurementPosition(3);
//    }

//    void OnDisable()
//    {
//        sharedUI.ClearRotateHandler();
//    }

//    public void HandleRotate()
//    {
//        sharedObjects.MoveToState(5, 15);
//    }
//}
using UnityEngine;

public class Slide15Mechanism : MonoBehaviour, ISlideRotateHandler
{
    public int slideIndex = 15;

    public SharedUIController sharedUI;
    public SharedObjectController sharedObjects;

    [Header("Screw Gauge")]
    public ScrewGaugeMechanism screwGauge;   // measurement source

    bool completed = false;

    void OnEnable()
    {
        completed = false;

        // Rotate button still needed for interaction
        if (sharedUI)
            sharedUI.SetRotateHandler(this);

        // 🔑 Measurement mode
        if (sharedObjects)
            sharedObjects.SetMeasurementMode();

        // 🔑 Reading 4 → measurement position index 3
        if (screwGauge)
            screwGauge.SetMeasurementPosition(3);
    }

    void OnDisable()
    {
        if (sharedUI)
            sharedUI.ClearRotateHandler();
    }

    // Rotate is allowed, but DOES NOT unlock the slide
    public void HandleRotate()
    {
        if (sharedObjects)
            sharedObjects.MoveToState(5, 15);
    }

    void Update()
    {
        if (completed) return;

        float current = MeasurementSession.Instance.currentGaugeValue;
        float expected = MeasurementSession.Instance.expectedGaugeValue;

        // 🔑 Match displayed precision
        current = Mathf.Round(current * 1000f) / 1000f;
        expected = Mathf.Round(expected * 1000f) / 1000f;

        if (Mathf.Approximately(current, expected))
        {
            CompleteSlide();
        }
    }

    void CompleteSlide()
    {
        completed = true;

        Debug.Log("✅ Slide 15 completed: Correct reading taken");

        SlideProgressManager.Instance.MarkCompleted(slideIndex);
    }
}
