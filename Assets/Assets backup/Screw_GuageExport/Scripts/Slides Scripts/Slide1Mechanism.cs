
////using UnityEngine;
////using System.Collections.Generic;
////using UnityEngine.InputSystem;

////public class Slide1Mechanism : MonoBehaviour
////{
////    [Header("DEV / BUILD OVERRIDE")]
////    public bool autoCompleteSlide = false;

////    // ─────────────────────────────────────────────
////    // ELEMENT-WISE SETUP
////    // ─────────────────────────────────────────────
////    [System.Serializable]
////    public class SelectionElement
////    {
////        public GameObject selectableObject;
////        public bool isCorrect;
////        public GameObject correctIndicator;
////        public GameObject wrongIndicator;
////    }

////    [Header("Selectable Elements")]
////    public List<SelectionElement> elements;

////    [Header("Audio")]
////    public AudioSource audioSource;
////    public AudioClip correctSFX;
////    public AudioClip wrongSFX;

////    bool inputEnabled = false;
////    int globalSlide;

////    // ─────────────────────────────────────────────
////    void OnEnable()
////    {
////        if (GlobalSlideNavigation.Instance == null) return;

////        globalSlide = GlobalSlideNavigation.Instance.currentSlide;

////        ResetAllIndicators();

////        // 🔥 DEV AUTO COMPLETE
////        if (autoCompleteSlide)
////        {
////            AutoComplete();
////            return;
////        }

////        // 🔁 COMING BACK TO SLIDE (IMPORTANT FIX)
////        if (SlideProgressManager.Instance != null &&
////            SlideProgressManager.Instance.IsCompleted(globalSlide))
////        {
////            inputEnabled = false;
////            ShowCorrectIndicatorIfCompleted();
////            return;
////        }

////        // 🆕 FIRST TIME ENTRY
////        inputEnabled = true;
////    }

////    void OnDisable()
////    {
////        inputEnabled = false;
////    }

////    // ─────────────────────────────────────────────
////    void Update()
////    {
////        if (!inputEnabled) return;

////        if (Mouse.current?.leftButton.wasPressedThisFrame == true)
////            DetectClick(Mouse.current.position.ReadValue());

////        if (Touchscreen.current?.primaryTouch.press.wasPressedThisFrame == true)
////            DetectClick(Touchscreen.current.primaryTouch.position.ReadValue());
////    }

////    // ─────────────────────────────────────────────
////    void DetectClick(Vector2 screenPos)
////    {
////        Ray ray = Camera.main.ScreenPointToRay(screenPos);
////        if (!Physics.Raycast(ray, out RaycastHit hit)) return;

////        foreach (var element in elements)
////        {
////            if (hit.collider.gameObject == element.selectableObject)
////            {
////                CheckAnswer(element);
////                break;
////            }
////        }
////    }

////    // ─────────────────────────────────────────────
////    void CheckAnswer(SelectionElement element)
////    {
////        ResetAllIndicators();

////        // ✅ CORRECT
////        if (element.isCorrect)
////        {
////            if (element.correctIndicator)
////                element.correctIndicator.SetActive(true);

////            if (audioSource && correctSFX)
////                audioSource.PlayOneShot(correctSFX);

////            if (SlideProgressManager.Instance != null)
////                SlideProgressManager.Instance.MarkCompleted(globalSlide);

////            inputEnabled = false;
////        }
////        // ❌ WRONG
////        else
////        {
////            if (element.wrongIndicator)
////            {
////                element.wrongIndicator.SetActive(true);
////                Invoke(nameof(HideAllWrongIndicators), 3f);
////            }

////            if (audioSource && wrongSFX)
////                audioSource.PlayOneShot(wrongSFX);
////        }
////    }

////    // ─────────────────────────────────────────────
////    void ResetAllIndicators()
////    {
////        foreach (var e in elements)
////        {
////            if (e.correctIndicator)
////                e.correctIndicator.SetActive(false);

////            if (e.wrongIndicator)
////                e.wrongIndicator.SetActive(false);
////        }
////    }

////    void HideAllWrongIndicators()
////    {
////        foreach (var e in elements)
////        {
////            if (e.wrongIndicator)
////                e.wrongIndicator.SetActive(false);
////        }
////    }

