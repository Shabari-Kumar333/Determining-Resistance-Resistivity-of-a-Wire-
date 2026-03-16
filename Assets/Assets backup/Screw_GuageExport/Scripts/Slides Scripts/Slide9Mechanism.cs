//using UnityEngine;

//public class Slide9Mechanism : MonoBehaviour
//{
//    public ObservationTableManager table;

//    void OnEnable()
//    {
//        // Set 0, Step 1 (Reading 1 Input 2)
//        MeasurementSession.Instance.SetStep(0, 1);

//        table.Refresh();
//    }
//}
using UnityEngine;

public class Slide9Mechanism : MonoBehaviour
{
    public int slideIndex = 9;

    public ObservationTableManager table;

    bool completed = false;

    void OnEnable()
    {
        completed = false;

        // Set 0, Step 1 (Reading 1 Input 2)
        MeasurementSession.Instance.SetStep(0, 1);

        table.Refresh();
    }

    // 🔑 CALLED BY VALIDATION CONTROLLER
    public void OnValidationResult(bool isCorrect)
    {
        if (completed) return;

        if (!isCorrect)
        {
            Debug.Log("❌ Slide 9 validation failed");
            return;
        }

        CompleteSlide();
    }

    void CompleteSlide()
    {
        completed = true;

        Debug.Log("✅ Slide 9 completed: Validation correct");

        SlideProgressManager.Instance.MarkCompleted(slideIndex);
    }
}
