
//using UnityEngine;

//public class ObservationTableManager : MonoBehaviour
//{
//    public InputSlotController[] slots;

//    public void Refresh()
//    {
//        var session = MeasurementSession.Instance;

//        // 🔒 Lock all inputs first
//        foreach (var slot in slots)
//        {
//            if (slot == null) continue;
//            slot.ForceLock();
//        }

//        // 🔓 Unlock ONLY matching table slot
//        foreach (var slot in slots)
//        {
//            if (slot == null) continue;

//            // 🔑 Ignore Final Mean / special inputs
//            if (slot.setIndex < 0 || slot.stepIndex < 0)
//                continue;

//            if (slot.setIndex == session.currentSet &&
//                slot.stepIndex == session.currentStep)
//            {
//                slot.Unlock();
//                return;
//            }
//        }

//    }
//    public float GetEnteredValue()
//    {
//        var session = MeasurementSession.Instance;

//        foreach (var slot in slots)
//        {
//            if (slot == null) continue;

//            // Match the currently active slot
//            if (slot.setIndex == session.currentSet &&
//                slot.stepIndex == session.currentStep)
//            {
//                return slot.GetValue(); // 🔑 read user input
//            }
//        }

//        return float.NaN;
//    }

//}








////using UnityEngine;

////public class ObservationTableManager : MonoBehaviour
////{
////    public InputSlotController[] slots;

////    // 🔄 Call whenever step/set changes
////    public void Refresh()
////    {
////        var session = MeasurementSession.Instance;

////        // 🔒 Lock ALL inputs first
////        foreach (var slot in slots)
////        {
////            if (slot == null) continue;
////            slot.ForceLock();
////        }

////        // 🔓 Unlock ONLY active slot
////        foreach (var slot in slots)
////        {
////            if (slot == null) continue;

////            if (slot.setIndex == session.currentSet &&
////                slot.stepIndex == session.currentStep)
////            {
////                slot.Unlock();   // ✅ typing enabled
////                return;
////            }
////        }
////    }

////    // ✅ Call AFTER validation is correct
////    public void LockCurrentSlotAsCompleted()
////    {
////        var session = MeasurementSession.Instance;

////        foreach (var slot in slots)
////        {
////            if (slot == null) continue;

////            if (slot.setIndex == session.currentSet &&
////                slot.stepIndex == session.currentStep)
////            {
////                slot.MarkCompleted();   // 🔒 lock + correct SFX
////                return;
////            }
////        }
////    }

////    public float GetEnteredValue()
////    {
////        var session = MeasurementSession.Instance;

////        foreach (var slot in slots)
////        {
////            if (slot == null) continue;

////            if (slot.setIndex == session.currentSet &&
////                slot.stepIndex == session.currentStep)
////            {
////                return slot.GetValue();
////            }
////        }

////        return float.NaN;
////    }
////}
using UnityEngine;

public class ObservationTableManager : MonoBehaviour
{
    public InputSlotController[] slots;

    public void Refresh()
    {
        var session = MeasurementSession.Instance;

        // 🔒 TEMP lock all
        foreach (var slot in slots)
            if (slot) slot.ForceLock();

        // 🔓 Unlock active slot only
        foreach (var slot in slots)
        {
            if (!slot) continue;

            //if (slot.setIndex == session.currentSet &&
            //    slot.stepIndex == session.currentStep)
            //{
            //    slot.Unlock();
            //    return;
            //}
            if (slot.setIndex == session.currentSet &&
    slot.stepIndex == session.currentStep)
            {
                if (!slot.IsCompleted())   // 🛑 guard
                    slot.Unlock();

                return;
            }

        }
    }

    public float GetEnteredValue()
    {
        var session = MeasurementSession.Instance;

        foreach (var slot in slots)
        {
            if (slot &&
                slot.setIndex == session.currentSet &&
                slot.stepIndex == session.currentStep)
                return slot.GetValue();
        }
        return float.NaN;
    }
}
