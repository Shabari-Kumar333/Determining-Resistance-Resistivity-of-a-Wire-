using UnityEngine;

public class Slide28ButtonUnlock : MonoBehaviour
{
    private bool completed = false;

    // 🔘 This must match Inspector EXACTLY
    public void OnButtonPressed()
    {
        if (completed) return;

        completed = true;

        // 🔓 Use CURRENT GLOBAL SLIDE (SAFE)
        int slideIndex = GlobalSlideNavigation.Instance.currentSlide;
        SlideProgressManager.Instance.MarkCompleted(slideIndex);

        Debug.Log($"✅ Slide {slideIndex} unlocked (Button Press)");
    }
}
