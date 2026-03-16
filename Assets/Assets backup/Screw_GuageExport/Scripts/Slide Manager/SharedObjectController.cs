using UnityEngine;
using System.Collections.Generic;

public class SharedObjectController : MonoBehaviour
{
    [Header("Screw Gauge Reference (REQUIRED)")]
    public ScrewGaugeMechanism screwGauge;

    // ==========================
    // 🔒 LEGACY STATE STORAGE ONLY
    // ==========================
    private Dictionary<int, int> slideWireStateMap = new Dictionary<int, int>();

    // ==========================
    // 🔑 STATE QUERY
    // ==========================
    public bool HasWireStateForSlide(int slideIndex)
    {
        return slideWireStateMap.ContainsKey(slideIndex);
    }

    // ==========================
    // 🔘 ROTATE BUTTON (NO-OP)
    // ==========================
    public void MoveWireToNextState(int slideIndex)
    {
        int current = 0;
        if (slideWireStateMap.ContainsKey(slideIndex))
            current = slideWireStateMap[slideIndex];

        slideWireStateMap[slideIndex] = current + 1;

        Debug.Log($"[SharedObjectController] Wire logic removed | Slide {slideIndex}, State {current + 1}");
    }

    // ==========================
    // 🔙 SLIDE RESTORE (NO-OP)
    // ==========================
    public void RestoreWireForSlide(int slideIndex)
    {
        // intentionally empty
    }

    // ==========================
    // 🔒 LEGACY API (DO NOT REMOVE)
    // ==========================
    public void MoveToState(int state)
    {
        Debug.Log($"[SharedObjectController] MoveToState({state}) ignored");
    }

    public void MoveToState(int state, int slideIndex)
    {
        slideWireStateMap[slideIndex] = state;
        Debug.Log($"[SharedObjectController] MoveToState({state}, slide {slideIndex}) ignored");
    }

    // ==========================
    // 🧠 SCREW GAUGE MODE CONTROL
    // ==========================
    public void SetMeasurementMode()
    {
        if (screwGauge)
            screwGauge.EnableMeasurementMode();
    }

    public void SetFreeMode()
    {
        if (screwGauge)
            screwGauge.EnableFreeMode();
    }
}
