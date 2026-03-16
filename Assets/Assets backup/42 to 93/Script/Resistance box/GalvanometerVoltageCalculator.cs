using UnityEngine;
using TMPro;

public class GalvanometerVoltageCalculator : MonoBehaviour
{
    [Header("Reference (SOURCE OF TRUTH)")]
    public S_Rightgap s_Rightgap;

    [Header("Galvanometer Settings")]
    public float K = 0.5f;
    public float maxVoltage = 30f;

    [Header("UI")]
    public TextMeshProUGUI valueText;

    private float galvanometerVoltage;

    void Update()
    {
        if (s_Rightgap == null || valueText == null) return;

        // 🔥 Calculate galvanometer voltage
        float rawVoltage =
            K * (s_Rightgap.jockeyPositionCm - s_Rightgap.balanceLength);

        rawVoltage = Mathf.Clamp(rawVoltage, -maxVoltage, maxVoltage);

        // 🔢 Round to ONE decimal
        galvanometerVoltage = Mathf.Round(rawVoltage * 10f) / 10f;

        // 🖥️ Update UI (ONE decimal)
        valueText.text = galvanometerVoltage.ToString("F1") + " V";
    }
}
