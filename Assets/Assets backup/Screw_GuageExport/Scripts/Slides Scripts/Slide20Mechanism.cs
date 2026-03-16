//using UnityEngine;

//public class Slide20Mechanism : MonoBehaviour
//{
//    public ObservationTableManager table;

//    void OnEnable()
//    {
//        var session = MeasurementSession.Instance;

//        // 🔑 IMPORTANT: EXIT MEAN MODE
//        session.meanMode = MeasurementSession.MeanMode.None;

//        // 🔑 Row 3, Reading 3 → Input 1
//        session.SetStep(2, 0);

//        // 🔑 Expected value = CURRENT GAUGE VALUE
//        session.expectedGaugeValue = session.currentGaugeValue;

//        Debug.Log(
//            $"[SLIDE 20] Reading Mode | Expected={session.expectedGaugeValue}"
//        );

//        table.Refresh();
//    }
//}
using UnityEngine;

public class Slide20Mechanism : MonoBehaviour
{
    public int slideIndex = 20;
    public ObservationTableManager table;

    bool completed = false;

    void OnEnable()
    {
        completed = false;

        var session = MeasurementSession.Instance;
        session.meanMode = MeasurementSession.MeanMode.None;
        session.SetStep(2, 0);
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
