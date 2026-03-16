//using UnityEngine;

//public class Slide8Mechanism : MonoBehaviour
//{
//    public SharedUIController sharedUI;
//    public SharedObjectController sharedObjects;

//    [Header("Screw Gauge")]
//    public ScrewGaugeMechanism screwGauge;   // 🔑 ADD THIS

//    void OnEnable()
//    {
//        sharedUI.HideAll();
//        sharedUI.ShowSlider(true);

//        sharedObjects.SetMeasurementMode();

//        // 🔑 NEW: SECOND measurement position
//        screwGauge.SetMeasurementPosition(1);
//    }
//}
using UnityEngine;

public class Slide8Mechanism : MonoBehaviour
{
    public int slideIndex = 8;

    public SharedUIController sharedUI;
    public SharedObjectController sharedObjects;

    [Header("Screw Gauge")]
    public ScrewGaugeMechanism screwGauge;

    bool completed = false;

    void OnEnable()
    {
        completed = false;

        // UI setup (existing behavior)
        if (sharedUI)
        {
            sharedUI.HideAll();
            sharedUI.ShowSlider(true);
        }

        // Measurement mode
        if (sharedObjects)
            sharedObjects.SetMeasurementMode();

        // 🔑 Set expected value for Slide 8
        if (screwGauge)
            screwGauge.SetMeasurementPosition(1);
    }

    void Update()
    {
        if (completed) return;

        float current = MeasurementSession.Instance.currentGaugeValue;
        float expected = MeasurementSession.Instance.expectedGaugeValue;

        // 🔑 Match UI precision (3 decimals)
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

        Debug.Log("✅ Slide 8 completed: Measurement matched");

        // 🔓 REPORT COMPLETION
        SlideProgressManager.Instance.MarkCompleted(slideIndex);
    }
}
