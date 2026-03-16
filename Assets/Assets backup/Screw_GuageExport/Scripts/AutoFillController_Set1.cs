//////using UnityEngine;
//////using TMPro;

//////public class AutoFillController_Set1 : MonoBehaviour
//////{
//////    public static AutoFillController_Set1 Instance;

//////    [Header("UI")]
//////    public GameObject autoFillButton;

//////    // ─────────────────────────────────────────────
//////    // ACTIVE INPUT STATE
//////    // ─────────────────────────────────────────────
//////    private TMP_InputField activeInput;
//////    private InputSlotController activeSlot;

//////    private float correctValue;

//////    void Awake()
//////    {
//////        if (Instance == null)
//////            Instance = this;
//////        else
//////            Destroy(gameObject);

//////        autoFillButton.SetActive(false);
//////    }

//////    // =====================================================
//////    // REGISTER INPUT (FROM Set1InputBinder)
//////    // =====================================================
//////    public void RegisterInput(
//////        TMP_InputField input,
//////        int setIndex,
//////        int stepIndex
//////    )
//////    {
//////        activeInput = input;
//////        Debug.Log($"[AUTO FILL] Registered TMP_InputField (Set {setIndex}, Step {stepIndex})");
//////    }

//////    // =====================================================
//////    // ✅ OVERLOAD #1 (OLD – KEEP)
//////    // =====================================================
//////    public void EnableAutoFill(float expected)
//////    {
//////        correctValue = expected;
//////        autoFillButton.SetActive(true);
//////    }

//////    // =====================================================
//////    // ✅ OVERLOAD #2 (NEW – FIXES YOUR ERROR)
//////    // CALLED FROM ValidatorController
//////    // =====================================================
//////    public void EnableAutoFill(float expected, InputSlotController slot)
//////    {
//////        activeSlot = slot;
//////        activeInput = slot.input;

//////        correctValue = expected;

//////        Debug.Log($"[AUTO FILL] Enabled for slot Set={slot.setIndex} Step={slot.stepIndex}");

//////        autoFillButton.SetActive(true);
//////    }

//////    // =====================================================
//////    // AUTO FILL BUTTON CLICK
//////    // =====================================================
//////    public void OnAutoFillPressed()
//////    {
//////        if (activeInput == null || activeSlot == null)
//////        {
//////            Debug.LogWarning("[AUTO FILL] No active slot!");
//////            return;
//////        }

//////        // 1️⃣ Fill correct value
//////        activeInput.text = correctValue.ToString("F3");

//////        // 2️⃣ Run normal validation (NO DUPLICATION)
//////        ValidatorController validator =
//////            FindObjectOfType<ValidatorController>();

//////        if (validator != null)
//////            validator.OnCheckPressed();

//////        // 3️⃣ Hide button
//////        autoFillButton.SetActive(false);
//////    }

//////    // =====================================================
//////    // SAFETY RESET
//////    // =====================================================
//////    public void Hide()
//////    {
//////        autoFillButton.SetActive(false);
//////        activeInput = null;
//////        activeSlot = null;
//////    }
//////    // =====================================================
//////    // ✅ OVERLOAD #3 (FIXES PARAMETER ORDER ERROR)
//////    // =====================================================
//////    public void EnableAutoFill(InputSlotController slot, float expected)
//////    {
//////        EnableAutoFill(expected, slot);
//////    }

//////}
////using UnityEngine;
////using TMPro;

////public class AutoFillController_Set1 : MonoBehaviour
////{
////    public static AutoFillController_Set1 Instance;

////    [Header("UI")]
////    public GameObject autoFillButton;

////    // ─────────────────────────────────────────────
////    // ACTIVE INPUT STATE
////    // ─────────────────────────────────────────────
////    private TMP_InputField activeInput;
////    private InputSlotController activeSlot;

////    private float correctValue;

////    void Awake()
////    {
////        if (Instance == null)
////            Instance = this;
////        else
////            Destroy(gameObject);

////        autoFillButton.SetActive(false);
////    }

////    // =====================================================
////    // ✅ RESTORED — REQUIRED BY Set1InputBinder
////    // =====================================================
////    public void RegisterInput(
////        TMP_InputField input,
////        int setIndex,
////        int stepIndex
////    )
////    {
////        activeInput = input;
////        Debug.Log($"[AUTO FILL] Registered input (Set {setIndex}, Step {stepIndex})");
////    }

////    // =====================================================
////    // OLD OVERLOAD (KEEP)
////    // =====================================================
////    public void EnableAutoFill(float expected)
////    {
////        correctValue = expected;
////        autoFillButton.SetActive(true);
////    }

////    // =====================================================
////    // NEW OVERLOAD (Validator → AutoFill)
////    // =====================================================
////    public void EnableAutoFill(float expected, InputSlotController slot)
////    {
////        activeSlot = slot;
////        activeInput = slot.input;
////        correctValue = expected;

////        Debug.Log($"[AUTO FILL] Enabled for slot Set={slot.setIndex} Step={slot.stepIndex}");

