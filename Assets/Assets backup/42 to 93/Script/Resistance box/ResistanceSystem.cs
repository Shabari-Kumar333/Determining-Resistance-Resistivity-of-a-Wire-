using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ResistanceSystem : MonoBehaviour
{
    [Header("UI")]
    public Button nextSlideButton;

    //[Header("Plugs")]
    //public ResistancePlug[] allPlugs;

    [Header("Resistance Limits")]
    public float minResistance = 1.5f;
    public float maxResistance = 6.0f;

    [Header("Gap State")]
    [Tooltip("true = Right gap, false = Left gap")]
    public bool IsRightGap = true;

    [Header("Events")]
    public UnityEvent OnResistanceChanged;

    // 🔒 Single source of truth
    public float Resistance { get; private set; }

    void OnEnable()
    {
        RecalculateResistance();
    }

    void OnDisable()
    {
        if (nextSlideButton != null)
            nextSlideButton.interactable = true;
    }

    // ================================
    // Plug API (called by ResistancePlug)
    // ================================
    public void OnPlugRemoved(float ohms)
    {
        Resistance += ohms;
        OnResistanceUpdated();
    }

    public void OnPlugInserted(float ohms)
    {
        Resistance = Mathf.Max(0f, Resistance - ohms);
        OnResistanceUpdated();
    }

    // ================================
    // Gap API
    // ================================
    public void SetGap(bool rightGap)
    {
        IsRightGap = rightGap;
    }

    // ================================
    // Core Logic
    // ================================
    void OnResistanceUpdated()
    {
        Debug.Log($"Current Resistance: {Resistance} Ω");

        ValidateResistance();
        OnResistanceChanged?.Invoke();
    }

    void RecalculateResistance()
    {
        Resistance = 0f;

        ////foreach (ResistancePlug plug in allPlugs)
        //{
        //    if (plug == null) continue;

        //    // Plug OUT → adds resistance
        //    if (!plug.isPluggedIn)
        //        Resistance += plug.ohmsValue;
        //}

        OnResistanceUpdated();
    }

    void ValidateResistance()
    {
        if (nextSlideButton == null)
        {
            Debug.LogError("Next Slide Button not assigned!");
            return;
        }

        bool valid =
            Resistance >= minResistance &&
            Resistance <= maxResistance;

        nextSlideButton.interactable = valid;

        Debug.Log(valid
            ? "Resistance valid → Next enabled"
            : "Resistance invalid → Next locked");
    }
}
