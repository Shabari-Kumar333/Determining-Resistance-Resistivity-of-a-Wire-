
//using UnityEngine;
//using TMPro;

//public class FinalMeanValidator : MonoBehaviour
//{
//    [Header("References")]
//    public NumberPadController numberPad;
//    public TMP_InputField inputField;

//    [Header("Expected Final Mean")]
//    public float expectedFinalMean = 0.850f;

//    [Header("Icons")]
//    public GameObject correctIcon;
//    public GameObject wrongIcon;

//    [Header("Auto Fill")]
//    public GameObject autoFillButton;
//    public int autoFillAfterWrongCount = 2;

//    [Header("Validation")]
//    public float tolerance = 0.001f;

//    [Header("Audio")]
//    public AudioSource audioSource;
//    public AudioClip correctSFX;
//    public AudioClip wrongSFX;

//    bool isLocked = false;
//    int wrongAttempts = 0;

//    bool correctSfxPlayed = false;
//    bool wrongSfxPlayed = false;

//    // ─────────────────────────────────────────────
//    void OnEnable()
//    {
//        // 🔒 HARD ISOLATION
//        if (numberPad && inputField)
//        {
//            numberPad.SetActiveInput(null);
//            numberPad.SetActiveInput(inputField);
//        }

//        wrongAttempts = 0;
//        correctSfxPlayed = false;
//        wrongSfxPlayed = false;

//        if (autoFillButton)
//            autoFillButton.SetActive(false);

//        if (isLocked)
//        {
//            inputField.interactable = false;
//            ShowCorrect();
//        }
//        else
//        {
//            inputField.text = "";
//            inputField.interactable = true;
//            HideIcons();
//        }
//    }

//    // ─────────────────────────────────────────────
//    public void OnCheckPressed()
//    {
//        if (isLocked) return;

//        if (!float.TryParse(inputField.text, out float entered))
//        {
//            HandleWrong();
//            return;
//        }

//        if (Mathf.Abs(entered - expectedFinalMean) <= tolerance)
//        {
//            HandleCorrect();
//        }
//        else
//        {
//            HandleWrong();
//        }
//    }

//    // ─────────────────────────────────────────────
//    public void OnAutoFillPressed()
//    {
//        if (isLocked) return;

//        inputField.text = expectedFinalMean.ToString("F3");
//        HandleCorrect();
//    }

//    // ─────────────────────────────────────────────
//    void HandleCorrect()
//    {
//        isLocked = true;
//        inputField.interactable = false;

//        ShowCorrect();

//        if (!correctSfxPlayed)
//        {
//            PlayCorrectSFX();
//            correctSfxPlayed = true;
//        }

//        if (autoFillButton)
//            autoFillButton.SetActive(false);

//        SlideProgressManager.Instance.MarkCompleted(25);
//    }

//    void HandleWrong()
//    {
//        wrongAttempts++;
//        inputField.text = "";

//        ShowWrong();

//        if (!wrongSfxPlayed)
//        {
//            PlayWrongSFX();
//            wrongSfxPlayed = true;
//        }

//        if (wrongAttempts >= autoFillAfterWrongCount && autoFillButton)
//        {
//            autoFillButton.SetActive(true);
//        }
//    }

//    // ─────────────────────────────────────────────
//    void HideIcons()
//    {
//        if (correctIcon) correctIcon.SetActive(false);
//        if (wrongIcon) wrongIcon.SetActive(false);
//    }

//    void ShowCorrect()
//    {
//        if (correctIcon) correctIcon.SetActive(true);
//        if (wrongIcon) wrongIcon.SetActive(false);
//    }

//    void ShowWrong()
//    {
//        if (correctIcon) correctIcon.SetActive(false);
//        if (wrongIcon) wrongIcon.SetActive(true);
//    }

//    // ─────────────────────────────────────────────
//    // AUDIO
//    // ─────────────────────────────────────────────
//    void PlayCorrectSFX()
//    {
//        if (audioSource && correctSFX)
//            audioSource.PlayOneShot(correctSFX);
//    }

