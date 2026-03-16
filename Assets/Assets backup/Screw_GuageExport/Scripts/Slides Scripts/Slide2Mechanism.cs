//////////////using UnityEngine;
//////////////using UnityEngine.UI;

//////////////public class Slide2Mechanism : MonoBehaviour
//////////////{
//////////////    // ================== SCREW GAUGE ==================
//////////////    [Header("Screw Gauge")]
//////////////    public ScrewGaugePositionController screwGaugeController;
//////////////    public Transform screwGaugeTarget;

//////////////    // ================== QUIZ ==================
//////////////    [System.Serializable]
//////////////    public class OptionData
//////////////    {
//////////////        public Button button;
//////////////        public Image indicator;
//////////////    }

//////////////    [Header("Quiz Options")]
//////////////    public OptionData[] options;

//////////////    [Header("Indicator Sprites")]
//////////////    public Sprite spriteBlue;
//////////////    public Sprite spriteGreen;
//////////////    public Sprite spriteRed;     // 🔥 WRONG indicator restored

//////////////    [Header("Correct Answer Index")]
//////////////    public int correctAnswerIndex = 2;

//////////////    // ================== WHY UI ==================
//////////////    [Header("Why Buttons")]
//////////////    public GameObject whyButtonGreen;   // shown if correct
//////////////    public GameObject whyButtonRed;     // shown if wrong

//////////////    [Header("Why Panels")]
//////////////    public GameObject whyCorrectPanel;
//////////////    public GameObject whyWrongPanel;

//////////////    private bool correctAnswered = false;

//////////////    // ================== LIFECYCLE ==================
//////////////    void OnEnable()
//////////////    {
//////////////        ResetQuiz();
//////////////        AssignListeners();

//////////////        if (screwGaugeController && screwGaugeTarget)
//////////////            screwGaugeController.MoveToTarget(screwGaugeTarget, true);
//////////////    }

//////////////    void OnDisable()
//////////////    {
//////////////        ResetWhyUI();

//////////////        if (screwGaugeController)
//////////////            screwGaugeController.RestoreOriginal();
//////////////    }

//////////////    // ================== SETUP ==================
//////////////    void AssignListeners()
//////////////    {
//////////////        foreach (var opt in options)
//////////////            opt.button.onClick.RemoveAllListeners();

//////////////        for (int i = 0; i < options.Length; i++)
//////////////        {
//////////////            int index = i;
//////////////            options[i].button.onClick.AddListener(() => OnOptionSelected(index));
//////////////        }
//////////////    }

//////////////    // ================== QUIZ LOGIC ==================
//////////////    void OnOptionSelected(int index)
//////////////    {
//////////////        if (correctAnswered)
//////////////            return;

//////////////        ResetWhyUI(); // hide previous why buttons/panels

//////////////        if (index == correctAnswerIndex)
//////////////        {
//////////////            // ✅ CORRECT
//////////////            options[index].indicator.sprite = spriteGreen;
//////////////            correctAnswered = true;

//////////////            whyButtonGreen.SetActive(true);

//////////////            // lock all options
//////////////            foreach (var opt in options)
//////////////                opt.button.interactable = false;
//////////////        }
//////////////        else
//////////////        {
//////////////            // ❌ WRONG
//////////////            options[index].indicator.sprite = spriteRed;

//////////////            whyButtonRed.SetActive(true);

//////////////            // disable only this wrong option
//////////////            options[index].button.interactable = false;
//////////////        }
//////////////    }

//////////////    // ================== WHY BUTTON CALLBACKS ==================
//////////////    public void OnWhyGreenPressed()
//////////////    {
//////////////        whyCorrectPanel.SetActive(true);
//////////////    }

//////////////    public void OnWhyRedPressed()
//////////////    {
//////////////        whyWrongPanel.SetActive(true);
//////////////    }

//////////////    // ================== RESET ==================
//////////////    void ResetQuiz()
//////////////    {
//////////////        correctAnswered = false;

