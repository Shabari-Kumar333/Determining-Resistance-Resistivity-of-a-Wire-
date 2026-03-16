//using UnityEngine;

//public class Slide21Mechanism : MonoBehaviour, ISlideRotateHandler
//{
//    public int slideIndex = 21;
//    public SharedUIController sharedUI;
//    public WirePoseController wire;
//    public Transform pos3_90;

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
//        wire.RotateOnSlide(slideIndex, pos3_90);
//    }
//}
using UnityEngine;

public class Slide21Mechanism : MonoBehaviour, ISlideRotateHandler
{
    public int slideIndex = 21;

    public SharedUIController sharedUI;
    public WirePoseController wire;
    public Transform pos3_90;

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
            wire.RotateOnSlide(slideIndex, pos3_90);

        // 🔓 UNLOCK (ONLY ONCE)
        if (completed) return;

        completed = true;

        Debug.Log("✅ Slide 21 completed: Rotate pressed");

        SlideProgressManager.Instance.MarkCompleted(slideIndex);
    }
}