//    void PlayWrongSFX()
//    {
//        if (audioSource && wrongSFX)
//            audioSource.PlayOneShot(wrongSFX);
//    }
//}
using UnityEngine;
using TMPro;

public class FinalMeanValidator : MonoBehaviour
{
    [Header("References")]
    public NumberPadController numberPad;
    public TMP_InputField inputField;

    [Header("Expected Final Mean")]
    public float expectedFinalMean = 0.850f;

    [Header("Icons")]
    public GameObject correctIcon;
    public GameObject wrongIcon;

    [Header("Auto Fill")]
    public GameObject autoFillButton;
    public int autoFillAfterWrongCount = 2;

    [Header("Validation")]
    public float tolerance = 0.001f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip correctSFX;
    public AudioClip wrongSFX;

    bool isLocked = false;
    int wrongAttempts = 0;

    bool correctSfxPlayed = false;
    bool wrongSfxPlayed = false;

    // ─────────────────────────────────────────────
    void OnEnable()
    {
        if (numberPad && inputField)
        {
            numberPad.SetActiveInput(null);
            numberPad.SetActiveInput(inputField);
        }

        wrongAttempts = 0;

        // initial reset
        correctSfxPlayed = false;
        wrongSfxPlayed = false;

        if (autoFillButton)
            autoFillButton.SetActive(false);

        if (isLocked)
        {
            inputField.interactable = false;
            ShowCorrect();
        }
        else
        {
            inputField.text = "";
            inputField.interactable = true;
            HideIcons();
        }
    }

    // ─────────────────────────────────────────────
    public void OnCheckPressed()
    {
        if (isLocked) return;

        // ✅ CRITICAL FIX: reset SFX flags PER ATTEMPT
        correctSfxPlayed = false;
        wrongSfxPlayed = false;

        if (!float.TryParse(inputField.text, out float entered))
        {
            HandleWrong();
            return;
        }

        if (Mathf.Abs(entered - expectedFinalMean) <= tolerance)
        {
            HandleCorrect();
        }
        else
        {
            HandleWrong();
        }
    }

    // ─────────────────────────────────────────────
    public void OnAutoFillPressed()
    {
        if (isLocked) return;

        // reset guards for autofill path also
        correctSfxPlayed = false;
        wrongSfxPlayed = false;

        inputField.text = expectedFinalMean.ToString("F3");
        HandleCorrect();
    }

    // ─────────────────────────────────────────────
    void HandleCorrect()
    {
        isLocked = true;
        inputField.interactable = false;

        ShowCorrect();

        if (!correctSfxPlayed)
        {
            PlayCorrectSFX();
            correctSfxPlayed = true;
        }

        if (autoFillButton)
            autoFillButton.SetActive(false);

        SlideProgressManager.Instance.MarkCompleted(25);
    }

    void HandleWrong()
    {
        wrongAttempts++;
        inputField.text = "";

        ShowWrong();

        if (!wrongSfxPlayed)
        {
            PlayWrongSFX();
            wrongSfxPlayed = true;
        }

        if (wrongAttempts >= autoFillAfterWrongCount && autoFillButton)
        {
            autoFillButton.SetActive(true);
        }
    }

    // ─────────────────────────────────────────────
    void HideIcons()
    {
        if (correctIcon) correctIcon.SetActive(false);
        if (wrongIcon) wrongIcon.SetActive(false);
    }

    void ShowCorrect()
    {
        if (correctIcon) correctIcon.SetActive(true);
        if (wrongIcon) wrongIcon.SetActive(false);
    }

    void ShowWrong()
    {
        if (correctIcon) correctIcon.SetActive(false);
        if (wrongIcon) wrongIcon.SetActive(true);
    }

    // ─────────────────────────────────────────────
    // AUDIO
    // ─────────────────────────────────────────────
    void PlayCorrectSFX()
    {
        if (audioSource && correctSFX)
            audioSource.PlayOneShot(correctSFX);
    }

    void PlayWrongSFX()
    {
        if (audioSource && wrongSFX)
            audioSource.PlayOneShot(wrongSFX);
    }
}