//////////////        ResetWhyUI();

//////////////        foreach (var opt in options)
//////////////        {
//////////////            opt.indicator.sprite = spriteBlue;
//////////////            opt.button.interactable = true;
//////////////        }
//////////////    }

//////////////    void ResetWhyUI()
//////////////    {
//////////////        whyButtonGreen.SetActive(false);
//////////////        whyButtonRed.SetActive(false);

//////////////        whyCorrectPanel.SetActive(false);
//////////////        whyWrongPanel.SetActive(false);
//////////////    }
//////////////}
////////////using UnityEngine;
////////////using UnityEngine.UI;

////////////public class Slide2Mechanism : MonoBehaviour
////////////{
////////////    // ================== SCREW GAUGE ==================
////////////    [Header("Screw Gauge")]
////////////    public ScrewGaugePositionController screwGaugeController;
////////////    public Transform screwGaugeTarget;

////////////    // ================== QUIZ ==================
////////////    [System.Serializable]
////////////    public class OptionData
////////////    {
////////////        public Button button;
////////////        public Image indicator;
////////////    }

////////////    [Header("Quiz Options")]
////////////    public OptionData[] options;

////////////    [Header("Indicator Sprites")]
////////////    public Sprite spriteBlue;
////////////    public Sprite spriteGreen;
////////////    public Sprite spriteRed;

////////////    [Header("Correct Answer Index")]
////////////    public int correctAnswerIndex = 2;

////////////    // ================== WHY UI ==================
////////////    [Header("Why Buttons")]
////////////    public GameObject whyButtonGreen;
////////////    public GameObject whyButtonRed;

////////////    [Header("Why Panels")]
////////////    public GameObject whyCorrectPanel;
////////////    public GameObject whyWrongPanel;

////////////    private bool correctAnswered = false;

////////////    // ================== LIFECYCLE ==================
////////////    void OnEnable()
////////////    {
////////////        ResetQuiz();
////////////        AssignListeners();

////////////        if (screwGaugeController && screwGaugeTarget)
////////////            screwGaugeController.MoveToTarget(screwGaugeTarget, true);
////////////    }

////////////    void OnDisable()
////////////    {
////////////        ResetWhyUI();

////////////        if (screwGaugeController)
////////////            screwGaugeController.RestoreOriginal();
////////////    }

////////////    // ================== SETUP ==================
////////////    void AssignListeners()
////////////    {
////////////        foreach (var opt in options)
////////////            opt.button.onClick.RemoveAllListeners();

////////////        for (int i = 0; i < options.Length; i++)
////////////        {
////////////            int index = i;
////////////            options[i].button.onClick.AddListener(() => OnOptionSelected(index));
////////////        }
////////////    }

////////////    // ================== QUIZ LOGIC ==================
////////////    void OnOptionSelected(int index)
////////////    {
////////////        if (correctAnswered)
////////////            return;

////////////        ResetWhyUI();

////////////        if (index == correctAnswerIndex)
////////////        {
////////////            // ✅ CORRECT
////////////            options[index].indicator.sprite = spriteGreen;
////////////            correctAnswered = true;

////////////            whyButtonGreen.SetActive(true);

////////////            // 🔓 UNLOCK SLIDE 2
////////////            if (SlideProgressManager.Instance != null)
////////////                SlideProgressManager.Instance.MarkCompleted(2);

////////////            // lock all options
////////////            foreach (var opt in options)
////////////                opt.button.interactable = false;
////////////        }
////////////        else
////////////        {
////////////            // ❌ WRONG
////////////            options[index].indicator.sprite = spriteRed;

////////////            whyButtonRed.SetActive(true);

////////////            // disable only this wrong option
////////////            options[index].button.interactable = false;
////////////        }
////////////    }

////////////    // ================== WHY BUTTON CALLBACKS ==================
////////////    public void OnWhyGreenPressed()
////////////    {
////////////        whyCorrectPanel.SetActive(true);
////////////    }

