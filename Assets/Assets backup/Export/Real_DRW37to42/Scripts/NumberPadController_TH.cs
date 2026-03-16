
////////using UnityEngine;
////////using TMPro;
////////using UnityEngine.UI;

////////public class NumPadController_TH : MonoBehaviour
////////{
////////    [Header("UI Connections")]
////////    public TMP_Text inputDisplay;
////////    public GameObject correctObject;
////////    public GameObject wrongObject;

////////    [Header("Settings")]
////////    public string correctCode = "10";
////////    public int maxLimit = 5;

////////    [Header("Audio")]
////////    public AudioSource audioSource;
////////    public AudioClip correctSFX;
////////    public AudioClip wrongSFX;

////////    private string currentInput = "";

////////    void Start()
////////    {
////////        currentInput = "";
////////        UpdateDisplay();

////////        if (correctObject != null) correctObject.SetActive(false);
////////        if (wrongObject != null) wrongObject.SetActive(false);
////////    }

////////    public void AddCharacter(string number)
////////    {
////////        ResetFeedback();

////////        if (currentInput.Length < maxLimit)
////////        {
////////            currentInput += number;
////////            UpdateDisplay();
////////        }
////////    }

////////    public void Backspace()
////////    {
////////        ResetFeedback();

////////        if (currentInput.Length > 0)
////////        {
////////            currentInput = currentInput.Substring(0, currentInput.Length - 1);
////////            UpdateDisplay();
////////        }
////////    }

////////    public void AutoFill()
////////    {
////////        ResetFeedback();
////////        currentInput = "10";
////////        UpdateDisplay();
////////        ValidateAnswer();
////////    }

////////    public void ValidateAnswer()
////////    {
////////        Debug.Log($"Checking: {currentInput} vs {correctCode}");

////////        if (currentInput.Trim() == correctCode)
////////        {
////////            if (correctObject) correctObject.SetActive(true);
////////            if (wrongObject) wrongObject.SetActive(false);

////////            PlayCorrectSFX();
////////            Debug.Log("Result: CORRECT");
////////        }
////////        else
////////        {
////////            if (correctObject) correctObject.SetActive(false);
////////            if (wrongObject) wrongObject.SetActive(true);

////////            PlayWrongSFX();
////////            Debug.Log("Result: WRONG");
////////        }
////////    }

////////    void ResetFeedback()
////////    {
////////        if (correctObject) correctObject.SetActive(false);
////////        if (wrongObject) wrongObject.SetActive(false);
////////    }

////////    void UpdateDisplay()
////////    {
////////        if (inputDisplay != null)
////////            inputDisplay.text = currentInput;
////////    }

////////    void PlayCorrectSFX()
////////    {
////////        if (audioSource && correctSFX)
////////            audioSource.PlayOneShot(correctSFX);
////////    }

////////    void PlayWrongSFX()
////////    {
////////        if (audioSource && wrongSFX)
////////            audioSource.PlayOneShot(wrongSFX);
////////    }
////////}
//////using UnityEngine;
//////using TMPro;
//////using UnityEngine.UI;

//////public class NumPadController_TH : MonoBehaviour
//////{
//////    [Header("LOCK SETTINGS")]
//////    public int slideIndex = 0;   // 🔑 SET THIS IN INSPECTOR

//////    [Header("UI Connections")]
//////    public TMP_Text inputDisplay;
//////    public GameObject correctObject;
//////    public GameObject wrongObject;

//////    [Header("Settings")]
//////    public string correctCode = "10";
//////    public int maxLimit = 5;

//////    [Header("Audio")]
//////    public AudioSource audioSource;
//////    public AudioClip correctSFX;
//////    public AudioClip wrongSFX;

//////    private string currentInput = "";
//////    private bool completed = false;

//////    void Start()
//////    {
//////        currentInput = "";
//////        UpdateDisplay();

//////        if (correctObject) correctObject.SetActive(false);
//////        if (wrongObject) wrongObject.SetActive(false);
//////    }

//////    public void AddCharacter(string number)
//////    {
//////        if (completed) return;

//////        ResetFeedback();

//////        if (currentInput.Length < maxLimit)
//////        {
//////            currentInput += number;
//////            UpdateDisplay();
//////        }
//////    }

//////    public void Backspace()
//////    {
//////        if (completed) return;

//////        ResetFeedback();

//////        if (currentInput.Length > 0)
//////        {
//////            currentInput = currentInput.Substring(0, currentInput.Length - 1);
//////            UpdateDisplay();
//////        }
//////    }

