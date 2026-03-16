//using UnityEngine;

//public class Slide6Mechanism : MonoBehaviour
//{
//    public MeasurementSession session;
//    public ObservationTableManager table;

//    private void OnEnable()
//    {
//        session.SetStep(0, 0);
//        table.Refresh();
//    }
//}
using UnityEngine;

public class Slide6Mechanism : MonoBehaviour
{
    public int slideIndex = 6;

    public MeasurementSession session;
    public ObservationTableManager table;

    bool completed = false;

    void OnEnable()
    {
        completed = false;

        session.SetStep(0, 0);
        table.Refresh();
    }

    // 🔑 THIS IS CALLED BY VALIDATION CONTROLLER
    public void OnValidationResult(bool isCorrect)
    {
        if (completed) return;

        if (!isCorrect)
        {
            Debug.Log("❌ Slide 6 validation failed");
            return;
        }

        CompleteSlide();
    }

    void CompleteSlide()
    {
        completed = true;

        Debug.Log("✅ Slide 6 completed: Validation correct");

        SlideProgressManager.Instance.MarkCompleted(slideIndex);
    }
}
