//using UnityEngine;

//public class Slide24Mechanism : MonoBehaviour
//{
//    public ObservationTableManager table;

//    void OnEnable()
//    {
//        var session = MeasurementSession.Instance;

//        // 🔑 Same logic as Slide10, only set index changes
//        session.SetSetMeanStage(2);
//        session.expectedGaugeValue = session.GetSetMean(2);

//        Debug.Log($"[MEAN EXPECTED] Set=2 Mean={session.expectedGaugeValue}");

//        table.Refresh();
//    }
//}
using UnityEngine;

public class Slide24Mechanism : MonoBehaviour
{
    public int slideIndex = 24;
    public ObservationTableManager table;

    bool completed = false;

    void OnEnable()
    {
        completed = false;

        var session = MeasurementSession.Instance;
        session.SetSetMeanStage(2);
        session.expectedGaugeValue = session.GetSetMean(2);

        table.Refresh();
    }

    public void OnValidationResult(bool isCorrect)
    {
        if (completed || !isCorrect) return;

        completed = true;
        SlideProgressManager.Instance.MarkCompleted(slideIndex);
    }
}
