//////using UnityEngine;

//////public class Slide3Mechanism : MonoBehaviour
//////{
//////    [Header("Wire")]
//////    public SharedObjectController sharedObjects;

//////    [Header("Screw Gauge")]
//////    public ScrewGaugePositionController positionController;
//////    public ScrewGaugeMechanism screwGauge;
//////    public Transform slide3Target;

//////    private const int SLIDE_INDEX = 3;

//////    void OnEnable()
//////    {
//////        // 1️⃣ Move screw gauge to correct position
//////        if (positionController && slide3Target)
//////            positionController.MoveToTarget(slide3Target, true);

//////        // 2️⃣ Set screw gauge mode
//////        if (screwGauge)
//////            screwGauge.EnableFreeMode();

//////        // 3️⃣ RESTORE wire state (DO NOT FORCE)
//////        if (sharedObjects)
//////            sharedObjects.RestoreWireForSlide(SLIDE_INDEX);
//////    }

//////    void OnDisable()
//////    {
//////        // ❌ Do nothing
//////    }
//////}
////using UnityEngine;

////public class Slide3Mechanism : MonoBehaviour
////{
////    public SharedObjectController sharedObjects;
////    public SharedUIController sharedUI;

////    public ScrewGaugePositionController positionController;
////    public ScrewGaugeMechanism screwGauge;
////    public Transform slide3Target;

////    private const int SLIDE_INDEX = 3;

////    void OnEnable()
////    {
////        if (positionController && slide3Target)
////            positionController.MoveToTarget(slide3Target, true);

////        if (screwGauge)
////            screwGauge.EnableFreeMode();

////        if (sharedObjects)
////            sharedObjects.RestoreWireForSlide(SLIDE_INDEX);

////        // ⚠️ UI OVERRIDE (not recommended)
////        if (sharedUI)
////            sharedUI.ShowSlider(true);
////    }
////}
//using UnityEngine;

//public class Slide3Mechanism : MonoBehaviour
//{
//    public SharedObjectController sharedObjects;
//    public SharedUIController sharedUI;

//    public ScrewGaugePositionController positionController;
//    public ScrewGaugeMechanism screwGauge;
//    public Transform slide3Target;

//    private const int SLIDE_INDEX = 3;

//    bool completed = false;

//    void OnEnable()
//    {
//        completed = false;

//        if (positionController && slide3Target)
//            positionController.MoveToTarget(slide3Target, true);

//        if (screwGauge)
//            screwGauge.EnableFreeMode();

//        if (sharedObjects)
//            sharedObjects.RestoreWireForSlide(SLIDE_INDEX);

//        if (sharedUI)
//            sharedUI.ShowSlider(true);
//    }

//    void Update()
//    {
//        if (completed) return;
//        if (screwGauge == null) return;

//        // ✅ CORRECT, SAFE CHECK
//        if (screwGauge.IsSpindleFullyOut())
//        {
//            CompleteSlide();
//        }
//    }

//    void CompleteSlide()
//    {
//        completed = true;

//        Debug.Log("✅ Slide 3 completed: Spindle fully taken out");

//        // 🔓 REPORT COMPLETION (ONLY THIS)
//        SlideProgressManager.Instance.MarkCompleted(SLIDE_INDEX);

//        // Optional UI feedback only
//        if (sharedUI)
//            sharedUI.ShowSlider(false);
//    }
//}
using UnityEngine;

public class Slide3Mechanism : MonoBehaviour
{
    public SharedObjectController sharedObjects;
    public SharedUIController sharedUI;

    public ScrewGaugePositionController positionController;
    public ScrewGaugeMechanism screwGauge;
    public Transform slide3Target;

    private const int SLIDE_INDEX = 3;

    bool completed = false;

    void OnEnable()
    {
        completed = false;

        if (positionController && slide3Target)
            positionController.MoveToTarget(slide3Target, true);

        if (screwGauge)
            screwGauge.EnableFreeMode();

        if (sharedObjects)
            sharedObjects.RestoreWireForSlide(SLIDE_INDEX);

        // ✅ KEEP SLIDER VISIBLE
        if (sharedUI)
            sharedUI.ShowSlider(true);
    }

    void Update()
    {
        if (completed) return;
        if (screwGauge == null) return;

        // ✅ CHECK ONLY FOR COMPLETION
        if (screwGauge.IsSpindleFullyOut())
        {
            CompleteSlide();
        }
    }

    void CompleteSlide()
    {
        completed = true;

        Debug.Log("✅ Slide 3 completed: Spindle fully taken out");

        // 🔓 REPORT COMPLETION ONLY
        SlideProgressManager.Instance.MarkCompleted(SLIDE_INDEX);

        // ❌ DO NOT hide slider here
        // ❌ DO NOT touch screw gauge input
    }
}
