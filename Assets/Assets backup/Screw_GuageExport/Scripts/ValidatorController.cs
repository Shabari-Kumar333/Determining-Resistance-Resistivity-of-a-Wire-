
//using UnityEngine;

//public class ValidatorController : MonoBehaviour
//{
//    public MeasurementSession session;
//    public NumberPadController numberPad;

//    private InputSlotController activeSlot;

//    [Header("Validation")]
//    public float tolerance = 0.01f;

//    [Header("Auto Fill Rules")]
//    public int autoFillAfterWrongCount = 2;

//    int wrongAttemptCount = 0;

//    // ─────────────────────────────────────────────
//    // SLOT MANAGEMENT
//    // ─────────────────────────────────────────────
//    public void SetActiveSlot(InputSlotController slot)
//    {
//        activeSlot = slot;

//        if (slot != null && numberPad != null)
//            numberPad.SetActiveInput(slot.input);

//        wrongAttemptCount = 0;
//    }

//    // ─────────────────────────────────────────────
//    // CHECK BUTTON
//    // ─────────────────────────────────────────────
//    public void OnCheckPressed()
//    {
//        if (activeSlot == null)
//            return;

//        // 🔥 reset before EVERY check
//        activeSlot.ResetValidationState();

//        // -------------------------------
//        // PARSE INPUT
//        // -------------------------------
//        if (!float.TryParse(activeSlot.input.text, out float entered))
//        {
//            HandleWrongAnswer(0f);
//            return;
//        }

//        // -------------------------------
//        // EXPECTED VALUE (UNCHANGED)
//        // -------------------------------
//        float expected;
//        switch (session.meanMode)
//        {
//            case MeasurementSession.MeanMode.SetMean:
//                expected = session.GetSetMean(session.currentSet);
//                break;

//            case MeasurementSession.MeanMode.FinalMean:
//                expected = session.GetFinalMean();
//                break;

//            default:
//                expected = session.currentGaugeValue;
//                break;
//        }

//        // -------------------------------
//        // VALIDATION
//        // -------------------------------
//        if (Mathf.Abs(entered - expected) > session.tolerance)
//        {
//            HandleWrongAnswer(expected);
//            return;
//        }

//        // ===============================
//        // ✅ CORRECT (UNCHANGED LOGIC)
//        // ===============================
//        if (session.meanMode == MeasurementSession.MeanMode.FinalMean)
//        {
//            session.SaveFinalMean(activeSlot.input.text);
//        }
//        else
//        {
//            session.SaveReadingText(
//                activeSlot.setIndex,
//                activeSlot.stepIndex,
//                activeSlot.input.text
//            );
//        }

//        activeSlot.ShowCorrectWithSFX();

//        // 🔒 ONLY CHANGE: permanent lock
//        activeSlot.MarkCompleted();

//        activeSlot = null;
//        wrongAttemptCount = 0;

//        NotifySlideValidationSuccess();
//    }

//    // ─────────────────────────────────────────────
//    // ❌ WRONG HANDLER (UNCHANGED)
//    // ─────────────────────────────────────────────
//    void HandleWrongAnswer(float expected)
//    {
//        wrongAttemptCount++;

//        activeSlot.ShowWrongWithSFX();

//        activeSlot.input.text = "";

//        if (wrongAttemptCount >= autoFillAfterWrongCount)
//        {
//            AutoFillController_Set1.Instance?.EnableAutoFill(
//                activeSlot,
//                expected
//            );
//        }
//    }

//    // ─────────────────────────────────────────────
//    // SLIDE UNLOCK ROUTER (UNCHANGED)
//    // ─────────────────────────────────────────────
//    void NotifySlideValidationSuccess()
//    {
//        FindObjectOfType<Slide6Mechanism>()?.OnValidationResult(true);
//        FindObjectOfType<Slide9Mechanism>()?.OnValidationResult(true);
//        FindObjectOfType<Slide10Mechanism>()?.OnValidationResult(true);

//        FindObjectOfType<Slide13Mechanism>()?.OnValidationResult(true);
//        FindObjectOfType<Slide16Mechanism>()?.OnValidationResult(true);
//        FindObjectOfType<Slide17Mechanism>()?.OnValidationResult(true);

//        FindObjectOfType<Slide20Mechanism>()?.OnValidationResult(true);
//        FindObjectOfType<Slide23Mechanism>()?.OnValidationResult(true);
//        FindObjectOfType<Slide24Mechanism>()?.OnValidationResult(true);
//    }
//}
//using UnityEngine;

//public class ValidatorController : MonoBehaviour
//{
//    public MeasurementSession session;
//    public NumberPadController numberPad;

//    private InputSlotController activeSlot;

//    [Header("Validation")]
//    public float tolerance = 0.01f;