//////    public void AutoFill()
//////    {
//////        if (completed) return;

//////        ResetFeedback();
//////        currentInput = correctCode;
//////        UpdateDisplay();
//////        ValidateAnswer();
//////    }

//////    public void ValidateAnswer()
//////    {
//////        if (completed) return;

//////        Debug.Log($"Checking: {currentInput} vs {correctCode}");

//////        if (currentInput.Trim() == correctCode)
//////        {
//////            completed = true;

//////            if (correctObject) correctObject.SetActive(true);
//////            if (wrongObject) wrongObject.SetActive(false);

//////            PlayCorrectSFX();

//////            Debug.Log("Result: CORRECT");

//////            // 🔓 UNLOCK SLIDE
//////            SlideProgressManager.Instance.MarkCompleted(slideIndex);
//////        }
//////        else
//////        {
//////            if (correctObject) correctObject.SetActive(false);
//////            if (wrongObject) wrongObject.SetActive(true);

//////            PlayWrongSFX();
//////            Debug.Log("Result: WRONG");
//////        }
//////    }

//////    void ResetFeedback()
//////    {
//////        if (correctObject) correctObject.SetActive(false);
//////        if (wrongObject) wrongObject.SetActive(false);
//////    }

//////    void UpdateDisplay()
//////    {
//////        if (inputDisplay)
//////            inputDisplay.text = currentInput;
//////    }

//////    void PlayCorrectSFX()
//////    {
//////        if (audioSource && correctSFX)
//////            audioSource.PlayOneShot(correctSFX);
//////    }

//////    void PlayWrongSFX()
//////    {
//////        if (audioSource && wrongSFX)
//////            audioSource.PlayOneShot(wrongSFX);
//////    }
//////}
////using UnityEngine;
////using TMPro;
////using UnityEngine.UI;

////public class NumPadController_TH : MonoBehaviour
////{
////    [Header("UI Connections")]
////    public TMP_Text inputDisplay;
////    public GameObject correctObject;
////    public GameObject wrongObject;

////    [Header("Settings")]
////    public string correctCode = "10";
////    public int maxLimit = 5;

////    [Header("Audio")]
////    public AudioSource audioSource;
////    public AudioClip correctSFX;
////    public AudioClip wrongSFX;

////    private string currentInput = "";
////    private bool completed = false;

////    void OnEnable()
////    {
////        // Reset every time slide opens
////        completed = false;
////        currentInput = "";
////        UpdateDisplay();

////        if (correctObject) correctObject.SetActive(false);
////        if (wrongObject) wrongObject.SetActive(false);
////    }

////    // ===============================
////    // INPUT HANDLING
////    // ===============================
////    public void AddCharacter(string number)
////    {
////        if (completed) return;

////        ResetFeedback();

////        if (currentInput.Length < maxLimit)
////        {
////            currentInput += number;
////            UpdateDisplay();
////        }
////    }

////    public void Backspace()
////    {
////        if (completed) return;

////        ResetFeedback();

////        if (currentInput.Length > 0)
////        {
////            currentInput = currentInput.Substring(0, currentInput.Length - 1);
////            UpdateDisplay();
////        }
////    }

////    public void AutoFill()
////    {
////        if (completed) return;

////        ResetFeedback();
////        currentInput = correctCode;
////        UpdateDisplay();
////        ValidateAnswer();
////    }

////    // ===============================
////    // CHECK BUTTON
////    // ===============================
////    public void ValidateAnswer()
////    {
////        if (completed) return;

////        Debug.Log($"[NUMPAD] Checking: {currentInput} vs {correctCode}");

////        if (currentInput.Trim() == correctCode)
////        {
////            completed = true;

////            if (correctObject) correctObject.SetActive(true);
////            if (wrongObject) wrongObject.SetActive(false);

////            PlayCorrectSFX();

////            // 🔓 UNLOCK USING GLOBAL SLIDE INDEX (CRITICAL FIX)
////            if (GlobalSlideNavigation.Instance != null &&
////                SlideProgressManager.Instance != null)
////            {
////                int globalSlide = GlobalSlideNavigation.Instance.currentSlide;
////                SlideProgressManager.Instance.MarkCompleted(globalSlide);