////////////    public void OnWhyRedPressed()
////////////    {
////////////        whyWrongPanel.SetActive(true);
////////////    }

////////////    // ================== RESET ==================
////////////    void ResetQuiz()
////////////    {
////////////        correctAnswered = false;

////////////        ResetWhyUI();

////////////        foreach (var opt in options)
////////////        {
////////////            opt.indicator.sprite = spriteBlue;
////////////            opt.button.interactable = true;
////////////        }
////////////    }

////////////    void ResetWhyUI()
////////////    {
////////////        whyButtonGreen.SetActive(false);
////////////        whyButtonRed.SetActive(false);

////////////        whyCorrectPanel.SetActive(false);
////////////        whyWrongPanel.SetActive(false);
////////////    }
////////////}
//////////using UnityEngine;
//////////using UnityEngine.UI;

//////////public class Slide2Mechanism : MonoBehaviour
//////////{
//////////    // ================== SCREW GAUGE ==================
//////////    [Header("Screw Gauge")]
//////////    public ScrewGaugePositionController screwGaugeController;
//////////    public Transform screwGaugeTarget;

//////////    // ================== QUIZ ==================
//////////    [System.Serializable]
//////////    public class OptionData
//////////    {
//////////        public Button button;
//////////        public Image indicator;
//////////    }

//////////    [Header("Quiz Options")]
//////////    public OptionData[] options;

//////////    [Header("Indicator Sprites")]
//////////    public Sprite spriteBlue;
//////////    public Sprite spriteGreen;
//////////    public Sprite spriteRed;

//////////    [Header("Correct Answer Index")]
//////////    public int correctAnswerIndex = 2;

//////////    // ================== WHY UI ==================
//////////    [Header("Why Buttons")]
//////////    public GameObject whyButtonGreen;
//////////    public GameObject whyButtonRed;

//////////    [Header("Why Panels")]
//////////    public GameObject whyCorrectPanel;

//////////    [Tooltip("Order must match option index (except correct option)")]
//////////    public GameObject[] whyWrongPanels; // 3 panels for 3 wrong options

//////////    private bool correctAnswered = false;
//////////    private int lastWrongIndex = -1;

//////////    // ================== LIFECYCLE ==================
//////////    void OnEnable()
//////////    {
//////////        ResetQuiz();
//////////        AssignListeners();

//////////        if (screwGaugeController && screwGaugeTarget)
//////////            screwGaugeController.MoveToTarget(screwGaugeTarget, true);
//////////    }

//////////    void OnDisable()
//////////    {
//////////        ResetWhyUI();

//////////        if (screwGaugeController)
//////////            screwGaugeController.RestoreOriginal();
//////////    }

//////////    // ================== SETUP ==================
//////////    void AssignListeners()
//////////    {
//////////        foreach (var opt in options)
//////////            opt.button.onClick.RemoveAllListeners();

//////////        for (int i = 0; i < options.Length; i++)
//////////        {
//////////            int index = i;
//////////            options[i].button.onClick.AddListener(() => OnOptionSelected(index));
//////////        }
//////////    }

//////////    // ================== QUIZ LOGIC ==================
//////////    void OnOptionSelected(int index)
//////////    {
//////////        if (correctAnswered)
//////////            return;

//////////        ResetWhyUI();

//////////        if (index == correctAnswerIndex)
//////////        {
//////////            // ✅ CORRECT
//////////            options[index].indicator.sprite = spriteGreen;
//////////            correctAnswered = true;

//////////            whyButtonGreen.SetActive(true);

//////////            // 🔓 UNLOCK SLIDE 2
//////////            if (SlideProgressManager.Instance != null)
//////////                SlideProgressManager.Instance.MarkCompleted(2);

//////////            // lock all options
//////////            foreach (var opt in options)
//////////                opt.button.interactable = false;
//////////        }
//////////        else
//////////        {
//////////            // ❌ WRONG
//////////            options[index].indicator.sprite = spriteRed;
//////////            lastWrongIndex = index;

