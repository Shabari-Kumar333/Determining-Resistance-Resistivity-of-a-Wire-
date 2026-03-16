using UnityEngine;
using TMPro;
using System.Collections;

public class GlobalSlideNavigation : MonoBehaviour
{
    public static GlobalSlideNavigation Instance;

    public int totalSlides = 80;
    public int currentSlide = 1;

    public TMP_Text counterText;
    public SlideSetController setController;

    public int set1End = 25;
    public int set2End = 28;

    int activeSet = -1;
    bool switching = false;

    // ================= AUDIO =================
    [Header("SFX")]
    public AudioSource audioSource;
    public AudioClip nextSFX;
    public AudioClip prevSFX;
    // ❌ blockedSFX REMOVED

    void Awake()
    {
        Instance = this;
        Debug.Log("✅ GlobalSlideNavigation READY");
    }

    void Start()
    {
        Navigate();
    }

    public void Next()
    {
        if (switching || currentSlide >= totalSlides)
            return;

        // 🔒 LOCK CHECK (silent block)
        if (!SlideProgressManager.Instance.IsCompleted(currentSlide))
        {
            Debug.Log("🔒 BLOCKED → Slide " + currentSlide);
            return;
        }

        currentSlide++;
        PlayNextSFX();
        Navigate();
    }

    public void Previous()
    {
        if (switching || currentSlide <= 1)
            return;

        currentSlide--;
        PlayPrevSFX();
        Navigate();
    }

    void Navigate()
    {
        if (counterText)
            counterText.text = $"{currentSlide}/{totalSlides}";

        int targetSet =
            (currentSlide <= set1End) ? 1 :
            (currentSlide <= set2End) ? 2 : 3;

        if (targetSet != activeSet)
        {
            activeSet = targetSet;
            StartCoroutine(SwitchSet());
        }
        else
        {
            SetLocal();
        }
    }

    IEnumerator SwitchSet()
    {
        switching = true;
        setController.ActivateSet(activeSet);
        yield return null;
        SetLocal();
        switching = false;
    }

    void SetLocal()
    {
        int localIndex =
            (activeSet == 1) ? currentSlide - 1 :
            (activeSet == 2) ? currentSlide - 26 :
                               currentSlide - 29;

        setController.SetLocalSlide(activeSet, localIndex);
    }

    // ================= AUDIO HELPERS =================
    void PlayNextSFX()  
    {
        if (audioSource && nextSFX)
            audioSource.PlayOneShot(nextSFX);
    }

    void PlayPrevSFX()
    {
        if (audioSource && prevSFX)
            audioSource.PlayOneShot(prevSFX);
    }
}
