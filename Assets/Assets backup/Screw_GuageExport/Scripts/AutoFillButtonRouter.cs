using UnityEngine;

public class AutoFillButtonRouter : MonoBehaviour
{
    [Header("Handlers")]
    public AutoFillController_Set1 set1AutoFill;
    public FinalMeanValidator finalMeanValidator;

    [Header("Final Mean Slide")]
    public int finalMeanSlide = 25;

    public void OnAutoFillPressed()
    {
        int slide = GlobalSlideNavigation.Instance.currentSlide;

        Debug.Log($"[AUTO FILL ROUTER] Slide {slide}");

        if (slide == finalMeanSlide)
        {
            // ✅ Slide 25 → Final Mean
            finalMeanValidator?.OnAutoFillPressed();
        }
        else
        {
            // ✅ Slides 1–24 → Normal Auto Fill
            set1AutoFill?.OnAutoFillPressed();
        }
    }
}