////    // ✅ ONLY SHOW WHEN SLIDE IS COMPLETED
////    void ShowCorrectIndicatorIfCompleted()
////    {
////        if (SlideProgressManager.Instance == null ||
////            !SlideProgressManager.Instance.IsCompleted(globalSlide))
////            return;

////        foreach (var e in elements)
////        {
////            if (e.isCorrect && e.correctIndicator)
////                e.correctIndicator.SetActive(true);
////        }
////    }

////    // ─────────────────────────────────────────────
////    void AutoComplete()
////    {
////        if (SlideProgressManager.Instance != null)
////            SlideProgressManager.Instance.MarkCompleted(globalSlide);

////        ShowCorrectIndicatorIfCompleted();
////        inputEnabled = false;
////    }
////}
//using UnityEngine;
//using System.Collections.Generic;
//using UnityEngine.InputSystem;

//public class Slide1Mechanism : MonoBehaviour
//{
//    [Header("DEV / BUILD OVERRIDE")]
//    public bool autoCompleteSlide = false;

//    // ─────────────────────────────────────────────
//    [System.Serializable]
//    public class SelectionElement
//    {
//        public GameObject selectableObject;
//        public bool isCorrect;
//        public GameObject correctIndicator;
//        public GameObject wrongIndicator;
//    }

//    [Header("Selectable Elements")]
//    public List<SelectionElement> elements;

//    [Header("Audio")]
//    public AudioSource audioSource;
//    public AudioClip correctSFX;
//    public AudioClip wrongSFX;

//    bool inputEnabled = false;
//    int globalSlide;

//    // ─────────────────────────────────────────────
//    void OnEnable()
//    {
//        if (GlobalSlideNavigation.Instance == null) return;

//        globalSlide = GlobalSlideNavigation.Instance.currentSlide;

//        // 🔁 Coming back → show correct ONLY if completed
//        if (SlideProgressManager.Instance != null &&
//            SlideProgressManager.Instance.IsCompleted(globalSlide))
//        {
//            ShowCorrectIndicatorIfCompleted();
//            inputEnabled = false;
//        }
//        else
//        {
//            ResetAllIndicators();
//            inputEnabled = true;
//        }
//    }

//    // 🔴 IMPORTANT: hide indicators when leaving slide
//    void OnDisable()
//    {
//        inputEnabled = false;
//        ResetAllIndicators();
//    }

//    // ─────────────────────────────────────────────
//    void Update()
//    {
//        if (!inputEnabled) return;

//        if (Mouse.current?.leftButton.wasPressedThisFrame == true)
//            DetectClick(Mouse.current.position.ReadValue());

//        if (Touchscreen.current?.primaryTouch.press.wasPressedThisFrame == true)
//            DetectClick(Touchscreen.current.primaryTouch.position.ReadValue());
//    }

//    // ─────────────────────────────────────────────
//    void DetectClick(Vector2 screenPos)
//    {
//        Ray ray = Camera.main.ScreenPointToRay(screenPos);
//        if (!Physics.Raycast(ray, out RaycastHit hit)) return;

//        foreach (var element in elements)
//        {
//            if (hit.collider.gameObject == element.selectableObject)
//            {
//                CheckAnswer(element);
//                break;
//            }
//        }
//    }

//    // ─────────────────────────────────────────────
//    void CheckAnswer(SelectionElement element)
//    {
//        // ❌ DO NOT show correct indicator here
//        ResetAllIndicators();

//        if (element.isCorrect)
//        {
//            if (audioSource && correctSFX)
//                audioSource.PlayOneShot(correctSFX);

//            // ✔ Mark completed ONLY
//            if (SlideProgressManager.Instance != null)
//                SlideProgressManager.Instance.MarkCompleted(globalSlide);

//            inputEnabled = false;
//        }
//        else
//        {
//            if (element.wrongIndicator)
//            {
//                element.wrongIndicator.SetActive(true);
//                Invoke(nameof(HideAllWrongIndicators), 3f);
//            }

//            if (audioSource && wrongSFX)
//                audioSource.PlayOneShot(wrongSFX);
//        }
//    }

//    // ─────────────────────────────────────────────
//    void ResetAllIndicators()
//    {
//        foreach (var e in elements)
//        {
//            if (e.correctIndicator)
//                e.correctIndicator.SetActive(false);

