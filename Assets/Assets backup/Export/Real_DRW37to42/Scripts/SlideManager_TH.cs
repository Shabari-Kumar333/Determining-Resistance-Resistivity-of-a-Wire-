////using UnityEngine;

////public class SlideManager_TH : MonoBehaviour
////{
////    [Header("Slide Anchors")]
////    public Transform[] slideAnchors;

////    [Header("Slide Panels")]
////    public GameObject[] slideUIs;

////    [Header("Camera")]
////    public CameraMovementControllers cameraMover;

////    int currentSlide = 1;
////    int totalSlides = 0;

////    void Start()
////    {
////        Init();
////        RefreshSlide();
////    }

////    void OnEnable()
////    {
////        Init();
////        RefreshSlide();
////    }

////    void Init()
////    {
////        totalSlides = (slideUIs != null) ? slideUIs.Length : 0;
////        currentSlide = Mathf.Clamp(currentSlide, 1, Mathf.Max(1, totalSlides));
////    }

////    // 🔹 CALLED BY GlobalSlideNavigation → SlideSetController
////    public void SetSlide(int index)
////    {
////        currentSlide = Mathf.Clamp(index + 1, 1, totalSlides);
////        RefreshSlide();
////    }

////    void RefreshSlide()
////    {
////        UpdateSlideUI();
////        RequestCameraMove();
////    }

////    void RequestCameraMove()
////    {
////        if (!cameraMover || slideAnchors == null) return;

////        int idx = currentSlide - 1;
////        if (idx < 0 || idx >= slideAnchors.Length) return;

////        cameraMover.MoveToTarget(slideAnchors[idx]);
////    }

////    void UpdateSlideUI()
////    {
////        if (slideUIs == null) return;

////        // 🔥 hard reset (prevents invisible UI bug)
////        for (int i = 0; i < slideUIs.Length; i++)
////        {
////            if (slideUIs[i])
////                slideUIs[i].SetActive(false);
////        }

////        int index = currentSlide - 1;
////        if (index >= 0 && index < slideUIs.Length && slideUIs[index])
////            slideUIs[index].SetActive(true);
////    }
////}
//using UnityEngine;

//public class SlideManager_TH : MonoBehaviour
//{
//    [Header("Slide Anchors")]
//    public Transform[] slideAnchors;

//    [Header("Slide Panels")]
//    public GameObject[] slideUIs;

//    [Header("Camera")]
//    public CameraMovementControllers cameraMover;

//    int currentSlide = 1;
//    int totalSlides = 0;

//    void Start()
//    {
//        InitOnce();
//    }

//    void InitOnce()
//    {
//        totalSlides = (slideUIs != null) ? slideUIs.Length : 0;
//        currentSlide = Mathf.Clamp(currentSlide, 1, Mathf.Max(1, totalSlides));
//        RefreshSlide();
//    }

//    // 🔹 CALLED BY GlobalSlideNavigation → SlideSetController
//    public void SetSlide(int index)
//    {
//        currentSlide = Mathf.Clamp(index + 1, 1, totalSlides);
//        RefreshSlide();
//    }

//    void RefreshSlide()
//    {
//        UpdateSlideUI();
//        RequestCameraMove();
//    }

//    void RequestCameraMove()
//    {
//        if (!cameraMover || slideAnchors == null) return;

//        int idx = currentSlide - 1;
//        if (idx < 0 || idx >= slideAnchors.Length) return;

//        cameraMover.MoveToTarget(slideAnchors[idx]);
//    }

//    void UpdateSlideUI()
//    {
//        if (slideUIs == null) return;

//        for (int i = 0; i < slideUIs.Length; i++)
//        {
//            if (slideUIs[i])
//                slideUIs[i].SetActive(false);
//        }

//        int index = currentSlide - 1;
//        if (index >= 0 && index < slideUIs.Length && slideUIs[index])
//            slideUIs[index].SetActive(true);
//    }
//}
using UnityEngine;

public class SlideManager_TH : MonoBehaviour
{
    [Header("Slide Anchors")]
    public Transform[] slideAnchors;

    [Header("Slide Panels")]
    public GameObject[] slideUIs;

    int currentSlide = 1;
    int totalSlides = 0;

    void Start()
    {
        InitOnce();
    }

    void InitOnce()
    {
        totalSlides = (slideUIs != null) ? slideUIs.Length : 0;
        currentSlide = Mathf.Clamp(currentSlide, 1, Mathf.Max(1, totalSlides));
        RefreshSlide();
    }

    // 🔹 CALLED BY GlobalSlideNavigation → SlideSetController
    public void SetSlide(int index)
    {
        currentSlide = Mathf.Clamp(index + 1, 1, totalSlides);
        RefreshSlide();
    }

    void RefreshSlide()
    {
        UpdateSlideUI();
        RequestCameraMove();
    }

    // 🔑 GLOBAL CAMERA REQUEST
    void RequestCameraMove()
    {
        if (GlobalCameraController.Instance == null) return;
        if (slideAnchors == null) return;

        int idx = currentSlide - 1;
        if (idx < 0 || idx >= slideAnchors.Length) return;

        GlobalCameraController.Instance.MoveTo(slideAnchors[idx]);
    }

    void UpdateSlideUI()
    {
        if (slideUIs == null) return;

        for (int i = 0; i < slideUIs.Length; i++)
        {
            if (slideUIs[i])
                slideUIs[i].SetActive(false);
        }

        int index = currentSlide - 1;
        if (index >= 0 && index < slideUIs.Length && slideUIs[index])
            slideUIs[index].SetActive(true);
    }
}
