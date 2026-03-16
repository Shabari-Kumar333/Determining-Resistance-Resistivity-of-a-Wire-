//using UnityEngine;

//public class Slide10Mechanism : MonoBehaviour
//{
//    public ObservationTableManager table;

//    void OnEnable()
//    {
//        var session = MeasurementSession.Instance;

//        session.SetSetMeanStage(0);
//        session.expectedGaugeValue = session.GetSetMean(0);

//        Debug.Log($"[MEAN EXPECTED] Set=0 Mean={session.expectedGaugeValue}");

//        table.Refresh();
//    }
//}
using UnityEngine;

public class Slide10Mechanism : MonoBehaviour
{
    public int slideIndex = 10;

    public ObservationTableManager table;

    bool completed = false;

    void OnEnable()
    {
        completed = false;

        var session = MeasurementSession.Instance;

        // 🔑 Set mean entry stage
        session.SetSetMeanStage(0);

        // 🔑 Set expected mean value
        session.expectedGaugeValue = session.GetSetMean(0);

        Debug.Log($"[MEAN EXPECTED] Set=0 Mean={session.expectedGaugeValue}");

        table.Refresh();
    }

    // 🔑 CALLED BY VALIDATION CONTROLLER
    public void OnValidationResult(bool isCorrect)
    {
        if (completed) return;

        if (!isCorrect)
        {
            Debug.Log("❌ Slide 10 validation failed");
            return;
        }

        CompleteSlide();
    }

    void CompleteSlide()
    {
        completed = true;

        Debug.Log("✅ Slide 10 completed: Mean validation correct");

        SlideProgressManager.Instance.MarkCompleted(slideIndex);
    }
}
