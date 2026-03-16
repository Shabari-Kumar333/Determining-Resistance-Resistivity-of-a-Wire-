//using UnityEngine;

//public class Slide23Mechanism : MonoBehaviour
//{
//    public SharedObjectController sharedObjects;
//    public ObservationTableManager table;

//    void OnEnable()
//    {
//        sharedObjects.SetMeasurementMode();
//        MeasurementSession.Instance.SetStep(2, 1);
//        table.Refresh();
//    }
//}
using UnityEngine;

public class Slide23Mechanism : MonoBehaviour
{
    public int slideIndex = 23;
    public SharedObjectController sharedObjects;
    public ObservationTableManager table;

    bool completed = false;

    void OnEnable()
    {
        completed = false;
        sharedObjects.SetMeasurementMode();
        MeasurementSession.Instance.SetStep(2, 1);
        table.Refresh();
    }

    public void OnValidationResult(bool isCorrect)
    {
        if (completed || !isCorrect) return;

        completed = true;
        SlideProgressManager.Instance.MarkCompleted(slideIndex);
    }
}
