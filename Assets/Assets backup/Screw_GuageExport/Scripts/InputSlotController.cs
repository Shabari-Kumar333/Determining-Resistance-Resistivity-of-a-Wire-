
//using UnityEngine;
//using TMPro;
//using UnityEngine.UI;
//using System.Collections;
//using System.Globalization;

//public class InputSlotController : MonoBehaviour
//{
//    [Header("Identity")]
//    public int setIndex;
//    public int stepIndex;

//    [Header("UI")]
//    public TMP_InputField input;
//    public Image correctIcon;
//    public Image wrongIcon;

//    [Header("Audio")]
//    public AudioSource audioSource;
//    public AudioClip correctSFX;
//    public AudioClip wrongSFX;

//    public ValidatorController validator;

//    Coroutine hideRoutine;

//    bool successSfxPlayed = false;
//    bool wrongSfxPlayed = false;

//    bool isCompleted = false;   // 🔒 permanent state ONLY

//    void Awake()
//    {
//        input.interactable = false;
//        HideIcons();
//    }

//    // ─────────────── UNLOCK (for typing)
//    public void Unlock()
//    {
//        if (isCompleted) return;   // 🔒 never unlock completed slot

//        input.interactable = true;
//        input.text = "";

//        ResetValidationState();
//        validator.SetActiveSlot(this);
//        input.ForceLabelUpdate(); // 🔥 TMP refresh
//    }

//    // ─────────────── TEMP LOCK (Refresh)
//    public void ForceLock()
//    {
//        input.interactable = false;   // ❗ DO NOT touch isCompleted
//    }

//    // ─────────────── PERMANENT LOCK (Correct)
//    public void MarkCompleted()
//    {
//        isCompleted = true;
//        input.interactable = false;
//        ShowCorrectWithSFX();
//    }

//    public void ResetValidationState()
//    {
//        successSfxPlayed = false;
//        wrongSfxPlayed = false;

//        if (hideRoutine != null)
//            StopCoroutine(hideRoutine);

//        HideIcons();
//    }

//    void ShowCorrectUI()
//    {
//        correctIcon.gameObject.SetActive(true);
//        wrongIcon.gameObject.SetActive(false);
//        RestartHideTimer();
//    }

//    void ShowWrongUI()
//    {
//        wrongIcon.gameObject.SetActive(true);
//        correctIcon.gameObject.SetActive(false);
//        RestartHideTimer();
//    }

//    void HideIcons()
//    {
//        correctIcon.gameObject.SetActive(false);
//        wrongIcon.gameObject.SetActive(false);
//    }

//    public void ShowCorrectWithSFX()
//    {
//        if (successSfxPlayed) return;
//        successSfxPlayed = true;
//        ShowCorrectUI();
//        if (audioSource && correctSFX) audioSource.PlayOneShot(correctSFX);
//    }

//    public void ShowWrongWithSFX()
//    {
//        if (wrongSfxPlayed) return;
//        wrongSfxPlayed = true;
//        ShowWrongUI();
//        if (audioSource && wrongSFX) audioSource.PlayOneShot(wrongSFX);
//    }

//    void RestartHideTimer()
//    {
//        hideRoutine = StartCoroutine(HideIconsAfterDelay());
//    }

//    IEnumerator HideIconsAfterDelay()
//    {
//        yield return new WaitForSecondsRealtime(3f);
//        HideIcons();
//    }

//    public float GetValue()
//    {
//        float.TryParse(input.text, NumberStyles.Float,
//            CultureInfo.InvariantCulture, out float v);
//        return v;
//    }
//}
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Globalization;

public class InputSlotController : MonoBehaviour
{
    [Header("Identity")]
    public int setIndex;
    public int stepIndex;

    [Header("UI")]
    public TMP_InputField input;
    public Image correctIcon;
    public Image wrongIcon;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip correctSFX;
    public AudioClip wrongSFX;

    public ValidatorController validator;

    Coroutine hideRoutine;

    bool successSfxPlayed = false;
    bool wrongSfxPlayed = false;
    bool isCompleted = false;
    bool keepCorrectVisible = false;

    void Awake()
    {
        input.interactable = false;
        HideIcons();
    }

    public void Unlock()
    {
        if (isCompleted) return;

        input.interactable = true;
        input.text = "";

        ResetValidationState();
        validator.SetActiveSlot(this);
        input.ForceLabelUpdate();
    }

    public void ForceLock()
    {
        input.interactable = false;
    }

    //public void MarkCompleted()
    //{
    //    isCompleted = true;
    //    keepCorrectVisible = true;
    //    input.interactable = false;
    //    ShowCorrectWithSFX();
    //}
    public void MarkCompleted()
    {
        isCompleted = true;
        input.interactable = false;

        // Stop any running hide coroutine
        if (hideRoutine != null)
        {
            StopCoroutine(hideRoutine);
            hideRoutine = null;
        }

        correctIcon.gameObject.SetActive(true);
        wrongIcon.gameObject.SetActive(false);
    }

    //public void ResetValidationState()
    //{
    //    successSfxPlayed = false;
    //    wrongSfxPlayed = false;

    //    if (hideRoutine != null)
    //        StopCoroutine(hideRoutine);

    //    HideIcons();
    //}
    public void ResetValidationState()
    {
        if (isCompleted) return;   // 🛑 DO NOT RESET A CORRECT SLOT

        successSfxPlayed = false;
        wrongSfxPlayed = false;

        if (hideRoutine != null)
        {
            StopCoroutine(hideRoutine);
            hideRoutine = null;
        }

        HideIcons();
    }


    
    void ShowCorrectUI()
    {
        correctIcon.gameObject.SetActive(true);
        wrongIcon.gameObject.SetActive(false);
        // ❌ DO NOT hide correct icon
    }


    void ShowWrongUI()
    {
        wrongIcon.gameObject.SetActive(true);
        correctIcon.gameObject.SetActive(false);
        RestartHideTimer(); // ✅ wrong should auto-hide
    }


    //void HideIcons()
    //{
    //    correctIcon.gameObject.SetActive(false);
    //    wrongIcon.gameObject.SetActive(false);
    //}
    void HideIcons()
    {
        if (isCompleted) return;   // 🛑 CRITICAL FIX

        correctIcon.gameObject.SetActive(false);
        wrongIcon.gameObject.SetActive(false);
    }

    public bool IsCompleted()
    {
        return isCompleted;
    }

    public void ShowCorrectWithSFX()
    {
        if (successSfxPlayed) return;

        successSfxPlayed = true;
        ShowCorrectUI();

        if (audioSource && correctSFX)
            audioSource.PlayOneShot(correctSFX);
    }

    public void ShowWrongWithSFX()
    {
        if (wrongSfxPlayed) return;

        wrongSfxPlayed = true;
        ShowWrongUI();

        if (audioSource && wrongSFX)
            audioSource.PlayOneShot(wrongSFX);
    }
    void RestartHideTimer()
    {
        if (isCompleted) return;   // 🛑 NEVER HIDE AFTER CORRECT

        if (hideRoutine != null)
            StopCoroutine(hideRoutine);

        hideRoutine = StartCoroutine(HideIconsAfterDelay());
    }


    IEnumerator HideIconsAfterDelay()
    {
        yield return new WaitForSecondsRealtime(3f);
        HideIcons();
    }

    public float GetValue()
    {
        float.TryParse(input.text, NumberStyles.Float,
            CultureInfo.InvariantCulture, out float v);
        return v;
    }
}
