////using UnityEngine;

////public class Slide19Mechanism : MonoBehaviour, ISlideRotateHandler
////{
////    public SharedUIController sharedUI;
////    public SharedObjectController sharedObjects;
////    public ScrewGaugeMechanism screwGauge;   // 🔑 ADD

////    void OnEnable()
////    {
////        sharedUI.SetRotateHandler(this);

////        // 🔑 Reading 5
////        sharedObjects.SetMeasurementMode();
////        screwGauge.SetMeasurementPosition(4);
////    }

////    void OnDisable()
////    {
////        sharedUI.ClearRotateHandler();
////    }

////    public void HandleRotate()
////    {
////        sharedObjects.MoveToState(2, 19);
////    }
////}
//using UnityEngine;

//public class Slide19Mechanism : MonoBehaviour
//{
//    public SharedUIController sharedUI;
//    public SharedObjectController sharedObjects;
//    public ScrewGaugeMechanism screwGauge;

//    void OnEnable()
//    {
//        sharedUI.HideAll();
//        sharedUI.ShowSlider(true);

//        sharedObjects.SetMeasurementMode();

//        // 🔑 Reading 3 – first measurement position
//        screwGauge.SetMeasurementPosition(4);
//    }
//}
using UnityEngine;

public class Slide19Mechanism : MonoBehaviour
{
    public int slideIndex = 19;

    public SharedUIController sharedUI;
    public SharedObjectController sharedObjects;

    [Header("Screw Gauge")]
    public ScrewGaugeMechanism screwGauge;

    bool completed = false;

    void OnEnable()
    {
        completed = false;

        if (sharedUI)
        {
            sharedUI.HideAll();
            sharedUI.ShowSlider(true);
        }

        if (sharedObjects)
            sharedObjects.SetMeasurementMode();

        // 🔑 Reading 3 – measurement position index 4
        if (screwGauge)
            screwGauge.SetMeasurementPosition(4);
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

        Debug.Log("✅ Slide 19 completed: Correct reading taken");

        SlideProgressManager.Instance.MarkCompleted(slideIndex);
    }
}