//    [Header("Auto Fill Rules")]
//    public int autoFillAfterWrongCount = 2;

//    int wrongAttemptCount = 0;

//    // ─────────────────────────────────────────────
//    // SLOT MANAGEMENT
//    // ─────────────────────────────────────────────
//    public void SetActiveSlot(InputSlotController slot)
//    {
//        activeSlot = slot;

//        if (slot != null && numberPad != null)
//            numberPad.SetActiveInput(slot.input);

//        wrongAttemptCount = 0;
//    }

//    // ─────────────────────────────────────────────
//    // CHECK BUTTON (UNCHANGED)
//    // ─────────────────────────────────────────────
//    public void OnCheckPressed()
//    {
//        if (activeSlot == null)
//            return;

//        activeSlot.ResetValidationState();

//        if (!float.TryParse(activeSlot.input.text, out float entered))
//        {
//            HandleWrongAnswer(0f);
//            return;
//        }

//        float expected;
//        switch (session.meanMode)
//        {
//            case MeasurementSession.MeanMode.SetMean:
//                expected = session.GetSetMean(session.currentSet);
//                break;

//            case MeasurementSession.MeanMode.FinalMean:
//                expected = session.GetFinalMean();
//                break;

//            default:
//                expected = session.currentGaugeValue;
//                break;
//        }

//        if (Mathf.Abs(entered - expected) > session.tolerance)
//        {
//            HandleWrongAnswer(expected);
//            return;
//        }

//        if (session.meanMode == MeasurementSession.MeanMode.FinalMean)
//        {
//            session.SaveFinalMean(activeSlot.input.text);
//        }
//        else
//        {
//            session.SaveReadingText(
//                activeSlot.setIndex,
//                activeSlot.stepIndex,
//                activeSlot.input.text
//            );
//        }

//        activeSlot.ShowCorrectWithSFX();
//        activeSlot.MarkCompleted();

//        activeSlot = null;
//        wrongAttemptCount = 0;

//        NotifySlideValidationSuccess();
//    }

//    // ─────────────────────────────────────────────
//    // ✅ AUTO-FILL FORCE CORRECT (NEW)
//    // ─────────────────────────────────────────────
//    public void ForceAcceptCorrect(InputSlotController slot, float value)
//    {
//        if (slot == null) return;

//        string text = value.ToString("F3");

//        if (session.meanMode == MeasurementSession.MeanMode.FinalMean)
//        {
//            session.SaveFinalMean(text);
//        }
//        else
//        {
//            session.SaveReadingText(
//                slot.setIndex,
//                slot.stepIndex,
//                text
//            );
//        }

//        slot.ShowCorrectWithSFX();
//        slot.MarkCompleted();

//        activeSlot = null;
//        wrongAttemptCount = 0;

//        NotifySlideValidationSuccess();
//    }

//    // ─────────────────────────────────────────────
//    // WRONG HANDLER (UNCHANGED)
//    // ─────────────────────────────────────────────
//    void HandleWrongAnswer(float expected)
//    {
//        wrongAttemptCount++;

//        activeSlot.ShowWrongWithSFX();
//        activeSlot.input.text = "";

//        if (wrongAttemptCount >= autoFillAfterWrongCount)
//        {
//            AutoFillController_Set1.Instance?.EnableAutoFill(
//                activeSlot,
//                expected
//            );
//        }
//    }
//    public float GetExpectedValue()
//    {
//        switch (session.meanMode)
//        {
//            case MeasurementSession.MeanMode.SetMean:
//                return session.GetSetMean(session.currentSet);

//            case MeasurementSession.MeanMode.FinalMean:
//                return session.GetFinalMean();

//            default:
//                return session.currentGaugeValue;
//        }
//    }

//    public float GetExpectedValue()
//    {
//        switch (session.meanMode)
//        {
//            case MeasurementSession.MeanMode.SetMean:
//                return session.GetSetMean(session.currentSet);

//            case MeasurementSession.MeanMode.FinalMean:
//                return session.GetFinalMean();

//            default:
//                return session.currentGaugeValue;
//        }
//    }


//    // ─────────────────────────────────────────────
//    // SLIDE UNLOCK ROUTER (UNCHANGED)
//    // ─────────────────────────────────────────────
//    void NotifySlideValidationSuccess()
//    {
//        FindObjectOfType<Slide6Mechanism>()?.OnValidationResult(true);
//        FindObjectOfType<Slide9Mechanism>()?.OnValidationResult(true);
//        FindObjectOfType<Slide10Mechanism>()?.OnValidationResult(true);

//        FindObjectOfType<Slide13Mechanism>()?.OnValidationResult(true);
//        FindObjectOfType<Slide16Mechanism>()?.OnValidationResult(true);
//        FindObjectOfType<Slide17Mechanism>()?.OnValidationResult(true);

