//using UnityEngine;

//public class Slide4Mechanism : MonoBehaviour, ISlideRotateHandler
//{
//    public int slideIndex = 4;
//    public SharedUIController sharedUI;
//    public WirePoseController wire;
//    public Transform pos1;

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
//        wire.RotateOnSlide(slideIndex, pos1);
//    }
//}
using UnityEngine;

public class Slide4Mechanism : MonoBehaviour, ISlideRotateHandler
{
    public int slideIndex = 4;
    public SharedUIController sharedUI;
    public WirePoseController wire;
    public Transform pos1;

    bool completed = false;

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
        // 🔁 Existing behavior (DO NOT TOUCH)
        if (wire)
            wire.RotateOnSlide(slideIndex, pos1);

        // 🔓 COMPLETE SLIDE (ONLY ONCE)
        if (completed) return;

        completed = true;

        Debug.Log("✅ Slide 4 completed: Rotate pressed");

        SlideProgressManager.Instance.MarkCompleted(slideIndex);
    }
}