//////////            whyButtonRed.SetActive(true);

//////////            // disable only this wrong option
//////////            options[index].button.interactable = false;
//////////        }
//////////    }

//////////    // ================== WHY BUTTON CALLBACKS ==================
//////////    public void OnWhyGreenPressed()
//////////    {
//////////        whyCorrectPanel.SetActive(true);
//////////    }

//////////    public void OnWhyRedPressed()
//////////    {
//////////        if (lastWrongIndex < 0 || lastWrongIndex >= whyWrongPanels.Length)
//////////            return;

//////////        whyWrongPanels[lastWrongIndex].SetActive(true);
//////////    }

//////////    // ================== RESET ==================
//////////    void ResetQuiz()
//////////    {
//////////        correctAnswered = false;
//////////        lastWrongIndex = -1;

//////////        ResetWhyUI();

//////////        foreach (var opt in options)
//////////        {
//////////            opt.indicator.sprite = spriteBlue;
//////////            opt.button.interactable = true;
//////////        }
//////////    }

//////////    void ResetWhyUI()
//////////    {
//////////        whyButtonGreen.SetActive(false);
//////////        whyButtonRed.SetActive(false);

//////////        whyCorrectPanel.SetActive(false);

//////////        foreach (var panel in whyWrongPanels)
//////////            panel.SetActive(false);
//////////    }
//////////}
////////using UnityEngine;
////////using UnityEngine.UI;

////////public class Slide2Mechanism : MonoBehaviour
////////{
////////    // ================== SCREW GAUGE ==================
////////    [Header("Screw Gauge")]
////////    public ScrewGaugePositionController screwGaugeController;
////////    public Transform screwGaugeTarget;

////////    // ================== QUIZ ==================
////////    [System.Serializable]
////////    public class OptionData
////////    {
////////        [Header("UI")]
////////        public Button button;
////////        public Image indicator;

////////        [Header("Why Panels")]
////////        public GameObject whyWrongPanel;   // null for correct option
////////    }

////////    [Header("Quiz Options")]
////////    public OptionData[] options;

////////    [Header("Indicator Sprites")]
////////    public Sprite spriteBlue;
////////    public Sprite spriteGreen;
////////    public Sprite spriteRed;

////////    [Header("Correct Answer Index")]
////////    public int correctAnswerIndex = 2;

////////    // ================== WHY UI ==================
////////    [Header("Why Buttons")]
////////    public GameObject whyButtonGreen;
////////    public GameObject whyButtonRed;

////////    [Header("Why Correct Panel")]
////////    public GameObject whyCorrectPanel;

////////    private bool correctAnswered = false;
////////    private int lastWrongIndex = -1;

////////    // ================== LIFECYCLE ==================
////////    void OnEnable()
////////    {
////////        AssignListeners();

////////        if (screwGaugeController && screwGaugeTarget)
////////            screwGaugeController.MoveToTarget(screwGaugeTarget, true);

////////        // ✅ CHECK IF SLIDE ALREADY COMPLETED
////////        if (SlideProgressManager.Instance != null &&
////////            SlideProgressManager.Instance.IsCompleted(2))
////////        {
////////            ApplyCorrectState();
////////        }
////////        else
////////        {
////////            ResetQuiz();
////////        }
////////    }

////////    void OnDisable()
////////    {
////////        ResetWhyUI();

////////        if (screwGaugeController)
////////            screwGaugeController.RestoreOriginal();
////////    }

////////    // ================== SETUP ==================
////////    void AssignListeners()
////////    {
////////        for (int i = 0; i < options.Length; i++)
////////        {
////////            int index = i;
////////            options[i].button.onClick.RemoveAllListeners();
////////            options[i].button.onClick.AddListener(() => OnOptionSelected(index));
////////        }
////////    }

////////    // ================== QUIZ LOGIC ==================
////////    void OnOptionSelected(int index)
////////    {
////////        if (correctAnswered)
////////            return;

////////        ResetWhyUI();