////        autoFillButton.SetActive(true);
////    }

////    // =====================================================
////    // PARAM ORDER SAFETY
////    // =====================================================
////    public void EnableAutoFill(InputSlotController slot, float expected)
////    {
////        EnableAutoFill(expected, slot);
////    }

////    // =====================================================
////    // AUTO-FILL BUTTON CLICK
////    // =====================================================
////    public void OnAutoFillPressed()
////    {
////        if (activeInput == null || activeSlot == null)
////        {
////            Debug.LogWarning("[AUTO FILL] No active slot!");
////            return;
////        }

////        // 1️⃣ Insert correct value
////        activeInput.text = correctValue.ToString("F3");
////        activeInput.ForceLabelUpdate();

////        // 2️⃣ FORCE correct (no validation path)
////        ValidatorController validator =
////            FindObjectOfType<ValidatorController>();

////        if (validator != null)
////            validator.ForceAcceptCorrect(activeSlot, correctValue);

////        // 3️⃣ Hide AutoFill button
////        autoFillButton.SetActive(false);
////    }

////    // =====================================================
////    // SAFETY RESET
////    // =====================================================
////    public void Hide()
////    {
////        autoFillButton.SetActive(false);
////        activeInput = null;
////        activeSlot = null;
////    }
////}
//using UnityEngine;
//using TMPro;

//public class AutoFillController_Set1 : MonoBehaviour
//{
//    public static AutoFillController_Set1 Instance;

//    [Header("UI")]
//    public GameObject autoFillButton;

//    TMP_InputField activeInput;
//    InputSlotController activeSlot;

//    void Awake()
//    {
//        if (Instance == null) Instance = this;
//        else Destroy(gameObject);

//        autoFillButton.SetActive(false);
//    }
//    public void ForceAcceptCorrect(InputSlotController slot, float value)
//    {
//        // Save value
//        if (session.meanMode == MeasurementSession.MeanMode.FinalMean)
//            session.SaveFinalMean(value.ToString("F3"));
//        else
//            session.SaveReadingText(slot.setIndex, slot.stepIndex, value.ToString("F3"));

//        // UI + SFX
//        slot.ShowCorrectWithSFX();
//        slot.MarkCompleted();

//        NotifySlideValidationSuccess();
//    }

//    // CALLED FROM VALIDATOR
//    public void EnableAutoFill(InputSlotController slot)
//    {
//        activeSlot = slot;
//        activeInput = slot.input;
//        autoFillButton.SetActive(true);
//    }

//    public void OnAutoFillPressed()
//    {
//        if (activeSlot == null) return;

//        // 🔥 GET REAL EXPECTED VALUE AT PRESS TIME
//        ValidatorController validator = FindObjectOfType<ValidatorController>();
//        float correctValue = validator.GetExpectedValue();

//        // 1️⃣ Fill correct value
//        activeInput.text = correctValue.ToString("F3");

//        // 2️⃣ FORCE ACCEPT (NO VALIDATION)
//        validator.ForceAcceptCorrect(activeSlot, correctValue);

//        // 3️⃣ Cleanup
//        autoFillButton.SetActive(false);
//        activeSlot = null;
//        activeInput = null;
//    }
//}
using UnityEngine;
using TMPro;

public class AutoFillController_Set1 : MonoBehaviour
{
    public static AutoFillController_Set1 Instance;

    [Header("UI")]
    public GameObject autoFillButton;

    TMP_InputField activeInput;
    InputSlotController activeSlot;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        autoFillButton.SetActive(false);
    }

    // 🔁 REQUIRED BY Set1InputBinder (DO NOT REMOVE)
    public void RegisterInput(TMP_InputField input, int setIndex, int stepIndex)
    {
        activeInput = input;
    }

    // 🔁 OLD CALL STYLE (KEEP)
    public void EnableAutoFill(float expected)
    {
        autoFillButton.SetActive(true);
    }

    // 🔁 OLD CALL STYLE (KEEP)
    public void EnableAutoFill(InputSlotController slot, float expected)
    {
        EnableAutoFill(slot);
    }

    // ✅ NEW CORRECT PATH
    public void EnableAutoFill(InputSlotController slot)
    {
        activeSlot = slot;
        activeInput = slot.input;
        autoFillButton.SetActive(true);
    }

    // ▶ BUTTON CLICK
    public void OnAutoFillPressed()
    {
        if (activeSlot == null) return;

        ValidatorController validator =
            FindObjectOfType<ValidatorController>();

        if (validator == null) return;

        // 🔥 Get correct value NOW (not earlier)
        float correctValue = validator.GetExpectedValue();

        // Fill
        activeInput.text = correctValue.ToString("F3");

        // Force accept (NO validation)
        validator.ForceAcceptCorrect(activeSlot, correctValue);

        autoFillButton.SetActive(false);
        activeSlot = null;
        activeInput = null;
    }

    public void Hide()
    {
        autoFillButton.SetActive(false);
        activeSlot = null;
        activeInput = null;
    }
}
