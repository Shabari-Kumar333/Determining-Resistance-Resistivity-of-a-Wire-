//using UnityEngine;

//public class Slide13Mechanism : MonoBehaviour
//{
//    public ObservationTableManager table;

//    void OnEnable()
//    {
//        var session = MeasurementSession.Instance;

//        // 🔑 IMPORTANT: EXIT MEAN MODE
//        session.meanMode = MeasurementSession.MeanMode.None;

//        // 🔑 Row 2, Reading 2 → Input 1
//        session.SetStep(1, 0);

//        // 🔑 Expected value must be CURRENT GAUGE VALUE
//        session.expectedGaugeValue = session.currentGaugeValue;

//        Debug.Log(
//            $"[SLIDE 13] Reading Mode | Expected={session.expectedGaugeValue}"
//        );

//        table.Refresh();
//    }
//}
using UnityEngine;

public class Slide13Mechanism : MonoBehaviour
{
    public int slideIndex = 13;
    public ObservationTableManager table;

    bool completed = false;

    void OnEnable()
    {
        completed = false;

        var session = MeasurementSession.Instance;
        session.meanMode = MeasurementSession.MeanMode.None;
        session.SetStep(1, 0);
        session.expectedGaugeValue = session.currentGaugeValue;

        table.Refresh();
    }

    public void OnValidationResult(bool isCorrect)
    {
        if (completed || !isCorrect) return;

        completed = true;
        SlideProgressManager.Instance.MarkCompleted(slideIndex);
    }
}
