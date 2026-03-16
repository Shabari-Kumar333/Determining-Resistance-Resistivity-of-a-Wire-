//using UnityEngine;

//public class Slide12Mechanism : MonoBehaviour
//{
//    public SharedObjectController sharedObjects;

//    [Header("Measurement")]
//    //public WireMeasurementData wireMeasurement;
//    public int stateIndex = 2;   // Row 2 – Reading 1

//    [Header("Screw Gauge")]
//    public ScrewGaugeMechanism screwGauge;

//    void OnEnable()
//    {
//        // 🔑 Enter measurement mode
//        sharedObjects.SetMeasurementMode();

//        // 🔑 Set correct measurement position
//        screwGauge.SetMeasurementPosition(stateIndex);

//        // ❌ DO NOT reapply wire state
//        // wireMeasurement.ApplyState(stateIndex);
//    }
//}
using UnityEngine;

public class Slide12Mechanism : MonoBehaviour
{
    public int slideIndex = 12;

    public SharedObjectController sharedObjects;

    [Header("Measurement")]
    public int stateIndex = 2;   // Row 2 – Reading 1

    [Header("Screw Gauge")]
    public ScrewGaugeMechanism screwGauge;

    bool completed = false;

    void OnEnable()
    {
        completed = false;

        // 🔑 Enter measurement mode
        if (sharedObjects)
            sharedObjects.SetMeasurementMode();

        // 🔑 Set correct measurement position
        if (screwGauge)
            screwGauge.SetMeasurementPosition(stateIndex);
    }

    void Update()
    {
        if (completed) return;

        float current = MeasurementSession.Instance.currentGaugeValue;
        float expected = MeasurementSession.Instance.expectedGaugeValue;

        // 🔑 Match display precision (3 decimals)
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

        Debug.Log("✅ Slide 12 completed: Measurement matched");

        SlideProgressManager.Instance.MarkCompleted(slideIndex);
    }
}
