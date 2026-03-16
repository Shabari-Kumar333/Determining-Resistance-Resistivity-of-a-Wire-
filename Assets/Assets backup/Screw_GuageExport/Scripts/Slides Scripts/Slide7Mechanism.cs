//using UnityEngine;

//public class Slide7Mechanism : MonoBehaviour, ISlideRotateHandler
//{
//    public int slideIndex = 7;

//    public SharedUIController sharedUI;
//    public WirePoseController wire;

//    public Transform pos2;   // rotation target

//    void OnEnable()
//    {
//        // 🔑 SAME PATTERN AS SLIDE 11
//        sharedUI.SetRotateHandler(this);
//        wire.OnSlideEnter(slideIndex);

//        Debug.Log("[SLIDE 7] Entered, ready to rotate");
//    }

//    void OnDisable()
//    {
//        sharedUI.ClearRotateHandler();
//    }

//    public void HandleRotate()
//    {
//        wire.RotateOnSlide(slideIndex, pos2);
//    }
//}
using UnityEngine;

public class Slide7Mechanism : MonoBehaviour, ISlideRotateHandler
{
    public int slideIndex = 7;

    public SharedUIController sharedUI;
    public WirePoseController wire;

    public Transform pos2;   // rotation target

    bool completed = false;
    public ScrewGaugeMechanism screwGauge;

    void Start()
    {
        screwGauge.EnableSliderInteraction();
        screwGauge.EnableFreeMode();
    }

    void OnEnable()
    {
        completed = false;

        // Existing behavior (DO NOT CHANGE)
        if (sharedUI)
            sharedUI.SetRotateHandler(this);

        if (wire)
            wire.OnSlideEnter(slideIndex);

        Debug.Log("[SLIDE 7] Entered, ready to rotate");
    }

    void OnDisable()
    {
        if (sharedUI)
            sharedUI.ClearRotateHandler();
    }

    public void HandleRotate()
    {
        // 🔄 Existing rotate logicsss
        if (wire)
            wire.RotateOnSlide(slideIndex, pos2);

        // 🔓 LOCK MECHANISM (ONLY ONCE)
        if (completed) return;

        completed = true;

        Debug.Log("✅ Slide 7 completed: Rotate pressed");

        SlideProgressManager.Instance.MarkCompleted(slideIndex);
    }
}
