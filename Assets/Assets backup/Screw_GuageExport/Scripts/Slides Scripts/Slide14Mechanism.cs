//using UnityEngine;

//public class Slide14Mechanism : MonoBehaviour, ISlideRotateHandler
//{
//    public int slideIndex = 14;
//    public SharedUIController sharedUI;
//    public WirePoseController wire;
//    public Transform pos2_90;

//    void OnEnable()
//    {
//        sharedUI.SetRotateHandler(this);
//        wire.OnSlideEnter(slideIndex);
//    }

//    void OnDisable()
//    {
//        sharedUI.ClearRotateHandler();
//    }

//    public void HandleRotate()
//    {
//        wire.RotateOnSlide(slideIndex, pos2_90);
//    }
//}
using UnityEngine;

public class Slide14Mechanism : MonoBehaviour, ISlideRotateHandler
{
    public int slideIndex = 14;

    public SharedUIController sharedUI;
    public WirePoseController wire;
    public Transform pos2_90;

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

        if (sharedUI)
            sharedUI.SetRotateHandler(this);

        if (wire)
            wire.OnSlideEnter(slideIndex);
    }

    void OnDisable()
    {
        if (sharedUI)
            sharedUI.ClearRotateHandler();
    }

    public void HandleRotate()
    {
        // Existing rotate behavior
        if (wire)
            wire.RotateOnSlide(slideIndex, pos2_90);

        // 🔓 UNLOCK SLIDE (ONLY ONCE)
        if (completed) return;

        completed = true;

        Debug.Log("✅ Slide 14 completed: Rotate pressed");

        SlideProgressManager.Instance.MarkCompleted(slideIndex);
    }
}
