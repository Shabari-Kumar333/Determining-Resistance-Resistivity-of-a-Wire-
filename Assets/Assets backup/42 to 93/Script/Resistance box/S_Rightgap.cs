using TMPro;
using UnityEngine;

public class S_Rightgap : MonoBehaviour

{
    [Header("Resistance Settings")]
    public ResistanceSystem resistanceBoxManager;
    public float unknownResistanceX = 2.715f;

    [Header("Meter Bridge Wire")]
    public Transform minPoint;
    public Transform maxPoint;
    public float totalWireLength = 100f;

    [Header("Jockey")]
    public Transform jockey;

    [Header("Galvanometer Settings")]
    public float K = 0.5f;
    public float maxVoltage = 30f;
    public TextMeshProUGUI galvanometerText;

    private Vector3 dragAxis;
    private Vector3 railOrigin;
    private float minDot;
    private float maxDot;

    public float jockeyPositionCm;
    public float balanceLength;
    public float galvanometerVoltage;

  
    void Start()
    {
        railOrigin = minPoint.position;
        dragAxis = (maxPoint.position - minPoint.position).normalized;

        minDot = 0f;
        maxDot = Vector3.Dot(maxPoint.position - railOrigin, dragAxis);
    }

    void Update()
    {
        if (resistanceBoxManager == null || jockey == null) return;

        // 1️⃣ Resistance box value
        float R = resistanceBoxManager.Resistance;
        // S in left → use right box


        float rawBalanceLength;

        if (resistanceBoxManager.IsRightGap)
        {
            // Box on LEFT, unknown on RIGHT
            rawBalanceLength = totalWireLength * R / (R + unknownResistanceX);
        }
        else
        {
            // Box on RIGHT, unknown on LEFT
            rawBalanceLength = totalWireLength * unknownResistanceX / (R + unknownResistanceX);

        }



        balanceLength = Mathf.Round(rawBalanceLength * 10f) / 10f;


        // 3️⃣ Jockey position in cm
        float dot = Vector3.Dot(jockey.position - railOrigin, dragAxis);
        dot = Mathf.Clamp(dot, minDot, maxDot);

        float rawJockeyCm = Mathf.InverseLerp(minDot, maxDot, dot) * 100f;
        jockeyPositionCm = Mathf.Round(rawJockeyCm * 10f) / 10f;

        // 4️⃣ Galvanometer voltage
        float rawVoltage = K * (jockeyPositionCm - balanceLength);
        rawVoltage = Mathf.Clamp(rawVoltage, -maxVoltage, maxVoltage);
        galvanometerVoltage = Mathf.Round(rawVoltage * 10f) / 10f;

        // 5️⃣ UI
        if (galvanometerText != null)
            galvanometerText.text = galvanometerVoltage.ToString("F2") + " V";

        // 6️⃣ Debug
        Debug.Log(
            $"VG = {galvanometerVoltage:F1} V | " +
            $"Jockey = {jockeyPositionCm:F1} cm | " +
            $"Balance = {balanceLength:F1} cm"
        );
    }
}
