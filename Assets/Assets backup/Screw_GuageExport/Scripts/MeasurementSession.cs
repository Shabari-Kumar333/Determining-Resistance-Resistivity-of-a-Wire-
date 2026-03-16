
using UnityEngine;
using System.Collections.Generic;

public class MeasurementSession : MonoBehaviour
{
    public static MeasurementSession Instance;

    public enum MeanMode
    {
        None,
        SetMean,
        FinalMean
    }

    [Header("Runtime")]
    public float currentGaugeValue;
    public float expectedGaugeValue;

    [Header("Validation")]
    public float tolerance = 0.001f;

    [Header("Table Index")]
    public int currentSet;
    public int currentStep;

    [Header("Mean State")]
    public MeanMode meanMode = MeanMode.None;

    // =====================================================
    // 🔑 CANONICAL STORAGE (STRING → NO ROUNDING LOSS)
    // =====================================================
    private Dictionary<int, string> readingText = new();
    private Dictionary<int, string> meanText = new();

    [Header("Final Mean")]
    public string finalMeanText = "";
    public bool finalMeanLocked = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // =====================================================
    // SLIDE API (UNCHANGED)
    // =====================================================
    public void SetStep(int set, int step)
    {
        currentSet = set;
        currentStep = step;
        meanMode = MeanMode.None;
    }

    public void SetSetMeanStage(int setIndex)
    {
        currentSet = setIndex;
        currentStep = 2;
        meanMode = MeanMode.SetMean;
    }

    public void SetFinalMeanStage()
    {
        meanMode = MeanMode.FinalMean;
    }

    // =====================================================
    // BACKWARD COMPATIBILITY
    // =====================================================
    public float GetSetMean(int setIndex)
    {
        if (!TryGetReadingText(setIndex, 0, out var aStr)) return 0f;
        if (!TryGetReadingText(setIndex, 1, out var bStr)) return 0f;

        float a = float.Parse(aStr);
        float b = float.Parse(bStr);

        return Round((a + b) / 2f);
    }

    // ⚠️ PROPERTY — DO NOT DUPLICATE
    public List<float> means
    {
        get
        {
            var list = new List<float>();
            for (int i = 0; i < 3; i++)
            {
                if (meanText.TryGetValue(i, out var v))
                    list.Add(float.Parse(v));
            }
            return list;
        }
    }

    public float Round(float v)
    {
        return Mathf.Round(v * 1000f) / 1000f;
    }

    // =====================================================
    // STRING STORAGE
    // =====================================================
    private int Key(int set, int step) => set * 10 + step;

    public void SaveReadingText(int set, int step, string value)
    {
        readingText[Key(set, step)] = value;
    }

    public bool TryGetReadingText(int set, int step, out string value)
    {
        return readingText.TryGetValue(Key(set, step), out value);
    }

    public void SaveMeanText(int set, string value)
    {
        meanText[set] = value;
    }

    public bool TryGetMeanText(int set, out string value)
    {
        return meanText.TryGetValue(set, out value);
    }

    // =====================================================
    // ✅ FINAL MEAN — SINGLE SOURCE OF TRUTH
    // =====================================================
    // =====================================================
    // ✅ FINAL MEAN — SAFE & CORRECT
    // =====================================================
    public float GetFinalMean()
    {
        float sum = 0f;
        int count = 0;

        for (int i = 0; i < 3; i++)
        {
            if (TryGetMeanText(i, out var meanStr))
            {
                sum += float.Parse(meanStr);
                count++;
            }
        }

        if (count == 0)
        {
            Debug.LogWarning("[FinalMean] No mean values found!");
            return 0f;
        }

        float finalMean = Round(sum / count);
        Debug.Log($"[FinalMean] Calculated = {finalMean}");
        return finalMean;
    }

    public void SaveFinalMean(string value)
    {
        finalMeanText = value;
        finalMeanLocked = true;
    }

    public bool HasFinalMean()
    {
        return finalMeanLocked;
    }
}