//        FindObjectOfType<Slide20Mechanism>()?.OnValidationResult(true);
//        FindObjectOfType<Slide23Mechanism>()?.OnValidationResult(true);
//        FindObjectOfType<Slide24Mechanism>()?.OnValidationResult(true);
//    }
//}
using UnityEngine;

public class ValidatorController : MonoBehaviour
{
    public MeasurementSession session;
    public NumberPadController numberPad;

    private InputSlotController activeSlot;

    [Header("Validation")]
    public float tolerance = 0.01f;

    [Header("Auto Fill Rules")]
    public int autoFillAfterWrongCount = 2;

    int wrongAttemptCount = 0;

    // ─────────────────────────────────────────────
    // SLOT MANAGEMENT
    // ─────────────────────────────────────────────
    public void SetActiveSlot(InputSlotController slot)
    {
        activeSlot = slot;

        if (slot != null && numberPad != null)
            numberPad.SetActiveInput(slot.input);

        wrongAttemptCount = 0;
    }

    // ─────────────────────────────────────────────
    // CHECK BUTTON (UNCHANGED LOGIC)
    // ─────────────────────────────────────────────
    public void OnCheckPressed()
    {
        if (activeSlot == null)
            return;

        activeSlot.ResetValidationState();

        if (!float.TryParse(activeSlot.input.text, out float entered))
        {
            HandleWrongAnswer();
            return;
        }

        float expected = GetExpectedValue();

        if (Mathf.Abs(entered - expected) > session.tolerance)
        {
            HandleWrongAnswer();
            return;
        }

        // ✅ SAVE VALUE (UNCHANGED)
        if (session.meanMode == MeasurementSession.MeanMode.FinalMean)
        {
            session.SaveFinalMean(activeSlot.input.text);
        }
        else
        {
            session.SaveReadingText(
                activeSlot.setIndex,
                activeSlot.stepIndex,
                activeSlot.input.text
            );
        }

        activeSlot.ShowCorrectWithSFX();
        activeSlot.MarkCompleted();

        activeSlot = null;
        wrongAttemptCount = 0;

        NotifySlideValidationSuccess();
    }

    // ─────────────────────────────────────────────
    // ✅ AUTO-FILL FORCE CORRECT (SAFE ADDITION)
    // ─────────────────────────────────────────────
    public void ForceAcceptCorrect(InputSlotController slot, float value)
    {
        if (slot == null) return;

        string text = value.ToString("F3");

        if (session.meanMode == MeasurementSession.MeanMode.FinalMean)
        {
            session.SaveFinalMean(text);
        }
        else
        {
            session.SaveReadingText(
                slot.setIndex,
                slot.stepIndex,
                text
            );
        }

        slot.ShowCorrectWithSFX();
        slot.MarkCompleted();

        activeSlot = null;
        wrongAttemptCount = 0;

        NotifySlideValidationSuccess();
    }

    // ─────────────────────────────────────────────
    // WRONG HANDLER (LOGIC PRESERVED)
    // ─────────────────────────────────────────────
    void HandleWrongAnswer()
    {
        wrongAttemptCount++;

        activeSlot.ShowWrongWithSFX();
        activeSlot.input.text = "";

        if (wrongAttemptCount >= autoFillAfterWrongCount)
        {
            AutoFillController_Set1.Instance?.EnableAutoFill(activeSlot);
        }
    }

    // ─────────────────────────────────────────────
    // EXPECTED VALUE (SINGLE SOURCE OF TRUTH)
    // ─────────────────────────────────────────────
    public float GetExpectedValue()
    {
        switch (session.meanMode)
        {
            case MeasurementSession.MeanMode.SetMean:
                return session.GetSetMean(session.currentSet);

            case MeasurementSession.MeanMode.FinalMean:
                return session.GetFinalMean();

            default:
                return session.currentGaugeValue;
        }
    }

    // ─────────────────────────────────────────────
    // SLIDE UNLOCK ROUTER (UNCHANGED)
    // ─────────────────────────────────────────────
    void NotifySlideValidationSuccess()
    {
        FindObjectOfType<Slide6Mechanism>()?.OnValidationResult(true);
        FindObjectOfType<Slide9Mechanism>()?.OnValidationResult(true);
        FindObjectOfType<Slide10Mechanism>()?.OnValidationResult(true);

        FindObjectOfType<Slide13Mechanism>()?.OnValidationResult(true);
        FindObjectOfType<Slide16Mechanism>()?.OnValidationResult(true);
        FindObjectOfType<Slide17Mechanism>()?.OnValidationResult(true);

        FindObjectOfType<Slide20Mechanism>()?.OnValidationResult(true);
        FindObjectOfType<Slide23Mechanism>()?.OnValidationResult(true);
        FindObjectOfType<Slide24Mechanism>()?.OnValidationResult(true);
    }
}
