//using UnityEngine;

//public class Slide22Mechanism : MonoBehaviour, ISlideRotateHandler
//{
//    public SharedUIController sharedUI;
//    public SharedObjectController sharedObjects;
//    public ScrewGaugeMechanism screwGauge;   // 🔑 ADD

//    void OnEnable()
//    {
//        sharedUI.SetRotateHandler(this);

//        // 🔑 Reading 6
//        sharedObjects.SetMeasurementMode();
//        screwGauge.SetMeasurementPosition(5);
//    }

//    void OnDisable()
//    {
//        sharedUI.ClearRotateHandler();
//    }

//    public void HandleRotate()
//    {
//        sharedObjects.MoveToState(4, 22);
//    }
//}
using UnityEngine;

public class Slide22Mechanism : MonoBehaviour, ISlideRotateHandler
{
    public int slideIndex = 22;

    public SharedUIController sharedUI;
    public SharedObjectController sharedObjects;

    [Header("Screw Gauge")]
    public ScrewGaugeMechanism screwGauge;

    bool completed = false;

    void OnEnable()
    {
        completed = false;

        // Rotate button is still required for interaction
        if (sharedUI)
            sharedUI.SetRotateHandler(this);

        // 🔑 Measurement mode
        if (sharedObjects)
            sharedObjects.SetMeasurementMode();

        // 🔑 Reading 6 → measurement position index 5
        if (screwGauge)
            screwGauge.SetMeasurementPosition(5);
    }

    void OnDisable()
    {
        if (sharedUI)
            sharedUI.ClearRotateHandler();
    }

    // Rotate does NOT unlock the slide
    public void HandleRotate()
    {
        if (sharedObjects)
            sharedObjects.MoveToState(4, 22);
    }

    void Update()
    {
        if (completed) return;

        float current = MeasurementSession.Instance.currentGaugeValue;
        float expected = MeasurementSession.Instance.expectedGaugeValue;

        // 🔑 Match displayed precision (3 decimals)
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

        Debug.Log("✅ Slide 22 completed: Correct reading taken");

        SlideProgressManager.Instance.MarkCompleted(slideIndex);
    }
}
