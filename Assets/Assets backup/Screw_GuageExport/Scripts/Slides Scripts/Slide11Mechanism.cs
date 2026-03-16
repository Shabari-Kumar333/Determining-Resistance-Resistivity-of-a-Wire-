//using UnityEngine;

//public class Slide11Mechanism : MonoBehaviour, ISlideRotateHandler
//{
//    public int slideIndex = 11;
//    public SharedUIController sharedUI;
//    public WirePoseController wire;
//    public Transform pos2;

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
//        wire.RotateOnSlide(slideIndex, pos2);
//    }
//}
using UnityEngine;

public class Slide11Mechanism : MonoBehaviour, ISlideRotateHandler
{
    public int slideIndex = 11;

    public SharedUIController sharedUI;
    public WirePoseController wire;
    public Transform pos2;

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
            wire.RotateOnSlide(slideIndex, pos2);

        // 🔓 LOCK UNLOCK (ONLY ONCE)
        if (completed) return;

        completed = true;

        Debug.Log("✅ Slide 11 completed: Rotate pressed");

        SlideProgressManager.Instance.MarkCompleted(slideIndex);
    }
}
