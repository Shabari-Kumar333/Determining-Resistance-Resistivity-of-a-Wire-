using UnityEngine;
using TMPro; // Essential for TextMeshPro

public class CmToMeterConverter : MonoBehaviour
{
    [Header("UI Connections")]
    public TMP_Text cmTextSource;   // Drag the text showing "10" here
    public TMP_Text meterResult;    // Drag the text showing "-- m" here

    void Update()
    {
        // This runs every frame to keep the meter value updated
        if (cmTextSource != null && meterResult != null)
        {
            ConvertToMeter(cmTextSource.text);
        }
    }

    void ConvertToMeter(string input)
    {
        // 1. Clean the input (remove "cm" if it's part of the text)
        string cleanInput = input.Replace("cm", "").Trim();

        // 2. If empty, show default
        if (string.IsNullOrEmpty(cleanInput))
        {
            meterResult.text = "0 m";
            return;
        }

        // 3. Try to convert text to number
        if (float.TryParse(cleanInput, out float cmValue))
        {
            // 4. Calculate Meters
            float finalMeter = cmValue / 100f;

            // 5. Update the result text
            meterResult.text = finalMeter.ToString("0.##") + " m";
        }
    }
}