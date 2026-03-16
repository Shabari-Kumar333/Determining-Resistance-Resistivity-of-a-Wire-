using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class ObservationTable3 : MonoBehaviour
{
    // =================================================
    // DATA SOURCES
    // =================================================
    [Header("Table 2 Data (Source of Calculation)")]
    public List<TMP_InputField> L;
    public List<TMP_InputField> S1;
    public List<TMP_InputField> Lp;
    public List<TMP_InputField> S2;

    // =================================================
    // THE 16 BOXES (Strict Order)
    // =================================================
    [Header("Step-by-Step Sequence")]
    [Tooltip("Drag the 16 texts here. The Mean Text MUST be the 16th (last) item.")]
    public List<TextMeshProUGUI> allCellsInOrder;

    // Step Tracking
    private int currentStepIndex = 0;

    // =================================================
    // ICONS
    // =================================================
    [Header("Feedback Icons")]
    public List<Image> correctIcons;
    public List<Image> wrongIcons;

    // =================================================
    // REFERENCES
    // =================================================
    [Header("References")]
    public numpadak numpad;

    // Constants
    private const decimal DELTA_L = 0.1m;
    private const int DECIMALS = 4;

    private void Start()
    {
        currentStepIndex = 0;
    }

    // =================================================
    // 1. PERMISSION CHECK
    // =================================================
    public bool IsCellAllowed(TextMeshProUGUI text)
    {
        if (allCellsInOrder == null || allCellsInOrder.Count == 0) return false;
        if (currentStepIndex >= allCellsInOrder.Count) return false;

        // Strict: You can only click the cell matching the current step
        return (text == allCellsInOrder[currentStepIndex]);
    }

    // =================================================
    // 2. CHECK BUTTON LOGIC
    // =================================================
    public void OnCheckButton()
    {
        if (currentStepIndex >= allCellsInOrder.Count) return;

        TextMeshProUGUI expectedCell = allCellsInOrder[currentStepIndex];
        TextMeshProUGUI activeCell = numpad.GetActiveText();

        // 1. Ensure user has clicked and typed in the correct cell
        if (activeCell != expectedCell) return;

        // 2. Calculate Expected Value
        string expectedValue = CalculateExpectedValueForStep(currentStepIndex);

        // Debug Log: Shows you exactly what value the system wants
        Debug.Log($"Checking Input. System Expects: {expectedValue}");

        if (expectedValue == "ERROR")
        {
            Debug.LogError("Math Error. Check Table 2 inputs.");
            return;
        }

        // 3. Process Check (Returns True/False)
        bool success = numpad.ProcessCheck(expectedValue);

        // 4. Update Icons
        if (correctIcons != null && currentStepIndex < correctIcons.Count)
        {
            if (correctIcons[currentStepIndex]) correctIcons[currentStepIndex].gameObject.SetActive(success);
            if (wrongIcons[currentStepIndex]) wrongIcons[currentStepIndex].gameObject.SetActive(!success);
        }

        // 5. Advance
        if (success)
        {
            currentStepIndex++;
            Debug.Log("Correct! Cell Locked. Please click the next one manually.");
        }
    }

    // =================================================
    // 3. MATH ENGINE
    // =================================================
    private string CalculateExpectedValueForStep(int index)
    {
        try
        {
            // === A. GRAND MEAN (Last Item) ===
            // This runs ONLY when you reach the 16th box
            if (index == allCellsInOrder.Count - 1)
            {
                return CalculateGrandMean();
            }

            // === B. NORMAL ROWS (Boxes 1-15) ===
            int row = index / 3;
            int col = index % 3;

            if (row >= L.Count) return "ERROR";

            decimal val_L = GetVal(L[row]);
            decimal val_S1 = GetVal(S1[row]);
            decimal val_Lp = GetVal(Lp[row]);
            decimal val_S2 = GetVal(S2[row]);

            decimal dS1 = CalcDeltaS(val_L, val_S1);
            decimal dS2 = CalcDeltaS(val_Lp, val_S2);

            if (col == 0) return dS1.ToString($"F{DECIMALS}");
            if (col == 1) return dS2.ToString($"F{DECIMALS}");
            if (col == 2) return Math.Round((dS1 + dS2) / 2m, DECIMALS).ToString($"F{DECIMALS}");

            return "ERROR";
        }
        catch { return "ERROR"; }
    }

    private string CalculateGrandMean()
    {
        decimal sumOfRoundedMeans = 0;
        int validRows = 0;

        // Loop through all input rows
        for (int r = 0; r < L.Count; r++)
        {
            // Skip empty rows
            if (string.IsNullOrEmpty(L[r].text) || string.IsNullOrEmpty(S1[r].text))
                continue;

            decimal val_L = GetVal(L[r]);
            decimal val_S1 = GetVal(S1[r]);
            decimal val_Lp = GetVal(Lp[r]);
            decimal val_S2 = GetVal(S2[r]);

            decimal dS1 = CalcDeltaS(val_L, val_S1);
            decimal dS2 = CalcDeltaS(val_Lp, val_S2);

            // IMPORTANT: We use the Rounded Row Mean to match manual calculations
            decimal rowMean = Math.Round((dS1 + dS2) / 2m, DECIMALS);

            sumOfRoundedMeans += rowMean;
            validRows++;
        }

        if (validRows == 0) return "0";

        // Average of the Rounded Means
        return Math.Round(sumOfRoundedMeans / validRows, DECIMALS).ToString($"F{DECIMALS}");
    }

    private decimal GetVal(TMP_InputField f)
    {
        if (f == null || string.IsNullOrEmpty(f.text)) return 0;
        return decimal.Parse(f.text);
    }

    private decimal CalcDeltaS(decimal l, decimal s)
    {
        if (l <= 0 || l >= 100) return 0;
        return Math.Round(s * ((DELTA_L / l) + (DELTA_L / (100m - l))), DECIMALS, MidpointRounding.AwayFromZero);
    }
}