////                Debug.Log($"✅ Slide {globalSlide} unlocked by NumPad");
////            }
////            else
////            {
////                Debug.LogError("❌ GlobalSlideNavigation or SlideProgressManager missing");
////            }
////        }
////        else
////        {
////            if (correctObject) correctObject.SetActive(false);
////            if (wrongObject) wrongObject.SetActive(true);

////            PlayWrongSFX();

////            Debug.Log("❌ NumPad answer WRONG");
////        }
////    }

////    // ===============================
////    // UI HELPERS
////    // ===============================
////    void ResetFeedback()
////    {
////        if (correctObject) correctObject.SetActive(false);
////        if (wrongObject) wrongObject.SetActive(false);
////    }

////    void UpdateDisplay()
////    {
////        if (inputDisplay)
////            inputDisplay.text = currentInput;
////    }

////    // ===============================
////    // AUDIO
////    // ===============================
////    void PlayCorrectSFX()
////    {
////        if (audioSource && correctSFX)
////            audioSource.PlayOneShot(correctSFX);
////    }

////    void PlayWrongSFX()
////    {
////        if (audioSource && wrongSFX)
////            audioSource.PlayOneShot(wrongSFX);
////    }
////}
//using UnityEngine;
//using TMPro;
//using UnityEngine.UI;

//public class NumPadController_TH : MonoBehaviour
//{
//    [Header("UI Connections")]
//    public TMP_Text inputDisplay;
//    public GameObject correctObject;
//    public GameObject wrongObject;

//    [Header("Settings")]
//    public string correctCode = "10";
//    public int maxLimit = 5;

//    [Header("Audio")]
//    public AudioSource audioSource;
//    public AudioClip correctSFX;
//    public AudioClip wrongSFX;

//    string currentInput = "";
//    bool completed = false;

//    string saveKey; // 🔑 UNIQUE KEY PER SLIDE

//    // ===============================
//    // SLIDE ENABLE
//    // ===============================
//    void OnEnable()
//    {
//        completed = false;

//        int slide = GlobalSlideNavigation.Instance.currentSlide;
//        saveKey = $"NUMPAD_VALUE_SLIDE_{slide}";

//        // 🔁 RESTORE IF ALREADY COMPLETED
//        if (PlayerPrefs.HasKey(saveKey))
//        {
//            currentInput = PlayerPrefs.GetString(saveKey);
//            completed = true;

//            UpdateDisplay();
//            ShowCorrect();

//            Debug.Log($"🔁 Restored NumPad value for slide {slide}: {currentInput}");
//        }
//        else
//        {
//            currentInput = "";
//            UpdateDisplay();
//            HideIcons();
//        }
//    }

//    // ===============================
//    // INPUT HANDLING
//    // ===============================
//    public void AddCharacter(string number)
//    {
//        if (completed) return;

//        ResetFeedback();

//        if (currentInput.Length < maxLimit)
//        {
//            currentInput += number;
//            UpdateDisplay();
//        }
//    }

//    public void Backspace()
//    {
//        if (completed) return;

//        ResetFeedback();

//        if (currentInput.Length > 0)
//        {
//            currentInput = currentInput.Substring(0, currentInput.Length - 1);
//            UpdateDisplay();
//        }
//    }

//    public void AutoFill()
//    {
//        if (completed) return;

//        ResetFeedback();
//        currentInput = correctCode;
//        UpdateDisplay();
//        ValidateAnswer();
//    }

//    // ===============================
//    // CHECK BUTTON
//    // ===============================
//    public void ValidateAnswer()
//    {
//        if (completed) return;

//        Debug.Log($"[NUMPAD] Checking: {currentInput} vs {correctCode}");

//        if (currentInput.Trim() == correctCode)
//        {
//            completed = true;

//            // ✅ STORE VALUE
//            PlayerPrefs.SetString(saveKey, currentInput);
//            PlayerPrefs.Save();

//            ShowCorrect();
//            PlayCorrectSFX();

//            // 🔓 UNLOCK CURRENT GLOBAL SLIDE
//            int globalSlide = GlobalSlideNavigation.Instance.currentSlide;
//            SlideProgressManager.Instance.MarkCompleted(globalSlide);

//            Debug.Log($"✅ Slide {globalSlide} unlocked & value stored");
//        }
//        else
//        {
//            ShowWrong();
//            PlayWrongSFX();

//            Debug.Log("❌ NumPad answer WRONG");
//        }
//    }

//    // ===============================
//    // UI HELPERS
//    // ===============================
//    void UpdateDisplay()
//    {
//        if (inputDisplay)
//            inputDisplay.text = currentInput;
//    }