////////        if (index == correctAnswerIndex)
////////        {
////////            // ✅ CORRECT
////////            ApplyCorrectState();

////////            if (SlideProgressManager.Instance != null)
////////                SlideProgressManager.Instance.MarkCompleted(2);
////////        }
////////        else
////////        {
////////            // ❌ WRONG
////////            options[index].indicator.sprite = spriteRed;
////////            lastWrongIndex = index;

////////            whyButtonRed.SetActive(true);
////////            options[index].button.interactable = false;
////////        }
////////    }

////////    // ================== APPLY CORRECT STATE ==================
////////    void ApplyCorrectState()
////////    {
////////        correctAnswered = true;

////////        // Set indicators
////////        for (int i = 0; i < options.Length; i++)
////////        {
////////            options[i].indicator.sprite =
////////                (i == correctAnswerIndex) ? spriteGreen : spriteBlue;

////////            options[i].button.interactable = false;
////////        }

////////        whyButtonGreen.SetActive(true);
////////        whyButtonRed.SetActive(false);
////////    }

////////    // ================== WHY BUTTON CALLBACKS ==================
////////    public void OnWhyGreenPressed()
////////    {
////////        whyCorrectPanel.SetActive(true);
////////    }

////////    public void OnWhyRedPressed()
////////    {
////////        if (lastWrongIndex < 0)
////////            return;

////////        GameObject wrongPanel = options[lastWrongIndex].whyWrongPanel;
////////        if (wrongPanel != null)
////////            wrongPanel.SetActive(true);
////////    }

////////    // ================== RESET ==================
////////    void ResetQuiz()
////////    {
////////        correctAnswered = false;
////////        lastWrongIndex = -1;

////////        ResetWhyUI();

////////        foreach (var opt in options)
////////        {
////////            opt.indicator.sprite = spriteBlue;
////////            opt.button.interactable = true;
////////        }
////////    }

////////    void ResetWhyUI()
////////    {
////////        whyButtonGreen.SetActive(false);
////////        whyButtonRed.SetActive(false);

////////        whyCorrectPanel.SetActive(false);

////////        foreach (var opt in options)
////////        {
////////            if (opt.whyWrongPanel != null)
////////                opt.whyWrongPanel.SetActive(false);
////////        }
////////    }
////////}
//////using UnityEngine;
//////using UnityEngine.UI;

//////public class Slide2Mechanism : MonoBehaviour
//////{
//////    [System.Serializable]
//////    public class OptionData
//////    {
//////        public Button optionButton;
//////        public Image indicator;

//////        [Header("Why Button (shown on selection)")]
//////        public GameObject whyButton;   // Unique per option

//////        [Header("Why Panel (opened by that button)")]
//////        public GameObject whyPanel;    // Unique per option
//////    }

//////    [Header("Quiz Options")]
//////    public OptionData[] options;

//////    [Header("Sprites")]
//////    public Sprite spriteBlue;
//////    public Sprite spriteGreen;
//////    public Sprite spriteRed;

//////    [Header("Correct Answer Index")]
//////    public int correctAnswerIndex;

//////    bool answeredCorrectly = false;

//////    // ─────────────────────────────────────────────
//////    void OnEnable()
//////    {
//////        ResetQuiz();
//////        AssignListeners();
//////    }

//////    // ─────────────────────────────────────────────
//////    void AssignListeners()
//////    {
//////        for (int i = 0; i < options.Length; i++)
//////        {
//////            int index = i;
//////            options[i].optionButton.onClick.RemoveAllListeners();
//////            options[i].optionButton.onClick.AddListener(() => OnOptionSelected(index));
//////        }
//////    }

//////    // ─────────────────────────────────────────────
//////    void OnOptionSelected(int index)
//////    {
//////        if (answeredCorrectly)
//////            return;

//////        HideAllWhyButtons();

//////        if (index == correctAnswerIndex)
//////        {
//////            // ✅ CORRECT
//////            options[index].indicator.sprite = spriteGreen;
//////            options[index].whyButton.SetActive(true);

