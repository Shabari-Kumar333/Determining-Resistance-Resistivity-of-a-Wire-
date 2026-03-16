//using UnityEngine;

//public class SlideProgressManager : MonoBehaviour
//{
//    public static SlideProgressManager Instance;

//    [Header("Slide Settings")]
//    public int totalSlides = 80;

//    [Header("TEMP DEV / BUILD OVERRIDE")]
//    [Tooltip("If true, ALL slides are treated as completed (lock disabled)")]
//    public bool disableLockForBuild = true; // 🔥 MASTER LOCK BYPASS

//    bool[] completed;

//    void Awake()
//    {
//        if (Instance != null)
//        {
//            Destroy(gameObject);
//            return;
//        }

//        Instance = this;
//        DontDestroyOnLoad(gameObject);

//        completed = new bool[totalSlides + 1];

//        Debug.Log("✅ SlideProgressManager ALIVE");
//    }

//    void Start()
//    {
//        // 🔥 DEV SAFETY: Force first slide unlocked (ignored if bypass = true)

//    }



//    // ================= LOCK CHECK =================
//    public bool IsCompleted(int slide)
//    {
//        // 🚀 TEMP BUILD OVERRIDE
//        if (disableLockForBuild)
//            return true;

//        // 🛑 Safety check
//        if (slide < 0 || slide >= completed.Length)
//            return false;

//        return completed[slide];
//    }

//    // ================= REAL UNLOCK (FOR LATER) =================
//    public void MarkCompleted(int slide)
//    {
//        if (slide < 0 || slide >= completed.Length)
//            return;

//        completed[slide] = true;
//        Debug.Log($"✅ Slide {slide} marked as completed");
//    }
//}
using UnityEngine;

public class SlideProgressManager : MonoBehaviour
{
    public static SlideProgressManager Instance;

    [Header("Slide Settings")]
    public int totalSlides = 80;

    [Header("LOCK SETTINGS")]
    [Tooltip("Lock is applied only up to this slide index (inclusive)")]
    public int lockUntilSlide = 28;

    [Header("TEMP DEV / BUILD OVERRIDE")]
    [Tooltip("If true, ALL slides are treated as completed (lock disabled)")]
    public bool disableLockForBuild = false;

    bool[] completed;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        completed = new bool[totalSlides + 1];

        Debug.Log("✅ SlideProgressManager ALIVE");
    }

    // ================= LOCK CHECK =================
    public bool IsCompleted(int slide)
    {
        // 🔥 MASTER BYPASS
        if (disableLockForBuild)
            return true;

        // 🔓 AFTER SLIDE 28 → ALWAYS UNLOCKED
        if (slide > lockUntilSlide)
            return true;

        // 🛑 SAFETY
        if (slide < 0 || slide >= completed.Length)
            return false;

        return completed[slide];
    }

    // ================= REAL UNLOCK =================
    public void MarkCompleted(int slide)
    {
        if (slide < 0 || slide >= completed.Length)
            return;

        completed[slide] = true;
        Debug.Log($"✅ Slide {slide} marked as completed");
    }
}
