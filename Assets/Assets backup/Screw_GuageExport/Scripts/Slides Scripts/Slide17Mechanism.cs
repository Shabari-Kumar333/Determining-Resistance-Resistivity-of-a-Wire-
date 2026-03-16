//using UnityEngine;

//public class Slide17Mechanism : MonoBehaviour
//{
//    public ObservationTableManager table;

//    void OnEnable()
//    {
//        var session = MeasurementSession.Instance;

//        // 🔑 Enter SET MEAN mode for Row 2
//        session.SetSetMeanStage(1);

//        // 🔑 Calculate expected mean from Row 2 readings
//        session.expectedGaugeValue = session.GetSetMean(1);

//        Debug.Log(
//            $"[ROW 2 MEAN EXPECTED] Set=1 Mean={session.expectedGaugeValue}"
//        );

//        // 🔓 Enable mean input field
//        table.Refresh();
//    }
//}
using UnityEngine;

public class Slide17Mechanism : MonoBehaviour
{
    public int slideIndex = 17;
    public ObservationTableManager table;

    bool completed = false;

    void OnEnable()
    {
        completed = false;

        var session = MeasurementSession.Instance;
        session.SetSetMeanStage(1);
        session.expectedGaugeValue = session.GetSetMean(1);

        table.Refresh();
    }

    public void OnValidationResult(bool isCorrect)
    {
        if (completed || !isCorrect) return;

        completed = true;
        SlideProgressManager.Instance.MarkCompleted(slideIndex);
    }
}