//////            answeredCorrectly = true;

//////            if (SlideProgressManager.Instance != null)
//////                SlideProgressManager.Instance.MarkCompleted(2);

//////            LockAllOptions();
//////        }
//////        else
//////        {
//////            // ❌ WRONG
//////            options[index].indicator.sprite = spriteRed;
//////            options[index].whyButton.SetActive(true);

//////            options[index].optionButton.interactable = false;
//////        }
//////    }

//////    // ─────────────────────────────────────────────
//////    void LockAllOptions()
//////    {
//////        foreach (var opt in options)
//////            opt.optionButton.interactable = false;
//////    }

//////    // ─────────────────────────────────────────────
//////    void HideAllWhyButtons()
//////    {
//////        foreach (var opt in options)
//////            opt.whyButton.SetActive(false);
//////    }

//////    // ─────────────────────────────────────────────
//////    public void OpenWhyPanel(GameObject panel)
//////    {
//////        panel.SetActive(true);
//////    }

//////    public void CloseWhyPanel(GameObject panel)
//////    {
//////        panel.SetActive(false);
//////    }

//////    // ─────────────────────────────────────────────
//////    void ResetQuiz()
//////    {
//////        answeredCorrectly = false;

//////        foreach (var opt in options)
//////        {
//////            opt.indicator.sprite = spriteBlue;
//////            opt.optionButton.interactable = true;
//////            opt.whyButton.SetActive(false);
//////            opt.whyPanel.SetActive(false);
//////        }
//////    }
//////}
////using UnityEngine;
////using UnityEngine.UI;

////public class Slide2Mechanism : MonoBehaviour
////{
////    [System.Serializable]
////    public class OptionData
////    {
////        [Header("Option UI")]
////        public Button optionButton;
////        public Image indicator;

////        [Header("Why Button (enable only this one)")]
////        public GameObject whyButton;   // Assign ONE unique Why button here
////    }

////    [Header("Quiz Options")]
////    public OptionData[] options;

////    [Header("Indicator Sprites")]
////    public Sprite spriteBlue;
////    public Sprite spriteGreen;
////    public Sprite spriteRed;

////    [Header("Correct Answer Index")]
////    public int correctAnswerIndex;

////    private bool answeredCorrectly = false;

////    // ─────────────────────────────────────────────
////    void OnEnable()
////    {
////        ResetQuiz();
////        AssignListeners();
////    }

////    // ─────────────────────────────────────────────
////    void AssignListeners()
////    {
////        for (int i = 0; i < options.Length; i++)
////        {
////            int index = i;
////            options[i].optionButton.onClick.RemoveAllListeners();
////            options[i].optionButton.onClick.AddListener(() => OnOptionSelected(index));
////        }
////    }

////    // ─────────────────────────────────────────────
////    void OnOptionSelected(int index)
////    {
////        if (answeredCorrectly)
////            return;

////        HideAllWhyButtons();

////        if (index == correctAnswerIndex)
////        {
////            // ✅ CORRECT
////            options[index].indicator.sprite = spriteGreen;
////            options[index].whyButton.SetActive(true);

////            answeredCorrectly = true;

////            if (SlideProgressManager.Instance != null)
////                SlideProgressManager.Instance.MarkCompleted(2);

////            LockAllOptions();
////        }
////        else
////        {
////            // ❌ WRONG
////            options[index].indicator.sprite = spriteRed;
////            options[index].whyButton.SetActive(true);

////            options[index].optionButton.interactable = false;
////        }
////    }

////    // ─────────────────────────────────────────────
////    void HideAllWhyButtons()
////    {
////        foreach (var opt in options)
////        {
////            if (opt.whyButton != null)
////                opt.whyButton.SetActive(false);
////        }
////    }

////    // ─────────────────────────────────────────────
////    void LockAllOptions()
////    {
////        foreach (var opt in options)
////            opt.optionButton.interactable = false;
////    }

