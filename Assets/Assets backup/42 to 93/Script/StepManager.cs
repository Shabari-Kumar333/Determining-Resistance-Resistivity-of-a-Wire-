//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Events;

//public class StepPlayer : MonoBehaviour
//{
//    [Header("Elements")]
//    public List<GameObject> elements;

//    [Header("Forward Events (on enter)")]
//    public List<UnityEvent> forwardEvents;

//    [Header("Backward Events (on exit)")]
//    public List<UnityEvent> backwardEvents;

//    private int currentIndex = -1;

//    // ---------------- FORWARD ----------------
//    public void PlayForward()
//    {
//        if (currentIndex >= elements.Count - 1)
//            return;

//        currentIndex++;
//        ShowElement(currentIndex);
//        PlayForwardEvent(currentIndex);
//    }

//    // ---------------- BACKWARD ----------------
//    public void PlayBackward()
//    {
//        if (currentIndex <= 0)
//            return;

//        StartCoroutine(BackwardRoutine());
//    }

//    private IEnumerator BackwardRoutine()
//    {
//        // 1️⃣ Play backward event of CURRENT element
//        PlayBackwardEvent(currentIndex);

//        // ⏳ wait ONE frame so event finishes first
//        yield return null;

//        // 2️⃣ Move to previous element
//        currentIndex--;
//        ShowElement(currentIndex);
//    }

//    // ---------------- ELEMENT CONTROL ----------------
//    private void ShowElement(int index)
//    {
//        for (int i = 0; i < elements.Count; i++)
//        {
//            if (elements[i] != null)
//                elements[i].SetActive(false);
//        }

//        if (elements[index] != null)
//            elements[index].SetActive(true);
//    }

//    // ---------------- EVENTS ----------------
//    private void PlayForwardEvent(int index)
//    {
//        if (index < forwardEvents.Count)
//            forwardEvents[index]?.Invoke();
//    }

//    private void PlayBackwardEvent(int index)
//    {
//        if (index < backwardEvents.Count)
//            backwardEvents[index]?.Invoke();
//    }
//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StepPlayer : MonoBehaviour
{
    [Header("Elements")]
    public List<GameObject> elements;

    [Header("Forward Events (on enter)")]
    public List<UnityEvent> forwardEvents;

    [Header("Backward Events (on exit)")]
    public List<UnityEvent> backwardEvents;

    // 🔒 Internal step index
    int currentIndex = -1;

    [Header("Reset Events (always called before enter)")]
    public List<UnityEvent> resetEvents;


    // =====================================================
    // 🔥 IMPORTANT: RESET WHEN SET-3 IS ENABLED
    // =====================================================
    void OnEnable()
    {
        ResetSteps();
    }

    void ResetSteps()
    {
        currentIndex = -1;

        // Disable all elements safely
        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i])
                elements[i].SetActive(false);
        }
    }

    // =====================================================
    // FORWARD
    // =====================================================
    public void PlayForward()
    {
        if (currentIndex >= elements.Count - 1)
            return;

        int nextIndex = currentIndex + 1;

        // 🧼 RESET TARGET SLIDE STATE
        PlayResetEvent(nextIndex);

        currentIndex = nextIndex;

        ShowElement(currentIndex);
        PlayForwardEvent(currentIndex);

        PrintIndex("Forward");
    }



    // =====================================================
    // BACKWARD
    // =====================================================
    public void PlayBackward()
    {
        if (currentIndex <= 0)
            return;

        StartCoroutine(BackwardRoutine());
    }

    IEnumerator BackwardRoutine()
    {
        // 🚪 Exit current slide
        PlayBackwardEvent(currentIndex);

        yield return null;

        int prevIndex = currentIndex - 1;

        // 🧼 RESET TARGET SLIDE STATE
        PlayResetEvent(prevIndex);

        currentIndex = prevIndex;

        ShowElement(currentIndex);
        PlayForwardEvent(currentIndex); // optional re-enter

        PrintIndex("Backward");
    }

    void PlayResetEvent(int index)
    {
        if (index >= 0 && index < resetEvents.Count)
            resetEvents[index]?.Invoke();
    }


    // =====================================================
    // ELEMENT CONTROL
    // =====================================================
    void ShowElement(int index)
    {
        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i])
                elements[i].SetActive(false);
        }

        if (index >= 0 && index < elements.Count && elements[index])
            elements[index].SetActive(true);
    }

    // =====================================================
    // EVENTS
    // =====================================================
    void PlayForwardEvent(int index)
    {
        if (index < forwardEvents.Count)
            forwardEvents[index]?.Invoke();
    }

    void PlayBackwardEvent(int index)
    {
        if (index >= 0 && index < backwardEvents.Count)
            backwardEvents[index]?.Invoke();
    }


    // =====================================================
    // 🔑 REQUIRED FOR GLOBAL NAVIGATION
    // =====================================================

    void PrintIndex(string direction)
    {
        Debug.Log($"[StepPlayer] {direction} → Current Index: {currentIndex}");
    }

    public int GetCurrentIndex()
    {
        return currentIndex;
    }
    void DebugWhoDisabled(GameObject target)
    {
        if (!target) return;

        Transform t = target.transform;
        while (t != null)
        {
            Debug.Log(
                $"[DEBUG] {t.name} | activeSelf={t.gameObject.activeSelf} | activeInHierarchy={t.gameObject.activeInHierarchy}"
            );
            t = t.parent;
        }
    }
    
}