//    void ResetFeedback()
//    {
//        HideIcons();
//    }

//    void HideIcons()
//    {
//        if (correctObject) correctObject.SetActive(false);
//        if (wrongObject) wrongObject.SetActive(false);
//    }

//    void ShowCorrect()
//    {
//        if (correctObject) correctObject.SetActive(true);
//        if (wrongObject) wrongObject.SetActive(false);
//    }

//    void ShowWrong()
//    {
//        if (correctObject) correctObject.SetActive(false);
//        if (wrongObject) wrongObject.SetActive(true);
//    }

//    // ===============================
//    // AUDIO
//    // ===============================
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
using UnityEngine.UI;

public class NumPadController_TH : MonoBehaviour
{
    // ===============================
    // UI CONNECTIONS
    // ===============================
    [Header("UI Connections")]
    public TMP_Text inputDisplay;
    public GameObject correctObject;
    public GameObject wrongObject;

    [Header("Hint / AutoFill")]
    public GameObject autoFillButton;

    // ===============================
    // SETTINGS
    // ===============================
    [Header("Settings")]
    public string correctCode = "10";
    public int maxLimit = 5;

    // ===============================
    // AUDIO
    // ===============================
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip correctSFX;
    public AudioClip wrongSFX;

    // ===============================
    // INTERNAL STATE
    // ===============================
    string currentInput = "";
    bool completed = false;

    // ===============================
    // SLIDE ENTER (RESET STATE)
    // ===============================
    void OnEnable()
    {
        completed = false;
        currentInput = "";

        UpdateDisplay();
        HideIcons();
        HideAutoFill();
    }
    void HideAutoFill()
    {
        if (autoFillButton)
            autoFillButton.SetActive(false);
    }
    void ShowAutoFill()
    {
        if (autoFillButton)
            autoFillButton.SetActive(true);
    }


    // ===============================
    // INPUT HANDLING
    // ===============================
    public void AddCharacter(string number)
    {
        if (completed) return;

        ResetFeedback();

        if (currentInput.Length < maxLimit)
        {
            currentInput += number;
            UpdateDisplay();
        }
    }

    public void Backspace()
    {
        if (completed) return;

        ResetFeedback();

        if (currentInput.Length > 0)
        {
            currentInput = currentInput.Substring(0, currentInput.Length - 1);
            UpdateDisplay();
        }
    }

    public void AutoFill()
    {
        if (completed) return;

        currentInput = correctCode;
        UpdateDisplay();

        HideAutoFill();   // 👈 hide button again
        ValidateAnswer(); // this will lock & unlock slide
    }


    // ===============================
    // CHECK BUTTON
    // ===============================
    public void ValidateAnswer()
    {
        if (completed) return;

        Debug.Log($"[NUMPAD] Checking: {currentInput} vs {correctCode}");

        if (currentInput.Trim() == correctCode)
        {
            completed = true;

            ShowCorrect();
            PlayCorrectSFX();

            // 🔓 UNLOCK CURRENT SLIDE
            if (GlobalSlideNavigation.Instance != null &&
                SlideProgressManager.Instance != null)
            {
                int slide = GlobalSlideNavigation.Instance.currentSlide;
                SlideProgressManager.Instance.MarkCompleted(slide);

                Debug.Log($"✅ Slide {slide} unlocked");
            }
            else
            {
                Debug.LogError("❌ Slide managers missing");
            }
        }
        else
        {
            ShowWrong();
            PlayWrongSFX();

            ShowAutoFill(); // 👈 SHOW AUTOFILL AFTER WRONG

            Debug.Log("❌ NumPad answer WRONG");
        }

    }

    // ===============================
    // UI HELPERS
    // ===============================
    void UpdateDisplay()
    {
        if (inputDisplay)
            inputDisplay.text = currentInput;
    }

    void ResetFeedback()
    {
        HideIcons();
    }

    void HideIcons()
    {
        if (correctObject) correctObject.SetActive(false);
        if (wrongObject) wrongObject.SetActive(false);
    }

    void ShowCorrect()
    {
        if (correctObject) correctObject.SetActive(true);
        if (wrongObject) wrongObject.SetActive(false);
    }

    void ShowWrong()
    {
        if (correctObject) correctObject.SetActive(false);
        if (wrongObject) wrongObject.SetActive(true);
    }

    // ===============================
    // AUDIO
    // ===============================
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