////    // ─────────────────────────────────────────────
////    void ResetQuiz()
////    {
////        answeredCorrectly = false;

////        foreach (var opt in options)
////        {
////            opt.indicator.sprite = spriteBlue;
////            opt.optionButton.interactable = true;

////            if (opt.whyButton != null)
////                opt.whyButton.SetActive(false);
////        }
////    }
////}
//using UnityEngine;
//using UnityEngine.UI;

//public class Slide2Mechanism : MonoBehaviour
//{
//    [System.Serializable]
//    public class OptionData
//    {
//        [Header("Option UI")]
//        public Button optionButton;
//        public Image indicator;

//        [Header("Why Button (enable only this one)")]
//        public GameObject whyButton;
//    }

//    [Header("Quiz Options")]
//    public OptionData[] options;

//    [Header("Indicator Sprites")]
//    public Sprite spriteBlue;
//    public Sprite spriteGreen;
//    public Sprite spriteRed;

//    [Header("Correct Answer Index")]
//    public int correctAnswerIndex;

//    private bool answeredCorrectly = false;
//    private const int SLIDE_INDEX = 2; // 🔴 IMPORTANT: slide number

//    // ─────────────────────────────────────────────
//    void OnEnable()
//    {
//        AssignListeners();

//        // ✅ IF USER COMES BACK TO COMPLETED SLIDE
//        if (SlideProgressManager.Instance != null &&
//            SlideProgressManager.Instance.IsCompleted(SLIDE_INDEX))
//        {
//            ApplyCompletedState();
//        }
//        else
//        {
//            ResetQuiz();
//        }
//    }

//    // ─────────────────────────────────────────────
//    void AssignListeners()
//    {
//        for (int i = 0; i < options.Length; i++)
//        {
//            int index = i;
//            options[i].optionButton.onClick.RemoveAllListeners();
//            options[i].optionButton.onClick.AddListener(() => OnOptionSelected(index));
//        }
//    }

//    // ─────────────────────────────────────────────
//    void OnOptionSelected(int index)
//    {
//        if (answeredCorrectly)
//            return;

//        HideAllWhyButtons();

//        if (index == correctAnswerIndex)
//        {
//            // ✅ CORRECT
//            options[index].indicator.sprite = spriteGreen;
//            options[index].whyButton.SetActive(true);

//            answeredCorrectly = true;

//            if (SlideProgressManager.Instance != null)
//                SlideProgressManager.Instance.MarkCompleted(SLIDE_INDEX);

//            LockAllOptions();
//        }
//        else
//        {
//            // ❌ WRONG
//            options[index].indicator.sprite = spriteRed;
//            options[index].whyButton.SetActive(true);

//            options[index].optionButton.interactable = false;
//        }
//    }

//    // ─────────────────────────────────────────────
//    void ApplyCompletedState()
//    {
//        answeredCorrectly = true;

//        HideAllWhyButtons();

//        for (int i = 0; i < options.Length; i++)
//        {
//            options[i].indicator.sprite =
//                (i == correctAnswerIndex) ? spriteGreen : spriteBlue;

//            options[i].optionButton.interactable = false;
//        }

//        // show correct why button only
//        options[correctAnswerIndex].whyButton.SetActive(true);
//    }

//    // ─────────────────────────────────────────────
//    void HideAllWhyButtons()
//    {
//        foreach (var opt in options)
//        {
//            if (opt.whyButton != null)
//                opt.whyButton.SetActive(false);
//        }
//    }

//    // ─────────────────────────────────────────────
//    void LockAllOptions()
//    {
//        foreach (var opt in options)
//            opt.optionButton.interactable = false;
//    }

//    // ─────────────────────────────────────────────
//    void ResetQuiz()
//    {
//        answeredCorrectly = false;

//        foreach (var opt in options)
//        {
//            opt.indicator.sprite = spriteBlue;
//            opt.optionButton.interactable = true;

//            if (opt.whyButton != null)
//                opt.whyButton.SetActive(false);
//        }
//    }
//}
using UnityEngine;
using UnityEngine.UI;