//            if (e.wrongIndicator)
//                e.wrongIndicator.SetActive(false);
//        }
//    }

//    void HideAllWrongIndicators()
//    {
//        foreach (var e in elements)
//        {
//            if (e.wrongIndicator)
//                e.wrongIndicator.SetActive(false);
//        }
//    }

//    // ✅ SHOW ONLY ON RETURN
//    void ShowCorrectIndicatorIfCompleted()
//    {
//        foreach (var e in elements)
//        {
//            if (e.isCorrect && e.correctIndicator)
//                e.correctIndicator.SetActive(true);
//        }
//    }
//}
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Slide1Mechanism : MonoBehaviour
{
    [Header("DEV / BUILD OVERRIDE")]
    public bool autoCompleteSlide = false;

    [System.Serializable]
    public class SelectionElement
    {
        public GameObject selectableObject;
        public bool isCorrect;
        public GameObject correctIndicator;
        public GameObject wrongIndicator;
    }

    [Header("Selectable Elements")]
    public List<SelectionElement> elements;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip correctSFX;
    public AudioClip wrongSFX;

    bool inputEnabled = false;
    int globalSlide;

    // ─────────────────────────────────────────────
    void OnEnable()
    {
        if (GlobalSlideNavigation.Instance == null) return;

        globalSlide = GlobalSlideNavigation.Instance.currentSlide;

        // 🔁 WHEN SLIDE BECOMES ACTIVE
        if (SlideProgressManager.Instance != null &&
            SlideProgressManager.Instance.IsCompleted(globalSlide))
        {
            // Slide already solved → show correct indicator
            ShowCorrectIndicator();
            inputEnabled = false;
        }
        else
        {
            // Fresh entry
            ResetAllIndicators();
            inputEnabled = true;
        }
    }

    // 🔴 HIDE INDICATORS WHEN LEAVING SLIDE
    void OnDisable()
    {
        inputEnabled = false;
        ResetAllIndicators();
    }

    // ─────────────────────────────────────────────
    void Update()
    {
        if (!inputEnabled) return;

        if (Mouse.current?.leftButton.wasPressedThisFrame == true)
            DetectClick(Mouse.current.position.ReadValue());

        if (Touchscreen.current?.primaryTouch.press.wasPressedThisFrame == true)
            DetectClick(Touchscreen.current.primaryTouch.position.ReadValue());
    }

    // ─────────────────────────────────────────────
    void DetectClick(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        if (!Physics.Raycast(ray, out RaycastHit hit)) return;

        foreach (var element in elements)
        {
            if (hit.collider.gameObject == element.selectableObject)
            {
                CheckAnswer(element);
                break;
            }
        }
    }

    // ─────────────────────────────────────────────
    void CheckAnswer(SelectionElement element)
    {
        ResetAllIndicators();

        if (element.isCorrect)
        {
            // ✅ SHOW CORRECT IMMEDIATELY
            if (element.correctIndicator)
                element.correctIndicator.SetActive(true);

            if (audioSource && correctSFX)
                audioSource.PlayOneShot(correctSFX);

            if (SlideProgressManager.Instance != null)
                SlideProgressManager.Instance.MarkCompleted(globalSlide);

            inputEnabled = false;
        }
        else
        {
            if (element.wrongIndicator)
            {
                element.wrongIndicator.SetActive(true);
                Invoke(nameof(HideAllWrongIndicators), 3f);
            }

            if (audioSource && wrongSFX)
                audioSource.PlayOneShot(wrongSFX);
        }
    }

    // ─────────────────────────────────────────────
    void ShowCorrectIndicator()
    {
        foreach (var e in elements)
        {
            if (e.isCorrect && e.correctIndicator)
                e.correctIndicator.SetActive(true);
        }
    }

    void ResetAllIndicators()
    {
        foreach (var e in elements)
        {
            if (e.correctIndicator)
                e.correctIndicator.SetActive(false);

            if (e.wrongIndicator)
                e.wrongIndicator.SetActive(false);
        }
    }

    void HideAllWrongIndicators()
    {
        foreach (var e in elements)
        {
            if (e.wrongIndicator)
                e.wrongIndicator.SetActive(false);
        }
    }
}
