//using UnityEngine;

//public class Slide18Mechanism : MonoBehaviour, ISlideRotateHandler
//{
//    public int slideIndex = 18;
//    public SharedUIController sharedUI;
//    public WirePoseController wire;
//    public Transform pos3;

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
//        wire.RotateOnSlide(slideIndex, pos3);
//    }
//}
using UnityEngine;

public class Slide18Mechanism : MonoBehaviour, ISlideRotateHandler
{
    public int slideIndex = 18;

    public SharedUIController sharedUI;
    public WirePoseController wire;
    public Transform pos3;

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
            wire.RotateOnSlide(slideIndex, pos3);

        // 🔓 UNLOCK (ONLY ONCE)
        if (completed) return;

        completed = true;

        Debug.Log("✅ Slide 18 completed: Rotate pressed");

        SlideProgressManager.Instance.MarkCompleted(slideIndex);
    }
}