public class Slide2Mechanism : MonoBehaviour
{
    // ================== SCREW GAUGE ==================
    [Header("Screw Gauge")]
    public ScrewGaugePositionController screwGaugeController;
    public Transform screwGaugeTarget;

    // ================== QUIZ ==================
    [System.Serializable]
    public class OptionData
    {
        [Header("Option UI")]
        public Button optionButton;
        public Image indicator;

        [Header("Why Button (enable only this one)")]
        public GameObject whyButton;
    }

    [Header("Quiz Options")]
    public OptionData[] options;

    [Header("Indicator Sprites")]
    public Sprite spriteBlue;
    public Sprite spriteGreen;
    public Sprite spriteRed;

    [Header("Correct Answer Index")]
    public int correctAnswerIndex;

    private bool answeredCorrectly = false;
    private const int SLIDE_INDEX = 2;

    // ─────────────────────────────────────────────
    void OnEnable()
    {
        AssignListeners();

        // ✅ MOVE SCREW GAUGE INTO POSITION
        if (screwGaugeController && screwGaugeTarget)
            screwGaugeController.MoveToTarget(screwGaugeTarget, true);

        // ✅ RESTORE COMPLETED STATE IF NEEDED
        if (SlideProgressManager.Instance != null &&
            SlideProgressManager.Instance.IsCompleted(SLIDE_INDEX))
        {
            ApplyCompletedState();
        }
        else
        {
            ResetQuiz();
        }
    }

    // ─────────────────────────────────────────────
    void OnDisable()
    {
        // 🔁 RESTORE SCREW GAUGE WHEN LEAVING SLIDE
        if (screwGaugeController)
            screwGaugeController.RestoreOriginal();
    }

    // ─────────────────────────────────────────────
    void AssignListeners()
    {
        for (int i = 0; i < options.Length; i++)
        {
            int index = i;
            options[i].optionButton.onClick.RemoveAllListeners();
            options[i].optionButton.onClick.AddListener(() => OnOptionSelected(index));
        }
    }

    // ─────────────────────────────────────────────
    void OnOptionSelected(int index)
    {
        if (answeredCorrectly)
            return;

        HideAllWhyButtons();

        if (index == correctAnswerIndex)
        {
            // ✅ CORRECT
            options[index].indicator.sprite = spriteGreen;
            options[index].whyButton.SetActive(true);

            answeredCorrectly = true;

            if (SlideProgressManager.Instance != null)
                SlideProgressManager.Instance.MarkCompleted(SLIDE_INDEX);

            LockAllOptions();
        }
        else
        {
            // ❌ WRONG
            options[index].indicator.sprite = spriteRed;
            options[index].whyButton.SetActive(true);

            options[index].optionButton.interactable = false;
        }
    }

    // ─────────────────────────────────────────────
    void ApplyCompletedState()
    {
        answeredCorrectly = true;
        HideAllWhyButtons();

        for (int i = 0; i < options.Length; i++)
        {
            options[i].indicator.sprite =
                (i == correctAnswerIndex) ? spriteGreen : spriteBlue;

            options[i].optionButton.interactable = false;
        }

        // Show correct Why button only
        options[correctAnswerIndex].whyButton.SetActive(true);
    }

    // ─────────────────────────────────────────────
    void HideAllWhyButtons()
    {
        foreach (var opt in options)
        {
            if (opt.whyButton != null)
                opt.whyButton.SetActive(false);
        }
    }

    // ─────────────────────────────────────────────
    void LockAllOptions()
    {
        foreach (var opt in options)
            opt.optionButton.interactable = false;
    }

    // ─────────────────────────────────────────────
    void ResetQuiz()
    {
        answeredCorrectly = false;

        foreach (var opt in options)
        {
            opt.indicator.sprite = spriteBlue;
            opt.optionButton.interactable = true;

            if (opt.whyButton != null)
                opt.whyButton.SetActive(false);
        }
    }
}
