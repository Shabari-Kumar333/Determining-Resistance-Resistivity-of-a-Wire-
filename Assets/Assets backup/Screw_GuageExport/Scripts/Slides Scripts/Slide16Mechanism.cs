//using UnityEngine;

//public class Slide16Mechanism : MonoBehaviour
//{
//    public SharedObjectController sharedObjects;
//    public ObservationTableManager table;

//    void OnEnable()
//    {
//        sharedObjects.SetMeasurementMode();
//        MeasurementSession.Instance.SetStep(1, 1);
//        table.Refresh();
//    }
//}
//using UnityEngine;

//public class Slide16Mechanism : MonoBehaviour
//{
//    public ObservationTableManager table;

//    void OnEnable()
//    {
//        // 🔑 Row 2, Reading 2 → Input 2
//        // setIndex = 1, stepIndex = 1
//        MeasurementSession.Instance.SetStep(1, 1);

//        table.Refresh();
//    }
//}
using UnityEngine;

public class Slide16Mechanism : MonoBehaviour
{
    public int slideIndex = 16;
    public ObservationTableManager table;

    bool completed = false;

    void OnEnable()
    {
        completed = false;
        MeasurementSession.Instance.SetStep(1, 1);
        table.Refresh();
    }

    public void OnValidationResult(bool isCorrect)
    {
        if (completed || !isCorrect) return;

        completed = true;
        SlideProgressManager.Instance.MarkCompleted(slideIndex);
    }
}
